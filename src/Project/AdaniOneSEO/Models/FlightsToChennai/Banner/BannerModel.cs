using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Models.FlightsToDestination.Banner
{
    public class BannerModel
    {
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual Image ImagePath { get; set; }
        [SitecoreField]
        public virtual Image MImagePath { get; set; }
        [SitecoreField]
        public virtual Image TImagePath { get; set; }
    }
}