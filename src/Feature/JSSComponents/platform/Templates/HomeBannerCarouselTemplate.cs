using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Templates
{
    public static class HomeBannerCarouselTemplate
    {

        public static readonly ID TemplateID = new ID("{B0CC9688-FE1D-421B-A6E1-E45D50049C73}");

        public static class Fields
        {
            public static readonly ID BannerGalleryList = new ID("{E5DC77D9-D3CB-4829-8D57-D584E7EB1CBA}");
        }

        public static class CarouselItem
        {
            public static class Fields
            {
                public static readonly ID isOverlayRequired = new ID("{9DED5876-924B-4E78-A982-DED6E0B6ECB2}");

            }
        }
    }
}
