using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.EV.Project.Models.BaseModel;

namespace Adani.EV.Project.Models
{
    public class SocialMediaLinksModel : IId
    {      
        public string Id { get; set; }        
        public List<SocialMediaLinksWidgetItem> widgetItems { get; set; }
    }

    public class SocialMediaLinksWidgetItem : IId, ICtaText, ICtaLink, IImage
    {     
        public string Id { get; set; }  
        public string Imagesrc { get; set; }   
        public string CtaText { get; set; }     
        public string CtaLink { get; set; }
    }
}