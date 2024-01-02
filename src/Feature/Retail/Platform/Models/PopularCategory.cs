using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Models
{
    public class PopularCategoryModel
    {
        public string Title { get; set; }

        public List<Categories> Categories { get; set; }
    }

    public class Categories
    {
        public string CategoryTitle { get; set; }

       public List<CategoryDetails> CategoryDetails { get; set; }
    }

    public class CategoryDetails
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CTA { get; set; }



    }

}