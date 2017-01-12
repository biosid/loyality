using System.Collections.Generic;

namespace Vtb24.Site.Services.MyInfoService.Models.Inputs
{
    public class UpdateMyInfoParams
    {

        public string Email { get; set; }

        public UpdateFieldOptions[] CustomFields { get; set; }

        public class UpdateFieldOptions
        {
            public int FieldId { get; set; }

            public string Value { get; set; }
        }
    }
}