namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    public static class ResultCodes
    {
        public const int INVALID_PARAMETER_VALUE = -1;

        public const int SUCCESS = 0;
        public const int UNKNOWN_ERROR = 1;

        public const int RULE_NOT_FOUND = 100;

        public const int INVALID_PRIORITY = 101;

        public const int RULE_DOMAIN_NOT_FOUND = 200;

        public const int INVALID_TARGET_AUDIENCE = 500;

        public const int NOT_HAVE_PERMISSION = 1000;
    }
}