using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.FAQ
{
    public class FAQModel
    {
        public virtual string acceptenceDesc { get; set; }
        public virtual string yesBtnLabel { get; set; }
        public virtual string noBtnLabel { get; set; }
        public virtual string expandBtnLabel { get; set; }
        public virtual string collapseBtnLabel { get; set; }
        public virtual string defaultCount { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<FAQData> data { get; set; }
    }
    public class FAQData
    {
        [SitecoreField]
        public virtual string heading { get; set; }
        [SitecoreField]
        public virtual string description { get; set; }
        [SitecoreField]
        public virtual string categoryID { get; set; }
        [SitecoreField]
        public virtual string categoryLabel { get; set; }
        [SitecoreField]
        public virtual string questionID { get; set; }
    }
}