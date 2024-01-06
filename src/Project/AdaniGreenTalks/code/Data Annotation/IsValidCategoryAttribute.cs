using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website
{
    public class IsValidCategoryAttribute : ValidationAttribute
    {
        string _isValidCategory = string.Empty;
       

        public override bool IsValid(object value)
        {
            string categoryName = value as string;
            if (!string.IsNullOrEmpty(categoryName))
            {
                if (categoryName != "Education" && categoryName != "Healthcare" && categoryName != "Agriculture" && categoryName != "Livelihood Concern" && categoryName != "Technology" && categoryName != " Natural Resources")
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