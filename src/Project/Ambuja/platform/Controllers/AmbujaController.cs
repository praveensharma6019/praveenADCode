using Project.AmbujaCement.Website.Models.DealerLocatorPage;
using Project.AmbujaCement.Website.Templates.DealersPage;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Controllers
{
    public class AmbujaController : Controller
    {
        // GET: Ambuja
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dealers
        public ActionResult GetDealers(string stateName, string cityName, string areaName)
        {
            stateName = stateName?.Replace(" ","")?.ToLower();
            cityName = cityName?.Replace(" ", "")?.ToLower();
            areaName = areaName?.Replace(" ", "")?.ToLower();

            var dealerDetailsModel = new DealersDetailsModel();
            var dealersLabelItem = Sitecore.Context.Database.GetItem(DealersTemplate.DealersLabelTemplate.DealersLabelItemId);

            var dealersLabelModel = new DealersLabelsModel();
            dealersLabelModel.nameLabel = dealersLabelItem.Fields[DealersTemplate.DealersLabelTemplate.NameLabelFieldId].Value;
            dealersLabelModel.mobileNoLabel = dealersLabelItem.Fields[DealersTemplate.DealersLabelTemplate.MobileNoLabelFieldId].Value;
            dealersLabelModel.pincodeLabel = dealersLabelItem.Fields[DealersTemplate.DealersLabelTemplate.PincodeLabelFieldId].Value;
            dealersLabelModel.dealersNearbyLabel = dealersLabelItem.Fields[DealersTemplate.DealersLabelTemplate.DealersNearbyLabelFieldId].Value;
            dealersLabelModel.resultsLabel = dealersLabelItem.Fields[DealersTemplate.DealersLabelTemplate.ResultsLabelFieldId].Value;
            dealersLabelModel.buttonLabel = dealersLabelItem.Fields[DealersTemplate.DealersLabelTemplate.ButtonLabelFieldId].Value;
            dealersLabelModel.overlayHeading = dealersLabelItem.Fields[DealersTemplate.DealersLabelTemplate.OverlayHeadingFieldId].Value;

            dealerDetailsModel.labels = dealersLabelModel;

            dealerDetailsModel.details = new List<DealerDetails>();
            var allDealerFolderItem = Sitecore.Context.Database.GetItem(DealersTemplate.AllDealersFolderId);
            foreach (Sitecore.Data.Items.Item dealerItem in allDealerFolderItem.GetChildren())
            {
                if(dealerItem != null)
                {
                    string dealerItemStateId = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.StateIDFieldId].Value;
                    string dealerItemCityId = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.CityIDFieldId].Value;
                    string dealerItemAreaId = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.AreaIDFieldId].Value;

                    string dealerItemState = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.StateFieldId].Value.ToLower();
                    dealerItemState = dealerItemState.Replace(" ", "");
                    string dealerItemCity = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.CityFieldId].Value.ToLower();
                    dealerItemCity = dealerItemCity.Replace(" ", "");
                    string dealerItemArea = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.AreaFieldId].Value.ToLower();
                    dealerItemArea = dealerItemArea.Replace(" ", "");


                    bool stateMatch = dealerItemState.Equals(stateName);
                    bool cityMatch = string.IsNullOrEmpty(cityName) || dealerItemCity.Equals(cityName);
                    bool areaMatch = string.IsNullOrEmpty(areaName) || dealerItemArea.Equals(areaName);

                    if (stateMatch && cityMatch && areaMatch)
                    {
                        var dealerDetails = new DealerDetails();
                        dealerDetails.icon = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.IconFieldId].Value;
                        dealerDetails.imageAlt = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.ImageAltFieldId].Value;
                        dealerDetails.name = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.NameFieldId].Value;
                        dealerDetails.contact = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.ContactFieldId].Value;
                        dealerDetails.pincode = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.PinCodeFieldId].Value;
                        dealerDetails.organisation = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.OrganizationFieldId].Value;
                        dealerDetails.stateId = dealerItemStateId;
                        dealerDetails.state = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.StateFieldId].Value;
                        dealerDetails.cityId = dealerItemCityId;
                        dealerDetails.city = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.CityFieldId].Value;
                        dealerDetails.areaId = dealerItemAreaId;
                        dealerDetails.area = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.AreaFieldId].Value;
                        dealerDetails.address = dealerItem.Fields[DealersTemplate.DealersDetailsTemplate.AddressFieldId].Value;
                        dealerDetailsModel.details.Add(dealerDetails);
                    }
                }
            }

            return Json(dealerDetailsModel, JsonRequestBehavior.AllowGet);
        }
    }
}