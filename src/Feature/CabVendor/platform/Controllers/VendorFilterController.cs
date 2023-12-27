using System;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Controllers
{
    public class VendorFilterController : ApiController
    {
        private ILogRepository logRepository;
        private readonly IHelper _helper;
        public VendorFilterController(ILogRepository _logRepository, IHelper helper)
        {
            this.logRepository = _logRepository;
            this._helper = helper;
        }

        [HttpPost]
        [Route("api/GetFilteredVendorData")]
        public IHttpActionResult GetVendor([FromBody] VendorFilters filter)
        {
            ResponseFilters responseData = new ResponseFilters();
            ResultFilters resultData = new ResultFilters();

            resultData.result = GetFilteredVendorData(ref filter);
            if (resultData.result != null)
            {
                responseData.status = true;
                responseData.data = resultData;
            }
            return Json(responseData);
        }

        private Object GetFilteredVendorData(ref VendorFilters filter)
        {
            List<Vendor> result = new List<Vendor>();
            VendorDetails venderInfo = new VendorDetails();
            Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(filter.language);
            Sitecore.Data.Database contextDB = Sitecore.Context.Database;

            Item CabVendersInfoFolder = contextDB.GetItem(VendorConstant.CabVendorInfoFolderID.ToString(), language);
            if (CabVendersInfoFolder != null && CabVendersInfoFolder.HasChildren)
            {
                string airportCode = filter.airport;
                string cabCode = filter.cabCode;
                string terminal = filter.terminal;
                string terminalGate = filter.terminalGate;
                Item vendorItemList = CabVendersInfoFolder.GetChildren().Where(a => a.Name.ToLower() == airportCode.ToLower().ToString()).FirstOrDefault();
                if (vendorItemList != null && vendorItemList.HasChildren)
                {
                    var getChildren = vendorItemList.GetChildren();
                    foreach (Item item in getChildren)
                    {
                        Item vendor = _helper.GetDropLinkItem(item.Fields[VendorConstant.Vendor]);
                        if (vendor.Fields[VendorConstant.Code].Value.ToLower() == cabCode.ToLower() && item.Fields[VendorConstant.Terminal].Value == terminal && item.Fields[VendorConstant.Gate].Value == terminalGate)
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
                }
                return venderInfo;
            }
            else
            {
                return venderInfo;
            }
        }
    }
}