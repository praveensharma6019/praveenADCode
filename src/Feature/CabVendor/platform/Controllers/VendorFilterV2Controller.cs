using System;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Controllers
{
    public class VendorFilterV2Controller : ApiController
    {
        private ILogRepository logRepository;
        private readonly IHelper _helper;
        public VendorFilterV2Controller(ILogRepository _logRepository, IHelper helper)
        {
            this.logRepository = _logRepository;
            this._helper = helper;
        }

        [HttpPost]
        [Route("api/v2/GetFilteredVendorData")]
        public IHttpActionResult GetVendorV2([FromBody] VendorFiltersV2 filter)
        {
            ResponseFilters responseData = new ResponseFilters();
            ResultFilters resultData = new ResultFilters();

            resultData.result = GetFilteredVendorDataV2(ref filter);
            if (resultData.result != null)
            {
                responseData.status = true;
                responseData.data = resultData;
            }
            return Json(responseData);
        }

        private Object GetFilteredVendorDataV2(ref VendorFiltersV2 filter)
        {
            List<Vendor> result = new List<Vendor>();
            VendorDetails venderInfo = new VendorDetails();
            ContentJSONList contentJSONList = null;
            Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(filter.language);
            Sitecore.Data.Database contextDB = Sitecore.Context.Database;

            Item CabVendersInfoFolder = contextDB.GetItem(VendorConstant.CabVendorInfoFolderID.ToString(), language);
            if (CabVendersInfoFolder != null && CabVendersInfoFolder.HasChildren) 
            {
                string airportCode = filter.airport;
                string vendorCode = filter.vendorCode;
                string tripType = filter.tripType;
                string cabSchedule = filter.cabSchedule;
                string cabBookingType = filter.cabBookingType;
                bool preBooking = filter.isPreBooking;
                string cancellationCode = filter.cancellationCode;
                Item vendorItemList = CabVendersInfoFolder.GetChildren().Where(a => a.Name.ToLower() == airportCode.ToLower().ToString()).FirstOrDefault();
                if (vendorItemList != null && vendorItemList.HasChildren)
                {
                    var getChildren = vendorItemList.GetChildren();
                    foreach (Item item in getChildren)
                    {
                        Item vendor = _helper.GetDropLinkItem(item.Fields[VendorConstant.Vendor]);
                        Item triptype = _helper.GetDropLinkItem(item.Fields[VendorConstant.TripType]);
                        Item cabschedule = _helper.GetDropLinkItem(item.Fields[VendorConstant.CabSchedule]);
                        Item cabbookingtype = _helper.GetDropLinkItem(item.Fields[VendorConstant.CabBookingType]);
                        if (vendor.Fields[VendorConstant.Code].Value.ToLower() == vendorCode.ToLower() && triptype.Fields[VendorConstant.ItemTitle].Value.ToLower() == tripType.ToLower() && cabschedule.Fields[VendorConstant.ItemTitle].Value.ToLower() == cabSchedule.ToLower() && cabbookingtype.Fields[VendorConstant.ItemTitle].Value.ToLower() == cabBookingType.ToLower())
                        {

                            venderInfo.vendorName = !string.IsNullOrEmpty(vendor.Fields[VendorConstant.Title].Value) ? vendor.Fields[VendorConstant.Title].Value : string.Empty;

                            Item airportName = _helper.GetDropLinkItem(item.Fields[VendorConstant.Airport]);
                            venderInfo.cityName = !string.IsNullOrEmpty(airportName.Name.ToString()) ? airportName.Name.ToString() : string.Empty;
                            if (airportName != null && airportName.HasChildren)
                            {
                                Item airportItem = airportName.GetChildren().Where(a => a.TemplateID.Guid == VendorConstant.AirportTemplateID).FirstOrDefault();
                                if (airportItem != null)
                                {
                                    venderInfo.airportName = !string.IsNullOrEmpty(airportItem.Fields[VendorConstant.AirportName].Value) ? airportItem.Fields[VendorConstant.AirportName].Value : string.Empty;
                                }
                            }

                            venderInfo.terminalName = !string.IsNullOrEmpty(item.Fields[VendorConstant.Terminal].Value) ? item.Fields[VendorConstant.Terminal].Value : string.Empty;
                            venderInfo.gate = !string.IsNullOrEmpty(item.Fields[VendorConstant.Gate].Value) ? item.Fields[VendorConstant.Gate].Value : string.Empty;
                            venderInfo.cabLocation = !string.IsNullOrEmpty(item.Fields[VendorConstant.CabLocation].Value) ? item.Fields[VendorConstant.CabLocation].Value : string.Empty;
                            venderInfo.cancellationCTAText = _helper.GetLinkText(item, VendorConstant.CancellationCTA.ToString());
                            venderInfo.cancellationCTA = _helper.GetLinkURL(item, VendorConstant.CancellationCTA.ToString());
                            venderInfo.shareMessage = !string.IsNullOrEmpty(item.Fields[VendorConstant.ShareMessage].Value) ? item.Fields[VendorConstant.ShareMessage].Value : string.Empty;
                            venderInfo.policeHelpline = !string.IsNullOrEmpty(item.Fields[VendorConstant.PoliceHelpline].Value) ? item.Fields[VendorConstant.PoliceHelpline].Value : string.Empty;
                            venderInfo.scheduleCabDetailShareTime = !string.IsNullOrEmpty(item.Fields[VendorConstant.ScheduleCabDetailShareTime].Value) ? item.Fields[VendorConstant.ScheduleCabDetailShareTime].Value : string.Empty;
                            venderInfo.trackingPrecautionMessage = !string.IsNullOrEmpty(item.Fields[VendorConstant.TrackingPrecautionMessage].Value) ? item.Fields[VendorConstant.TrackingPrecautionMessage].Value : string.Empty;
                            venderInfo.cancellationCTAWeb = _helper.GetLinkURL(item, VendorConstant.CancellationCTAWeb.ToString());
                            venderInfo.cancellationCTAWebText = _helper.GetLinkText(item, VendorConstant.CancellationCTAWeb.ToString());
                            venderInfo.tripType = triptype.Fields[VendorConstant.ItemTitle].Value;
                            venderInfo.cabSchedule = cabschedule.Fields[VendorConstant.ItemTitle].Value;
                            venderInfo.cabBookingType = cabbookingtype.Fields[VendorConstant.ItemTitle].Value;
                            MultilistField stepsToBoardObj = item?.Fields[VendorConstant.StepsToBoardDetails];
                            venderInfo.BookingStep = new List<BookingSteps>();
                            if (stepsToBoardObj != null && stepsToBoardObj.GetItems().Length > 0)
                            {
                                foreach (var step in stepsToBoardObj.GetItems())
                                {
                                    BookingSteps stepsObj = new BookingSteps();
                                    stepsObj.stepTitle = !string.IsNullOrEmpty(step.Fields[VendorConstant.Title].Value) ? step.Fields[VendorConstant.Title].Value : string.Empty;
                                    stepsObj.stepImage = !string.IsNullOrEmpty(step.Fields[VendorConstant.Image].Value) ? _helper.GetImageURL(step.Fields[VendorConstant.Image]) : string.Empty;
                                    stepsObj.stepDescription = !string.IsNullOrEmpty(step.Fields[VendorConstant.Description].Value) ? step.Fields[VendorConstant.Description].Value : string.Empty;
                                    venderInfo.BookingStep.Add(stepsObj);
                                }
                            }
                            contentJSONList = new ContentJSONList();

                            if (!preBooking)
                            {
                                Item postbooking = item.Children.Where(x => x.TemplateID == VendorConstant.CabPostBookingTemplateID).FirstOrDefault();
                                if(postbooking != null && postbooking.HasChildren)
                                {
                                    //Important Information
                                    Item importantInfo = postbooking.Children.Where(x => x.TemplateID == VendorConstant.CabImportantInformationTemplateID).FirstOrDefault();
                                    if (importantInfo != null && importantInfo.HasChildren)
                                    {
                                        IEnumerable<Item> importantInfoData = importantInfo.Children.Where(x => x.TemplateID == VendorConstant.TitleWithRichText).ToList();
                                        if (importantInfoData != null && importantInfoData.Count() > 0)
                                        {
                                            foreach (var child in importantInfoData)
                                            {
                                                ContentJSON contentJSON = new ContentJSON();

                                                contentJSON.title = child.Fields.Contains(VendorConstant.Title) ? child.Fields[VendorConstant.Title].Value.ToString() : "";

                                                if (child.HasChildren)
                                                {
                                                    List<Item> lineItems = child.Children.Where(x => x.TemplateID == VendorConstant.TitleWithLinkTemplateID).ToList();

                                                    foreach (Item lineItem in lineItems)
                                                    {
                                                        LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                        if (linewithLinks != null)
                                                        {
                                                            contentJSON.lines.Add(linewithLinks);
                                                        }
                                                    }
                                                }

                                                contentJSONList.cabImpInfo = contentJSON;
                                                venderInfo.infoDetails = contentJSONList;
                                            }
                                        }
                                    }

                                    //Detailed Important Information
                                    Item detailedImportantInfo = postbooking.Children.Where(x => x.TemplateID == VendorConstant.CabDetailedImportantInformationTemplateID).FirstOrDefault();
                                    if (detailedImportantInfo != null && detailedImportantInfo.HasChildren)
                                    {
                                        IEnumerable<Item> detailedImportantInfoData = detailedImportantInfo.Children.Where(x => x.TemplateID == VendorConstant.TitleWithRichText).ToList();
                                        if (detailedImportantInfoData != null && detailedImportantInfoData.Count() > 0)
                                        {
                                            foreach (var child in detailedImportantInfoData)
                                            {
                                                ContentJSON contentJSON = new ContentJSON();

                                                contentJSON.title = child.Fields.Contains(VendorConstant.Title) ? child.Fields[VendorConstant.Title].Value.ToString() : "";

                                                if (child.HasChildren)
                                                {
                                                    List<Item> lineItems = child.Children.Where(x => x.TemplateID == VendorConstant.TitleWithLinkTemplateID).ToList();

                                                    foreach (Item lineItem in lineItems)
                                                    {
                                                        LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                        if (linewithLinks != null)
                                                        {
                                                            contentJSON.lines.Add(linewithLinks);
                                                        }
                                                    }
                                                }

                                                contentJSONList.cabDetailedImpInfo = contentJSON;
                                                venderInfo.infoDetails = contentJSONList;
                                            }
                                        }
                                    }

                                    //Cancellation Policy
                                    Item cancellationPolicy = postbooking.Children.Where(x => x.TemplateID == VendorConstant.CabCancellationPolicyTemplateID).FirstOrDefault();
                                    if (cancellationPolicy != null && cancellationPolicy.HasChildren)
                                    {
                                        IEnumerable<Item> cancellationPolicyData = cancellationPolicy.Children.Where(x => x.TemplateID == VendorConstant.TitleWithRichText).ToList();
                                        if (cancellationPolicyData != null && cancellationPolicyData.Count() > 0)
                                        {
                                            foreach (var child in cancellationPolicyData)
                                            {
                                                ContentJSON contentJSON = new ContentJSON();

                                                contentJSON.title = child.Fields.Contains(VendorConstant.Title) ? child.Fields[VendorConstant.Title].Value.ToString() : "";

                                                if (child.HasChildren)
                                                {
                                                    List<Item> lineItems = child.Children.Where(x => x.TemplateID == VendorConstant.TitleWithLinkTemplateID).ToList();

                                                    foreach (Item lineItem in lineItems)
                                                    {
                                                        if (!string.IsNullOrEmpty(cancellationCode))
                                                        {
                                                            string autoIdValue = lineItem.Fields[VendorConstant.LinkAutoId].ToString();

                                                            if (!string.IsNullOrEmpty(autoIdValue) && cancellationCode.ToLower() == autoIdValue.ToLower())
                                                            {
                                                                LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                                if (linewithLinks != null)
                                                                {
                                                                    contentJSON.lines.Add(linewithLinks);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                            if (linewithLinks != null)
                                                            {
                                                                contentJSON.lines.Add(linewithLinks);
                                                            }
                                                        }
                                                    }
                                                }

                                                contentJSONList.cabCancellationPolicy = contentJSON;
                                                venderInfo.infoDetails = contentJSONList;
                                            }
                                        }
                                    }
                                }
                            }

                            else
                            {
                                Item prebooking = item.Children.Where(x => x.TemplateID == VendorConstant.CabPreBookingTemplateID).FirstOrDefault();
                                if (prebooking != null && prebooking.HasChildren)
                                {
                                    //Important Information
                                    Item importantInfo = prebooking.Children.Where(x => x.TemplateID == VendorConstant.CabImportantInformationTemplateID).FirstOrDefault();
                                    if (importantInfo != null && importantInfo.HasChildren)
                                    {
                                        IEnumerable<Item> importantInfoData = importantInfo.Children.Where(x => x.TemplateID == VendorConstant.TitleWithRichText).ToList();
                                        if (importantInfoData != null && importantInfoData.Count() > 0)
                                        {
                                            foreach (var child in importantInfoData)
                                            {
                                                ContentJSON contentJSON = new ContentJSON();

                                                contentJSON.title = child.Fields.Contains(VendorConstant.Title) ? child.Fields[VendorConstant.Title].Value.ToString() : "";

                                                if (child.HasChildren)
                                                {
                                                    List<Item> lineItems = child.Children.Where(x => x.TemplateID == VendorConstant.TitleWithLinkTemplateID).ToList();

                                                    foreach (Item lineItem in lineItems)
                                                    {
                                                        LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                        if (linewithLinks != null)
                                                        {
                                                            contentJSON.lines.Add(linewithLinks);
                                                        }
                                                    }
                                                }

                                                contentJSONList.cabImpInfo = contentJSON;
                                                venderInfo.infoDetails = contentJSONList;
                                            }
                                        }
                                    }

                                    //Detailed Important Information
                                    Item detailedImportantInfo = prebooking.Children.Where(x => x.TemplateID == VendorConstant.CabDetailedImportantInformationTemplateID).FirstOrDefault();
                                    if (detailedImportantInfo != null && detailedImportantInfo.HasChildren)
                                    {
                                        IEnumerable<Item> detailedImportantInfoData = detailedImportantInfo.Children.Where(x => x.TemplateID == VendorConstant.TitleWithRichText).ToList();
                                        if (detailedImportantInfoData != null && detailedImportantInfoData.Count() > 0)
                                        {
                                            foreach (var child in detailedImportantInfoData)
                                            {
                                                ContentJSON contentJSON = new ContentJSON();

                                                contentJSON.title = child.Fields.Contains(VendorConstant.Title) ? child.Fields[VendorConstant.Title].Value.ToString() : "";

                                                if (child.HasChildren)
                                                {
                                                    List<Item> lineItems = child.Children.Where(x => x.TemplateID == VendorConstant.TitleWithLinkTemplateID).ToList();

                                                    foreach (Item lineItem in lineItems)
                                                    {
                                                        LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                        if (linewithLinks != null)
                                                        {
                                                            contentJSON.lines.Add(linewithLinks);
                                                        }
                                                    }
                                                }

                                                contentJSONList.cabDetailedImpInfo = contentJSON;
                                                venderInfo.infoDetails = contentJSONList;
                                            }
                                        }
                                    }

                                    //Cancellation Policy
                                    Item cancellationPolicy = prebooking.Children.Where(x => x.TemplateID == VendorConstant.CabCancellationPolicyTemplateID).FirstOrDefault();
                                    if (cancellationPolicy != null && cancellationPolicy.HasChildren)
                                    {
                                        IEnumerable<Item> cancellationPolicyData = cancellationPolicy.Children.Where(x => x.TemplateID == VendorConstant.TitleWithRichText).ToList();
                                        if (cancellationPolicyData != null && cancellationPolicyData.Count() > 0)
                                        {
                                            foreach (var child in cancellationPolicyData)
                                            {
                                                ContentJSON contentJSON = new ContentJSON();

                                                contentJSON.title = child.Fields.Contains(VendorConstant.Title) ? child.Fields[VendorConstant.Title].Value.ToString() : "";

                                                if (child.HasChildren)
                                                {
                                                    List<Item> lineItems = child.Children.Where(x => x.TemplateID == VendorConstant.TitleWithLinkTemplateID).ToList();
                                                            foreach (Item lineItem in lineItems)
                                                            {
                                                                if (!string.IsNullOrEmpty(cancellationCode))
                                                                {
                                                                    string autoIdValue = lineItem.Fields[VendorConstant.LinkAutoId].ToString();

                                                                    if (!string.IsNullOrEmpty(autoIdValue) && cancellationCode.ToLower() == autoIdValue.ToLower())
                                                                    {
                                                                        LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                                        if (linewithLinks != null)
                                                                        {
                                                                            contentJSON.lines.Add(linewithLinks);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                                    if (linewithLinks != null)
                                                                    {
                                                                        contentJSON.lines.Add(linewithLinks);
                                                                    }
                                                                }

                                                    }
                                                }

                                                contentJSONList.cabCancellationPolicy = contentJSON;
                                                venderInfo.infoDetails = contentJSONList;
                                            }
                                        }
                                    }
                                }
                            }
                            

                                String[] Seperator = { "<p>", "</p>" };

                                venderInfo.rideNowText = !string.IsNullOrEmpty(item.Fields[VendorConstant.RideNowText].Value) ? item.Fields[VendorConstant.RideNowText].Value.Split(Seperator, StringSplitOptions.None).ToList() : new List<string>();
                                venderInfo.rideNowText.RemoveAll(a => a == "\n" || a == "");

                                venderInfo.rideLaterText = !string.IsNullOrEmpty(item.Fields[VendorConstant.RideLaterText].Value) ? item.Fields[VendorConstant.RideLaterText].Value.Split(Seperator, StringSplitOptions.None).ToList() : new List<string>();
                                venderInfo.rideLaterText.RemoveAll(a => a == "\n" || a == "");

                                venderInfo.stepsToBoard = !string.IsNullOrEmpty(item.Fields[VendorConstant.StepstoBoard].Value) ? item.Fields[VendorConstant.StepstoBoard].Value.Split(Seperator, StringSplitOptions.None).ToList() : new List<string>();
                                venderInfo.stepsToBoard.RemoveAll(a => a == "\n" || a == "");

                                IEnumerable<Item> contactDetailsItems = item.Children.Where(x => x.TemplateID == VendorConstant.ContactDetailsTemplateID).ToList();
                                if (contactDetailsItems != null && contactDetailsItems.Count() > 0)
                                {
                                    ContactDetailsItem contactItems = new ContactDetailsItem();
                                    foreach (var contact in contactDetailsItems)
                                    {
                                        if (contact != null)
                                        {
                                            if (contact.Name == "Phone")
                                            {
                                                ContactDetail phone = new ContactDetail();
                                                phone.name = contact.Fields["Name"].Value;
                                                phone.title = contact.Fields["Title"].Value;
                                                phone.richText = contact.Fields["RichText"].Value;
                                                contactItems.phone = phone;
                                            }
                                            if (contact.Name == "Email")
                                            {
                                                ContactDetail email = new ContactDetail();
                                                email.name = contact.Fields["Name"].Value;
                                                email.title = contact.Fields["Title"].Value;
                                                email.richText = contact.Fields["RichText"].Value;
                                                contactItems.email = email;
                                            }
                                        }
                                    }
                                    venderInfo.contactDetail = contactItems;
                                }
                            }
                    }
                    return venderInfo;
                }
                else
                {
                    return venderInfo;
                }
            }
            return venderInfo;
        }
        public LinewithLinks GetLineItemsInsurance(Item item)
        {
            LinewithLinks linewithLinks = null;
            if (item != null)
            {
                    linewithLinks = new LinewithLinks();
                    linewithLinks.title = item.Fields.Contains(VendorConstant.LinkTitle) ? item.Fields[VendorConstant.LinkTitle].ToString() : "";
                    linewithLinks.description = item.Fields.Contains(VendorConstant.LinkDesc) ? item.Fields[VendorConstant.LinkDesc].Value.ToString() : "";
                    linewithLinks.image = item.Fields.Contains(VendorConstant.LinkImage) ? _helper.GetImageURL(item.Fields[VendorConstant.LinkImage]) : "";  
                    linewithLinks.autoId = item.Fields.Contains(VendorConstant.LinkAutoId) ? item.Fields[VendorConstant.LinkAutoId].ToString() : "";
                    if (item.HasChildren)
                        {
                            List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == VendorConstant.TitleWithLinkTemplateID).ToList();
                            foreach (Item itemLink in lineLinkItems)
                            {
                                LineLinks lineLinks = new LineLinks();
                                lineLinks.link = itemLink.Name.Trim();
                                lineLinks.linkText = itemLink.Fields.Contains(VendorConstant.LinkText) ? itemLink.Fields[VendorConstant.LinkText].Value.ToString() : "";
                                lineLinks.linkURL = itemLink.Fields.Contains(VendorConstant.LinkURL) ? _helper.LinkUrl(itemLink.Fields[VendorConstant.LinkURL]) : "";
                                lineLinks.title = itemLink.Fields.Contains(VendorConstant.LinkTitle) ? itemLink.Fields[VendorConstant.LinkTitle].Value.ToString() : "";
                                lineLinks.description = itemLink.Fields.Contains(VendorConstant.LinkDesc) ? itemLink.Fields[VendorConstant.LinkDesc].Value.ToString() : "";
                                lineLinks.uniqueId = itemLink.Fields.Contains(VendorConstant.LinkUniqueId) ? itemLink.Fields[VendorConstant.LinkUniqueId].Value.ToString() : "";
                                lineLinks.image = itemLink.Fields.Contains(VendorConstant.LinkImage) ? _helper.GetImageURL(itemLink.Fields[VendorConstant.LinkImage]) : "";
                                linewithLinks.links.Add(lineLinks);
                            }
                        }
                }
            return linewithLinks;
        }
    }
}