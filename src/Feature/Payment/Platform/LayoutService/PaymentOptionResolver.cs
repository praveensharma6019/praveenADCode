using Adani.SuperApp.Airport.Feature.Payment.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Payment.Platform.LayoutService
{
    public class PaymentOptionContentResolver : RenderingContentsResolver
    {
        private readonly IPaymentOptionService paymentOptionService;

        public PaymentOptionContentResolver(IPaymentOptionService paymentOptionService)
        {
            this.paymentOptionService = paymentOptionService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string Type = string.Empty;
            string ChannelType= string.Empty;
            var dataSourceItem = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            string url = HttpContext.Current.Request.Url.ToString();
            if(!string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["type"]))
            {
                Type = Sitecore.Context.Request.QueryString["type"].ToLower();
            }
            if(!string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["channeltype"]))
            {
                ChannelType = Sitecore.Context.Request.QueryString["channeltype"].ToLower();
            }
            
            if (dataSourceItem == null)
            {
                throw new NullReferenceException();
            }
            
            return this.paymentOptionService.GetPaymentOptions(dataSourceItem, Type, ChannelType, url);
        }
    }
}