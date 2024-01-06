using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Sitecore.Foundation.DependencyInjection.Attributes
{
    public class ValidateIpAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                // Allow null or empty values if needed
                return false;
            }

            IPAddress ipAddress;
            if (IPAddress.TryParse(value.ToString(), out ipAddress))
            {
                // Check if the parsed IP address is valid
                return true;
            }

            return false;
        }
    }

}