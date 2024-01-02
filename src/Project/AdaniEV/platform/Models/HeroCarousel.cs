using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.EV.Project.Models.BaseModel;

namespace Adani.EV.Project.Models
{
    public class HeroCarouselModel : IId, IType
    {      
        public string Id { get; set; }    
        public string Type { get; set; }
        public List<HeroCarouselWidgetItem> widgetItems { get; set; }
    }

    public class HeroCarouselWidgetItem : IId, ITitle, ISubTitle, IImage
    {     
        public string Id { get; set; }  
        public string Imagesrc { get; set; }   
        public string Title { get; set; }     
        public string SubTitle { get; set; }
    }
}