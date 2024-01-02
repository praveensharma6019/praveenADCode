using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class LocationRootResolverService : ILocationRootResolverService
    {
        
        private readonly ILogRepository _logRepository;
        public LocationRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public LocationData GetLocationDataList(Rendering rendering)
        {
            LocationData locationDataList = new LocationData();
            try
            {

                locationDataList.desc = GetLocationData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" LocationDataRootResolverService GetLocationDataList gives -> " + ex.Message);
            }

            return locationDataList;
        }

        public List<Object> GetLocationData(Rendering rendering)
        {
            List<Object> locationDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                LocationDataItem locationdata;
                if (datasource.TemplateID == Templates.LocationFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.Location.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Sitecore.Data.Items.Item item in children)
                        {
                            locationdata = new LocationDataItem();

                            locationdata.Description = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";

                            locationDataList.Add(locationdata);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" LocationDataService LocationDataList gives -> " + ex.Message);
            }

            return locationDataList;
        }

    }
}