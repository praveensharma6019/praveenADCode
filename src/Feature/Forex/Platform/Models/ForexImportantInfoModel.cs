using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Forex.Platform.Models
{
    public class ForexImportantInfoModel
    {

        public class ForexImportantInfoJSON
        {
            public string title { get; set; }
            public List<LineswithLinks> lines { get; set; }

            public ForexImportantInfoJSON()
            {
                title = "";
                lines = new List<LineswithLinks>();
            }
        }

        public class ForexImportantInfoList
        {
            public List<ForexImportantInfoJSON> InfoList { get; set; }

            public ForexImportantInfoList()
            {
                InfoList = new List<ForexImportantInfoJSON>();
            }
        }

        public class LineswithLinks
        {
            public string line { get; set; }
            public string iconURL { get; set; }
            public List<LineLinks> links { get; set; }

            public LineswithLinks()
            {
                line = string.Empty;
                links = new List<LineLinks>();
            }
        }

        public class LineLinks
        {
            public string link { get; set; }
            public string linkText { get; set; }
            public string linkURL { get; set; }

            public LineLinks()
            {
                linkText = string.Empty;
                link = string.Empty;
                linkURL = string.Empty;
            }
        }
    }
}