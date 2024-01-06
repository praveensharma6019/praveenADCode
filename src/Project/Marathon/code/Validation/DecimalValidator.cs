using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.Marathon.Website.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DecimalValidator:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Regex decimalAmount = new Regex("^[0-9]{1,7}([.]?[0-9]{0,2})$");
            if (decimalAmount.IsMatch(value.ToString()))
            {
                return true;
            }
            return false;
        }
    }
}