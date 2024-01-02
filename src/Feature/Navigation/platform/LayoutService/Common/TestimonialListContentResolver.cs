using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Globalization;
using System.Linq;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService
{
    public class TestimonialListContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly INavigationRootResolver RootResolver;

        public TestimonialListContentResolver(INavigationRootResolver rootResolver)
        {
            RootResolver = rootResolver;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var root = RootResolver.GetTestimonialList(this.GetContextItem(rendering, renderingConfig));

            return new
            {
                data = root.Select(x => new
                {
                    isVideoTestimonial = x.Fields[TestimonialList.Fields.isVideoFieldName].Value != null && x.Fields[TestimonialList.Fields.isVideoFieldName].Value.ToString() == "1" ? "true" :
                                        x.Fields[TestimonialList.Fields.isVideoFieldName].Value != null && x.Fields[TestimonialList.Fields.isVideoFieldName].Value.ToString() == "0" ? "false" : "false",
                    useravtar = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, TestimonialList.Fields.ImageFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(x, TestimonialList.Fields.ImageFieldName) : "",
                    useravtaralt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(x, TestimonialList.Fields.ImageFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(x, TestimonialList.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                    useravtartitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(x, TestimonialList.Fields.ImageFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(x, TestimonialList.Fields.ImageFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "",
                    title = x.Fields[TestimonialList.Fields.TitleFieldName].Value != null ? x.Fields[TestimonialList.Fields.TitleFieldName].Value : "",
                    description = x.Fields[TestimonialList.Fields.SummaryFieldName].Value != null ? x.Fields[TestimonialList.Fields.SummaryFieldName].Value : "",
                    author = x.Fields[TestimonialList.Fields.AuthorieldName].Value != null ? x.Fields[TestimonialList.Fields.AuthorieldName].Value : "",
                    propertylocation = x.Fields[TestimonialList.Fields.Property_locationFieldName].Value != null ? x.Fields[TestimonialList.Fields.Property_locationFieldName].Value : "",
                    iframesrc = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(x, TestimonialList.Fields.VideoMp4FieldName) != null ?
                               Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(x, TestimonialList.Fields.VideoMp4FieldName) : "",
                    SEOName = x.Fields[TestimonialList.Fields.SEOName].Value != null ? x.Fields[TestimonialList.Fields.SEOName].Value : "",
                    SEODescription = x.Fields[TestimonialList.Fields.SEODescription].Value != null ? x.Fields[TestimonialList.Fields.SEODescription].Value : "",
                    UploadDate = x.Fields[TestimonialList.Fields.UploadDate].Value != null ? x.Fields[TestimonialList.Fields.UploadDate].Value : "",
                })

            };
        }
    }
}