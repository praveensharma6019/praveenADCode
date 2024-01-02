using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.AboutUsPage;
using Project.AmbujaCement.Website.Models.DealerLocatorPage;
using Project.AmbujaCement.Website.Models.ProductList;
using Sitecore.Mvc.Presentation;
using System;
using Sitecore.Data.Items;
using System.Linq;
using Project.AmbujaCement.Website.Templates;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Services.AboutUs
{
    public class DealerLocatorService : IDealerLocatorListService
    {
        private readonly IMvcContext _mvcContext;
        public DealerLocatorService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public DealerLocatorDataModel GetDealerLocatorData(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getModel = _mvcContext.GetDataSourceItem<DealerLocatorDataModel>();
                return getModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DealerLocatorDetailsModel GetDealerDetailResponse(Rendering rendering)

        {
            List<DealerLocatorDetailsModel> commonOption = new List<DealerLocatorDetailsModel>();
            DealerLocatorDetailsModel dealerLocatorDetails = new DealerLocatorDetailsModel();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            var dataType = Utils.GetValue(datasource, BaseTemplates.DealerDetailsTemplate.Type);
            try
            {
                if (dataType != null)
                {
                    List<LocatorDetails> locatorDetails = new List<LocatorDetails>();
                    if (dataType == "state" || dataType == "State")
                    {

                        LocatorDetails locator = new LocatorDetails();
                        locator.Id = Utils.GetValue(datasource, BaseTemplates.DealerDetailsTemplate.Id);
                        locator.Label = Utils.GetValue(datasource, BaseTemplates.DealerDetailsTemplate.Label);
                        locator.Type = Utils.GetValue(datasource, BaseTemplates.DealerDetailsTemplate.Type);
                        locatorDetails.Add(locator);
                    }
                    if (dataType == "city" || dataType == "City")
                    {
                        var dataParent = datasource.Parent;
                        LocatorDetails locator = new LocatorDetails();
                        Item areas = null;
                        locator.Id = Utils.GetValue(dataParent, BaseTemplates.DealerDetailsTemplate.Id);
                        locator.Label = Utils.GetValue(dataParent, BaseTemplates.DealerDetailsTemplate.Label);
                        locator.Type = Utils.GetValue(dataParent, BaseTemplates.DealerDetailsTemplate.Type);
                        locator.cityOptions = GetCityOptions(datasource, areas);
                        locatorDetails.Add(locator);
                    }
                    if (dataType == "area" || dataType == "Area")
                    {
                        var dataCity = datasource.Parent;
                        var dataState = dataCity.Parent;
                        LocatorDetails locator = new LocatorDetails();
                        locator.Id = Utils.GetValue(dataState, BaseTemplates.DealerDetailsTemplate.Id);
                        locator.Label = Utils.GetValue(dataState, BaseTemplates.DealerDetailsTemplate.Label);
                        locator.Type = Utils.GetValue(dataState, BaseTemplates.DealerDetailsTemplate.Type);
                        locator.cityOptions = GetCityOptions(dataCity, datasource);
                        //locator.cityOptions = GetAreaOptions(datasource);
                        locatorDetails.Add(locator);
                    }
                    dealerLocatorDetails.details = locatorDetails;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dealerLocatorDetails;
        }
        private List<city> GetCityOptions(Item item, Item area)
        {
            List<city> citylist = new List<city>();
            List<area> arealist = new List<area>();
            
            if (item != null)
            {
                if (area != null)
                {
                    arealist.Add(new area
                    {
                        Id = Utils.GetValue(area, BaseTemplates.DealerDetailsTemplate.Id),
                        Label = Utils.GetValue(area, BaseTemplates.DealerDetailsTemplate.Label),
                        Type = Utils.GetValue(area, BaseTemplates.DealerDetailsTemplate.Type)
                    });
                }
                citylist.Add(new city
                {
                    Id = Utils.GetValue(item, BaseTemplates.DealerDetailsTemplate.Id),
                    Label = Utils.GetValue(item, BaseTemplates.DealerDetailsTemplate.Label),
                    Type = Utils.GetValue(item, BaseTemplates.DealerDetailsTemplate.Type),
                    areaOptions = arealist
                });
                //if (area != null)
                //{
                //    var arealist = new List<area>();
                //    arealist.Add(new area
                //    {
                //        Id = Utils.GetValue(area, BaseTemplates.DealerDetailsTemplate.Id),
                //        Label = Utils.GetValue(area, BaseTemplates.DealerDetailsTemplate.Label),
                //        Type = Utils.GetValue(area, BaseTemplates.DealerDetailsTemplate.Type)
                //    });
                //    citylist
                //}
                return citylist;
            }
            return null;
        }
        private List<area> GetAreaOptions(Item item)
        {
            if (item != null)
            {
                var list = new List<area>();
                list.Add(new area
                {
                    Id = Utils.GetValue(item, BaseTemplates.DealerDetailsTemplate.Id),
                    Label = Utils.GetValue(item, BaseTemplates.DealerDetailsTemplate.Label),
                    Type = Utils.GetValue(item, BaseTemplates.DealerDetailsTemplate.Type)
                });
                return list;
            }
            return null;
        }

    }
}