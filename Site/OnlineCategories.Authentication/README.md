# Механизм аутентификации для онлайн категорий

## Аутентификационный фильтр `[BonusGatewayAuthenticationFilterAttribute]` для MVC5+

Навешивается на Action'ы, либо контроллеры. Фильтр ищет UserTicket в URL запроса и получает по нему 
информацию о пользователе. При испехе, в качестве _Identity_ устанавливается экземпляр `BonusGatewayIdentity`

    [BonusGatewayAuthenticationFilter]
    public ActionResult Index() {
          var client = User.Identity as BonusGatewayIdentity;
          return View(client);
    }

## Настройки AppSettings

*   `bonus_gateway::user_ticket_parameter` - параметр URL, который содержит UserTicket. 
    По умолчанию: `userTicket`
*   `bonus_gateway::identity_caching_time_minutes` - время, на которое кэшируется информация о пользователе, 
    полученная по UserTicket. По умолчанию: 20 минут.