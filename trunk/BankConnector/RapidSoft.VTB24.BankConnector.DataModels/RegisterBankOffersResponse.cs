//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RapidSoft.VTB24.BankConnector.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class RegisterBankOffersResponse
    {
        public long Id { get; set; }
        public string PartnerOrderNum { get; set; }
        public string ClientId { get; set; }
        public RegisterBankOfferResult OrderActionResult { get; set; }
        public Nullable<System.Guid> EtlSessionId { get; set; }
        public System.DateTime InsertedDate { get; set; }
    }
}