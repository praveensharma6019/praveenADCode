using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Awards
{
    public class SearchModel
    {
        [SitecoreField]
        public virtual bool Isactive { get; set; }
    }
}