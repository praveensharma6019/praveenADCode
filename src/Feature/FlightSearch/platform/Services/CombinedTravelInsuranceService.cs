using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public class CombinedTravelInsuranceService : ICombinedTravelInsurance
    {
        private readonly IHelper helper;

        public CombinedTravelInsuranceService(IHelper helper)
        {
            this.helper = helper;
        }

        public CombinedTravelInsurance GetCombinedTravelInsuranceData(Item datasource)
        {
            CombinedTravelInsurance travelInsurance = new CombinedTravelInsurance();
            if (datasource != null && datasource.Children.Count > 0)
            {
                var insuranceObj = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.InsuranceTemplateId).FirstOrDefault();
                if(insuranceObj != null)
                {
                    TravelInsurance travelObj = new TravelInsurance();
                    travelObj.ModelTitle = insuranceObj?.Fields[Templates.TravelInsuranceConstant.Title]?.Value;
                    travelObj.BrandLogo = insuranceObj.Fields[Templates.TravelInsuranceConstant.BrandLogo] != null ? helper.GetImageURL(insuranceObj, Templates.TravelInsuranceConstant.BrandLogo) : String.Empty;
                    travelObj.Heading = insuranceObj?.Fields[Templates.TravelInsuranceConstant.Heading]?.Value;
                    travelObj.Error = insuranceObj?.Fields[Templates.TravelInsuranceConstant.Error]?.Value;
                    travelObj.TravelLogo = insuranceObj.Fields[Templates.TravelInsuranceConstant.TravelLogo] != null ? helper.GetImageURL(insuranceObj, Templates.TravelInsuranceConstant.TravelLogo) : String.Empty;
                    travelObj.disclaimer = GetDisclaimerData(insuranceObj);
                    travelObj.info = GetInfoData(insuranceObj);
                    travelObj.benefits = GetBenefitsData(insuranceObj);
                    travelObj.breakups = GetBreakupsData(insuranceObj);
                    travelObj.options = GetOptionsData(insuranceObj);
                    travelInsurance.TravelInsuranceDetails = travelObj;
                }

                var zeroCancellationObj = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.ZeroCancellationConstant.ZeroCancellationTemplateId).FirstOrDefault();
                if (zeroCancellationObj != null)
                {
                    ZeroCancellation cancellationObj = new ZeroCancellation();
                    cancellationObj.AmountLabel = !string.IsNullOrEmpty(zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.AmountLabel].Value) ? zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.AmountLabel].Value : string.Empty;
                    cancellationObj.RefundLabel = !string.IsNullOrEmpty(zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.RefundLabel].Value) ? zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.RefundLabel].Value : string.Empty;
                    cancellationObj.Icon = zeroCancellationObj.Fields[Templates.TravelInsuranceConstant.Icon] != null ? helper.GetImageURL(zeroCancellationObj, Templates.TravelInsuranceConstant.Icon) : string.Empty;
                    cancellationObj.ZeroCancellationHeading = !string.IsNullOrEmpty(zeroCancellationObj.Fields[Templates.TravelInsuranceConstant.Heading].Value) ? zeroCancellationObj.Fields[Templates.TravelInsuranceConstant.Heading].Value : string.Empty;
                    cancellationObj.TnCLabel = !string.IsNullOrEmpty(zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.TnCLabel].Value) ? zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.TnCLabel].Value : string.Empty;
                    cancellationObj.Disclaimer = !string.IsNullOrEmpty(zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.Disclaimer].Value) ? zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.Disclaimer].Value : string.Empty;
                    cancellationObj.TnCLabelLink = !string.IsNullOrEmpty(zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.TnCLabelLink].Value) ? zeroCancellationObj.Fields[Templates.ZeroCancellationConstant.TnCLabelLink].Value : string.Empty;
                    cancellationObj.Description = GetDescriptionData(zeroCancellationObj);
                    cancellationObj.ModelBox = GetModelBoxData(zeroCancellationObj);
                    travelInsurance.ZeroCancellationDetails = cancellationObj;
                }                              
            }
            return travelInsurance;
        }

        public ZeroCancellationDescription GetDescriptionData(Item datasource)
        {
            ZeroCancellationDescription cancellationObj = new ZeroCancellationDescription();
            ZeroCancellationPlaceholder zeroCancellationPlaceholderObj = new ZeroCancellationPlaceholder();
            var descriptionItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.ZeroCancellationConstant.DescriptionTemplateId).FirstOrDefault();
            cancellationObj.Label = !string.IsNullOrEmpty(descriptionItem.Fields[Templates.TravelInsuranceConstant.Label].Value) ? descriptionItem.Fields[Templates.TravelInsuranceConstant.Label].Value : string.Empty ;
            cancellationObj.LabelText = !string.IsNullOrEmpty(descriptionItem.Fields[Templates.TravelInsuranceConstant.LabelText].Value) ? descriptionItem.Fields[Templates.TravelInsuranceConstant.LabelText].Value : string.Empty;
            zeroCancellationPlaceholderObj.HowToWork = !string.IsNullOrEmpty(descriptionItem.Fields[Templates.ZeroCancellationConstant.HowToWork].Value) ? descriptionItem.Fields[Templates.ZeroCancellationConstant.HowToWork].Value : string.Empty;
            zeroCancellationPlaceholderObj.HowToWorkLink = !string.IsNullOrEmpty(descriptionItem.Fields[Templates.ZeroCancellationConstant.HowToWorkLink].Value) ? helper.LinkUrl(descriptionItem.Fields[Templates.ZeroCancellationConstant.HowToWorkLink]) : string.Empty;
            cancellationObj.Placeholder = zeroCancellationPlaceholderObj;
            return cancellationObj;
        }
        public ZeroCancellationModelBox GetModelBoxData(Item datasource)
        {
            ZeroCancellationModelBox modelBoxObj = new ZeroCancellationModelBox();
            ZeroBreakup zeroBreakupObj = null;
            var modelBoxItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.ZeroCancellationConstant.ModelBoxTemplateId).FirstOrDefault();
            modelBoxObj.Heading = !string.IsNullOrEmpty(modelBoxItem.Fields[Templates.TravelInsuranceConstant.Heading].Value) ? modelBoxItem.Fields[Templates.TravelInsuranceConstant.Heading].Value : string.Empty;
            modelBoxObj.AdditionalBenefit = !string.IsNullOrEmpty(modelBoxItem.Fields[Templates.ZeroCancellationConstant.AdditionalBenefit].Value) ? modelBoxItem.Fields[Templates.ZeroCancellationConstant.AdditionalBenefit].Value : string.Empty;

            //withoutZeroBreakupItem
            zeroBreakupObj = new ZeroBreakup();
            var withoutZeroBreakupItem = modelBoxItem.Children.Where(x => x.ID.ToString() == Templates.ZeroCancellationConstant.WithoutZeroBreakupItemId).FirstOrDefault();
            zeroBreakupObj.Heading = !string.IsNullOrEmpty(withoutZeroBreakupItem.Fields[Templates.TravelInsuranceConstant.Heading].Value) ? withoutZeroBreakupItem.Fields[Templates.TravelInsuranceConstant.Heading].Value : string.Empty;
            zeroBreakupObj.Icon = !string.IsNullOrEmpty(withoutZeroBreakupItem.Fields[Templates.TravelInsuranceConstant.Icon].Value) ? withoutZeroBreakupItem.Fields[Templates.TravelInsuranceConstant.Icon].Value : string.Empty;
            if(withoutZeroBreakupItem.GetChildren() != null && withoutZeroBreakupItem.GetChildren().Count > 0)
            {
                List<BreakupModel> breakupModelListObj = new List<BreakupModel>();
                foreach (var item in withoutZeroBreakupItem.GetChildren().InnerChildren)
                {
                    BreakupModel breakupModelObj = new BreakupModel();
                    breakupModelObj.Code = !string.IsNullOrEmpty(item.Fields[Templates.ZeroCancellationConstant.Code].Value) ? item.Fields[Templates.ZeroCancellationConstant.Code].Value : string.Empty;
                    breakupModelObj.Label = !string.IsNullOrEmpty(item.Fields[Templates.TravelInsuranceConstant.Label].Value) ? item.Fields[Templates.TravelInsuranceConstant.Label].Value : string.Empty;
                    breakupModelObj.Hint = !string.IsNullOrEmpty(item.Fields[Templates.ZeroCancellationConstant.Hint].Value) ? item.Fields[Templates.ZeroCancellationConstant.Hint].Value : string.Empty;
                    breakupModelListObj.Add(breakupModelObj);
                }
                zeroBreakupObj.Breakup = breakupModelListObj;
            }
            modelBoxObj.WithoutZeroBreakup = zeroBreakupObj;

            //WithZeroBreakupItem
            zeroBreakupObj = new ZeroBreakup();
            var withZeroBreakupItem = modelBoxItem.Children.Where(x => x.ID.ToString() == Templates.ZeroCancellationConstant.WithZeroBreakupItemId).FirstOrDefault();
            //ZeroBreakup zeroBreakupObj1 = new ZeroBreakup();
            zeroBreakupObj.Heading = !string.IsNullOrEmpty(withZeroBreakupItem.Fields[Templates.TravelInsuranceConstant.Heading].Value) ? withZeroBreakupItem.Fields[Templates.TravelInsuranceConstant.Heading].Value : string.Empty;
            zeroBreakupObj.Icon = !string.IsNullOrEmpty(withZeroBreakupItem.Fields[Templates.TravelInsuranceConstant.Icon].Value) ? withZeroBreakupItem.Fields[Templates.TravelInsuranceConstant.Icon].Value : string.Empty;
            if (withZeroBreakupItem.GetChildren() != null && withZeroBreakupItem.GetChildren().Count > 0)
            {
                List<BreakupModel> breakupModelListObj = new List<BreakupModel>();
                foreach (var item in withZeroBreakupItem.GetChildren().InnerChildren)
                {
                    BreakupModel breakupModelObj = new BreakupModel();
                    breakupModelObj.Code = !string.IsNullOrEmpty(item.Fields[Templates.ZeroCancellationConstant.Code].Value) ? item.Fields[Templates.ZeroCancellationConstant.Code].Value : string.Empty;
                    breakupModelObj.Label = !string.IsNullOrEmpty(item.Fields[Templates.TravelInsuranceConstant.Label].Value) ? item.Fields[Templates.TravelInsuranceConstant.Label].Value : string.Empty;
                    breakupModelObj.Hint = !string.IsNullOrEmpty(item.Fields[Templates.ZeroCancellationConstant.Hint].Value) ? item.Fields[Templates.ZeroCancellationConstant.Hint].Value : string.Empty;
                    breakupModelListObj.Add(breakupModelObj);
                }
                zeroBreakupObj.Breakup = breakupModelListObj;
            }
            modelBoxObj.WithZeroBreakup = zeroBreakupObj;

            //PleaseNote
            PleaseNoteModel pleaseNoteModelObj = new PleaseNoteModel();
            var pleaseNoteItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.ZeroCancellationConstant.PleaseNoteTemplateId).FirstOrDefault();
            pleaseNoteModelObj.Heading = !string.IsNullOrEmpty(pleaseNoteItem.Fields[Templates.TravelInsuranceConstant.Heading].Value) ? pleaseNoteItem.Fields[Templates.TravelInsuranceConstant.Heading].Value : string.Empty;
            if(pleaseNoteItem.GetChildren() != null && pleaseNoteItem.GetChildren().Count > 0)
            {
                List<NotesModel> notesListObj = new List<NotesModel>();
                foreach(var item in pleaseNoteItem.GetChildren().InnerChildren)
                {
                    NotesModel notesModelObj = new NotesModel();
                    notesModelObj.Label = !string.IsNullOrEmpty(item.Fields[Templates.TravelInsuranceConstant.Label].Value) ? item.Fields[Templates.TravelInsuranceConstant.Label].Value : string.Empty;
                    Placeholder placeholderObj = new Placeholder();
                    placeholderObj.TNC = !string.IsNullOrEmpty(item.Fields[Templates.ZeroCancellationConstant.tnc].Value) ? item.Fields[Templates.ZeroCancellationConstant.tnc].Value : string.Empty;
                    placeholderObj.TNCLink = !string.IsNullOrEmpty(item.Fields[Templates.ZeroCancellationConstant.tncLink].Value) ? helper.LinkUrl(item.Fields[Templates.ZeroCancellationConstant.tncLink]) : string.Empty;
                    notesModelObj.Placeholder = placeholderObj;
                    notesListObj.Add(notesModelObj);
                }
                pleaseNoteModelObj.Notes = notesListObj;
            }
            modelBoxObj.PleaseNote = pleaseNoteModelObj;

            //CancellationProcess
            CancellationProcessModel CancellationProcessModelObj = new CancellationProcessModel();
            var CancellationProcessModelItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.ZeroCancellationConstant.CancellationProcessTemplateId).FirstOrDefault();
            CancellationProcessModelObj.Heading = !string.IsNullOrEmpty(CancellationProcessModelItem.Fields[Templates.TravelInsuranceConstant.Heading].Value) ? CancellationProcessModelItem.Fields[Templates.TravelInsuranceConstant.Heading].Value : string.Empty;
            if (CancellationProcessModelItem.GetChildren() != null && CancellationProcessModelItem.GetChildren().Count > 0)
            {
                List<CancellationProcessModelNotesModel> notesListObj = new List<CancellationProcessModelNotesModel>();
                foreach (var item in CancellationProcessModelItem.GetChildren().InnerChildren)
                {
                    CancellationProcessModelNotesModel notesModelObj = new CancellationProcessModelNotesModel();
                    notesModelObj.Label = !string.IsNullOrEmpty(item.Fields[Templates.TravelInsuranceConstant.Label].Value) ? item.Fields[Templates.TravelInsuranceConstant.Label].Value : string.Empty;
                    CancellationProcessPlaceholder placeholderObj = new CancellationProcessPlaceholder();
                    placeholderObj.Vendor = !string.IsNullOrEmpty(item.Fields[Templates.ZeroCancellationConstant.vendor].Value) ? item.Fields[Templates.ZeroCancellationConstant.vendor].Value : string.Empty;
                    placeholderObj.VendorLink = !string.IsNullOrEmpty(item.Fields[Templates.ZeroCancellationConstant.vendorLink].Value) ? helper.LinkUrl(item.Fields[Templates.ZeroCancellationConstant.vendorLink]) : string.Empty;
                    notesModelObj.Placeholder = placeholderObj;
                    notesListObj.Add(notesModelObj);
                }
                CancellationProcessModelObj.Notes = notesListObj;
            }
            modelBoxObj.CancellationProcess = CancellationProcessModelObj;

            return modelBoxObj;
        }
        public Disclaimer GetDisclaimerData(Item datasource)
        {
            Disclaimer disclaimer = new Disclaimer();
            Placeholder placeholder = new Placeholder();
            var disclaimerItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.DisclaimerTemplateId).FirstOrDefault();
            placeholder.TNC = disclaimerItem?.Fields[Templates.TravelInsuranceConstant.TNC]?.Value;
            placeholder.TNCLink = disclaimerItem.Fields[Templates.TravelInsuranceConstant.TNCLink] != null ? helper.LinkUrl(disclaimerItem.Fields[Templates.TravelInsuranceConstant.TNCLink]) : String.Empty;
            placeholder.TNCApp = disclaimerItem?.Fields[Templates.TravelInsuranceConstant.TNCApp]?.Value;
            placeholder.Amount = disclaimerItem?.Fields[Templates.TravelInsuranceConstant.Amount]?.Value;
            disclaimer.Label = disclaimerItem?.Fields[Templates.TravelInsuranceConstant.Label]?.Value;           
            disclaimer.Placeholder = placeholder;
            return disclaimer;
        }

        public Information GetInfoData(Item datasource)
        {
            Information info = new Information();
            Placeholder placeholder = new Placeholder();
            var infoItem = datasource.Children.Where(x => x.TemplateID.ToString() == Templates.TravelInsuranceConstant.InfoTemplateId).FirstOrDefault();
            placeholder.TNC = infoItem?.Fields[Templates.TravelInsuranceConstant.TNC]?.Value;
            placeholder.TNCLink = infoItem.Fields[Templates.TravelInsuranceConstant.TNCLink] != null ? helper.LinkUrl(infoItem.Fields[Templates.TravelInsuranceConstant.TNCLink]) : String.Empty;
            placeholder.Amount = infoItem?.Fields[Templates.TravelInsuranceConstant.Amount]?.Value;
            placeholder.TNCApp = infoItem?.Fields[Templates.TravelInsuranceConstant.TNCApp]?.Value;
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
                    Title = benefitsItem?.Fields[Templates.TravelInsuranceConstant.Title]?.Value,
                    Description = benefitsItem?.Fields[Templates.TravelInsuranceConstant.Description]?.Value,
                    Icon = benefitsItem?.Fields[Templates.TravelInsuranceConstant.Icon]?.Value,
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
                    Amount = breakupsItem?.Fields[Templates.TravelInsuranceConstant.Amount]?.Value,
                    Label = breakupsItem?.Fields[Templates.TravelInsuranceConstant.Label]?.Value
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
                    Id = optionsItem?.Fields[Templates.TravelInsuranceConstant.Id]?.Value,
                    Tag = optionsItem?.Fields[Templates.TravelInsuranceConstant.Tag]?.Value,
                    Label = optionsItem?.Fields[Templates.TravelInsuranceConstant.Label]?.Value
                };
                optionsList.Add(options);
            }
            return optionsList;
        }
    }
}