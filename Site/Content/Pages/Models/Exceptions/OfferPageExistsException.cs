using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vtb24.Site.Content.Models.Exceptions;

namespace Vtb24.Site.Content.Pages.Models.Exceptions
{
    public class OfferPageExistsException : ContentManagementServiceException
    {
        public OfferPageExistsException(int partnerId)
            : base(string.Format("У данного партнера уже существует оферта"))
        {
            PartnerId = partnerId;
        }

        public int PartnerId { get; private set; }
    }
}
