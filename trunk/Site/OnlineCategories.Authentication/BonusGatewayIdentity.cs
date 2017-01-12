using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Vtb24.OnlineCategories.Client.Models;

namespace Vtb24.OnlineCategory.Authentication
{
    public class BonusGatewayIdentity : ClaimsIdentity
    {
        private BonusGatewayIdentity(string userTicket, ClientInfo clientInfo, ICollection<Claim> claimsContainer) 
        : base(claimsContainer, "Vtb24CollectionBearerToken")
        {
            Map(userTicket, clientInfo, claimsContainer);
        }

        public string UserTicket { get; private set; }

        public string FirstName { get; private set; }


        public string LastName { get; private set; }

        public string MiddleName { get; private set; }

        public string Email { get; private set; }

        public string City { get; private set; }

        public int Balance { get; private set; }

        public decimal BonusRate { get; private set; }

        public decimal BonusDelta { get; private set; }

        public int CalculateBonusPrice(decimal rubles)
        {
            return (int )Math.Ceiling(rubles * BonusRate + BonusDelta);
        }

        public override void AddClaims(IEnumerable<Claim> claims)
        {
            throw new InvalidOperationException();
        }

        public override void AddClaim(Claim claim)
        {
            throw new InvalidOperationException();
        }

        public static BonusGatewayIdentity Create(string userTicket, ClientInfo clientInfo)
        {
            return clientInfo == null ? null : new BonusGatewayIdentity(userTicket, clientInfo, new List<Claim>());
        }

        private void Map(string userTicket, ClientInfo client, ICollection<Claim> claimsContainer)
        {
            UserTicket = userTicket;
            FirstName = client.FirstName;
            MiddleName = client.MiddleName;
            LastName = client.LastName;
            Email = client.Email;
            City = client.City;
            Balance = (int) (client.Balance ?? 0);
            BonusRate = client.BonusRate;
            BonusDelta = client.BonusDelta;


            var fullName = string.Format("{0} {1}", client.FirstName, client.MiddleName).Trim();
            var culture = CultureInfo.InvariantCulture;

            claimsContainer.Add(new Claim(ClaimTypes.Name, fullName));
            claimsContainer.Add(new Claim("http://bonus.vtb24.ru/schemas/FirstName", FirstName));
            claimsContainer.Add(new Claim("http://bonus.vtb24.ru/schemas/LastName", LastName));
            claimsContainer.Add(new Claim("http://bonus.vtb24.ru/schemas/Email", Email ?? ""));
            claimsContainer.Add(new Claim("http://bonus.vtb24.ru/schemas/MiddleName", MiddleName));
            claimsContainer.Add(new Claim("http://bonus.vtb24.ru/schemas/Balance", Balance.ToString(culture)));
            claimsContainer.Add(new Claim("http://bonus.vtb24.ru/schemas/BonusDelta", BonusDelta.ToString(culture)));
            claimsContainer.Add(new Claim("http://bonus.vtb24.ru/schemas/BonusRate", BonusRate.ToString(culture)));
            claimsContainer.Add(new Claim("http://bonus.vtb24.ru/schemas/City", City));
            // необходимые для работы Antiforgety в MVC без доп. конфигурации
            claimsContainer.Add(new Claim(ClaimTypes.NameIdentifier, UserTicket));
            claimsContainer.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", 
                "Vtb24CollectionBonusGateway"));
        }
    }
}
