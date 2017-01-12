namespace RapidSoft.VTB24.BankConnector.Service
{
    using System;
    using System.Text.RegularExpressions;

    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;

    public class BankSmsService : IBankSmsService
    {
        private static readonly Regex PhoneRegex = new Regex("^8[0-9]{10}$");

        private readonly IUnitOfWork uow;

        public BankSmsService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public SimpleBankConnectorResponse EnqueueSms(BankSmsType type, string phone, string password)
        {
            try
            {
                ValidatePhone(phone);

                var sms = ToSms(type, phone, password);

                uow.BankSmsRepository.Add(sms);

                uow.Save();

                return new SimpleBankConnectorResponse();
            }
            catch (Exception e)
            {
                return new SimpleBankConnectorResponse(e);
            }
        }

        private static BankSms ToSms(BankSmsType type, string phone, string password)
        {
            return new BankSms
            {
                TypeCode = ToSmsTypeCode(type),
                Phone = phone,
                DisplayPhone = ToDisplayPhone(phone),
                Password = password,
                InsertedDate = DateTime.Now
            };
        }

        private static void ValidatePhone(string phone)
        {
            if (phone == null)
            {
                throw new ArgumentNullException("не передан номер телефона");
            }

            if (!PhoneRegex.Match(phone).Success)
            {
                throw new ArgumentException("некорректный номер телефона");
            }
        }

        private static string ToDisplayPhone(string phone)
        {
            return
                "+7(" + phone.Substring(1, 3) + ")" +
                phone.Substring(4, 3) + "-" +
                phone.Substring(7, 2) + "-" +
                phone.Substring(9, 2);
        }

        private static int ToSmsTypeCode(BankSmsType type)
        {
            switch (type)
            {
                case BankSmsType.LoyaltyRegistration:
                    return 1;

                case BankSmsType.BankRegistration:
                    return 2;

                case BankSmsType.RegistrationDeniedUnknownClient:
                    return 3;

                case BankSmsType.RegistrationDeniedNoCards:
                    return 4;

                case BankSmsType.RegistrationDeniedAlreadyRegistered:
                    return 5;
            }

            throw new NotSupportedException("не поддерживаемый тип СМС: " + type);
        }
    }
}
