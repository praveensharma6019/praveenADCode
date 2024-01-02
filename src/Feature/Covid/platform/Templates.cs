using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.Covid
{
    public static class Templates
    {
        public static class Covid
        {
            public static readonly ID CovidTemplateID = new ID("{30351578-32A0-4A97-9CD0-23EC2960157C}");
            public static class Fields
            {
                public static readonly ID Summary = new ID("{274CBA70-1250-4D11-BD24-31FADC2E178F}");
                public static readonly ID Details = new ID("{15EF96E2-347C-46D1-9B41-E8F3EDE45ECE}");
                public static readonly ID CovidCarousal = new ID("{9052EC11-4D64-4F6A-983D-BD4EFEEA51CC}");
                public static readonly ID Image = new ID("{F8EE4BC3-77C0-403A-A0F8-8D278D0EC6DF}");
                public static readonly ID CTA = new ID("{C25D80BA-92E2-4A79-9CA0-D8BEEFE9EC7E}");
                public static readonly ID MobileImage = new ID("{8C152BE6-E409-4B42-8AA3-4B130137BE6E}");
                public static readonly ID WebImage = new ID("{783243BC-982E-4BB1-8145-BABCCEF1E247}");
                public static readonly ID ThumbnailImage = new ID("{19877ED9-5996-4E1B-894F-02398DEED82C}");
            }
        }

        public static class CovidCrausal
        {
            public static readonly ID CovidCrausalTemplateID = new ID("{9A687744-473C-49B7-ABCC-9D2986C7FB0A}");
            public static class Fields
            {
                public static readonly ID Summary = new ID("{A97A8DFF-3B01-413A-9129-93DE6532DB1B}");
                public static readonly ID Details = new ID("{2A353F53-7A5F-4A1C-8488-03CCD40DDFC3}");
                public static readonly ID Image = new ID("{062E4CE9-3763-4338-A5A5-3837B27DC82E}");
            }
        }

        public static class ServicesListCollection
        {
            public static readonly ID ServiceItemTemplateID = new ID("{0B2191D6-78AE-4742-AD47-ADC20306DE48}");
            public static string RenderingParamField = "Widget";
        }
    }
}