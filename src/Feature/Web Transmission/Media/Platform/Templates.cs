using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Media.Platform
{
    public class Templates
    {
        public static class ServicesListCollection
        {
            public static readonly ID ServiceItemTemplateID = new ID("{0B2191D6-78AE-4742-AD47-ADC20306DE48}");
            public static string RenderingParamField = "Widget";
        }
        public static class CustomContentCollection
        {
            public static readonly ID TitleWithRichTextTemplateID = new ID("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");
        }
    }
}