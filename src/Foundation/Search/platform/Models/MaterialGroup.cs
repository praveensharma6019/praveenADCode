using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.Models
{
    public class MaterialGroup
    {
        public string link { get; set; }

        public string title { get; set; }

        public string code { get; set; }

        public string path { get; set; }

        public string cdnPath { get; set; }

        public bool active { get; set; }

        public string thumbnailImage { get; set; }

        public string mainImage { get; set; }

        public string iconImage { get; set; }
    }
}