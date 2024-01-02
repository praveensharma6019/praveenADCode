using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System.Linq;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService
{
    public class TownshipContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly INavigationRootResolver RootResolver;

        public TownshipContentResolver(INavigationRootResolver rootResolver)
        {
            RootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var root = RootResolver.GetPropertyDetails(this.GetContextItem(rendering, renderingConfig));
            int y = 0;

            return new
            {
                items = root.Select(x => new
                {
                    index = y++,
                    src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, Township.Fields.ImageFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, Township.Fields.ImageFieldName) : "",
                    mobileimage = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, Township.Fields.mobileimageFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, Township.Fields.mobileimageFieldName) : "",
                    alt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(x, Township.Fields.ImageFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(x, Township.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                    imgLabel = x.Fields[Township.Fields.readmoreID].Value != null ? x.Fields[Township.Fields.readmoreID].Value : "",
                    title = x.Fields[Township.Fields.TitleFieldName].Value != null ? x.Fields[Township.Fields.TitleFieldName].Value : "",
                    desc = x.Fields[Township.Fields.SummaryFieldName].Value != null ? x.Fields[Township.Fields.SummaryFieldName].Value : "",
                    btnText = x.Fields[Township.Fields.BTNTextFieldName].Value != null ? x.Fields[Township.Fields.BTNTextFieldName].Value : "",
                    btnLink = x.Fields[Township.Fields.Link] != null ? Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(x, Township.Fields.LinkName) : ""
                })
            };
        }
    }
}