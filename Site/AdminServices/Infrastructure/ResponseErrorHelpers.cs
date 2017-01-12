using Vtb24.Arms.AdminServices.ActionsManagement.Models;
using Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions;
using Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions;
using Vtb24.Arms.AdminServices.TargetingManagement.Models;

namespace Vtb24.Arms.AdminServices.Infrastructure
{
    internal static class ResponseErrorHelpers
    {
        public static void AssertSuccess(this AdminMechanicsService.ResultBase response)
        {
            if (response.Success)
            {
                return;
            }

            ThrowPromoActionException(response.ResultCode, response.ResultDescription);

            throw new ActionsManagementServiceException(response.ResultCode, response.ResultDescription);
        }

        public static void AssertSuccess(this TargetAudienceService.ResultBase response)
        {
            if (response.Success)
            {
                return;
            }

            ThrowPromoActionException(response.ResultCode, response.ResultDescription);

            throw new TargetingManagementServiceException(response.ResultCode, response.ResultDescription);
        }

        public static void AssertSuccess(this AdminBankConnectorService.SimpleBankConnectorResponse response)
        {
            if (response.Success)
            {
                return;
            }

            ThrowAdminBankConnectorException(response.ResultCode, response.Error);

            throw new AdminVtbBankConnectorServiceException(response.ResultCode, response.Error);
        }

        public static void AssertSuccess(this CatalogAdminService.ResultBase response)
        {
            if (response.Success)
            {
                return;
            }

            ThrowCatalogAdminException(response.ResultCode, response.ResultDescription);

            throw new CatalogManagementServiceException(response.ResultCode, response.ResultDescription);
        }

        private static void ThrowPromoActionException(int code, string description)
        {
            const int BASE_RULE_CONFLICT = 101;
            const int ACCESS_DENIED = 1000;

            switch (code)
            {
                case BASE_RULE_CONFLICT:
                    throw new ActionsBaseRuleConflictException(code, description);

                case ACCESS_DENIED:
                    throw new AccessDeniedException();
            }
        }

        private static void ThrowAdminBankConnectorException(int code, string description)
        {
            const int CUSTOM_FIELD_ALREADY_EXISTS = 4;
            const int ACCESS_DENIED = 5;
            const int USER_NOT_FOUND = 13;
            const int PHONE_NUMBER_ALREADY_USED = 14;

            switch (code)
            {
                case CUSTOM_FIELD_ALREADY_EXISTS:
                    throw new AdminVtbBankConnectorCustomFieldAlreadyExists(code, description);

                case ACCESS_DENIED:
                    throw new AccessDeniedException();

                case USER_NOT_FOUND:
                    throw new AdminVtbBankConnectorUserNotFound(code, description);

                case PHONE_NUMBER_ALREADY_USED:
                    throw new AdminVtbBankConnectorPhoneNumberAlreadyUsed(code, description);
            }
        }

        private static void ThrowCatalogAdminException(int code, string description)
        {
            const int ENTITY_NOT_FOUND = 2;
            const int ENTITY_ALREADY_EXISTS = 9;
            const int CATEGORY_NAME_ALREADY_EXIST = 20;
            const int KLADR_ALREADY_BINDED = 802;
            const int PARTNER_NAME_ALREADY_EXIST = 902;
            const int ACCESS_DENIED = 1000;

            switch (code)
            {
                case ENTITY_NOT_FOUND:
                    throw new EntityNotFoundException(code, description);

                case ENTITY_ALREADY_EXISTS:
                    throw new EntityAlreadyExistsException(code, description);

                case CATEGORY_NAME_ALREADY_EXIST:
                    throw new CategoryNameAlreadyExistsException(code, description);

                case KLADR_ALREADY_BINDED:
                    throw new DeliveryLocationKladrAlreadyBindedException(code, description);

                case PARTNER_NAME_ALREADY_EXIST:
                    throw new PartnerNameAlreadyExistsException(code, description);

                case ACCESS_DENIED:
                    throw new AccessDeniedException();
            }
        }
    }
}
