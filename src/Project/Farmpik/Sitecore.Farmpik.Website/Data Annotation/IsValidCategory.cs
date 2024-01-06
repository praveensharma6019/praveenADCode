using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Farmpik.Website.Data_Annotations
{
    public class IsValidCategoryAttribute : ValidationAttribute
    {
        string _isValidCategory = string.Empty;


        public override bool IsValid(object value)
        {
            string categoryName = value as string;
            if (!string.IsNullOrEmpty(categoryName))
            {
                if (categoryName != "Business Inquiry" && categoryName != "General Inquiry" && categoryName != "Media Inquiry" && categoryName != "Website Feedback")
                {
                    return false;
                }
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_isValidCategory.ToString());
        }
    }
}