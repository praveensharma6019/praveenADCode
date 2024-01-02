using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.OurExpertise
{
    public class VideoBannerModel : ImageModel
    {
        public virtual string Heading { get; set; }
        public virtual bool Autoplay { get; set; }
        public virtual Link VideoSource { get; set; }
        public virtual Link VideoSourceMobile { get; set; }
        public virtual Link VideoSourceTablet { get; set; }
    }
}