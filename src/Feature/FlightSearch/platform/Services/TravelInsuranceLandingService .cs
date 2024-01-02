using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using static Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models.TravelInsurance;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public class TravelInsuranceLandingService : ITravelInsuranceLanding
    {
        private readonly IHelper helper;

        public TravelInsuranceLandingService(IHelper helper)
        {
            this.helper = helper;
        }

        public TravelInsuranceLanding GetTravelInsuranceData(Item datasource)
        {
            TravelInsuranceLanding travelInsurance = new TravelInsuranceLanding();
            FlightBookWidget flightwidget = new FlightBookWidget();
            if (datasource != null && datasource.Children.Count > 0)
            {
                travelInsurance.Title = datasource.Fields[Templates.TravelInsuranceConstant.Title].Value;
                travelInsurance.BannerImage = datasource.Fields[Templates.TravelInsuranceConstant.BannerImage] != null ? helper.GetImageURL(datasource, Templates.TravelInsuranceConstant.BannerImage) : String.Empty;
                travelInsurance.MWebBannerImage = datasource.Fields[Templates.TravelInsuranceConstant.MobileBannerImage] != null ? helper.GetImageURL(datasource, Templates.TravelInsuranceConstant.MobileBannerImage) : String.Empty;
                travelInsurance.Description = datasource.Fields[Templates.TravelInsuranceConstant.Description].Value;
                flightwidget.Enable = datasource.Fields[Templates.TravelInsuranceConstant.FlightBookingWidgetEnable].Value;
                flightwidget.Title = datasource.Fields[Templates.TravelInsuranceConstant.FlightBookingWidgetTitle].Value;
                flightwidget.BookFlightText = datasource.Fields[Templates.TravelInsuranceConstant.BookFlightText].Value;
                flightwidget.BookFlightLink = datasource.Fields[Templates.TravelInsuranceConstant.BookFlightLink] != null ?((Sitecore.Data.Fields.LinkField)datasource.Fields[Templates.TravelInsuranceConstant.BookFlightLink]).Url : String.Empty;
                travelInsurance.FlightBookWidget = flightwidget;
                travelInsurance.Benefits = GetBenefitsData(datasource);
                travelInsurance.Breakups = GetBreakupDetails(datasource);
               
            }
            return travelInsurance;
        }


        public BenefitsLanding GetBenefitsData(Item datasource)
        {
            BenefitsLanding benefitsLanding = new BenefitsLanding();
            Info info = new Info();
            
            var benefitsLandingItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.BenefitsLandingTemplateID).FirstOrDefault();
            if (benefitsLandingItem == null) return null;
            benefitsLanding.Title = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.Title].Value;
            info.Label = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.Label].Value;
            info.Amount = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.Amount].Value;
            info.Icon = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.Icon] != null ? helper.GetImageURL(benefitsLandingItem, Templates.TravelInsuranceConstant.Icon) : String.Empty;
            benefitsLanding.Info = info;
            benefitsLanding.Details=GetBenefitsDetail(benefitsLandingItem);
            DisclaimerLanding disclaimer = new DisclaimerLanding();
            
            disclaimer.TNC = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.TNC].Value;
            disclaimer.BrandTitle = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.BrandTitle].Value;
            disclaimer.TNCLink = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.TNCLink] != null ? helper.LinkUrl(benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.TNCLink]) : String.Empty;
            disclaimer.Label = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.DisclaimerLabel].Value;
            disclaimer.BrandLogo = benefitsLandingItem.Fields[Templates.TravelInsuranceConstant.BrandLogo] != null ? helper.GetImageURL(benefitsLandingItem, Templates.TravelInsuranceConstant.BrandLogo) : String.Empty;
            benefitsLanding.Disclaimer = disclaimer;
            return benefitsLanding;
        }

        public List<DetailLanding> GetBenefitsDetail(Item item) {
            List<DetailLanding> benefitsList = new List<DetailLanding>();
           
            var childItems = item.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.BenefitsDetailTemplateID);
            foreach (var childItem in childItems)
            {
             DetailLanding benefitsDetail = new DetailLanding();
             benefitsDetail.Title = childItem.Fields[Templates.TravelInsuranceConstant.Title].Value;
             benefitsDetail.Description = childItem.Fields[Templates.TravelInsuranceConstant.Description].Value;
             benefitsDetail.IconUrl = childItem.Fields[Templates.TravelInsuranceConstant.IconUrl] != null ? helper.GetImageURL(childItem, Templates.TravelInsuranceConstant.IconUrl) : String.Empty;
              benefitsList.Add(benefitsDetail);
            }
            return benefitsList;

        }

        public BreakupsLanding GetBreakupDetails(Item item) { 
         BreakupsLanding breakupDetails = new BreakupsLanding();
            breakupDetails.Details = GetDetailsList(item);
            breakupDetails.Heading = GetHeadingList(item);
            return breakupDetails;
        }


        public List<Details> GetDetailsList(Item item) { 
         List<Details> details = new List<Details> ();
         var childItems = item.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.CommonFolderTemplateID).FirstOrDefault();
         var breakupDetailItems = childItems.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.BreakupTemplateID);

            foreach (var breakupItem in breakupDetailItems) {
                Details detailsItem = new Details();
                detailsItem.Label = breakupItem.Fields[Templates.TravelInsuranceConstant.Label].Value;
                detailsItem.Amount = breakupItem.Fields[Templates.TravelInsuranceConstant.Amount].Value;
                details.Add(detailsItem);
            }
            return details;
        }

        public List<Heading> GetHeadingList(Item item)
        {
            List<Heading> headings = new List<Heading>();
            var childItems = item.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.BreakUpHeading).FirstOrDefault();
            var breakupDetailItems = childItems.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.BreakUpHeadingChildren);

            foreach (var breakupItem in breakupDetailItems)
            {
                Heading detailsItem = new Heading();
                detailsItem.Title = breakupItem.Fields[Templates.TravelInsuranceConstant.Description].Value;

                headings.Add(detailsItem);
            }
            return headings;
        }
    }
}