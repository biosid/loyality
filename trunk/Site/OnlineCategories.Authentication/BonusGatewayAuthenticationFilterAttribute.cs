using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Vtb24.OnlineCategories.Client;

namespace Vtb24.OnlineCategory.Authentication
{
    public class BonusGatewayAuthenticationFilterAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public BonusGatewayAuthenticationFilterAttribute() :
            this(new BonusGatewayClient())
        {
        }

        public BonusGatewayAuthenticationFilterAttribute(IBonusGatewayClient bonusGatewayClient)
        {
            _gatewayClient = bonusGatewayClient;
        }

        private readonly IBonusGatewayClient _gatewayClient;

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var ticketKey = ConfigurationManager.AppSettings["bonus_gateway::user_ticket_parameter"] ?? "userTicket";
            var userTicket = filterContext.HttpContext.Request.QueryString[ticketKey];
            if (string.IsNullOrWhiteSpace(userTicket))
            {
                return;
            }

            var identity = GetIdentityFromToken(userTicket, filterContext.HttpContext.Cache);
            if (identity != null)
            {
                filterContext.Principal = new ClaimsPrincipal(identity);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var principal = (ClaimsPrincipal) filterContext.HttpContext.User;
            var identity = principal.Identities.FirstOrDefault(i => i.AuthenticationType == "Vtb24CollectionBearerToken");
            
            if (identity == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();   
            }
        }
        private ClaimsIdentity GetIdentityFromToken(string userTicket, Cache cache)
        {
            var cacheKey = "vtb24_identity::" + userTicket;
            var identity = cache[cacheKey] as BonusGatewayIdentity;

            if (identity == null)
            {
                identity = ResolveUserTicket(userTicket);
                var cTimeMin = int.Parse(ConfigurationManager.AppSettings["bonus_gateway::identity_caching_time_minutes"] ?? "20");
                var cTime = DateTime.Now.AddMinutes(cTimeMin);
                var cExp = Cache.NoSlidingExpiration;
                cache.Add(cacheKey, identity, null, cTime, cExp, CacheItemPriority.Normal, null);
            }

            return identity;
        }

        private BonusGatewayIdentity ResolveUserTicket(string userTicket)
        {
            var client = _gatewayClient.ResolveClient(userTicket);
            return client == null ? null : BonusGatewayIdentity.Create(userTicket, client);
        }
    }
}