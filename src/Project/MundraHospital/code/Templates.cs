using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.MundraHospital.Website
{
    public class Templates
    {
        public struct ServicesTab
        {
            public static readonly ID ID = new ID("{5F307D6D-247F-47F3-B21C-A7EEA92159CB}");

            public struct Fields
            {
                public static readonly ID SelectedTabImage = new ID("{7B128F2B-079A-4C88-9E31-C67E79B03209}");
                public static readonly ID UnselectedTabImage = new ID("{0D7E59E1-6CA6-4C45-AE53-2CDE8C443BB6}");
                public static readonly ID Link = new ID("{7FF712D6-CA51-4528-B04C-3AE8DDF56EE4}");
            }
        }

        public struct ImageBanner
        {
            public static readonly ID ID = new ID("{9199E414-DFCB-472E-BA74-316FDBE6CD8F}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{CA02FC6F-C456-4E94-944F-DCD5D88ECD6D}");
                public static readonly ID Description = new ID("{0DC162E5-206C-4E75-8296-37F9ED5AA882}");
                public static readonly ID Image = new ID("{3A497D87-4ADC-453E-9480-97CE28E40F06}");
            }
        }
    }
}