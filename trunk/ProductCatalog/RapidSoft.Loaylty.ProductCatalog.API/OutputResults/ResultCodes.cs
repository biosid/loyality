namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public sealed class ResultCodes
    {
        #region Common

        public const int ARGUMENT_OUT_OF_RANGE = -2;
        public const int INVALID_PARAMETER_VALUE = -1;
        public const int SUCCESS = 0;
        public const int UNKNOWN_ERROR = 1;
        public const int NOT_FOUND = 2;
        public const int CERCULAR_REFERENCE = 7;
        public const int ALLREADY_EXISTS = 9;
        public const int OVERFLOW = 11;

        public const int INTERSECT = 10;

        public const int NOT_HAVE_PERMISSION = 1000;

        #endregion Common

        public const int WISHLIST_BASKET_IS_FULL = 4;
        public const int ERROR_DELETING_NOT_EMPTY_CATEGORY = 6;
        public const int CAN_NOT_CALC_PRICE = 300;

        public const int NOT_UNIQUE_EXTERNAL_ORDER_ID = 10;

        public const int QUANTITY_OVERFLOW = 20;

        public const int WRONG_WORKFLOW = 55;

        #region Product
        
        public const int PRODUCT_NOT_DELIVERED = 700;
        public const int PRODUCT_INVALID_STATUS = 701;
        public const int PRODUCT_WRONG_BONUS_PRICE = 710;
        
        #endregion

        public const int PROVIDER_CONNECTOR_CALL_ERROR = 400;
        public const int PROVIDER_CHECK_ORDER_DECLINED = 410;
        public const int PROVIDER_CONFIRM_ORDER_DECLINED = 420;
        
        public const int BASKET_ITEM_NOT_FOUND = 600;
        public const int BASKET_ITEMS_FROM_DIFFERENT_CLIENTS = 601;
        public const int BASKET_ITEMS_FROM_DIFFERENT_PARTNERS = 602;
		public const int BASKET_ITEMS_WITH_EMAIL_AND_ANOTHER_DELIVERY_TYPE = 603;
        public const int FIX_PRICE_DECLINED = 610;
        
        public const int INVALID_DELIVARY_KLADR = 800;
        public const int INVALID_DELIVARY_ADDRESS = 801;
        public const int KLADR_USED_BY_ANOTHER_Location = 802;
        public const int KLADR_NOT_FOUND_BY_GEOBASE = 803;

        public const int CUSTOM_COMMIT_METHOD_IS_INVALID = 1101;
        
        #region Category
        
        public const int CATEGORY_WITH_NAME_EXISTS = 20;
        public const int PARENT_CATEGORY_NOT_FOUND = 3;
        public const int PARENT_CATEGORY_IS_DYNAMIC = 5;
        public const int CATEGORY_NOT_FOUND = 500;
        public const int CATEGORY_INVALID_MOVE = 501;

        public const int CATEGORIES_INTERSECT = 503;

        #endregion Category

        #region Partner
        
        public const int PARTNER_NOT_FOUND = 901;
        public const int PARTNER_WITH_NAME_EXISTS = 902;
        public const int PARTNER_CATALOG_NOT_FOUND = 903;
        public const int PARTNER_ID_CAN_NOT_BE_CHANGED = 904;
        public const int CARRIER__NOT_FOUND = 905;
        public const int PARTNER_CANT_IMPORT_CATALOG = 906;

        #endregion Partner
    }
}