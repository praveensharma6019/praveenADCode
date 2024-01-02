using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.About_Page
{
    public class AboutPageModel
    {
        public string SectionID { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }  
        public string MoreContent { get; set; } 
        public string ReadMore { get; set; }
        public string ReadLess { get; set; }
    }
}