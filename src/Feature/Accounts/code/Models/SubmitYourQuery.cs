using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class SubmitYourQuery
    {
        public SubmitYourQuery()
        {
            TypeofQueryCategoryList = new List<TypeofArea>();
            TypeofAreaList = new List<TypeofArea>();

            TypeofCityList = new List<ListItem> {
                    new ListItem{ Value="Mumbai", Text="Mumbai"}
                     
                };

        }
        
        public string CategoryName { get; set; }
        public string Discription { get; set; }
        public string CompanyName { get; set; }
        public string SubCategory { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Captcha { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<TypeofArea> TypeofQueryCategoryList { get; set; }

        public List<TypeofArea> TypeofAreaList { get; set; }

        public List<ListItem> TypeofCityList { get; set; }
    }

    [Serializable]
    public class TypeofQueryCategory
    {
        public string Name { get; set; }
        public string Value { get; set; }


    }

    [Serializable]
    public class TypeofArea
    {
        public string Name { get; set; }
        public string Value { get; set; }

    }

   
}