using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class ServiceTypePackagesRepository : IServiceTypePackages
    {
        private readonly ILogRepository _logRepository;
        
        private readonly IHelper _helper;
        public ServiceTypePackagesRepository(ILogRepository logRepository,  IHelper helper)
        {
           
            this._logRepository = logRepository;
            this._helper = helper;
        }
        public PackageCard GetServiceTypePackage(string serviceTypeId, string travelSectorId, string airportCode)
        {
            PackageCard _obj = new PackageCard();
            try
            {
                Sitecore.Data.Items.Item datasourceItem = Sitecore.Context.Database.GetItem(Templates.PranaamAirportItem.PranaamAirportItemId);
                if (datasourceItem != null)
                {
                    var airport = datasourceItem.GetChildren().FirstOrDefault(x => x.Fields["AirportCode"].Value == airportCode);
                    var servicetype = airport != null ? airport.Children.FirstOrDefault(x => x.Fields["Id"].Value == serviceTypeId) : null;
                    var travelsector = servicetype != null ? servicetype.Children.FirstOrDefault(x => x.Fields["TravelSectorId"].Value == travelSectorId) : null;
                    var packages = travelsector != null ? travelsector.GetChildren() : null;
                    if(packages.Count > 0 && packages != null)
                    {
                        List<PackageItems> pckgList = new List<PackageItems>();
                        foreach(var item in packages.InnerChildren)
                        {
                            PackageItems pckgItem = new PackageItems();
                            pckgItem.PackageId = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.Id].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.Id].Value.ToString() : string.Empty;
                            pckgItem.PackageTitle = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.Title].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.Title].Value.ToString() : string.Empty;
                            pckgItem.IsRecommended = _helper.GetCheckboxOption(item.Fields[Templates.PranaamPackages.Fields.IsRecommended]);
                            pckgItem.alt = _helper.GetImageAlt(item, Templates.PranaamPackages.Fields.StanderedImage.ToString());
                            pckgItem.src = _helper.GetImageURL(item, Templates.PranaamPackages.Fields.StanderedImage.ToString());
                            pckgItem.cardDesc = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.Description].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.Description].Value.ToString() : string.Empty;
                            pckgItem.finalPrice = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.NewPrice].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.NewPrice].Value.ToString() : string.Empty;
                            pckgItem.btnText = _helper.GetLinkText(item, Templates.PranaamPackages.Fields.CTA.ToString());
                            pckgItem.btnUrl = _helper.GetLinkURL(item, Templates.PranaamPackages.Fields.CTA.ToString());
                            pckgItem.btnVariant = _helper.GetDropLinkValue(item.Fields[Templates.PranaamPackages.Fields.ButtonVariant]);
                            pckgList.Add(pckgItem);
                        }
                        _obj.items = pckgList;
                    }
                }
                else return null;
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetServiceTypePackage throws Exception -> " + ex.Message);
            }
            return _obj;
        }
    }
}