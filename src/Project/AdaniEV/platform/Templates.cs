using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.EV.Project
{
    public static class Templates
    {

        #region BaseTemplate

        public static class BaseFieldTemplates
        {
     
            public static class Fields
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");
                public static readonly ID videoSrc = new ID("{4774B865-54FF-4E5A-9241-AD2F28457063}");         
                public static readonly ID backgroundColor = new ID("{C5B69A6E-FABE-4523-AAF7-BAD82637AC22}");  

            }
            //{19231206-116E-4F6D-AE08-1892F5354C35}
        }
        #endregion

        public static class NavbarArtical
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Feilds
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID widgetItems = new ID("{13EDC58D-C856-428F-8210-40D61B4E649B}");

                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                    public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                    public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                    public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");
                    public static readonly ID videoSrc = new ID("{4774B865-54FF-4E5A-9241-AD2F28457063}");

                }
            }
            //{19231206-116E-4F6D-AE08-1892F5354C35}
        }

        public static class ArticleFeaturedFilters
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Fields
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID widgetItems = new ID("{19231206-116E-4F6D-AE08-1892F5354C35}");


                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                    public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                    public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                    public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");
                    public static readonly ID videoSrc = new ID("{4774B865-54FF-4E5A-9241-AD2F28457063}");


                }
            }
            //{19231206-116E-4F6D-AE08-1892F5354C35}
        }
        public static class ArticleVideoGalleryCarousel
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Fields
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID widgetItems = new ID("{E24A79BF-4D83-4B73-8FB7-2F16FA4C32B3}");


                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                    public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                    public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                    public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");
                    public static readonly ID videoSrc = new ID("{4774B865-54FF-4E5A-9241-AD2F28457063}");

                }
            }
        }


        public static class ArticleFeaturedCard
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Fields
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID widgetItems = new ID("{14476289-4EBB-4108-9DA3-30A7115D9680}");


                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                    public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                    public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                    public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");
                    public static readonly ID videoSrc = new ID("{4774B865-54FF-4E5A-9241-AD2F28457063}");
                    public static readonly ID CaptionSource = new ID("{5D786427-21B0-4026-9CD3-41D98E764549}");
                    public static readonly ID CaptionDate = new ID("{1F227981-612D-45C5-AA64-4F92BAE13B00}");

                }
            }
            //{19231206-116E-4F6D-AE08-1892F5354C35}
        }

        public static class Faq
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Feilds
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                public static readonly ID widgetItems = new ID("{7F6970E4-95DB-4C00-8405-B3BFD3F7C8D3}");

                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                    public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");

                }
            }
        }
        public static class LegalNavbar
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Feilds
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                public static readonly ID widgetItems = new ID("{DDE11B14-5CD0-46CC-8CAA-E5DA560AD16B}");

                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                }
            }
        }



        public static class AddVehicle
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Feilds
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                public static readonly ID widgetItems = new ID("{F68D3E6A-3A99-4CD4-AE56-F6217646CF54}");

                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                    public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                }
            }
        }
        public static class AddVehicleForm
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Feilds
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID widgetItems = new ID("{37CA5072-2ABF-4AA5-A88A-8B9077860F65}");

                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                    public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                }
            }
        }

        public static class FAQContactUs
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Feilds
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");

            }
        }

        public static class LegalTermsAndCondition
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Feilds
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID text = new ID("{817FF415-20D6-4090-9D78-B62F227F42DE}");

            }
        }

        public static class LegalPrivacyPolicy
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Feilds
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID text = new ID("{17B3208C-4844-4C9F-A7B3-197C154325D9}");

            }
        }

        public static class ArticleDetailsDiscoverMore
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Fields
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID widgetItems = new ID("{8AD7348E-6F76-45E9-A4E4-2CF74E9F1442}");


                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                    public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                    public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                    public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");
                    public static readonly ID videoSrc = new ID("{4774B865-54FF-4E5A-9241-AD2F28457063}");
                    public static readonly ID CaptionSource = new ID("{5D786427-21B0-4026-9CD3-41D98E764549}");
                    public static readonly ID CaptionDate = new ID("{1F227981-612D-45C5-AA64-4F92BAE13B00}");

                }
            }
        }
        public static class ArticleDetailsSocialMediaLinks
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Fields
            {
                public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID title = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                public static readonly ID author = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                public static readonly ID readTime = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                public static readonly ID date = new ID("{1F227981-612D-45C5-AA64-4F92BAE13B00}");


                public static class Widget
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID widgetItems = new ID("{BE90FB2F-1D4C-491C-9131-B0C6DCD0AA63}");

                }

                public static class WidgetContent
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID text = new ID("{5DBA3AB6-4412-43CF-BC0C-8083C42567EB}");
                    public static readonly ID widgetItems = new ID("{E1C4858B-6636-48C2-A045-3FCEDD329A33}");

                }

                public static class widgetItem
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                    public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                    public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");

                }
                public static class widgetItemcontent
                {
                    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                    public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");

                }
            }
        }

        public static class ArticleDetailsTemplates
        {
            public static readonly ID ArticleDetailsSocialMediaLinksDataID = new ID("{6A66CE52-1A09-46D4-99F1-42379BB43A05}");
            public static readonly ID ArticleDetailsSocialMediaLinksData = new ID("{A7EFB087-4550-4372-A8F8-94A4623D4BB7}");
        }

        public static class ContactusBanner
        {
            public const string TemplatedId = "{CA6F5947-A47A-4512-BB68-5576ABF92098}";
            public static class Fields
            {
                public static ID Title = ID.Parse("{F182247D-A8ED-40B8-A1BD-653CA5AD71DB}");
                public static ID image = ID.Parse("{E78E6CFB-137E-47E8-A8E4-49C825F9ECFB}");
            }
        }

        public static class ContactusReachout
        {

            public const string TemplatedId = "{CA6F5947-A47A-4512-BB68-5576ABF92098}";
            public static class Fields
            {
                public static ID Title = ID.Parse("{3194D8EF-A278-4DC0-800C-40E7D0111FC9}");
                public static ID Address = ID.Parse("{F92FAA46-BE07-46B0-B939-769784CEEBBC}");
                public static ID Addressicon = ID.Parse("{63BD2A3B-076A-4B3B-82CF-E585C9E15926}");
                public static ID contacticon = ID.Parse("{58D19B92-8C2F-4293-8BCC-82BC712200C4}");
                public static ID contactPhone = ID.Parse("{7DD76410-0039-43B9-9D37-B8184C81ACD9}");
                public static ID emailicon = ID.Parse("{D911B47D-800F-4D31-BE64-7D329CBE831A}");
                public static ID email = ID.Parse("{8B99BAF8-01F3-45FA-89A8-008EA4CBCE05}");
            }
        }

        public static class Contactusformdetails
        {
            public const string TemplatedId = "{CA6F5947-A47A-4512-BB68-5576ABF92098}";
            public static class Fields
            {
                public static ID title = ID.Parse("{F383D1D5-7598-4A21-B5D5-20CE10B7C650}");
                public static ID Details = ID.Parse("{65FCE0D2-DC8B-41DF-A0FB-20D6CA0515D3}");
                public static ID AgreementInformation = ID.Parse("{B38BC1D2-A89A-4A9B-B3D6-0EBAE98CE793}");
                public static ID ButtonName = ID.Parse("{6EB52EF9-D7D0-46EE-B802-5A510C958D75}");
                public static ID Flagsrc = ID.Parse("{4530BAF9-D05B-45E3-A3B0-FAB156C290D4}");
            }



        }

        public static class Contactusfaq
        {
            public const string TemplatedId = "{CA6F5947-A47A-4512-BB68-5576ABF92098}";
            public static class Fields
            {
                public static ID FaqImage = ID.Parse("{FC4CA231-B96E-4F23-860F-5DE9C9FB9E52}");
                public static ID FaqTitle = ID.Parse("{1CA98169-5F5C-464D-B07E-B7DE0E9D1ABD}");
                public static ID Faqdetails = ID.Parse("{2EBD81FB-2E82-4BCA-9E62-14B249D606D3}");
                public static ID FaqLink = ID.Parse("{4763BF1B-7B58-4251-BCB0-64B8646AB9DA}");
            }
        }

        public static class Calculator
        {
            public const string TemplatedId = "{7F39D0F5-AAE6-49B7-8F04-1E772A99EE8C}";
            public static class Fields
            {
                public static ID widgets = ID.Parse("{3C673D12-CBD4-4D97-9370-50A88ED14E68}");
            }

        }

        public static class BannerNoteDetails
        {
            public const string TemplatedId = "{A31CD54C-3BEE-4FE3-B93E-1B01EC700D8B}";
            public static class Fields
            {
                public static ID Note1 = ID.Parse("{8410A75F-156B-47BC-8CC5-789CA8816F55}");
                public static ID Note2 = ID.Parse("{032A2757-F33F-465B-976C-9CF185812577}");
            }

        }

        public static class Benefits
        {
            public const string TemplatedId = "{244FDD9B-A869-4EB6-AAA7-5F293EA83B3D}";
            public static class Fields
            {
                public static ID BenefitsTitle = ID.Parse("{42343A94-34BF-4598-B58A-06BC9F5600CA}");
                public static ID BenefitsDetail = ID.Parse("{E62273EE-F6FB-435F-9F00-566B88C71510}");
                public static ID BenefitsGroupOfBenefits = ID.Parse("{E822E2C0-7E37-4BB8-BB21-77D0A18D78D5}");
            }

        }


        public static class CostCalculatorFuelCostDetails
        {
            public const string TemplatedId = "{551E544F-E243-4D1C-98E8-30E894A273D5}";
            public static class Fields
            {
                public static ID FuelCostIcon = ID.Parse("{42389DD7-3596-4396-939F-9D053E09D6F9}");
                public static ID FuelCostDetail = ID.Parse("{26335B7D-744F-498F-83A3-F7CCDD57536C}");

            }
        }

        public static class GroupOfBenefitItem
        {
            public const string TemplatedId = "{93EEFC70-2288-4340-840E-9E111DAD35F9}";
            public static class Fields
            {
                public static ID Id = ID.Parse("{BAD3C1F9-CE1D-404C-8BF9-611B8E255ECD}");
                public static ID Title = ID.Parse("{40569883-C03D-48AB-AB9B-30AD628AAFCE}");
                public static ID Detail = ID.Parse("{BD4F6E9E-3EA4-4608-A08C-CB48707A2A07}");
                public static ID Image = ID.Parse("{A8DC7808-5573-4F85-8669-08B143A64A76}");

            }
        }

        public static class CalculatorItem
        {
            public const string TemplatedId = "{8AC733D1-4AD5-490D-A284-21B6D01DB740}";
            public static class Fields
            {

                public static ID Title = ID.Parse("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static ID Image = ID.Parse("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");

            }
        }

        public static class SideNavbar
        {
            public static readonly ID TemplateID = new ID("{1C6143EE-2550-49CD-A8A4-4D9BFD7D92B9}");
            public static class Fields
            {
                public static readonly ID Name = new ID("{53F52DA0-BEF6-4152-AEF0-B9D327AD121E}");
                public static readonly ID SideNavbarTargetItems = new ID("{DDCC6224-5B59-4547-AD56-516415AC19F1}");
            }

            public static class LoginSideNavBarDetails
            {
                public static class Fields
                {
                    public static readonly ID Title = new ID("{D2340B56-6C5B-4ECE-A9E3-25E23CE6A5B7}");
                    public static readonly ID Image = new ID("{727A718B-DE86-436B-8AD4-B83F60B140FB}");
                    public static readonly ID CTALink = new ID("{5229710A-CFDD-4649-AFCB-BAA2080A6F87}");
                    public static readonly ID sideDrawerRightIcon = new ID("{F03A4736-CB57-4CC1-8498-687F3D58B991}");
                }
            }

            public static class ProfileSideNavBarDetails
            {
                public static class Fields
                {
                    public static readonly ID Title = new ID("{6A27DDB4-13A3-4133-9F07-FC3FBA025BE9}");
                    public static readonly ID Image = new ID("{22B1662E-F039-4330-904F-88FB1FB6F5F6}");
                    public static readonly ID CTALink = new ID("{43662D19-9D1F-4407-B867-C5B2DEC0A36B}");
                    public static readonly ID sideDrawerRightIcon = new ID("{A5AEB6D7-5AFD-4064-B9B1-A7D16EF0BEA2}");
                }
            }
        }

        public static class SideNavbarItem
        {
            public static readonly ID TemplateID = new ID("{A16D974D-10FD-4863-9ED5-CA3E644F4FF9}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID Image = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");
            }
        }

        public static class SideNavbarTargetItems
        {
            public static readonly ID TemplateID = new ID("{8837A520-6C0F-46F5-B665-E11912E2CDD9}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID Image = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                //public static readonly ID ctaLink = new ID("{FF7CCFA4-672D-4DA4-BEF6-3A3DDDF42821}");              
                public static readonly ID sideDrawerRightIcon = new ID("{E8D13286-D57A-4636-961B-6A7ADC4E18C4}");
                public static readonly ID SideNavbarItem = new ID("{AF620D11-6818-4EE7-8FB8-528136E1EF84}");
            }

        }

        public static class Sitemap
        {
            public static readonly ID TemplateID = new ID("{865AF641-36C9-4751-8D38-2A3EE50194E7}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{053FA76B-7CDB-46B2-B9C0-B1DAAD44D4B5}");
            }
        }

        public static class SitemapItem
        {
            public static readonly ID TemplateID = new ID("{EB550D2D-4C9B-4AAB-B0BF-69CBEDD84FB1}");
            public static class Fields
            {
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID Image = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
            }
        }


        public static class Aboutus
        {
            public static readonly ID TemplateID = new ID("{320D1264-A44D-4525-B2DB-013AA34539C8}");
            public static class Fields
            {
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID Description = new ID("{2BAB325C-1D9D-4A9F-8DC5-6E8CCF74F485}");
                public static readonly ID widgetItems = new ID("{95544DF5-5368-44C5-AF97-16ECA058F719}");
            }
        }

        public static class AboutusItem
        {
            public static readonly ID TemplateID = new ID("{EB550D2D-4C9B-4AAB-B0BF-69CBEDD84FB1}");
            public static class Fields
            {
                public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                public static readonly ID Description = new ID("{18311656-9991-4B9A-AF30-A9E7BEFC860D}");
                public static readonly ID Image = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                public static readonly ID ReadMore = new ID("{75458C8B-FAD1-40DC-9F19-BF03FFC50EAA}");
            }
        }

        public static class HeroCarousel
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {
                //public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                //public static readonly ID type = new ID("{54412E73-D0A0-4691-A868-B3AAA055BB6C}");
                //public static readonly ID ctaText = new ID("{D0E88D95-BC03-41DC-A34C-53B01C0A8FC5}");
                //public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                //public static readonly ID subTitle = new ID("{1533EED6-5FFC-4B24-A01F-218669C3342F}");
                //public static readonly ID imageSrc = new ID("{92F8B11C-4357-42D1-AE68-A86C637A8A59}");
                public static readonly ID widgetItems = new ID("{AE933556-0332-4D4A-95B8-928947AF9F27}");

                //public static class widgetItem
                //{
                //    public static readonly ID id = new ID("{F839AA70-C16A-4D14-8FE3-EACB70238386}");
                //    public static readonly ID title = new ID("{C31D93D6-2CA4-467F-933A-58E0D05D4B16}");
                //}
            }
        }

        public static class NavBar
        {
            public static readonly ID TemplateID = new ID("{3B17A246-9E0B-4344-B32E-FA2E9F9C92AC}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{7421A953-7E8D-4CD4-AB1B-09220EAEE571}");
            }
        }

        public static class QuickInfo
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{A413C16C-4626-4B48-A1E3-2D39A58CC722}");
            }
        }

        public static class QuickLink
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{8425122D-16BD-4800-95B9-DA9FB7DF8CD1}");              
            }
        }

        public static class WhySearchWithUs
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {               
                public static readonly ID widgetItems = new ID("{CF530DC3-7872-4E5A-9063-4168B9DB71DF}");
                
            }
        }

        public static class LatestEVNews
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{88DACC60-50AE-49F0-96F5-9B7B0706D565}");
            }
        }

        public static class ContactusInfo
        {
            public static readonly ID TemplateID = new ID("{D4D989DF-1038-4BBB-A0BC-805D87B07FCF}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{2EC2901E-D3E2-4D1A-AA9F-7B4EE3EF5B4C}");
            }
        }

        
        public static class EVNearBanner
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {             
                public static readonly ID widgetItems = new ID("{6C9F32C0-AEE9-4284-85BB-F523783C065B}");

            }
        }

        public static class ChargingStationBanner
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {
             
                public static readonly ID widgetItems = new ID("{317E20CF-B694-4E97-A457-C0125F06450A}");
            }
        }

        public static class FAQ
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {
               
                public static readonly ID widgetItems = new ID("{D226B1CE-6C59-4E01-8D8B-63CA07526929}");
              
            }
        }

        public static class FooterNavigation
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{A0CF0764-C7C5-40AF-8817-62DB2D12DC02}");
            }
        }

        public static class FooterNavigationWidgetItem
        {
            public static readonly ID TemplateID = new ID("{7F1E791D-52D8-47B5-8628-7D751A1AB1B0}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{C54F909D-D132-4FE9-BF52-C2B3B71E687C}");
            }
        }

        public static class SocialMediaLinks
        {
            public static readonly ID TemplateID = new ID("{40219813-C8F2-4F54-8D81-290C15C4AE5A}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{52B150BC-F135-4A15-9053-E5DE335B542E}");
            }
        }
        public static class Copyright
        {
            public static readonly ID TemplateID = new ID("{40219813-C8F2-4F54-8D81-290C15C4AE5A}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{375703C4-9A74-4D72-B56E-0A1A644BC363}");
            }
        }
        public static class Language
        {
            public static readonly ID TemplateID = new ID("{40219813-C8F2-4F54-8D81-290C15C4AE5A}");
            public static class Fields
            {
                public static readonly ID widgetItems = new ID("{375703C4-9A74-4D72-B56E-0A1A644BC363}");
            }
        }


    }
}
