using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.Models
{
    public class Category
    {
       // public string Name { get; set; }
        
        public string title { get; set; } 

        public string code { get; set; }

        public string showOnHomepage { get; set; }

        public string thumbnailImage { get; set; }

        public string mainImage { get; set; }

        public string iconImage { get; set; }

        public string cdnPath { get; set; }
            
        public string link { get; set; }

        public string categoryPath { get; set; }
    }
}