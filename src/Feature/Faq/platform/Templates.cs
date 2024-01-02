using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Faq.Platform
{
    public static class Templates
    {
        public static readonly ID CommonTextItem = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");
        public static readonly ID FolderID = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");
        public static readonly ID faqLinkID = new ID("{6063ADB0-BE98-484A-B2F8-0775C68B5F39}");
        public static readonly string faqLinkText = "FAQLink";
        public static readonly ID SeeallLink = new ID("{C9E9E8A6-9343-4721-BD91-7EDBBD689330}");
        public static readonly string seealltext = "See all link";
        public static readonly ID FAQList = new ID("{CE9826E3-8391-4AB9-BFF3-D7EA120EB348}");
        public static readonly ID LocationFAQList = new ID("{5645DA76-8F27-4763-AA4C-742E45361EC5}");
        public static readonly ID LocationFAQItem = new ID("{BFD1062B-55F3-4662-9535-33B34B640ED4}");
       
        public static class Faq
        {
            public static readonly ID Id = new ID("{FE549078-8C72-472A-8EA7-51053E0E1D73}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{5718E787-142B-41D9-B5A1-0B18F45B8236}");
                public static readonly ID Content = new ID("{45EFE66E-5AD2-4F1D-BAD5-FDF281688681}");
            }
        }
        public static class QuickLinsFaqTemplate
        {
            public static readonly ID Id = new ID("{209C0EB3-E14C-455B-928D-528D3285B351}");

            public static class CategoryFields
            {
                public static readonly ID Heading = new ID("{D382BB76-D6F4-49FC-AD7E-BA6138982E5D}");
                public static readonly ID Title = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
            }
        }
    }
}