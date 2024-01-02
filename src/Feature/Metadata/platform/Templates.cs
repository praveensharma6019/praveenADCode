using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform
{
    public class Templates
    {
        public static class HeadDataCollection
        {
            //public static readonly ID ServiceItemTemplateID = new ID("{0B2191D6-78AE-4742-AD47-ADC20306DE48}");
            public static string JavaScripts = "JavaScripts";
            public static string css = "Cascading Style Sheets";
            public static string Priority = "Priority";
            public static string ChangeFrequency = "ChangeFrequency";
            public static string RobotsContent = "RobotsContent";
            public static string AirportHome = "AirportHome";
            public static string DomesticFlights = "domestic-flights";
            public static string InternationalFlights = "international-flights";
            public static string AhmedabadXml = "svpia-ahmedabad-airport-sitemap.xml";
            public static string MumbaiXml = "csmia-mumbai-airport-sitemap.xml";
            public static string GuwahatiXml = "lgbia-guwahati-airport-sitemap.xml";
            public static string JaipurXml = "jaipur-airport-sitemap.xml";
            public static string LucknowXml = "ccsia-lucknow-airport-sitemap.xml";
            public static string MangaluruXml = "mangaluru-airport-sitemap.xml";
            public static string ThiruvananthapuramXml = "thiruvananthapuram-airport-sitemap.xml";
            public static string DomesticFlightsXml = "domestic-flights-sitemap.xml";
            public static string InternationalFlightsXml = "international-flights-sitemap.xml";
            public static string DomesticAirlinesXml = "domestic-airlines-sitemap.xml";
            public static string InternationalAirlinesXml = "international-airlines-sitemap.xml";
        }

        public static class fields
        {
            // public static readonly string MetadataTemplateId = "{4E98FA2A-DEE8-40AC-BC75-67396EE7160E}";
            public static readonly string MetaTitle = "MetaTitle";
            public static readonly string MetaDescription = "MetaDescription";
            public static readonly string Keywords = "Keywords";
            public static readonly string Canonical = "Canonical";
            public static readonly string Viewport = "Viewport";
            public static readonly string Robots = "Robots";
            public static readonly string OG_Title = "OG-Title";
            public static readonly string OG_Image = "OG-Image";
            public static readonly string OG_Description = "OG-Description";
        }
    }
}