using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class AboutusModel
    {
        public string Title { get; set; }
        public string Description { get; set; }     
        public List<AboutusItemModel> widgetItems { get; set; }=new List<AboutusItemModel>();
    }

    public class AboutusItemModel
    {
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public string readMore { get; set; }
    }
}