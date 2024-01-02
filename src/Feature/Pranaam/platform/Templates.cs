using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam
{
    public static class Templates
    {
        public static class AddOnServiceList {
            public static readonly ID AddOnServiceListTemplateID = new ID ("{D6FCBEC8-D47B-4286-B766-D4BD0FC5155D}");
            public static class Fields
            {
                public static readonly ID AddOnServiceTitle = new ID("{6986BCB6-F0BC-4A70-BB59-FAC587ABB3B5}");
                public static readonly ID AddOnServiceList = new ID("{67B9B975-9ADD-41ED-BAF8-C3DD14FA6311}");
            }
        }

        public static class AddOnService
        {
            public static readonly ID AddOnServiceTemplateID = new ID("{8A4342FE-6910-4DF6-9794-6E1606A621B4}");
            public static class Fields
            {
                public static readonly ID Id = new ID("{AA623225-5027-4099-9E45-823490E51676}");
                public static readonly ID Title = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
                public static readonly ID Description = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
                public static readonly ID ServiceImage = new ID("{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}");
                public static readonly ID KnowmoreCTA = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
                public static readonly ID OldPrice = new ID("{85208689-41DC-4CF3-A69E-83F4BA7976A4}");
                public static readonly ID NewPrice = new ID("{1FB91C95-7A41-45A0-AF4B-3FAE7522F11C}");
                public static readonly ID ServiceId = new ID("{AA623225-5027-4099-9E45-823490E51676}");
            }
        }

        public static class DeviceSpecificImage
        {
            public static readonly ID DeviceSpecificTemplateID = new ID("{FECFCC8B-5B30-48C2-BE22-5BBF814065B3}");
            public static class Fields
            {
                public static readonly ID DesktopImage = new ID("{783243BC-982E-4BB1-8145-BABCCEF1E247}");
                public static readonly ID MobileImage = new ID("{8C152BE6-E409-4B42-8AA3-4B130137BE6E}");
                public static readonly ID ThumbnailImage = new ID("{19877ED9-5996-4E1B-894F-02398DEED82C}");
            }
        }

        public static class CustomerFeedback
        {
            public static readonly ID CustomerFeedbackTemplateID = new ID("{DA6013DC-7AC3-455B-8885-BA9A9C145A83}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
                public static readonly ID FeedbackList = new ID("{624B083B-B95F-4B47-A88C-5AF129A42E4F}");
            }
        }

        public static class CustomerFeedbackCard
        {
            public static readonly ID CustomerFeedbackCardTemplateID = new ID("{3FF8FE42-8B22-4D1A-9CBF-B86BBF5842BC}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
                public static readonly ID Decription = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
                public static readonly ID ServiceImage = new ID("{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}");
                public static readonly ID KnowmoreCTA = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
            }
        }

        public static class PorterCard
        {
            public static readonly ID PorterCardTemplateID = new ID("{E91D6988-89A5-44F6-9CF6-41C62C43D070}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
                public static readonly ID Decription = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
                public static readonly ID ServiceImage = new ID("{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}");
                public static readonly ID KnowmoreCTA = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
                public static readonly ID OldPrice = new ID("{85208689-41DC-4CF3-A69E-83F4BA7976A4}");
                public static readonly ID NewPrice = new ID("{1FB91C95-7A41-45A0-AF4B-3FAE7522F11C}");
            }
        }

        public static class HowItWorks
        {
            public static readonly ID HowItWorksTemplateID = new ID("{FD3180FD-71D1-4DC6-987F-1C82A568F925}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
                public static readonly ID MP4Video = new ID("{5803CAFB-B210-4353-90D3-B8A4EB843054}");
                public static readonly ID OGGVideo = new ID("{73810B5A-F334-4889-8B5E-C7EBDDC9B075}");
                public static readonly ID ThumbnailImage = new ID("{2AC8CF16-DE30-49BC-A284-7A2B95C44566}");
                public static readonly ID Description = new ID("{44938BE6-A54E-4EF4-A8AB-1575A87EFA8F}");
                public static readonly ID VideoDate = new ID("{39BB2447-3C2D-4A88-95AC-5566FE6BC16B}");
                public static readonly ID VideoName = new ID("{3709EC01-6B52-4633-B4FB-278D4E7A7825}");
            }
        }

        public static class BookingSteps
        {
            public static readonly ID BookingStepsTemplateID = new ID("{422AB6AC-B497-4960-84D9-EA7A065B0E6B}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
            }
        }
        public static class Steps
        {
            public static readonly ID StepsTemplateID = new ID("{58230F30-C32F-4EF6-9C15-010EE3C79219}");
            public static class Fields
            {
                public static readonly ID BookingImg = new ID("{1B9D553C-8DA6-4F70-87E9-AABB33CACD21}");
                public static readonly ID BookingTitle = new ID("{5C2ADE41-732B-4891-9D7B-C1603F511E70}");
                public static readonly ID BookingValue = new ID("{E80F113E-61A0-493B-892A-15AC73EF5084}");
            }
        }
        
        public static class DepartureParent
        {
            
            public static readonly ID DepartureParentTemplateID = new ID("{A1238ECE-4F06-4DDE-B664-8817FBEC27C5}");
            public static class DepartureParentFields
            {
                public static readonly ID Tabs = new ID("{076CC391-496A-4729-BE69-13B458D3C636}");
            }
        }

        public static class DepartureTab
        {
            public static readonly ID DepartureTabTemplateID = new ID("{12023478-9464-4AB1-9E64-FB5FD579AF47}");
            public static class HomeFields
            {
                public static readonly ID Image = new ID("{49867869-C128-42E8-B64B-DF7E18694E8C}");
                public static readonly ID SmallDescription = new ID("{1644D90E-8769-4850-BC22-FB5410ACC108}");
                public static readonly ID PackageTitle = new ID("{C5E587FD-1846-4D92-A840-C1B2447582F9}");
                public static readonly ID KnowmoreCTA = new ID("{998AD433-C8CD-403C-B026-59D5551AB3F8}");
            }

            public static class TabFields
            {
                public static readonly ID Title = new ID("{1C2964DB-B666-4035-A50F-B03A4FF057D1}");
                public static readonly ID SubTitle = new ID("{A638E50F-5E5E-4C51-99F5-05DC9A693071}");
                public static readonly ID Description = new ID("{963D1472-30D5-472D-A788-365464596563}");
                public static readonly ID DepartureTypes = new ID("{234569C6-27C4-4495-923B-B1B2CF63B90A}");
                public static readonly ID ReadmoreCTA = new ID("{F9AEA75C-E0F5-453D-A480-A1C586F85A10}");
            }
        }

        public static class DepartureType
        {
            
            public static readonly ID DepartureTypeTemplateId = new ID("{9237C158-0029-4E02-86DE-484AD4416042}");
            public static class DepartureTypeFields
            {
                public static readonly ID DepartureTypeTitle = new ID("{520D0A95-8710-4EE9-8D4F-332E9F669191}");
                public static readonly ID DepartureTypeSubTitle = new ID("{12935855-783C-4F36-8C6C-1772F90FF47B}");
                public static readonly ID DepartureTypeDescription = new ID("{EA6FC0DF-A7DC-48A0-8807-2B653FF82346}");
                public static readonly ID TableHeaders = new ID("{1A9BE4D8-5181-4F0C-8345-0EE222E1DF0F}");
                public static readonly ID ChargesList = new ID("{D72A2086-9701-4C48-8AC8-30C16B36C903}");
                public static readonly ID ServiceOfferingsTitle = new ID("{FC113C39-5C38-4E5B-BB0E-D4795CD2195B}");
                public static readonly ID OfferingsList = new ID("{00673091-4049-4ABB-A273-70B2B47FF58B}");
                public static readonly ID AdditionalAddOnsTitle = new ID("{F821E186-D58E-45D2-92BD-879A9D485192}");
                public static readonly ID AdditionalAddOns = new ID("{82CCD2AC-84F2-4296-8CB2-A916FBB52683}");
                public static readonly ID ExpressBookingTitle = new ID("{419316AA-600A-4E0F-86C4-8BEB2C38F02F}");
                public static readonly ID ExpressBooking = new ID("{5D388F0B-FD6D-470C-B67E-671B97C215D6}");

            }
        }

        public static class DepartureCharge
        {
            public static readonly ID DepartureChargeTemplateId = new ID("{E30F49D1-5427-4565-8A14-F95976555190}");
            public static class DepartureChargeFields
            {
                public static readonly ID Guest = new ID("{A7F9E228-B72E-4983-9BE4-09E460DA9519}");
                public static readonly ID GuestCharges = new ID("{258340D1-6BA2-4703-9E98-31FEF57C62CD}");
                public static readonly ID GST = new ID("{CD8FDCBF-6381-499B-B349-EA4308184CD1}");
                public static readonly ID Total = new ID("{49084022-A946-4014-BCDE-B4C4CB2ABCF9}");
            }
        }
        public static class DepartureOfferings
        {
            public static readonly ID DepartureOfferingsTemplateId = new ID("{91D06145-8F8E-4DA8-B8C8-6D011B9D0234}");
            public static class DepartureChargeFields
            {
                public static readonly ID OfferingText = new ID("{44994B06-848C-4D79-BEC4-5C804DB5B857}");
                public static readonly ID Value = new ID("{BA257878-0B49-4883-A8BE-E491F7231E73}");
            }
        }

        public static class DepartureTableHeader
        {
            public static readonly ID DepartTableHeaderTemplateId = new ID("{80F9B2F5-AAAA-44F7-97E6-DDBD7ED9FD92}");
            public static class DepartTableHeaderFields
            {
                public static readonly ID Title = new ID("{30FA2056-BB68-4C9C-8324-30B10D3CA1F0}");
                public static readonly ID Value = new ID("{6BE12DEB-371F-4BA2-90A7-0C532E2DCDBE}");
            }
        }

        public static class DepartureCarousel
        {
            
            public static readonly ID DepartureCarouselTemplateId = new ID("{FE2CDB30-C64C-4859-83F6-CE768461210B}");

            public static class DepartureCarouselFields
            {
                public static readonly ID CarouselTitle = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
                public static readonly ID CarouselDescription = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
                public static readonly ID CarouselImage = new ID("{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}");
                public static readonly ID CarouselCTA = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
            }
        }

        public static class AddOnServiceTab
        {

            public static readonly ID AddOnServiceTabTemplateId = new ID("{31C75214-AC88-4BEC-A2C9-E1E36842563C}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{6986BCB6-F0BC-4A70-BB59-FAC587ABB3B5}");
                public static readonly ID Banner = new ID("{62D69DB8-697F-42D7-9FE8-9C7F3DEAFCE3}");
                public static readonly ID Tabs = new ID("{B2E6288F-54EF-49AA-8775-C6E11F78FBBE}");
                public static readonly ID Text = new ID("{E1106734-F32A-4E92-A5E0-4DFAE2F1DEE3}");
            }
        }
        public static class AddOnServiceTabCategory
        {

            public static readonly ID AddOnServiceTabCategoryTemplateId = new ID("{E44D6C5C-F688-489B-88B4-DA8B3DE84053}");
            public static class Fields
            {
                public static readonly ID CategoryName = new ID("{C31BB233-19B5-4C36-9EFC-41491B8C987E}");
                public static readonly ID ListOfServices = new ID("{85A92BA8-AEE0-45D0-832D-92D5D69CDDD7}");
            }
        }

        public static class PackageServices
        {

            public static readonly ID PackageServicesTemplateId = new ID("{C42D38DF-EECB-4466-B6C4-B00642323C9F}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{B31B17A0-2DA4-40A1-9A11-2D51A26D1A57}");
                public static readonly ID ServicesList = new ID("{5418C37A-6F77-4761-BBB5-218E2F98D418}");
            }
        }

        public static class PackageServicesData
        {

            public static readonly ID PackageServicesDataTemplateId = new ID("{91D06145-8F8E-4DA8-B8C8-6D011B9D0234}");
            public static class Fields
            {
                public static readonly ID OfferingsName = new ID("{BEE77992-D7B8-4EAA-8996-E21C27C9328A}");
                public static readonly ID OfferingsDescription = new ID("{44994B06-848C-4D79-BEC4-5C804DB5B857}");
                public static readonly ID Value = new ID("{BA257878-0B49-4883-A8BE-E491F7231E73}");
            }
        }

        public static class ServicesListCollection
        {
            public static readonly ID ServiceItemTemplateID = new ID("{0B2191D6-78AE-4742-AD47-ADC20306DE48}");
            public static string RenderingParamField = "Widget";
        }
        public static class PackageCard
        {

            public static readonly ID PackageCardTemplateId = new ID("{85BD7A06-FE9B-4D02-86A8-3D07A04BE328}");
            public static class Fields
            {
                public static readonly ID IconImage = new ID("{A66C8E34-D89D-4B89-BF3C-26469CB160EF}");
                public static readonly ID ThumbnailImage = new ID("{F5CB616A-C8D1-4F3A-899D-B7738FDDAC02}");
                public static readonly ID MainImage = new ID("{AB507F1F-3F00-45C0-A070-0F1ECF1CB4B8}");
            }
        }
        public static class DotNetPackageServices
        {
            public static readonly ID DotNetPackageTemplateId = new ID("{B50FB474-C181-4E11-81CA-F0C96FF2FF17}");
            public static class Fields
            {
                public static readonly ID SequenceNumber = new ID("{A68374BB-3CFA-4373-A1EF-BAF174B1851A}");
                public static readonly ID Name = new ID("{1C2964DB-B666-4035-A50F-B03A4FF057D1}");
                public static readonly ID ShortDesc = new ID("{963D1472-30D5-472D-A788-365464596563}");
                public static readonly ID PackageNumber = new ID("{4506B042-13BE-4F69-85AC-B81BFBF58EFE}");
                public static readonly ID PackageId = new ID("{F32657A5-0394-48AD-B15D-CF4044FC9033}");
                public static readonly ID BusinessUnitId = new ID("{5761762F-8B71-4B8E-8A99-27ABB65A768B}");
                public static readonly ID TravelSectorId = new ID("{CB13C636-F76C-41D1-B6CF-1E9190F23759}");
                public static readonly ID AirportMasterId = new ID("{ECEC83FA-9D33-4D08-AC66-CBEC1C4F440A}");
                public static readonly ID ServiceTypeId = new ID("{1B798266-368B-4FE8-B85C-6E19D4A2C584}");
                public static readonly ID PriceToPay = new ID("{F7C22FEA-4259-447D-B74B-414F2EEE2A4D}");
                public static readonly ID AddOnService = new ID("{27DD0E25-B65A-4111-ACA4-AEEDF253E1FF} ");
                public static readonly ID PackageServices = new ID("{69BBA952-6F87-4621-AE53-B3E8AA2BFDD3}");
            } 
        }

        public static class PranaamPackageDatasource
        {
            public static readonly ID PranaamPackageDatasourceTemplateId = new ID("{4EA84B63-F436-46E0-82BB-6CC1D00842BA}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
                public static readonly ID SelectPackages = new ID("{365CED50-4E6D-46A2-A372-85BB51B7F1CD}");
            }
        }

        public static class PranaamPackages
        {

            public static readonly ID PranaamPackagesTemplateId = new ID("{85BD7A06-FE9B-4D02-86A8-3D07A04BE328}");
            public static class Fields
            {
                public static readonly ID Id = new ID("{AA623225-5027-4099-9E45-823490E51676}");
                public static readonly ID Title = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
                public static readonly ID IsRecommended = new ID("{44108317-84B3-40CC-8652-BB93DF0CAC96}");
                public static readonly ID StanderedImage = new ID("{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}");
                public static readonly ID Description = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
                public static readonly ID NewPrice = new ID("{1FB91C95-7A41-45A0-AF4B-3FAE7522F11C}");
                public static readonly ID CTA = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
                public static readonly ID ButtonVariant = new ID("{2F974F4F-239A-42D8-972A-3C74E3DAE147}");
                public static readonly ID ServicesList = new ID("{5418C37A-6F77-4761-BBB5-218E2F98D418}");
                public static readonly ID ServiceTitle = new ID("{B31B17A0-2DA4-40A1-9A11-2D51A26D1A57}");
                public static readonly ID AddOns = new ID("{3521C918-72FB-4C93-8BB6-C4BB83B3FDBA}");
            }
        }

        public static class StandaloneProducts
        {

            public static readonly ID StandaloneProductsTemplateId = new ID("{41719187-AC33-4BAC-B664-D434DB3E98FC}");
            public static class Fields
            {
                public static readonly ID Id = new ID("{D2EBB6C6-454D-4242-83FD-233CA1B48170}");
                public static readonly ID Title = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
                public static readonly ID StanderedImage = new ID("{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}");
                public static readonly ID Description = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
                public static readonly ID CTA = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
            }
        }

        public static class ServiceCarousal
        {

            public static readonly ID ServiceCarousalTemplateId = new ID("{B3F3DB1A-D003-4395-A663-4E927D85096C}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
                public static readonly ID CarousalList = new ID("{0C637E13-9A5F-4A45-AE44-B3CFA17C2DE4}");
            }
        }

        public static class ServiceCarousalItem
        {

            public static readonly ID ServiceCarousalItemTemplateId = new ID("{606F7624-9BCA-4A0D-ACFD-322FFC8F82E3}");
            public static class Fields
            {
                public static readonly ID CTA = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
                public static readonly ID AppCTA = new ID("{1C40A7D0-8157-463E-9DCE-ACFAF3E065B8}");
                public static readonly ID Title = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
                public static readonly ID StanderedImage = new ID("{DB69B6E3-4E77-43F5-BCD4-3BE6E81C017B}");
                public static readonly ID Description = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
            }
        }

        public static class PackageServiceItem
        {

            public static readonly ID PackageServiceItemID = new ID("{B6923FD6-4DDD-48BE-BBC7-2D4F3FE84F9B}");
        }

        public static class AirportDetails
        {

            public static readonly ID AirportTemplateId = new ID("{85BD7A06-FE9B-4D02-86A8-3D07A04BE328}");
            public static class Fields
            {
                public static readonly ID AirportName = new ID("{41193DDB-A281-40F1-9D6B-76AD7543B2D3}");
                public static readonly ID IsPranaam = new ID("{14917591-72FC-4585-BFE5-186D92590B74}");
                public static readonly ID AirportId = new ID("{CE1F5003-5FDF-4C2E-A8EE-EE39C29903F3}");
                public static readonly ID AirportCode = new ID("{575C3178-B236-4052-816C-5CA244EC0FE2}");
            }
        }

        public static class AirportTerminalDetails
        {

            public static readonly ID AirportTerminalTemplateId = new ID("{85BD7A06-FE9B-4D02-86A8-3D07A04BE328}");
            public static class Fields
            {
                public static readonly ID TerminalName = new ID("{DDA53257-1D10-4F2E-AB2E-2F10D64E8A68}");
                public static readonly ID PranaamServiceAvailable = new ID("{96976B3D-A378-4715-885A-CA32F9B0A06F}");
                public static readonly ID PranaamPackages = new ID("{5A2684F6-ADED-4B21-943A-A8FC8C8BD117}");
                public static readonly ID StandaloneProducts = new ID("{8668BE54-E8CA-4945-9616-594E3E2035C8}");
            }
        }
        public static class AirportId
        {
            public static readonly ID AirportTemplateId = new ID("{486FBEBB-93F6-4E9D-859A-357010D47503}");
            public static readonly ID CityTemplateId = new ID("{D71EDD08-B897-45BB-A1FD-F90666A7F0CE}");
        }
        public static class PackageIdTemplate
        {
            public static readonly ID PackageTemplateId = new ID("{85BD7A06-FE9B-4D02-86A8-3D07A04BE328}");
        }

        public static class ServiceTypeTemplate
        {
            public static readonly ID ArrivalTemplateId = new ID("{4A39CBDE-6C37-4DB7-BBE2-45542791AF87}");
            public static readonly ID DepartureTemplateId = new ID("{B2872D8F-6D0B-41A5-BDD6-C1AF64A7F315}");
            public static readonly ID TransitTemplateId = new ID("{54902155-3CAD-479B-B3B7-947A51B551F4}");
        }

        public static class TravelSectorTemplate
        {
            public static readonly ID DomesticTemplateId = new ID("{4E0F2D23-845B-4373-A1C9-3AD2818C3BA1}");
            public static readonly ID InternationalTemplateId = new ID("{62592D03-3E36-47DF-9B99-D4201A5299EF}");
            public static readonly ID DomesticToInternationalTemplateId = new ID("{5D36815D-9D27-47D7-9CF5-FE08CBBC115D}");
            public static readonly ID InternationalToInternationalTemplateId = new ID("{A08C3115-2F4E-4F0B-A21F-C14E02B22323}");
            public static readonly ID InternationalToDomesticTemplateId = new ID("{5A5ECE27-F2A4-4EC7-9090-CBB8788EB7D9}");
            public static readonly ID DomesticToDomesticTemplateId = new ID("{ED63E401-B7A6-4009-A818-FAF7B9E3F2A5}");
        }

        public static class PranaamAirportItem
        {
            public static readonly ID PranaamAirportItemId = new ID("{0E6512D2-0E9E-4B2E-9659-58B6A7C7850A}");
        }

        public static class HowItWorkSteps
        {
            public static readonly ID HowItWorkStepsTemplateID = new ID("{6EF68A1F-DAEA-4791-BA3A-10D07A89618D}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{FEFEEBC2-6069-4B95-A4A0-CA6B0439DE41}");
                public static readonly ID Tabs = new ID("{9665DC1A-DD27-4B13-8DBE-B42EC34E5CDF}");
                public static readonly ID CTA = new ID("{F5B24C89-39AC-467D-A288-18560A6FAE33}");
                public static readonly ID AppCTA = new ID("{EB3384DF-00A2-488D-A042-84BD185160A2}");
            }
        }

        public static class HowItWorkStepsTabContent
        {
            public static readonly ID HowItWorkStepsTabContentTemplateID = new ID("{2B3496AB-34B3-4616-98A8-1CAF377FA92C}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{3D4F8111-B05D-4AF0-9D91-868DA45522A8}");
                public static readonly ID Card = new ID("{C351A9E8-0657-4C81-82F6-D83DE2380AA6}");
            }
        }
        public static class HowItWorkStepsCard
        {
            public static readonly ID HowItWorkStepsCardTemplateID = new ID("{849BD6B0-BE50-4EB4-9EDE-A39606BC87BB}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
                public static readonly ID Description = new ID("{9A9DEA93-3CB7-459A-84DC-6597CC0D984F}");
                public static readonly ID Value = new ID("{ACD5F79F-52AC-4BD0-AC9D-F61336568799}");
            }
        }

        public static class Cancellation
        {
            public static readonly ID CancellationTemplateID = new ID("{1C35A05A-22C0-4DB5-9AA7-9A76256A3873}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{D5DB46B6-329F-478B-B778-E58C505229DD}");
                public static readonly ID Description = new ID("{5E9E16E6-ADB6-41CE-BE8A-D3A3A39B18E8}");
                public static readonly ID TableHeader = new ID("{FEA57309-675C-44F3-B9BE-6DF0D10C80E9}");
                public static readonly ID TableRows = new ID("{179E9004-99FE-4E9A-BFEB-CBB329EF36F6}");
            }
        }

        public static class CancellationHeader
        {
            public static readonly ID CancellationHeaderTemplateID = new ID("{80F9B2F5-AAAA-44F7-97E6-DDBD7ED9FD92}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{30FA2056-BB68-4C9C-8324-30B10D3CA1F0}");
                public static readonly ID Value = new ID("{6BE12DEB-371F-4BA2-90A7-0C532E2DCDBE}");
            }
        }
        public static class CancellationRow
        {
            public static readonly ID CancellationTemplateID = new ID("{A3B26A56-823D-455E-AA64-4BC0D37B3A23}");
            public static class Fields
            {
                public static readonly ID SrNo = new ID("{F5B7031A-ACCA-4B2C-A819-92D7D64DDDF6}");
                public static readonly ID CancellationService = new ID("{7638BECC-054C-4277-AD65-26C0E728151B}");
                public static readonly ID CancellationCharge = new ID("{D7295ED7-632B-47D0-9FFB-6E55C97E253C}");
            }
        }
        public static class FooterIllustration
        {
            public static readonly ID FooterIllustrationTemplateID = new ID("{A3B26A56-823D-455E-AA64-4BC0D37B3A23}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{3C4071DB-EC18-443C-9955-BD44AA604FE6}");
                public static readonly ID DescriptionData = new ID("{4910B7F5-7728-4B63-A831-E336EB4F3B87}");
                public static readonly ID AppDesc = new ID("{ABEB3B98-6054-4322-B07A-C91066931682}");
                public static readonly ID CTA = new ID("{88E9DA8E-CDFD-4E09-B966-B55A624C90B4}");
            }
        }
        public static class Paragraph
        {
            public static readonly ID ParagraphTemplateID = new ID("{A67631E3-3F20-4E42-AB1C-11E6D6429318}x");
            public static class Fields
            {
                public static readonly ID ParagraphContent = new ID("{95AA0769-7F88-43A2-814F-9FA428F351BC}");
            }
        }
    }
}