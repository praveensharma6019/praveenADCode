using System;
using System.Web.Http;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
using Adani.SuperApp.Airport.Feature.CarParking.Platform.Models;


namespace Adani.SuperApp.Airport.Feature.CarParking.Platform.Controllers
{
    public class CarParkingFilterController : ApiController
    {
        private ILogRepository logRepository;
        private readonly IHelper _helper;
        public CarParkingFilterController(ILogRepository _logRepository, IHelper helper)
        {
            this.logRepository = _logRepository;
            this._helper = helper;
        }

        [HttpPost]
        [Route("api/v2/GetFilteredCarParkingData")]
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
                bool preBooking = filter.isPreBooking;
                string terminalCode = filter.TerminalCode;
                string parkingPackage = filter.parkingPackage;
                string facilityNumber = filter.facilityNumber;
                string productID = filter.productID;




                Item vendorItemList = CabVendersInfoFolder.GetChildren().Where(a => a.Name.ToLower() == airportCode.ToLower().ToString()).FirstOrDefault();
                if (vendorItemList != null && vendorItemList.HasChildren)
                {
                    var getChildren = string.IsNullOrEmpty(productID) ? vendorItemList.GetChildren().Where(x => terminalCode == "" || terminalCode.ToLower() == x.Fields["terminalCode"].Value.ToLower() || x.Fields["terminalCode"].Value.ToLower().Contains(terminalCode.ToLower()))
                        .Where(x => facilityNumber == "" || facilityNumber.ToLower() == x.Fields["facilityNumber"].Value.ToLower())
                        .Where(x => parkingPackage == "" || parkingPackage.ToLower() == x.Fields["parkingPackage"].Value.ToLower()) 
                        .Where(x=> string.IsNullOrEmpty(x.Fields["productID"].Value)) : vendorItemList.GetChildren()
                        .Where(x => productID.ToLower() == x.Fields["productID"].Value.ToLower());
                    foreach (Item item in getChildren)
                    {
                      
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
                           
                                                 
                            contentJSONList = new ContentJSONList();

                        if (!preBooking)
                        {
                            Item postbooking = item.Children.Where(x => x.TemplateID == VendorConstant.CabPostBookingTemplateID).FirstOrDefault();
                            if (postbooking != null && postbooking.HasChildren)
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
                                                    //if (!string.IsNullOrEmpty(cancellationCode))
                                                    //{
                                                    //    string autoIdValue = lineItem.Fields[VendorConstant.LinkAutoId].ToString();

                                                    //    if (!string.IsNullOrEmpty(autoIdValue) && cancellationCode.ToLower() == autoIdValue.ToLower())
                                                    //    {
                                                            LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                            if (linewithLinks != null)
                                                            {
                                                                contentJSON.lines.Add(linewithLinks);
                                                            }
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                    //    if (linewithLinks != null)
                                                    //    {
                                                    //        contentJSON.lines.Add(linewithLinks);
                                                    //    }
                                                    //}
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
                                                    //if (!string.IsNullOrEmpty(cancellationCode))
                                                    //{
                                                    // string autoIdValue = lineItem.Fields[VendorConstant.LinkAutoId].ToString();

                                                    //if (!string.IsNullOrEmpty(autoIdValue))
                                                    //{
                                                    LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                    if (linewithLinks != null)
                                                    {
                                                        contentJSON.lines.Add(linewithLinks);
                                                    }
                                                    //}
                                                    //}
                                                    //else
                                                    //{
                                                    //    LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem);
                                                    //    if (linewithLinks != null)
                                                    //    {
                                                    //        contentJSON.lines.Add(linewithLinks);
                                                    //    }
                                                    //}

                                                    //}
                                                }

                                                contentJSONList.cabCancellationPolicy = contentJSON;
                                                venderInfo.infoDetails = contentJSONList;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                            

                                //String[] Seperator = { "<p>", "</p>" };
                             

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