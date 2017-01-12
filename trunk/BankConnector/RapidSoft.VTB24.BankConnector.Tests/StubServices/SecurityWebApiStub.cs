using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RapidSoft.VTB24.Site.SecurityWebApi;

namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
    public class SecurityWebApiStub : StubBase, ISecurityWebApi
	{
        private static List<string> deniedPhones = new List<string>();
        private static List<string> registeredPhones = new List<string>();
        private static List<string> enabledClients = new List<string>();
        private static List<string> disabledClients = new List<string>(); 

        public static void CleanUpService()
        {
            registeredPhones = new List<string>();
            enabledClients.Clear();
            disabledClients.Clear();
        }

		public void CreateUser(CreateUserOptions parameters)
		{
			if (string.IsNullOrEmpty(parameters.ClientId))
			{
				throw new ArgumentException("CreateClientAccountOptions.ClientId is null or empty");
			}

			if (string.IsNullOrEmpty(parameters.PhoneNumber))
			{
				throw new ArgumentException("CreateClientAccountOptions.PhoneNumber is null or empty");
			}

			registeredPhones.Add(parameters.PhoneNumber);
		}

        public Task CreateUserAsync(CreateUserOptions parameters)
		{
			throw new NotImplementedException();
		}

	    public void DenyRegistrationRequest(DenyRegistrationRequestOptions options)
	    {
	        deniedPhones.Add(options.PhoneNumber);
	    }

	    public Task DenyRegistrationRequestAsync(DenyRegistrationRequestOptions options)
	    {
	        throw new NotImplementedException();
	    }

	    public void CreateUserAndPassword(CreateUserAndPasswordOptions options)
	    {
	        throw new NotImplementedException();
	    }

	    public Task CreateUserAndPasswordAsync(CreateUserAndPasswordOptions options)
	    {
	        throw new NotImplementedException();
	    }

	    public void DeleteUser(string phoneNUmber)
	    {
	        if (registeredPhones.Contains(phoneNUmber)) registeredPhones.Remove(phoneNUmber);
	    }

		public Task DeleteUserAsync(string login)
		{
			throw new NotImplementedException();
		}

	    public void DisableUser(string login)
	    {
	        disabledClients.Add(login);
	    }

	    public Task DisableUserAsync(string login)
	    {
	        throw new NotImplementedException();
	    }

	    public void EnableUser(string login)
	    {
            enabledClients.Add(login);
	    }

	    public Task EnableUserAsync(string login)
	    {
	        throw new NotImplementedException();
	    }

	    public Dictionary<string, User> BatchResolveUsersByClientId(string[] clientIds)
	    {
            var result = new Dictionary<string, User>();
	        var random = new Random();
            foreach (var clientId in clientIds)
            {
                if (clientId != "9271234567" && clientId != "notRegisred")
                    result[clientId] = new User
                    {
                        ClientId = clientId,
                        Id = random.Next(),
                        PhoneNumber = "9271234567",
                        RegistrationDate = DateTime.Now.AddDays(-50),
                        IsDisabled = false,
                        IsPasswordSet = true
                    };
            }
            return result;
        }

	    public Task<Dictionary<string, User>> BatchResolveUsersByClientIdAsync(string[] clientIds)
	    {
	        throw new NotImplementedException();
	    }

	    public Dictionary<string, User> BatchResolveUsersByPhone(string[] clientPhones)
	    {
	        var result = new Dictionary<string, User>();
		    foreach (var clientPhone in clientPhones)
		    {
                if (registeredPhones.Any(x => clientPhone.EndsWith(x)))
			    {
				    result[clientPhone] = new User
					                          {
						                          ClientId = clientPhone,
						                          Id = 1,
						                          PhoneNumber = clientPhone,
						                          RegistrationDate = DateTime.Now.AddDays(-50),
						                          IsDisabled = false,
						                          IsPasswordSet = true
					                          };
			    }
		    }

		    return result;
	    }

	    public Task<Dictionary<string, User>> BatchResolveUsersByPhoneAsync(string[] clientPhones)
	    {
	        var task = new Task<Dictionary<string, User>>(() => BatchResolveUsersByPhone(clientPhones));
            task.Start();
	        return task;
	    }

	    public ChangeUserPhoneNumberResult ChangeUserPhoneNumber(ChangePhoneNumberOptions options)
	    {
	        return new ChangeUserPhoneNumberResult { Success = true, Status = ChangeUserPhoneNumberStatus.Changed };
	    }

	    public Task<ChangeUserPhoneNumberResult> ChangeUserPhoneNumberAsync(ChangePhoneNumberOptions options)
	    {
	        throw new NotImplementedException();
	    }

        public ResetUserPasswordResult ResetUserPassword(ResetUserPasswordOptions options)
        {
            if (options.Login == "79876543210")
            {
                throw new Exception("STUB exception (always thrown if login = 79876543210)");
            }

            return new ResetUserPasswordResult { Success = true, Status = ResetUserPasswordStatus.Changed };
        }

        public Task<ResetUserPasswordResult> ResetUserPasswordAsync(ResetUserPasswordOptions options)
        {
            throw new NotImplementedException();
        }

        public string Echo(string message)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<string> EchoAsync(string message)
	    {
	        throw new NotImplementedException();
	    }
	}
}
