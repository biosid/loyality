using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Vtb24.Arms.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class SimpleEmailAddressAttribute : RegularExpressionAttribute
    {
        private const string PATTERN = @"([a-zA-Z0-9!#\$%&\._-]+)@([a-zA-Z0-9-]+\.)+([a-zA-Z]{2,63})";

        static SimpleEmailAddressAttribute()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(SimpleEmailAddressAttribute), typeof(RegularExpressionAttributeAdapter));
        }

        public SimpleEmailAddressAttribute() : base(PATTERN)
        {
        }
    }
}
