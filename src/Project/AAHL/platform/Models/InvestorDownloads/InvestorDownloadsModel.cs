using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.MediaCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.InvestorDownloads
{
    public class InvestorDownloadsModel
    {

        [SitecoreChildren]
        public virtual IEnumerable<InvestorDownloadsItem> Items { get; set; }
    }

    public class InvestorDownloadsItem
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual bool Isactive { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<InvestorDownloadChilditems> Childitems { get; set; }
    }
    public class InvestorDownloadChilditems
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField("Date")]
        public virtual IEnumerable<CategoriesChilditems> Date { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<InvestorDownloadsubitems> Subitems { get; set; }
    }
    public class InvestorDownloadsubitems : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string Year { get; set; }
        [SitecoreField]
        public virtual string IconImage { get; set; }

    }
}