using System.Threading.Tasks;

using RapidSoft.VTB24.BankConnector.Infrastructure.Exceptions;
using RapidSoft.VTB24.BankConnector.Infrastructure.Logging;

namespace RapidSoft.VTB24.LoadTesting
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using RapidSoft.Loaylty.Processing.ProcessingService;
    using RapidSoft.VTB24.Site.SecurityWebApi;
    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientInboxService;
    using ClientProfile = RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using Processing = RapidSoft.Loaylty.Processing.ProcessingService;

    //• Создать пользователей в лояльности: в профиле и процессинге(при регистрации после ответа банка)
    //• Активировать пользователей: в профиле и процессинге
    //• Начислить пользователям баллов в процессинге (todo)
    //• Создать учётки пользователям на фальшивые телефоны черезhttp://localhost:6761/SecurityWebApi.svc, метод CreateUserAndPassword
    //• Отправить пользователям несколько личных сообщений
    class User
    {
        public string ClientId;
        public string Email;
        public string MobilePhone;
        public string LastName;
        public string FirstName;
        public string MiddleName;
        public int Gender = 0;
    }

    class Program
    {
        private const int LoyaltyProgramId = 5;
        private static ClientProfileService clientProfileService;
        private static ProcessingService processingService;
        private static ISecurityWebApi securityWebApi;
        private static IClientInboxService clientInboxService;

        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Specify parameters! E.g. for 5 users - [Program].exe 5");
                Console.ReadLine();
                return;
            }

            uint usersCount;

            if (!uint.TryParse(args[0], out usersCount))
            {
                Console.WriteLine("Parameter must be valid int");
                Console.ReadLine();
                return;
            }

            CreateRandomUsers(usersCount);
        }

        private static void Init()
        {
            IUnityContainer container = new UnityContainer();
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(container);

            clientProfileService = container.Resolve<ClientProfileService>();
            processingService = container.Resolve<ProcessingService>();
            securityWebApi = container.Resolve<ISecurityWebApi>();
            clientInboxService = container.Resolve<IClientInboxService>();
        }

        private static void CreateRandomUsers(uint usersCount)
        {
            try
            {
                LogHelper.WriteInfo("Initialization started");
                Init();
                LogHelper.WriteInfo("Initialization finished");

                LogHelper.WriteInfo("Users creation started. Creating 100 users.");

                const int Step = 5;

                // TODO: Надо единицу добавлять если не кратно пяти.
                var threadCount = usersCount > Step ? usersCount / Step : 1;

                var tasks = new Task[threadCount];

                for (var i = 0; i < threadCount; i++)
                {
                    tasks[i] = Task.Factory.StartNew(() => CreateUsers(Step));
                }

                Task.WaitAll(tasks);

                LogHelper.WriteInfo("User creation finished");

                LogHelper.WriteInfo("Created users");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        private static List<User> CreateUsers(int userNum)
        {
            
            var result = new List<User>();
            var users = GenerateUsers(userNum);

            LogHelper.WriteInfo("Users created in memory. Beginning to call services.");

            var etlSessionId = Guid.NewGuid().ToString();

            LogHelper.WriteInfo("EtlSessionId is: " + etlSessionId);

            try
            {
                ProcessInExternalSystems(users, etlSessionId, result);
            }
            finally 
            {
                LogHelper.WriteInfo("All users:");
                users.ForEach(u => LogHelper.WriteInfo(string.Format("{0} {1} {2} {3} {4} {5}", u.ClientId, u.FirstName, u.MiddleName, u.LastName, u.MobilePhone, u.Email)));
                LogHelper.WriteInfo("Successfull users:");
                result.ForEach(u => LogHelper.WriteInfo(string.Format("{0} {1} {2} {3} {4} {5}", u.ClientId, u.FirstName, u.MiddleName, u.LastName, u.MobilePhone, u.Email)));
            }

            return result;
        }

        private static List<User> GenerateUsers(int userNum)
        {
            var users = new List<User>();
            for (var i = 0; i < userNum; i++)
            {
                users.Add(
                    new User
                        {
                            ClientId = "vtb_test_" + Faker.RandomNumber.Next(),
                            MobilePhone = "7998" + Faker.RandomNumber.Next(1000000, 9999999),
                            LastName = Faker.Name.Last(),
                            FirstName = Faker.Name.First(),
                            MiddleName = Faker.Name.First(),
                            Email = Faker.Internet.Email(Faker.Name.FullName())
                        });
            }
            
            //users.Add(new User
            //        {
            //            ClientId = "d698b2bd-c68e-40d8-8a9b-9b754a946f98",
            //            MobilePhone = "79190690556",
            //            LastName = "Севрюгина",
            //            FirstName = "Анастасия",
            //            MiddleName = "Михайловна",
            //            Email = "asevryugina@ozon.ru"
            //        });
            //users.Add(new User
            //        {
            //            ClientId = "f352c694-2a6d-42b2-9b89-270b725d524e",
            //            MobilePhone = "79043531901",
            //            LastName = "Кудряшова",
            //            FirstName = "Ольга",
            //            MiddleName = "Викторовна",
            //            Email = "ol.ka_080710@inbox.ru"
            //        });
            //users.Add(new User
            //        {
            //            ClientId = "5be037e1-3bec-4b60-a913-97d20c47771f",
            //            MobilePhone = "79040297703",
            //            LastName = "Полякова",
            //            FirstName = "Надежда",
            //            MiddleName = "Сергеевна",
            //            Email = "ster-vella@mail.ru"
            //        });
            //users.Add(new User
            //        {
            //            ClientId = "f2386488-1081-4f35-9c4d-a6783f35bc5a",
            //            MobilePhone = "79301644287",
            //            LastName = "Меликян",
            //            FirstName = "Инесса",
            //            MiddleName = "Кареновна",
            //            Email = "meinessa@yandex.ru"
            //        });
            //users.Add(new User
            //        {
            //            ClientId = "2d61a33c-6d7f-4c18-baf4-e80ff9f75904",
            //            MobilePhone = "79201723468",
            //            LastName = "Радушина",
            //            FirstName = "Анна",
            //            MiddleName = "Владимировна",
            //            Email = ""
            //        });

            return users;
        }

        private static void ProcessInExternalSystems(List<User> users, string etlSessionId, List<User> result)
        {
            var profileUsers = ClientProfileCreateClients(users, etlSessionId);
            LogHelper.WriteInfo(string.Format("1. Client profile created for {0} users.", profileUsers.Count));

            var processingUsers = ProcessingCreateClients(users.Where(u => profileUsers.Contains(u.ClientId)), etlSessionId);
            LogHelper.WriteInfo(string.Format("2. Processing profile created for {0} users.", processingUsers.Count));

            var profileActivated = ClientProfileActivateClients(
                users.Where(u => processingUsers.Contains(u.ClientId)), etlSessionId);
            LogHelper.WriteInfo(string.Format("3. In ClientProfile activated {0} users.", profileActivated.Count));

            var processingActivated = ProcessingActivateClients(
                users.Where(u => profileActivated.Contains(u.ClientId)), etlSessionId);
            LogHelper.WriteInfo(string.Format("4. In Processing activated {0} users.", processingActivated.Count));

            var securityCreated = SecurityCreateUsers(users.Where(u => processingActivated.Contains(u.ClientId)).ToList());
            LogHelper.WriteInfo(string.Format("5. Created in security {0} users.", securityCreated.Count));

            var messaged = ClientInboxSaveMessages(users.Where(u => securityCreated.Contains(u.ClientId)));
            LogHelper.WriteInfo(string.Format("6. Messages sent to {0} users.", messaged.Count));

            var payed = Deposit(users.Where(u => messaged.Contains(u.ClientId)), etlSessionId);
            LogHelper.WriteInfo(string.Format("7. Desposit set for {0} users.", payed));

            if (messaged.Count != payed)
            {
                LogHelper.WriteError(
                    string.Format(
                        "Some users' balance was not increased. Users with messages sent: {0}, users with balabce increased: {1}.",
                        messaged.Count,
                        payed));
            }

            result.AddRange(users.Where(u => messaged.Contains(u.ClientId)).ToList());
        }

        private static List<string> ClientInboxSaveMessages(IEnumerable<User> users)
        {
            var successfullUsers = new List<string>();
            
            foreach (var user in users)
            {
                var res = clientInboxService.SaveMessages(new[]
                    {
                        new MessageDto
                            {
                                ClientId = user.ClientId,
                                Subject = Faker.Lorem.Sentence(1),
                                Text = Faker.Lorem.Sentence(3)
                            }
                    });

                if (res.Success)
                {
                    successfullUsers.Add(user.ClientId);
                }
            }

            return successfullUsers;
        }

        private static List<string> SecurityCreateUsers (List<User> users)
        {
            var successfullUsers = new List<string>();

            foreach (var user in users)
            {
                try
                {
                    securityWebApi.CreateUser(new CreateClientAccountOptions
                                        {
                                            ClientId = user.ClientId,
                                            PhoneNumber = user.MobilePhone,
                                        });

                    successfullUsers.Add(user.ClientId);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.HandleGeneralException(ex);
                }
                
            }

            return successfullUsers;
        }

        private static int Deposit(IEnumerable<User> users, string etlSessionId)
        {
            var depositTransactions = users.Select(u => new BatchDepositByClientsRequestTypeDepositTransaction()
                {
                    BonusSum = 50000,
                    ClientExternalId = u.ClientId,
                });

            var res = processingService.BatchDepositByClients(new BatchDepositByClientsRequest()
                {
                    Request = new BatchDepositByClientsRequestType()
                        {
                            EtlSessionId = etlSessionId,
                            LoyaltyProgramId = 5,
                            DepositTransactions = depositTransactions.ToArray()
                        }
                });

            return res.Response.DepositByClientResults.Count(s => s.StatusCode == 0);
        }

        private static List<string> ClientProfileActivateClients(IEnumerable<User> users, string etlSessionId)
        {
            var facts = users.Select(user => new BatchActivateClientsRequestTypeClientActivationFact
            {
                ClientExternalId = user.ClientId
            }).ToArray();

            var res = clientProfileService.BatchActivateClients(new ClientProfile.BatchActivateClientsRequest
            {
                Request = new ClientProfile.BatchActivateClientsRequestType
                {
                    LoyaltyProgramId = LoyaltyProgramId,
                    EtlSessionId = etlSessionId, 
                    ClientActivationFacts = facts
                }
            });

            return res.Response.ClientActivationResults == null ? new List<string>() : res.Response.ClientActivationResults.Where(s => s.StatusCode == 0).Select(s => s.ClientId).ToList();
        }

        private static List<string> ProcessingActivateClients(IEnumerable<User> users, string etlSessionId)
        {
            var facts = users.Select(user => new BatchActivateClientsRequestTypeActivationFact
                {
                    ClientExternalId = user.ClientId
                }).ToArray();
            
            var res = processingService.BatchActivateClients(new Processing.BatchActivateClientsRequest
            {
                Request = new Processing.BatchActivateClientsRequestType
                                {
                                    LoyaltyProgramId = LoyaltyProgramId,
                                    EtlSessionId = etlSessionId,
                                    ActivationFacts = facts
                                }
            });

            return res.Response.ClientActivationResults == null ? new List<string>() : res.Response.ClientActivationResults.Where(s => s.StatusCode == 0).Select(s => s.ClientId).ToList();
        }

        private static List<string> ClientProfileCreateClients(IEnumerable<User> users, string etlSessionId)
        {
            var facts = users.Select(user => new ClientProfile.BatchCreateClientsRequestTypeClientRegistrationFact
                {
                    ClientId = user.ClientId,
                    ClientExternalId = user.ClientId,
                    Email = user.Email,
                    MobilePhone = user.MobilePhone,
                    Gender = user.Gender,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName
                }).ToArray();
            
           var response = clientProfileService.BatchCreateClients(new ClientProfile.BatchCreateClientsRequest
            {
                Request =
                    new ClientProfile.BatchCreateClientsRequestType
                        {
                            EtlSessionId = etlSessionId,
                            LoyaltyProgramId = LoyaltyProgramId,
                            ClientRegistrationFacts = facts
                        }
            });

            return response.Response.ClientRegistrationResults.Where(s => s.StatusCode == 0).Select(s => s.ClientId).ToList();
        }

        private static List<string> ProcessingCreateClients(IEnumerable<User> users, string etlSessionId)
        {
            var facts = users.Select(user => new Processing.BatchCreateClientsRequestTypeClientRegistrationFact
                {
                    ClientId = user.ClientId,
                    ClientExternalId = user.ClientId
                }
                ).ToArray();
            
            var response = processingService.BatchCreateClients(new Processing.BatchCreateClientsRequest
            {
                Request = new Processing.BatchCreateClientsRequestType
                {
                    LoyaltyProgramId = LoyaltyProgramId,
                    EtlSessionId = etlSessionId,
                    ClientRegistrationFacts = facts
                }
            });

            return response.Response.ClientRegistrationResults.Where(s => s.StatusCode == 0).Select(s => s.ClientId).ToList();
        }
    }
}
