using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.FAQ
{
    public static class Templates
    {
        public static readonly ID AccordionTemplateID = new ID("{4271329A-818A-441D-B241-0A8AEA76D425}");
        public static readonly ID HomePageID = new ID("{E511BFA0-6D6C-489D-86A9-17A44A3FB18D}");
        public static readonly ID AirportPageTemplateID = new ID("{18378AC0-4CFA-44AD-A38E-ADB5F071913B}");
        public static readonly ID FAQLandingPageTemplateID = new ID("{48C3ED69-40D8-4DF6-A8AB-723FDD5A70E2}");
        public static readonly ID FAQAccordionServiceRenderingID = new ID("{948847ED-43E0-4B10-A9CB-16D74EECC8D2}");

        public static readonly ID BombayAirportPageID = new ID("{B6F6CE42-E296-4F6F-BCD6-CD70DC77C02A}");
        public static readonly ID AhemedabadAirportPageID = new ID("{208EEC1D-66F9-4C93-8773-FD05FE10DF17}");
        public static readonly ID JaipurAirportPageID = new ID("{E01D4AB5-0E3D-4D09-BBC8-8AE6F05815D2}");
        public static readonly ID LucknowAirportPageID = new ID("{18378AC0-4CFA-44AD-A38E-ADB5F071913B}");
        public static readonly ID MangaluruAirportPageID = new ID("{F92F36AF-466A-4A53-BC33-EA194333138B}");
        public static readonly ID GuwahatiAirportPageID = new ID("{0F49FE93-3140-42DE-804B-8FEE912290F7}");
        public static readonly ID ThiruvanthapuramAirportPageID = new ID("{FB6C458F-2271-4743-AAAE-F02BE5BA56CD}");

        public static class FAQ
        {
            public static readonly ID FAQTemplateID = new ID("{4B832789-66B5-49F9-88C5-72B2ECC53418}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
                public static readonly ID FAQList = new ID("{8F516BB5-EE2B-403E-A0EC-615D5391D446}");
                // Fields add for Loyalty Module
                public static readonly ID CTAID = new ID("{8BD174D8-4628-4C12-9376-90AE79E8441E}");
                public static readonly ID FAQHTML = new ID("{61BA4988-4D1D-4103-BFFB-9B660432AD02}");
                public static readonly string CTAKey = "Link";

                public static readonly ID ApiTitle = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
                public static readonly ID ApiFAQList = new ID("{8F516BB5-EE2B-403E-A0EC-615D5391D446}");
                public static readonly ID ApiCTAID = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
                public static readonly ID ApiFAQHTML = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
                public static readonly string ApiCTAKey = "CTA";

            }
        }

        public static class FAQCard
        {
            public static readonly ID FAQCardTemplateID = new ID("{4E465098-9E12-4B94-88F0-65067235D8F5}");
            public static class Fields
            {
                public static readonly ID Question = new ID("{614CBD42-503A-4092-B70C-60F45B6DCB89}");
                public static readonly ID Answer = new ID("{F5EBF756-27EC-4023-AEEC-C2A2C4188E7E}");
            }
        }

        public static class FAQApiCard
        {
            public static readonly ID FAQCardTemplateID = new ID("{2CDB3FA6-F31A-40FB-91C8-772D7505687E}");
            public static class Fields
            {
                public static readonly ID Question = new ID("{15B33278-D089-4294-B41F-0C2AABB06F32}");
                public static readonly ID Answer = new ID("{E74A69B2-41BC-42C7-9BE4-5B401EF6B656}");
            }
        }
    }
}