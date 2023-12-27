using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class CartFNBservice : ICartFNBservice
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;


        public CartFNBservice(ILogRepository logRepository, IHelper helper, IWidgetService widgetservice)
        {
            _logRepository = logRepository;
            _helper = helper;
            _widgetservice = widgetservice;
        }

        public WidgetModel GetCartFNBAPIData(Rendering rendering, string isApp)
        {

            WidgetModel FNBcartData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                FNBcartData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                FNBcartData.widget.widgetItems = ParseCartFNBApidata(rendering,  isApp);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetCartFNBAPIData gives -> " + ex.Message);
            }


            return FNBcartData;
        }


        private List<object> ParseCartFNBApidata(Rendering rendering, string isApp)
        {
            List<Object> CartFNBData = new List<Object>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? RenderingContext.Current.Rendering.Item
               : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Info("Datasource not selected");
                }
                else
                {
                    string Temp = Cart.CartFNBTemplateID.ToString();

                    List<Item> cartFNB_list = new List<Item>();
                    if (datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList() != null)
                    {
                        cartFNB_list = datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList();
                    }

                    CartFNB FNBcartApiModel = null;

                    foreach (Sitecore.Data.Items.Item cartFNBItem in cartFNB_list)
                    {
                        FNBcartApiModel = new CartFNB();
                        FNBcartApiModel.Title = cartFNBItem.Fields[Cart.Title] != null ? cartFNBItem.Fields[Cart.Title].Value : string.Empty;
                        FNBcartApiModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)cartFNBItem.Fields[Cart.ImageSrc]);
                        FNBcartApiModel.Code = cartFNBItem.Fields[Cart.Code] != null ? cartFNBItem.Fields[Cart.Code].Value : string.Empty;

                        if ((Sitecore.Data.Fields.ImageField)cartFNBItem.Fields[Cart.MobileImage] != null)
                            FNBcartApiModel.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)cartFNBItem.Fields[Cart.MobileImage]);

                        if (isApp == "true")
                        {
                            if ((Sitecore.Data.Fields.ImageField)cartFNBItem.Fields[Cart.MobileImage] != null)
                            { FNBcartApiModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)cartFNBItem.Fields[Cart.MobileImage]); }
                            else { FNBcartApiModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)cartFNBItem.Fields[Cart.ImageSrc]); }
                        }

                        CartFNBData.Add(FNBcartApiModel);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ParseCartFNBApidata gives -> " + ex.Message);
            }

            return CartFNBData;
        }

       
        
    }
}