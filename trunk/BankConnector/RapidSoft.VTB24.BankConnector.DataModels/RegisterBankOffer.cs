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
    
    public partial class RegisterBankOffer
    {
        public long Id { get; set; }
        public string PartnerOrderNum { get; set; }
        public string ArticleName { get; set; }
        public decimal OrderBonusCost { get; set; }
        public string ProductId { get; set; }
        public string ClientId { get; set; }
        public string CardLast4Digits { get; set; }
        public string OrderAction { get; set; }
        public Nullable<System.Guid> EtlSessionId { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string OfferId { get; set; }
    }
}