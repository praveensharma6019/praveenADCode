using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System.Linq;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService
{
    public class SocialClubsContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly INavigationRootResolver RootResolver;

        public SocialClubsContentResolver(INavigationRootResolver rootResolver)
        {
            RootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var root = RootResolver.GetPropertyDetails(this.GetContextItem(rendering, renderingConfig));

            return new
            {
                items = root.Select(x => new
                {
                    title = x.Fields[SocialClubs.Fields.TitleFieldName].Value != null ? x.Fields[SocialClubs.Fields.TitleFieldName].Value : "",
                    src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, SocialClubs.Fields.thumbFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, SocialClubs.Fields.thumbFieldName) : "",
                    mobileimage = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, SocialClubs.Fields.mobileimageFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, SocialClubs.Fields.mobileimageFieldName) : "",
                    imgalt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(x, SocialClubs.Fields.thumbFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(x, SocialClubs.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                    imgtitle = x.Fields[SocialClubs.Fields.imgtype].Value != null ? x.Fields[SocialClubs.Fields.imgtype].Value : "",
                    imgLabel = x.Fields[SocialClubs.Fields.readmoreID].Value != null ? x.Fields[SocialClubs.Fields.readmoreID].Value : "",
                    heading = x.Fields[SocialClubs.Fields.headingFieldName].Value != null ? x.Fields[SocialClubs.Fields.headingFieldName].Value : "",
                    subheading = x.Fields[SocialClubs.Fields.subheadingFieldName].Value != null ? x.Fields[SocialClubs.Fields.subheadingFieldName].Value : "",
                    ctatitle = x.Fields[SocialClubs.Fields.CTA_TitleFieldName].Value != null ? x.Fields[SocialClubs.Fields.CTA_TitleFieldName].Value : "",
                    link = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(x, SocialClubs.Fields.linkName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(x, SocialClubs.Fields.linkName) : "",
                })
            };
        }
    }
}