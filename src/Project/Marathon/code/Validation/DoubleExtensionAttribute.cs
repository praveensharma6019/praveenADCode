using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DoubleExtensionAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool isValid = true;
            if (value != null)
            {
                HttpPostedFileBase file = value as HttpPostedFileBase;

                string filename = file.FileName;
                char ch = '.';
                int occurance = filename.Count(f => (f == ch));
                if (occurance > 1)
                {
                    return false;
                }
            }
            return isValid;
        }
    }
}