using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class RatusRevamp
    {
        public RatusRevamp()
        {
            TypeofCategoryList = new List<TypeofCategory>();

            RatingSelectList = new List<ListItem> {
                    new ListItem{ Value="5", Text="5-star"},
                    new ListItem{ Value="4", Text="4-star"},
                     new ListItem{ Value="3", Text="3-star"},
                      new ListItem{ Value="2", Text="2-star"},
                      new ListItem{ Value="1", Text="1-star"}
                };
        }




        public string CANumber { get; set; }

        public string CategoryName { get; set; }
        public string Rating { get; set; }
        public string AppreciationNote { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<TypeofCategory> TypeofCategoryList { get; set; }

        public List<ListItem> RatingSelectList { get; set; }
    }

    [Serializable]
    public class TypeofCategory
    {
        public string Name { get; set; }
    }
}