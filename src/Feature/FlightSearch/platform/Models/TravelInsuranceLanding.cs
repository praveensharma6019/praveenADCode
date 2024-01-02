using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models
{
    public class  TravelInsuranceLanding

    {
        public string Title { get; set; }
        public string BannerImage { get; set; }
        public string MWebBannerImage { get; set; }
        public string Description { get; set; }
        public FlightBookWidget FlightBookWidget { get; set; }
        public BenefitsLanding Benefits { get; set; }
        public BreakupsLanding Breakups { get; set; }
       
       

    }

    public class FlightBookWidget
    {
        public string Enable { get; set; }
        public string Title { get; set; }
        public string BookFlightText { get; set; }
        public string BookFlightLink { get; set; }
        
    }

  
        public class BenefitsLanding
        {
            public string Title { get; set; }
            public Info Info { get; set; }
            public List<DetailLanding> Details { get; set; }

           public DisclaimerLanding Disclaimer { get; set; }

    }

        public class Info
        {
            public string Label { get; set; }
            public string Amount { get; set; }
            public string Icon { get; set; }
        }

    public class DisclaimerLanding
    {
        public string Label { get; set; }
        public string TNC   { get; set; }
        public string TNCLink { get; set; }
        public string BrandTitle { get; set; }
        public string BrandLogo { get; set; }
    }

    public class DetailLanding
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string IconUrl { get; set; }
    }


    public class BreakupsLanding
    { 
        public List<Heading> Heading { get; set; } 
        public List<Details> Details { get; set; } 
    }

    public class Heading {
        public string Title { get; set; }
    }

    public class Details
    {
        public string Label { get; set; }
        public string Amount { get; set; }
    }
}