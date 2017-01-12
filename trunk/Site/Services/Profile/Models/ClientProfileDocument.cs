using System;

namespace Vtb24.Site.Services.Profile.Models
{
    public class ClientProfileDocument
    {
        public ClientProfileDocumentType Type { get; set; }
        
        public string Series { get; set; }
        
        public string Number { get; set; }
        
        public string IssuerName { get; set; }
        
        public string IssuerCode { get; set; }
        
        public DateTime? IssueDate { get; set; }

        public bool IsPrimary { get; set; }
    }
}
