using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService
{
    public class AboutAdaniHomeContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly INavigationRootResolver RootResolver;

        public AboutAdaniHomeContentResolver(INavigationRootResolver rootResolver)
        {
            RootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var root = RootResolver.AboutAdaniHouse(this.GetContextItem(rendering, renderingConfig));
     
            return new
            {
                heading = root.Fields[AboutAdaniHouse.Fields.headingFieldName] != null ? root.Fields[AboutAdaniHouse.Fields.headingFieldName].Value : "",
                about = root.Fields[AboutAdaniHouse.Fields.aboutFieldName] != null ? root.Fields[AboutAdaniHouse.Fields.aboutFieldName].Value : "",
                readMore = root.Fields[AboutAdaniHouse.Fields.readMoreFieldName] != null ? root.Fields[AboutAdaniHouse.Fields.readMoreFieldName].Value : "",
                Links = Helper.GetLinkURL(root, AboutAdaniHouse.Fields.LinkFieldName) != null ?
                                                     Helper.GetLinkURL(root, AboutAdaniHouse.Fields.LinkFieldName) : "",
                terms = root.Fields[AboutAdaniHouse.Fields.termsFieldName] != null ? root.Fields[AboutAdaniHouse.Fields.termsFieldName].Value : "",
                detailLink = root.Fields[AboutAdaniHouse.Fields.detailLinkFieldName] != null ? root.Fields[AboutAdaniHouse.Fields.detailLinkFieldName].Value : "",
                extrCharges = root.Fields[AboutAdaniHouse.Fields.extrChargesFieldName] != null ? root.Fields[AboutAdaniHouse.Fields.extrChargesFieldName].Value : "",

                readMoreLabel = root.Fields[AboutAdaniHouse.Fields.ReadMore] != null ? root.Fields[AboutAdaniHouse.Fields.ReadMore].Value : "",
                readLessLabel = root.Fields[AboutAdaniHouse.Fields.ReadLess] != null ? root.Fields[AboutAdaniHouse.Fields.ReadLess].Value : "",
                ImageDisclaimer = root.Fields[AboutAdaniHouse.Fields.ImageDisclaimer] != null ? root.Fields[AboutAdaniHouse.Fields.ImageDisclaimer].Value : "",
                EmiDisclaimer = root.Fields[AboutAdaniHouse.Fields.EmiDisclaimer] != null ? root.Fields[AboutAdaniHouse.Fields.EmiDisclaimer].Value : "",
                description = !string.IsNullOrEmpty(root.Fields[AboutAdaniHouse.Fields.description].Value.ToString()) ? Convert.ToString(root.Fields[AboutAdaniHouse.Fields.description].Value.Trim()) : "",
                headingDisclaimer = !string.IsNullOrEmpty(root.Fields[AboutAdaniHouse.Fields.headingDisclaimer].Value.ToString()) ? Convert.ToString(root.Fields[AboutAdaniHouse.Fields.headingDisclaimer].Value.Trim()) : "",
            };
        }
    }
}