using Adani.EV.Project.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class FooterBannerModel : IId, ITitle, IType, IImage, ICtaLink, ICtaText
    {
        public string Id { get; set; }
        public string Imagesrc { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }     
        public string CtaLink { get; set; }
        public string CtaText { get; set; }
    }
}