using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.LayoutService
{
    public class ContactUsFormContentResolver : RenderingContentsResolver
    {
        private readonly IContactUsFormService _contactUsFormService;

        public ContactUsFormContentResolver(IContactUsFormService contactUsFormService)
        {
            this._contactUsFormService = contactUsFormService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var dataSourceItem = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;

            if (dataSourceItem == null)
            {
                throw new NullReferenceException();
            }
            return this._contactUsFormService.GetContactUsForm(dataSourceItem);
        }
    }
}