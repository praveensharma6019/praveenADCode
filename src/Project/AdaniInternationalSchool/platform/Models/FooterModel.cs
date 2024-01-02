using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class FooterModel
    {
        public FooterModel()
        {
            Footerlinks = new List<LinksModel>();
        }

        public LinksModel Quicklinks { get; set; }
        public List<LinksModel> Footerlinks { get; set; }
        public CopyRightModel CopyRight { get; set; }
        public SchoolInfoModel SchoolInfo { get; set; }
    }
}