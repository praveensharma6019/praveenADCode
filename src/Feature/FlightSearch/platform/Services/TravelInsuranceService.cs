using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using static Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models.TravelInsurance;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public class TravelInsuranceService : ITravelInsurance
    {
        private readonly IHelper helper;

        public TravelInsuranceService(IHelper helper)
        {
            this.helper = helper;
        }

        public TravelInsurance GetTravelInsuranceData(Item datasource)
        {
            TravelInsurance travelInsurance = new TravelInsurance();
            if (datasource != null && datasource.Children.Count > 0)
            {
                travelInsurance.ModelTitle = datasource.Fields[Templates.TravelInsuranceConstant.Title].Value;
                travelInsurance.BrandLogo = datasource.Fields[Templates.TravelInsuranceConstant.BrandLogo] != null ? helper.GetImageURL(datasource, Templates.TravelInsuranceConstant.BrandLogo) : String.Empty;
                travelInsurance.Heading = datasource.Fields[Templates.TravelInsuranceConstant.Heading].Value;
                travelInsurance.Error = datasource.Fields[Templates.TravelInsuranceConstant.Error].Value;
                travelInsurance.TravelLogo = datasource.Fields[Templates.TravelInsuranceConstant.TravelLogo] != null ? helper.GetImageURL(datasource, Templates.TravelInsuranceConstant.TravelLogo) : String.Empty;
                travelInsurance.disclaimer = GetDisclaimerData(datasource);
                travelInsurance.info = GetInfoData(datasource);
                travelInsurance.benefits = GetBenefitsData(datasource);
                travelInsurance.breakups = GetBreakupsData(datasource);
                travelInsurance.options = GetOptionsData(datasource);
            }
            return travelInsurance;
        }

        public Disclaimer GetDisclaimerData(Item datasource)
        {
            Disclaimer disclaimer = new Disclaimer();
            Placeholder placeholder = new Placeholder();
            var disclaimerItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.DisclaimerTemplateId).FirstOrDefault();
            placeholder.TNC = disclaimerItem.Fields[Templates.TravelInsuranceConstant.TNC].Value;
            placeholder.TNCLink = disclaimerItem.Fields[Templates.TravelInsuranceConstant.TNCLink] != null ? helper.LinkUrl(disclaimerItem.Fields[Templates.TravelInsuranceConstant.TNCLink]) : String.Empty;
            placeholder.TNCApp = disclaimerItem.Fields[Templates.TravelInsuranceConstant.TNCApp].Value;
            disclaimer.Label = disclaimerItem.Fields[Templates.TravelInsuranceConstant.Label].Value;
            disclaimer.Placeholder = placeholder;
            return disclaimer;
        }

        public Information GetInfoData(Item datasource)
        {
            Information info = new Information();
            Placeholder placeholder = new Placeholder();
            var infoItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.InfoTemplateId).FirstOrDefault();
            placeholder.TNC = infoItem.Fields[Templates.TravelInsuranceConstant.TNC].Value;
            placeholder.TNCLink = infoItem.Fields[Templates.TravelInsuranceConstant.TNCLink] != null ? helper.LinkUrl(infoItem.Fields[Templates.TravelInsuranceConstant.TNCLink]) : String.Empty;
            placeholder.Amount = infoItem.Fields[Templates.TravelInsuranceConstant.Amount].Value;
            placeholder.TNCApp = infoItem.Fields[Templates.TravelInsuranceConstant.TNCApp].Value;
            info.Label = infoItem.Fields[Templates.TravelInsuranceConstant.Label].Value;
            info.Placeholder = placeholder;
            return info;
        }

        public List<Benefits> GetBenefitsData(Item datasource)
        {
            var benefitsFolder = datasource.Children.Where(x => x.Name.ToString() == Templates.TravelInsuranceConstant.BenefitsFolder).FirstOrDefault();
            List<Benefits> benefitsList = new List<Benefits>();
            Benefits benefits;
            foreach (Item benefitsItem in benefitsFolder.Children)
            {
                benefits = new Benefits
                {
                    Title = benefitsItem.Fields[Templates.TravelInsuranceConstant.Title].Value,
                    Description = benefitsItem.Fields[Templates.TravelInsuranceConstant.Description].Value,
                    Icon = benefitsItem.Fields[Templates.TravelInsuranceConstant.Icon].Value,
                    IconUrl = benefitsItem.Fields[Templates.TravelInsuranceConstant.IconUrl] != null ? helper.GetImageURL(benefitsItem, Templates.TravelInsuranceConstant.IconUrl) : String.Empty
            };
                benefitsList.Add(benefits);
            }
            return benefitsList;
        }

        public List<Breakups> GetBreakupsData(Item datasource)
        {
            var breakupsFolder = datasource.Children.Where(x => x.Name.ToString() == Templates.TravelInsuranceConstant.BreakupsFolder).FirstOrDefault();
            List<Breakups> breakupsList = new List<Breakups>();
            Breakups breakups;
            foreach (Item breakupsItem in breakupsFolder.Children)
            {
                breakups = new Breakups
                {
                    Amount = breakupsItem.Fields[Templates.TravelInsuranceConstant.Amount].Value,
                    Label = breakupsItem.Fields[Templates.TravelInsuranceConstant.Label].Value
                };
                breakupsList.Add(breakups);
            }
            return breakupsList;
        }

        public List<Options> GetOptionsData(Item datasource)
        {
            var optionsFolder = datasource.Children.Where(x => x.Name.ToString() == Templates.TravelInsuranceConstant.OptionsFolder).FirstOrDefault();
            List<Options> optionsList = new List<Options>();
            Options options;
            foreach (Item optionsItem in optionsFolder.Children)
            {
                options = new Options
                {
                    Id = optionsItem.Fields[Templates.TravelInsuranceConstant.Id].Value,
                    Tag = optionsItem.Fields[Templates.TravelInsuranceConstant.Tag].Value,
                    Label = optionsItem.Fields[Templates.TravelInsuranceConstant.Label].Value
                };
                optionsList.Add(options);
            }
            return optionsList;
        }
    }
}