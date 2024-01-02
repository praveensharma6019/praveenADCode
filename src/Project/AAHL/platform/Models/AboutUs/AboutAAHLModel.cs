using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.AboutUs
{
    public class AboutAAHLModel : DetailModel
    {
        public virtual Link LinkUrl { get; set; }
    }
}