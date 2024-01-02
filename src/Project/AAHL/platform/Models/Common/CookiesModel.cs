using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Common
{
    public class CookiesModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual string Decline { get; set; }
        public virtual string AcceptCookies { get; set; }
        public virtual string Class {  get; set; }
        public virtual string Expire {  get; set; }
        public virtual string Max_age {  get; set; }
        //public GtmDataModel GtmData { get; set; }
    }
    public class GtmDataModel
    {
        public virtual string Event { get; set; }
        public virtual string Category { get; set; }
        public virtual string Banner_category { get; set; }
        public virtual string Title { get; set; }
        public virtual string Section_title { get; set; }
        public virtual string Label { get; set; }
        public virtual string Index { get; set; }
        public virtual string Page_type { get; set; }
        public virtual string Sub_category { get; set; }
        public virtual string Click_text { get; set; }
    }
}