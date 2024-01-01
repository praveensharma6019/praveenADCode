using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website
{
    public static class BaseTemplates
    {
        public static class VideoGalleryTemplate
        {
            public static ID VideoTitle = ID.Parse("{B215B6B7-52F1-431B-8F25-48608144ACE3}");
            public static ID VideoDescription = ID.Parse("{BD8DFC10-158A-48B0-991D-C684712C2D6D}");
            public static ID VideoCategory = ID.Parse("{081A1DA8-2327-40EB-89AA-1E78FC0DCCA4}");
            public static ID VideoSubCategory = ID.Parse("{C34FFB15-8B6F-45FC-A9F5-B9BF977262E4}");
            public static ID VideoThumbnail = ID.Parse("{1759FFA1-EB79-4423-9CA8-0443471BF391}");
            public static ID VideoUrl = ID.Parse("{999FF9C7-AF53-411A-950E-6BBEFD54024A}");
            public static ID YoutubeVideoLink = ID.Parse("{A84C22D9-F09D-4D5B-92D5-22D1A7548BF1}");

            public static ID GalleryTitle = ID.Parse("{D889EF1A-FE50-48E6-8A0E-5AC15EE7625D}");
            public static ID GalleryBanner = ID.Parse("{4625CB34-46B7-4521-9879-66347C90D63F}");
            public static ID GalleryMobileBanner = ID.Parse("{98F77D04-A7EE-4952-8C66-0BF5C2B94FEB}");
            public static ID GalleryTabletBanner = ID.Parse("{5C05201E-8FBB-43F6-A0DB-C1DAE00FA41F}");

            public static ID SimilarVideosMultilistID = ID.Parse("{26CAE1B6-04EF-469D-877F-003BF9AB1AE7}");

        }
        public static class BannerWidgetTemplate
        {
            //Banner
            public static ID isAirportSelectNeeded = ID.Parse("{109D51F9-54FC-464A-B6CE-BF06AB5184D0}");
            public static ID isAgePopup = ID.Parse("{06CCBB5E-5311-4142-A7BE-FF5D8D69F848}");

            public static ID Title = ID.Parse("{12FB9EC7-CD3B-4F6E-8A46-4DF438C66F8E}");
            public static ID Description = ID.Parse("{820CE57B-4887-4EE9-A9C9-0C1E66DFE241}");
            public static ID Link = ID.Parse("{63767939-4CC0-4DCA-A92D-EE7B4447C01E}");
            public static ID Image = ID.Parse("{9775B51E-B023-4D05-B8DB-3AF8289FD750}");
            public static ID btnText = ID.Parse("{7647D2C9-301B-4C4F-A25C-198F8B5BCF58}");
            public static ID OfferEligibility = ID.Parse("{48F163A3-6089-4677-96BB-726FA13C510F}");
            public static ID gridNumber = ID.Parse("{0D6D4774-B3DA-4575-9DCE-5BC55C96E066}");
            public static ID cardBgColor = ID.Parse("{709562FC-A713-4F7C-BFA8-1FCF471D42A3}");
            public static ID listClass = ID.Parse("{690786B7-9487-45FE-8896-07CD879DC063}");

            public static ID CheckValidity = ID.Parse("{C68C9FAD-1046-42F5-A3F8-F4A9EC834915}");
            public static ID EffectiveTo = ID.Parse("{5BD6E10E-E504-4149-81D1-09EB65435BA8}");
            public static ID EffectiveFrom = ID.Parse("{87FE8BC9-8970-4F6A-91F4-30F8CB9B246A}");

            public static ID BannerLogo = ID.Parse("{BC714310-33E6-494E-8866-222EC45085B8}");

            public static ID SubTitle = ID.Parse("{FBB1D96D-A53B-44DF-91AE-9B760FE57225}");

            public static ID AutoId = ID.Parse("{C0679279-72D8-439B-82D5-C1ABAC4B1DDC}");

            public static ID UniqueId = ID.Parse("{AB5708DB-2667-4E1B-9B0D-74CDE820BFED}");
            public static ID MobileImage = ID.Parse("{697B71DB-17BF-46AE-8644-7B60A9A6E2A8}");

            public static ID BannerCategory = ID.Parse("{08332C30-E903-4D53-8C11-AE7FB06FD8CF}");
            public static ID BusinessUnit = ID.Parse("{6F4C57E8-24CD-46DB-B948-E3AF42BCDE6A}");
            public static ID Category = ID.Parse("{68531E40-5B4D-461B-A0C6-C0F3F3377B94}");
            public static ID FaqCategory = ID.Parse("{0A639378-E3AF-49D4-8C34-15DF66442FE0}");
            public static ID Label = ID.Parse("{CD250B55-9B99-4129-9E61-1883ED05595C}");
            public static ID Source = ID.Parse("{E841A6F2-C8C5-4E7B-A3A3-6DEEFE4008C9}");
            public static ID SubCategory = ID.Parse("{84DC69B7-FB62-4207-B7F2-1C1D138210D4}");
            public static ID Type = ID.Parse("{31930532-D904-4B30-997C-74AB69DA095B}");
            public static ID Event = ID.Parse("{118A113B-4331-497E-8173-30A231672A5D}");


            //Widget
            public static ID widgetId = ID.Parse("{5F88F5A4-7DD5-41C5-A562-03AE9DD2E868}");
            public static ID widgetType = ID.Parse("{1D3EA6CD-9EA8-4552-8890-458C1E776992}");
            public static ID title = ID.Parse("{45706823-6981-497D-8662-E678F6CD46CA}");
            public static ID subTitle = ID.Parse("{3A0DEC7F-BCB3-4C4A-A98E-61C338D7154F}");
            public static ID subItemRadius = ID.Parse("{7A394604-62CF-44A1-9B57-B99AE3FDAFB4}");
            public static ID subItemWidth = ID.Parse("{2EACA977-11C7-445A-921A-8D7E9342BDA2}");
            public static ID gridColumn = ID.Parse("{8624C5E7-BB93-4BA5-961A-6074A39CFB42}");
            public static ID aspectRatio = ID.Parse("{5C907F2A-40BA-4AE2-8564-66660459E46C}");
            public static ID borderRadius = ID.Parse("{F9AE0EA0-FF71-4D38-BCE3-DD0C00316F87}");
            public static ID backgroundColor = ID.Parse("{DC284427-0FEF-40A0-A5AD-F57FA4FBDE63}");

        }

        public static class DatasourcesID
        {
            public static ID domesticFlightFolder = ID.Parse("{3FDC2D71-8830-489C-86CE-53800018E124}");
            public static ID internationalFlightFolder = ID.Parse("{10D89E8A-0EBD-49BE-9B5B-556CDD3B364B}");
        }
        public static class CityToCityBannerWidgetTemplate
        {
            public static ID Title = ID.Parse("{1453A937-CFC5-41BC-B771-129F3325256F}");
            public static ID Description = ID.Parse("{68CEDB2B-76D4-4C32-8F0F-8FA102015A8D}");
            public static ID url = ID.Parse("{DE56D6D8-2D33-4474-98E4-6AA7C53D054C}");
            public static ID urlTarget = ID.Parse("{E8E83F96-8F35-47AF-8B17-79B9A875E56E}");
            public static ID urlName = ID.Parse("{04A9E614-4DEE-42F8-92C6-156776868EE9}");
            public static ID image = ID.Parse("{A59BFAAE-6AF0-4328-BF66-C66D9C06E635}");
       
        }

        public static class CityToCityDetails
        {
            public static ID CityName = ID.Parse("{27777497-5C2D-4C32-BED1-5B5D1E03FF51}");
            public static ID CityImage = ID.Parse("{1C100563-C74C-4123-801B-2761D6866EB9}");
            public static ID AirportName = ID.Parse("{FA4E02B9-115B-487D-8E25-26A21E354AAC}");
            public static ID AirportDescription = ID.Parse("{A733F246-7E95-4AB8-AC99-ED69881BF465}");
            public static ID AboutAirportHeading = ID.Parse("{0555E2A4-C529-4B5A-9751-4726317D8D66}");
            public static ID PlacesToVisitHeading = ID.Parse("{A3F82A03-93B1-44C4-8EEA-888F8F7F3D37}");
            public static ID AboutAirportDescription = ID.Parse("{412B2760-61DD-455B-81CD-3787907776B5}");
            public static ID PlacesToVisitInCity = ID.Parse("{F0CF3380-DCBE-4CF7-BF4A-9E1ABD45C20A}");
            public static ID BestTimeToVisitHeading = ID.Parse("{9969BDA9-BC53-4787-8C2E-0F00BA2C7EA1}");
            public static ID BestTimeToVisitDescription = ID.Parse("{E7C992DF-7AEA-4BF1-9C22-7F12E9A7D669}");
            public static ID AirportCityCode = ID.Parse("{91E58377-803F-4947-84AB-C64A093E0274}");
            public static ID AirportCityAdddress = ID.Parse("{F62EE2B9-D2E3-47DC-BB53-FE902F235D81}");
            public static ID CityType = ID.Parse("{6DC461E5-B1E8-44F6-9A7A-C0E3DAD443D5}");

            public class AirportInformationFields
            {
                public static ID Title = ID.Parse("{C4D34DF7-706D-4E8F-A574-C3F49B56D21F}");
                public static ID ToCity = ID.Parse("{581716E9-3E87-4FC4-BA24-0AB5AB984697}");
                public static ID ToCityDescription = ID.Parse("{4E58C7C4-F8DF-4649-B03A-BB8554D20AFE}");
                public static ID FromCity = ID.Parse("{7FBFB696-4DE7-4C84-A578-2C9C982662FB}");
                public static ID FromCityDescription = ID.Parse("{864F4FFD-26FE-4326-B9CE-5D651DA43E82}");
            }
        }

        public static class PlaceToVisitDetails
        {
            public static ID PlaceName = ID.Parse("{F4373AFA-1400-4573-9B91-539709F011E9}");
            public static ID PlaceLink = ID.Parse("{35456F83-4414-4FE6-AC87-58930868DB15}");
            public static ID LocationIcon = ID.Parse("{97D2DBE7-0AB5-4D72-AC8E-030B9D1C7A4B}");
        }
    }
}
