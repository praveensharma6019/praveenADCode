using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform;
using Sitecore.Mvc.Presentation;
using Adani.BAU.Transmission.Foundation.Theming.Platform.Services;
using Adani.BAU.Transmission.Foundation.Logging.Platform.Repositories;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Services
{
    public class DutyFreeHeader : IDutyFreeHeader
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetService;
        private readonly IHelper _helper;
        public DutyFreeHeader(ILogRepository logRepository, IWidgetService widgetService, IHelper helper)
        {
            this._widgetService = widgetService;
            this._logRepository = logRepository;
            this._helper = helper;
        }
        public DFHeaderlwidgets GetDutyFreeHeader(Rendering rendering, bool restricted = false)
        {
            DFHeaderlwidgets dFHeaderlwidgets = new DFHeaderlwidgets();
            try
            {
                _logRepository.Info("GetDutyFreeHeader Data Parsing Started");
                Item widget = null;

                IDictionary<string, string> paramDictionary = rendering.Parameters.ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (string key in paramDictionary.Keys.ToList())
                {
                    if (Sitecore.Data.ID.TryParse(paramDictionary[key], out var itemId))
                    {
                        widget = rendering.RenderingItem.Database.GetItem(itemId);
                    }
                }
                if (widget != null)
                {
                    dFHeaderlwidgets.widget = _widgetService.GetWidgetItem(widget);
                }
                else
                {
                    dFHeaderlwidgets.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                dFHeaderlwidgets.widget.widgetItems = GetDutyFreeHeaderList(rendering, restricted);
                _logRepository.Info("GetDutyFreeHeader Data Parsing Ended");
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetDutyFreeHeader gives -> " + ex.Message);
            }

            return dFHeaderlwidgets;
        }

        /// <summary>
        /// Implementation to get the header data
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        private List<Object> GetDutyFreeHeaderList(Rendering rendering, bool restricted)
        {
            List<Object> materialGroupsList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Info("Datasource not selected");
                }
                else
                {
                    var dataItems = restricted ? datasource.Children.Where(x => x.Fields["Age Restricted"].Value != "1").ToList() : datasource.Children.ToList();
                    MaterialGroup materialGroup;
                    foreach (Sitecore.Data.Items.Item item in dataItems)
                    {
                        if (item.TemplateID.Guid == Constant.MaterialGroupTemplate)
                        {
                            materialGroup = new MaterialGroup();
                            materialGroup.title = !string.IsNullOrEmpty(item.Fields[Constant.Title].Value.ToString()) ? item.Fields[Constant.Title].Value.ToString() : "";
                            materialGroup.code = !string.IsNullOrEmpty(item.Fields[Constant.MaterialGroupCode].Value.ToString()) ? item.Fields[Constant.MaterialGroupCode].Value.ToString() : "";
                            materialGroup.imageSrc = _helper.GetImageURL(item, Constant.Image);
                            materialGroup.linkUrl = _helper.GetLinkURL(item, Constant.Link);
                            materialGroup.cdnPath = !string.IsNullOrEmpty(item.Fields[Constant.CDNPath].Value.ToString()) ? item.Fields[Constant.CDNPath].Value.ToString() : "";
                            materialGroup.restricted = (materialGroup.code.ToLower().Trim().Equals(Constant.restrictedGroup)) ? true : false;
                            materialGroup.materialGroup = materialGroup.code;
                            materialGroup.sortorder = GetSortOrder(item);

                            #region Category Collection 
                            if (item.HasChildren)
                            {
                                Brand objbrand = null;
                                Category objcat = null;
                                List<Category> objcategory = new List<Category>();
                                foreach (Sitecore.Data.Items.Item category in item.Children)
                                {
                                    if (category.TemplateID.Guid == Constant.CategoryTemplate)
                                    {
                                        objcat = new Category();
                                        objcat.code = !string.IsNullOrEmpty(category.Fields[Constant.Code].Value.ToString()) ? category.Fields[Constant.Code].Value.ToString() : "";
                                        objcat.cdnPath = !string.IsNullOrEmpty(category.Fields[Constant.CDNPath].Value.ToString()) ? category.Fields[Constant.CDNPath].Value.ToString() : "";
                                        objcat.title = !string.IsNullOrEmpty(category.Fields[Constant.Name].Value.ToString()) ? category.Fields[Constant.Name].Value.ToString() : "";
                                        objcat.imageSrc = GetCatagoryImages(category);
                                        objcat.linkUrl = _helper.GetLinkURL(category, Constant.Link);
                                        objcat.category = objcat.code;
                                        objcat.restricted = materialGroup.restricted;

                                        if (!String.IsNullOrEmpty(category.Fields[Constant.Brands].ToString()))
                                        {
                                            Sitecore.Data.Fields.MultilistField multiselectField = category.Fields[Constant.Brands];
                                            List<Brand> objcatbreands = new List<Brand>();
                                            foreach (Sitecore.Data.Items.Item brand in multiselectField.GetItems())
                                            {
                                                objbrand = new Brand();
                                                objbrand.title = !string.IsNullOrEmpty(brand.Fields[Constant.Brand_Name].ToString()) ? _helper.ToTitleCase(brand.Fields[Constant.Brand_Name].ToString()) : "";
                                                objbrand.code = !string.IsNullOrEmpty(brand.Fields[Constant.Brand_Code].ToString()) ? brand.Fields[Constant.Brand_Code].ToString() : "";
                                                objbrand.description = !string.IsNullOrEmpty(brand.Fields[Constant.Brand_Description].ToString()) ? brand.Fields[Constant.Brand_Description].ToString() : "";
                                                objbrand.imageSrc = _helper.GetImageURL(brand, Constant.Image);
                                                objbrand.cdnPath = !string.IsNullOrEmpty(brand.Fields[Constant.Brand_CDN_Image].ToString()) ? brand.Fields[Constant.Brand_CDN_Image].ToString() : "";
                                                objbrand.brand = objbrand.code;
                                                objbrand.restricted = _helper.Sanitize(brand.Fields[Constant.Brand_Material_Group].Value).ToLower().Equals(Constant.restrictedGroup) ? true : false;
                                                objcatbreands.Add(objbrand);
                                            }
                                            objcat.children = objcatbreands;
                                        }

                                        objcategory.Add(objcat);
                                    }

                                }
                                materialGroup.children = objcategory;
                            }
                            #endregion

                            materialGroupsList.Add(materialGroup);
                        }
                        // Brand and Boutique
                        if (item.TemplateID.Guid == Constant.SubCategoryTemplate)
                        {
                            materialGroup = new MaterialGroup();
                            materialGroup.title = !string.IsNullOrEmpty(item.Fields[Constant.Name].Value.ToString()) ? item.Fields[Constant.Name].Value.ToString() : "";
                            materialGroup.code = !string.IsNullOrEmpty(item.Fields[Constant.Code].Value.ToString()) ? item.Fields[Constant.Code].Value.ToString() : "";
                            materialGroup.imageSrc = _helper.GetImageURL(item, Constant.Image);
                            if (!String.IsNullOrEmpty(item.Fields[Constant.Brands].ToString()))
                            {
                                Sitecore.Data.Fields.MultilistField multiselectField = item.Fields[Constant.Brands];
                                Category objLink = null;
                                List<Category> objLinks = new List<Category>();
                                foreach (Sitecore.Data.Items.Item brand in multiselectField.GetItems())
                                {
                                    objLink = new Category();
                                    objLink.title = !string.IsNullOrEmpty(brand.Fields[Constant.Brand_Name].Value.ToString()) ? brand.Fields[Constant.Brand_Name].Value.ToString() : "";
                                    objLink.code = !string.IsNullOrEmpty(brand.Fields[Constant.Brand_Code].ToString()) ? brand.Fields[Constant.Brand_Code].ToString() : "";
                                    objLink.brand = objLink.code;
                                    objLink.restricted = !string.IsNullOrEmpty(brand.Fields[Constant.Brand_Code].ToString()) ? brand.Fields[Constant.Brand_Material_Group].ToString().Trim().ToLower().Equals(Constant.restrictedGroup) : false;
                                    objLinks.Add(objLink);
                                }
                                materialGroup.children = objLinks;
                            }
                            materialGroupsList.Add(materialGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetDutyFreeHeaderList gives error ->" + ex.Message);
            }

            return materialGroupsList;
        }

        private List<string> GetCatagoryImages(Sitecore.Data.Items.Item item)
        {
            List<string> images = new List<string>();
            string imageurl = _helper.GetImageURL(item, Constant.Image);
            if (!string.IsNullOrEmpty(imageurl))
            {
                images.Add(imageurl);
            }
            imageurl = _helper.GetImageURL(item, Constant.MainImage);
            if (!string.IsNullOrEmpty(imageurl))
            {
                images.Add(imageurl);
            }
            imageurl = _helper.GetImageURL(item, Constant.Thumbnailimage);
            if (!string.IsNullOrEmpty(imageurl))
            {
                images.Add(imageurl);
            }
            imageurl = _helper.GetImageURL(item, Constant.IconImages);
            if (!string.IsNullOrEmpty(imageurl))
            {
                images.Add(imageurl);
            }
            return images;
        }

        private int GetSortOrder(Item item)
        {
            int order = 0;
            try
            {
                order = Convert.ToInt32(item.Fields[Constant.SortOrder].Value.ToString());
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetSortOrder gives error ->" + ex.Message);
            }
            return order;
        }
    }
}