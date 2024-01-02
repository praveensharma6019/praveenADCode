using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Investors
{
    public class InvestorModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<InvestorItem> Items { get; set; }
    }
    public class InvestorItem : ImageModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual Link LinkUrl { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<DownloadlinkItems> DownloadLinkUrl { get; set; }
        //public virtual Link DownloadLinkUrl { get; set; }
        public virtual string Class { get; set; }
    }
    public class DownloadlinkItems
    {
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
    }
}