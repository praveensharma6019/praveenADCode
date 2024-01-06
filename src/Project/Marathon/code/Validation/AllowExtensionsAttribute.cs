using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AllowExtensionsAttribute : ValidationAttribute
    {
        public string Extensions { get; set; } = "png,jpg,jpeg,pdf";


        public override bool IsValid(object value)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            bool isValid = true;

            List<string> allowedExtensions = this.Extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (file != null)
            {
                var fileName = file.FileName;

                isValid = allowedExtensions.Any(y => fileName.ToLower().EndsWith(y));
            }

            return isValid;
        }
    }
}