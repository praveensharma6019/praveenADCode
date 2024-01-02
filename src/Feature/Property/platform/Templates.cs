using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Property.Platform
{
    public static class Templates
    {
        public static class commonData
        {
            public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");
            public static class Fields
            {
                public static readonly ID ContactUSLabel = new ID("{441BFFD4-0FD7-462D-BE75-A32C2361B8CF}");
                public static readonly ID SeeAllLabel = new ID("{30801654-721A-48B4-928A-38277073B316}");
            }
        }
        public static class Property
        {
            public static readonly ID TemplateID = new ID("{38F56FB0-AB22-4BEB-A142-B4D531ACD492}");
            public static class Fields
            {
                public static readonly ID Location = new ID("{B380C8A2-8D5C-4A0E-ABD4-D0AC214E3824}");
                public static readonly ID ProjectStatus = new ID("{0350CF91-6ECB-4F5A-BC4C-430D0E32214B}");
                public static readonly ID SiteStatus = new ID("{C726F4E1-A95A-47E3-B101-76508BF7A465}");
                public static readonly ID ProjectArea = new ID("{68009697-D024-4247-8FA2-95E3A225236B}");
                public static readonly ID PropertyLogo = new ID("{03ACB328-43E8-45F4-8639-796126D43E23}");
                public static readonly ID PropertyLogoImg = new ID("{E8B52311-A1D9-4B04-B5A0-0682EE79EDBF}");

                public static readonly ID SimilarProjectTitle = new ID("{83337ED4-01E1-411B-890D-0933211BE469}");
                public static readonly ID SimilarProject = new ID("{03ACB328-43E8-45F4-8639-796126D43E23}");
                public static readonly ID ProjectLayout = new ID("{ABB0D7C1-C5CD-4184-ABEE-DEA53137963D}");
                public static readonly ID MapLink = new ID("{76ED815D-19E6-4593-B57E-04CEA5D03CBE}");
                public static readonly ID UnitSize = new ID("{3CF2B01F-A43C-477A-805B-565A0FB57AB4}");


                public static readonly ID Possession = new ID("{1503CC29-BC69-43B3-8D7E-067B76301F41}");
                public static readonly ID isProjectCompleted = new ID("{E99A2C77-88B6-4D2D-886F-A08DF8D24810}");
                public static readonly ID Broucher = new ID("{673B0698-703B-48EF-8CA9-B250C81CFD71}");
                public static readonly ID PropertyType = new ID("{75091E77-938C-4379-84FE-174833285D4C}");
                public static readonly ID Status = new ID("{8212329C-CF1E-45A6-8919-BF4E955A5DFE}");
                public static readonly ID Type = new ID("{04CF398B-5FE5-4A5D-93F4-AC750549D77E}");
                public static readonly ID MediaLibrary = new ID("{3BAB7A7E-77A4-4B8C-BE72-81CF5CBAB4E8}");
                public static readonly ID PriceLabel = new ID("{C6163940-69AB-4D50-A828-8194A127600E}");
                public static readonly ID Ownwards = new ID("{D3487E58-9C53-4BBC-827A-BA99FA1CE3C6}");
                public static readonly ID AreaLabel = new ID("{5915A6B2-FE92-4FC1-9FB9-A2DA8530FFA3}");
                public static readonly ID rating = new ID("{BAA14A71-7D04-4261-92D7-EEB3F4B82876}");
                public static readonly ID ratingCount = new ID("{8B1CEF2D-5205-41F5-963F-D7D0ED647B76}");
                public static readonly ID bestRating = new ID("{CFD6CB95-CE8A-4646-A547-031D81480AA0}");
                public static readonly ID ratingName = new ID("{CD4F1A38-800D-493F-BCA6-07A98B2CD246}");


                public static readonly ID Amenities = new ID("{BBC79AE1-EBCA-4684-9B1D-341B78077470}");
                public static readonly ID Highlights = new ID("{B8B98E20-486C-4125-8BC1-5048AE25489F}");
            }
        }
        public static class TownshipSlideBar
        {
            public static readonly ID TemplateID = new ID("{421C0D35-A8F9-47F5-A56A-2BEA4CAFFE7E}");
            public static class Fields
            {
                public static readonly ID Image = new ID("{27FE7B1E-DB46-48C1-857E-C53FCF77F5C8}");
                public static readonly string ImageFieldName = "Image";
                public static readonly ID Heading = new ID("{827A9102-9F34-43C9-9A67-EE8328F05F98}");
                public static readonly ID Details = new ID("{FC377362-2449-4773-89F5-8E1781A9810F}");
                public static readonly ID TownshipServices = new ID("{4C5FE920-1EF6-4860-8DCB-2619267010D6}");
                public static readonly ID TownshipServicesTitle = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
            }
        }

        public static class FaqTemplate
        {
            public static readonly ID TemplateID = new ID("{C4B7EA2A-4177-482E-BF76-99FC1FD8A404}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{D7541543-084F-45D9-A7A7-2B36BC0C5CDA}");
                public static readonly ID Body = new ID("{622B92CC-C125-4191-A226-F72F09741A50}");
            }
        }

        public static class FaqDataTemplate
        {
            public static readonly ID TemplateID = new ID("{074131E3-65F8-4189-A3BC-7989918D2824}");
            public static class Fields
            {
                public static readonly ID Id = new ID("{E0CE08B1-4B88-49F9-8CCB-6D8BCA304832}");
                public static readonly ID Heading = new ID("{D9210830-F0C7-4D06-BF33-575251245931}");
                public static readonly ID seeAll = new ID("{DEC41D93-CE62-4FAA-A121-5FFC500E8885}");
                public static readonly ID faqs = new ID("{E0FD6ADA-ACB4-4F85-91C6-0A5E3CC2B8F8}");
            }
        }

        public static class ExploreTownshipMedia
        {
            public static readonly ID TemplateID = new ID("{9AF406E4-37E0-4477-AD4E-90711AC90321}"); ///sitecore/templates/Feature/Realty/TownshipDetails/Explore Township Media
            public static class Fields
            {
                public static readonly ID Image = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string ImageFieldName = "thumb";
                public static readonly ID dataCols = new ID("{981C396A-E765-4433-8D8A-6520255B9344}");
                public static readonly ID imgtypeID = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
                public static readonly ID MediaSelection = new ID("{11382189-AEF9-45DC-835E-EFF5E7F25531}"); ///sitecore/templates/Feature/Realty/TownshipDetails/Explore Township Media/Explore Township Media/Media Selection
                public static readonly ID ID = new ID("{D3450D73-76F8-400A-8021-5B995337F7CC}");
            }
        }
        public static class TownshipMasterLayoutTemp
        {
            public static readonly ID TemplateID = new ID("{494F095B-B942-46AF-AFDC-6833DC5504A1}");
            public static class Fields
            {
                public static readonly ID left = new ID("{B55AAE29-37CA-44E2-9BD2-C79630D9EA3D}");
                public static readonly ID Image = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
                public static readonly string ImageFieldName = "thumb";
                public static readonly ID bottom = new ID("{DE649C9E-28A6-4E97-A995-0F7878264E6F}");
                public static readonly ID title = new ID("{4E835225-CDCC-4D73-8028-6F35A430BFF9}");
            }
        }

        public static class Amenites
        {
            public static readonly ID TemplateID = new ID("{0FD4E4FE-5047-437B-9F46-D6E5DC6235F5}");
            public static class Fields
            {
                public static readonly ID Caption = new ID("{090FAF11-8B09-48DC-ABEE-293CD48B35A0}");
                public static readonly ID Src = new ID("{84729260-6707-4276-B12A-F792FE27CA53}");
                public static readonly ID Link = new ID("{8BF70BE9-1B9E-4A22-AC13-73F9292F7617}");
                public static readonly ID status = new ID("{A03E13A0-10D7-4280-A1E5-C317884D3152}");
                public static readonly ID SrcMobile = new ID("{D43CB379-780A-447E-9C8A-2FF24698007C}");
            }
        }

        public static class ProjectAmenites
        {
            public static readonly ID TemplateID = new ID("{1C587667-F61D-40DB-A0CF-9C89E9D60C43}");
            public static class Fields
            {
                public static readonly ID ComponentId = new ID("{E4C7A575-958D-4087-8B05-74F366D25838}");
                public static readonly ID Heading = new ID("{02A395DB-A08D-4AD4-B698-4BAD9F62D419}");
                public static readonly ID disclaimer = new ID("{104E54F2-1D5C-401C-BCBF-F64CAA053C66}");
                public static readonly ID AmenitiesData = new ID("{7ED752C8-A228-4CE7-8FBD-282F8482D4EA}");
            }
        }

        public static class ProjectFeatures
        {
            public static readonly ID TemplateID = new ID("{1C6E62B3-B9EE-4C78-B1DD-689AF41B1765}");
            public static class Fields
            {
                public static readonly ID ComponentId = new ID("{D078604F-B7E3-40DB-ADB9-09FD798C921B}");
                public static readonly ID Heading = new ID("{D25761E5-81B2-41AE-8063-B80388E7899A}");

                public static readonly ID Features = new ID("{FD4D8527-A805-4868-8176-5C4475FB5AF6}");
            }
        }

        public static class FeatureItem
        {
            public static readonly ID TemplateID = new ID("{3636B29C-712D-4262-8BC6-4207E66E88DC}");
            public static class Fields
            {
                public static readonly ID Caption = new ID("{D25BF205-C602-4A03-9658-1FD3E6688028}");
                public static readonly ID Src = new ID("{05917F77-3B26-46DC-A352-63B60768A70F}");
                public static readonly ID Title = new ID("{3D9AC3CB-D85B-454C-A068-34C75A253407}");
                public static readonly ID Desc = new ID("{60DDFCB4-99AC-4506-9551-75033E3185A8}");
                public static readonly ID labelTerms = new ID("{12D635C2-DAC0-4A20-87CD-40362B71135F}");
                public static readonly ID srcMobile = new ID("{015633F1-1365-4419-8346-D247402230CF}");
            }
        }


        public static class Project
        {
            public static readonly ID TemplateID = new ID("{439CBFEF-A879-4830-BD15-615CB82AE1E4}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{5B75EB43-8110-4DBE-B97C-CAEFCDC2EC3E}");
                public static readonly ID SubTitle = new ID("{3A6FEBD4-313A-47FE-BC2A-850D18F43C05}");
                public static readonly ID Image = new ID("{5E87F2D4-99DA-4889-800C-A1262063187F}");
            }
        }
        public static class Commondata
        {
            public static readonly ID ItemID = new ID("{4A70335A-5B47-4C98-AD06-0F3749EA3675}");

        }

        public static class Highlights
        {
            public static readonly ID TemplateID = new ID("{D8274139-72C1-4436-9528-A0F01933EBD6}");
            public static class Fields
            {
                public static readonly ID Type = new ID("{D57D9F09-9E23-42CB-AE2F-D4C747B53A41}");
                public static readonly ID Src = new ID("{313DA465-A457-4267-85CC-15EF96D7AA03}");
                public static readonly ID ImgAlt = new ID("{4A675D0B-461E-4D70-8867-EEAB37038A3D}");
                public static readonly ID ImgTitle = new ID("{3D8D4841-CCC4-473C-A7EB-F879F960346F}");
                public static readonly ID LogoSrc = new ID("{144FBC2A-1B6F-46FC-95B8-F6C2484A6EB1}");
                public static readonly ID LogoTitle = new ID("{876840D0-E034-42C1-A803-11E86DFDD14F}");

                public static readonly ID AboutImg = new ID("{CF3F5026-BD8E-4A3C-86AD-83EF39C7E1B7}");
                public static readonly ID Icon = new ID("{BC7DC378-4E5C-4963-82DB-3661AC592739}");
                public static readonly ID IconDesc = new ID("{59E9FDFC-B8B0-40BE-8039-F83D26808AFF}");

                public static readonly ID Degree = new ID("{22FB77E7-F83C-41A8-A58F-C08CF78C2DB7}");
                public static readonly ID Tour = new ID("{59E9FDFC-B8B0-40BE-8039-F83D26808AFF}");
                public static readonly ID tabType = new ID("{026F83B3-DAE4-4094-B4DC-77D714E6A1AB}");
                public static readonly ID SrcMobile = new ID("{0E4C7B31-25A5-4607-BBB4-10A781B924E4}");
            }
        }

        public static class EmiCaluclator
        {
            public static readonly ID TemplateID = new ID("{19F74044-E5D7-4473-82AC-7700E8E4F304}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{B0F37934-8982-45F8-8904-22122B5098B1}");
                public static readonly ID LoanAmountTitle = new ID("{A0285D7E-4758-4F50-ABC2-677AFE8103C3}");
                public static readonly ID LoanAmountFrom = new ID("{A863C432-CD23-4DDE-8DF2-A5F570CB7AEF}");
                public static readonly ID LoanAmountTo = new ID("{337F3E5D-5DAA-4E89-B9FD-835D5ADE9B75}");
                public static readonly ID InterestRateTitle = new ID("{B94A892E-65CF-40A9-8E7E-06FA348B5668}");
                public static readonly ID InterestRateFrom = new ID("{73189415-2B6D-4F17-B376-5004A7B25FE6}");

                public static readonly ID InterestRateTo = new ID("{C17CFFBB-B91F-4461-9C85-F9C8A3E2DC82}");
                public static readonly ID LoanTenureTitle = new ID("{DCF17723-69C7-4D32-BDB3-81E00B3E8A10}");
                public static readonly ID LoanTenureFrom = new ID("{DE058BE5-2101-4A70-B66B-4A429BEA546E}");
                public static readonly ID LoanTenureTo = new ID("{6A9AA775-60B4-4112-94D1-932142A31A0D}");



                public static readonly ID Heading = new ID("{B0F37934-8982-45F8-8904-22122B5098B1}");
                public static readonly ID Rs = new ID("{A0285D7E-4758-4F50-ABC2-677AFE8103C3}");
                public static readonly ID Lakhs = new ID("{A863C432-CD23-4DDE-8DF2-A5F570CB7AEF}");
                public static readonly ID LoanAmountLabel = new ID("{337F3E5D-5DAA-4E89-B9FD-835D5ADE9B75}");
                public static readonly ID MinLoanAmountLabel = new ID("{B94A892E-65CF-40A9-8E7E-06FA348B5668}");
                public static readonly ID MaxLoanAmountLabel = new ID("{73189415-2B6D-4F17-B376-5004A7B25FE6}");
                public static readonly ID MinLoanAmount = new ID("{C17CFFBB-B91F-4461-9C85-F9C8A3E2DC82}");
                public static readonly ID MaxLoanAmount = new ID("{DCF17723-69C7-4D32-BDB3-81E00B3E8A10}");
                public static readonly ID Percent = new ID("{DE058BE5-2101-4A70-B66B-4A429BEA546E}");
                public static readonly ID InterestRateLabel = new ID("{6A9AA775-60B4-4112-94D1-932142A31A0D}");
                public static readonly ID MinInterestRateLabel = new ID("{89C80FC6-6FF1-4E8C-818C-AD6212C9CDF3}");
                public static readonly ID MaxInterestRateLabel = new ID("{C0040DC3-F0F1-4772-8E13-1A1418118D28}");
                public static readonly ID MinInterestRate = new ID("{42F08BC8-90B8-4E78-A6CD-64F7125FC2D3}");
                public static readonly ID MaxInterestRate = new ID("{0ADBD8F9-7D05-4108-867A-30D3BA8AECC2}");
                public static readonly ID LoanTenureLabel = new ID("{D695C2DB-0FDD-4EA6-A335-9A24863B8CC5}");
                public static readonly ID Years = new ID("{5FE3BE93-E14A-46D3-8552-865E56C2EFE6}");
                public static readonly ID MinLoanTenureLabel = new ID("{18458A09-B53C-4A11-8593-F740FB698625}");
                public static readonly ID MaxLoanTenureLabel = new ID("{AE84F8A0-DD46-4BD6-AB30-BD708CCF5767}");
                public static readonly ID MinLoanTenure = new ID("{FCE4DD21-FB3A-450B-B3F0-585620BAEA1A}");
                public static readonly ID MaxLoanTenure = new ID("{F5E9904B-9EC7-47D4-8F27-34EE8B6ADCFA}");
                public static readonly ID InterestAmountLabel = new ID("{01C41D46-CE84-4DB1-B99F-4EA2222D5882}");
                public static readonly ID InterestAmountMobileLabel = new ID("{35E499FF-BA97-4E7F-8837-9B789E6A4B69}");
                public static readonly ID PrincipalAmountLabel = new ID("{03F9A868-BF49-443E-9702-A69CB1C86617}");
                public static readonly ID PrincipalAmountMobileLabel = new ID("{FD5D7C7C-0347-46D8-918D-0117283217D1}");
                public static readonly ID TotalPayableAmountLabel = new ID("{7B2CEDF9-E7BA-4742-B075-36E3FFDC2880}");
                public static readonly ID TotalPayableAmountMobileLabel = new ID("{72B47293-81C4-47BB-BC3D-2EF29AE1F888}");
                public static readonly ID DefaultLoanAmount = new ID("{8992D4B1-A555-4205-9E7F-5CCDB5535B2A}");
                public static readonly ID DefaultInterestRate = new ID("{215BD332-2C4F-4E8D-96C9-F69F7F1583FF}");
                public static readonly ID DefaultLoanTenure = new ID("{6E501627-62C4-4A00-8E4E-A8F8658D5940}");
                public static readonly ID OurBankingPartnersLabel = new ID("{0E8858AA-EDBA-444B-84B5-48DF44A5AB50}");
                public static readonly ID BankingPartner = new ID("{F69E01D0-D4B8-4E38-B9C0-DAD276BEB0E9}");

            }
        }

        public static class BankingPartnerField
        {
            public static readonly ID TemplateID = new ID("{E23259D1-9E55-4835-8DCF-A74EB9A0613E}");
            public static class Fields
            {
                public static readonly ID Src = new ID("{76A484E4-0086-4B39-94BF-A53D8003F8F3}");
                public static readonly ID ImgAlt = new ID("{E060CBE9-FA16-4745-9F0F-A9126F7678D1}");
                public static readonly ID ImgTitle = new ID("{4213F2A9-BA36-48F2-A05C-DA1E01B1CA91}");
            }
        }

        public static class BankingPartnerItemField
        {
            public static readonly ID TemplateID = new ID("{6417AA6D-E351-4DF3-B565-44B01527DC4F}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{631C3CCD-C99E-443C-8889-DD2EC4C0331F}");
                public static readonly ID Image = new ID("{53FA8E4D-C7B1-4DAB-9943-E975F7F6D5F3}");
            }
        }

        public static class EmiCalculationTextFields
        {
            public static readonly ID TemplateID = new ID("{32CF3F33-4EE7-4BAD-9A81-89AADE04F3D3}");
            public static class Fields
            {
                public static readonly ID EmiCalculationText = new ID("{329DBF7A-F3ED-4193-A587-8AC5A7FEC02D}");
                public static readonly ID InterestAmountText = new ID("{47D7C687-4F6D-45A2-9561-9BCEC4B49E19}");

                public static readonly ID PrincipleAmountText = new ID("{DA4ECF59-4517-4EB2-BD35-7700A07AA64F}");
                public static readonly ID PaybleAmountText = new ID("{96EAD7E0-6774-4254-B0D7-F5FB3165CE2D}");
            }
        }

        public static class PropertyAboutField
        {
            public static readonly ID TemplateID = new ID("{5E6705B8-45D4-47EB-9326-9E3BFA089ACF}");
            public static class Fields
            {
                public static readonly ID ComponentID = new ID("{0108EC3F-5AA4-495F-94A2-6A0675C0F5F2}");
                public static readonly ID Heading = new ID("{0D4507E5-138A-4BD7-A773-B5D644EC5CB9}");
                public static readonly ID Title = new ID("{A9008B3F-449A-4F84-9611-176726B63200}");
                public static readonly ID Description = new ID("{EF310390-B623-40E5-93EA-09C7A92D1398}");

                public static readonly ID ReadMore = new ID("{FED9075F-AEDB-4A1C-945C-A866C085D480}");
                public static readonly ID ReadLess = new ID("{6ADD25D5-0C89-4DC8-8FB3-37E58269EC5E}");
            }
        }
        public static class ResidentialPropertyData
        {
            public static readonly ID LocationLandingTemplateID = new ID("{19FADD3A-572B-4BFE-ACB9-9649672754F0}");
            public static readonly ID ResidentialTemplateID = new ID("{F3DD9D89-7C47-4F61-9B35-45E291C10AF2}");
            public static readonly ID CommercialTemplateID = new ID("{4001FE34-F746-4006-8F5C-3E23071BD105}");
            public static class BaseFields
            {
                public static readonly ID Title = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID HeadingLabel = new ID("{32FE6912-30E7-47E7-87FE-13361D808F59}");
                public static readonly ID SubHeadingLabel = new ID("{A5CC1BA3-684E-4981-8319-B47E6F3E3DAD}");
            }
            public static class Fields
            {
                public static readonly ID LinkID = new ID("{115890C6-B02F-4AE2-979D-B1B76DAE97C0}");
                public static readonly ID PropertyLinkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string LinkFieldName = "Link";
                public static readonly ID PropertyLogo = new ID("{03ACB328-43E8-45F4-8639-796126D43E23}");
                public static readonly string PropertyLogoFieldName = "Property Logo";
                public static readonly ID Image = new ID("{E4FBF9B5-927B-476C-A0B7-78308243F6D0}");
                public static readonly string ImageFieldName = "Image";
                public static readonly ID Title = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly ID projectArea = new ID("{3CF2B01F-A43C-477A-805B-565A0FB57AB4}");
                public static readonly ID areaTitle = new ID("{19BAFA41-05CE-40EA-A9CB-60A35A6F230D}");
                public static readonly ID areaDesc = new ID("{68009697-D024-4247-8FA2-95E3A225236B}");
                public static readonly ID location = new ID("{B380C8A2-8D5C-4A0E-ABD4-D0AC214E3824}");
                public static readonly ID subType = new ID("{75091E77-938C-4379-84FE-174833285D4C}");
                public static readonly ID contactUsLabel = new ID("{282E3225-290D-4A88-80B7-8029F0F9E45F}");


                public static readonly ID imgtype = new ID("{9E4D0FF2-6C0D-46BA-AA41-FEE4CD383576}");
                public static readonly ID status = new ID("{8212329C-CF1E-45A6-8919-BF4E955A5DFE}");
                public static readonly ID category = new ID("{8212329C-CF1E-45A6-8919-BF4E955A5DFE}");

            }
        }
        /// <summary>
        /// internal image properties
        /// </summary>
        public static class ImageFeilds
        {
            public static readonly ID TemplateID = new ID("{F1828A2C-7E5D-4BBD-98CA-320474871548}");
            public static class Fields
            {
                public static readonly ID AltID = new ID("{65885C44-8FCD-4A7F-94F1-EE63703FE193}");
                public static readonly string AltFieldName = "Alt";
                public static readonly ID TitleID = new ID("{3F4B20E9-36E6-4D45-A423-C86567373F82}");
                public static readonly string TitleFieldName = "Title";
            }
        }

        public class DatumItem
        {

            public static readonly ID TemplateID = new ID("{7D3719D1-160C-4982-8925-59F57AE8C23A}");
            public static class Fields
            {
                public static readonly ID labelheading = new ID("{9F6772AF-8591-4984-B519-D2F09472839F}");

                public static readonly ID labeldata = new ID("{950D218C-F761-4C0F-AF09-82BDF25BA732}");
            }
        }

        public class Clubthanks
        {

            public static readonly ID TemplateID = new ID("{2BBEF6EB-E62F-452F-A306-3BB1BD8A69E8}");
            public static class Fields
            {
                public static readonly ID heading = new ID("{5AE4F816-5F55-4333-BB5F-DF861DC64FC2}");

                public static readonly ID para = new ID("{1139F68F-5339-4145-A9F3-5E36D0C43874}");

                public static readonly ID data = new ID("{67FAB4F9-4678-414F-AA0B-9A67CFA0BF44}");
            }

        }

        public class ContactCtaData
        {
            public static readonly ID TemplateID = new ID("{B5EADE49-889F-40F0-B6A6-9186AF64B3E6}");
            public static class Fields
            {

                public static readonly ID getInTouch = new ID("{95C759BF-2DAD-4923-B500-DE3218D3A3CA}");
                public static readonly ID heading = new ID("{8D11AD62-EE17-4E02-A040-E46995968004}");
                public static readonly ID desc = new ID("{42FC3AC8-45ED-428D-B289-90481B876A8F}");
                public static readonly ID button = new ID("{13215FEA-A9EC-4EF4-9E2F-901A86CA9A53}");
                public static readonly ID enquireNow = new ID("{8C725F2A-2621-4B60-84DB-4F3ED0268E0D}");
            }

        }

        public class ProjectNameItem
        {
            public static readonly ID TemplateID = new ID("{C20729FF-DE18-4F85-9E1C-CD82C34C8AF5}");
            public static class Fields
            {
                public static readonly ID title = new ID("{9EDF63A2-FEA0-4676-B6F3-326AB76FB08A}");
                public static readonly ID location = new ID("{A0B944FD-59AE-43DA-9702-7E3CE3E7A8AF}");
                public static readonly ID price = new ID("{251AE564-9159-436A-B7CF-2F30B5780C40}");
                public static readonly ID discountLabel = new ID("{939D1AAB-099D-46BC-AA82-FDC06C8FA064}");
                public static readonly ID discount = new ID("{58DAD02C-067E-4CAB-8D3A-98FAF846D11E}");
                public static readonly ID priceLabel = new ID("{E22629F6-E0B2-432F-B165-CA0424D7AE6B}");
                public static readonly ID Rs = new ID("{F437ECD3-ABB8-4F5E-B622-0B65C2136C6E}");
                public static readonly ID priceStartingAt = new ID("{40935746-E844-459B-8C13-639BC062CB88}");
                public static readonly ID sup = new ID("{5045362E-EB0E-4313-9734-136B770C2221}");
                public static readonly ID allInclusive = new ID("{EC86D459-4283-49C9-8638-DC7B5D3222BB}");
                public static readonly ID link = new ID("{B9DDFA06-B7FF-4209-8D14-DF940ACF99DE}");
                public static readonly string linkText = "link";

            }

        }

        public class NavbarTabsItem
        {
            public static readonly ID TemplateID = new ID("{20BCB98A-9F35-4135-8824-254EFC2E501D}");
            public static class Fields
            {
                public static readonly ID About = new ID("{B9DF80D8-CCF6-47E8-8FF8-15A89E91D2C2}");
                public static readonly ID Ameneties = new ID("{979052E3-4234-4334-ACD6-7BBF24411075}");
                public static readonly ID Projects = new ID("{E89F7DD8-AC43-4796-93A3-7C3728329A0E}");
                public static readonly ID MasterLayout = new ID("{22E2EE74-DA80-4B6C-B148-F7AB186BAE03}");
                public static readonly ID TypicalFloorPlan = new ID("{BD7E0C45-6C09-4469-B85A-FB1F9392216A}");
                public static readonly ID TypicalUnitPlan = new ID("{EBFC3935-88A5-46C3-8A17-88F3BBAB8C88}");
                public static readonly ID ExploreTownship = new ID("{A64724C7-A6AC-450B-B8F9-D3F91FD484A7}");
                public static readonly ID LocationMap = new ID("{7AB40611-219D-4480-BC31-D5673EC3A24E}");
                public static readonly ID video = new ID("{0D1C12C5-2CCE-4CF6-B279-C02FAFB181FC}");
                public static readonly ID projectHighlights = new ID("{5A620CD3-3210-4AEE-845D-3A1A733F5EF7}");

            }

        }

        public static class GalleryIconDataTemplate
        {
            public static readonly ID TemplateID = new ID("{C902C44D-D176-4CEA-BEC3-8396C00C0683}");
            public static class Fields
            {
                public static readonly ID Label = new ID("{26DEC4C1-C569-4EC8-83EC-913FD00AE4E5}");
                public static readonly ID Type = new ID("{2CDA8A4E-B9C8-4FE7-A42F-D29E83C4D923}");
                public static readonly ID Icon = new ID("{25ED8493-548C-485A-BEBE-5ED5F995A605}");
            }
        }

        public static class ReraDataTemplate
        {
            public static readonly ID TemplateID = new ID("{AE70670D-137A-4C2A-8D7E-1228C58F10ED}");
            public static class Fields
            {
                public static readonly ID Icon = new ID("{99E65018-592F-45FA-8958-B7E40BA7408F}");
                public static readonly ID Rera = new ID("{72DFBD2F-EBE3-45F7-9EB2-B7EC7BDDE188}");
                public static readonly ID ReraWebsite = new ID("{80F065D1-FD16-45EC-974A-EF07CDB5DB0F}");
                public static readonly ID ReraWebsiteLink = new ID("{ECE61189-109D-4B82-90DC-7130D5AADF84}");
                public static readonly ID ProjectListedOn = new ID("{0495F003-7B56-487C-8E3D-79F97CD0E2EB}");
                public static readonly ID ReraNumber = new ID("{4537E4CC-7A32-4F8E-8A2C-03745A7E6A84}");
                public static readonly ID As = new ID("{082E8E7A-27FF-4453-B3AF-B80CEAC98C93}");
                public static readonly ID DownloadLink = new ID("{3703A02B-100D-488A-A941-78BFA0FAEA4E}");

                public static readonly ID ModalTitle = new ID("{3737903E-AF4B-4C6A-A46A-9B21D401F75D}");
                public static readonly ID ReraNumberlabel = new ID("{118E44A6-D7FF-4009-BEC2-35FF8BBEC827}");
                public static readonly ID download = new ID("{32BF313D-C666-46C8-92BA-258CAB7992AB}");
                public static readonly ID reraModal = new ID("{37534914-DEF8-4C09-B58B-AF4E74EB40D5}");
                public static readonly ID reraHeading = new ID("{49CB8204-8933-4460-88C2-27FFF9BFC872}");
                public static readonly ID envHeading = new ID("{57F70205-71FF-4A15-A38B-435346FD8A9C}");
                public static readonly ID downloadRera = new ID("{822B6817-435E-42AB-9B52-EE6A077066B8}");
                public static readonly ID downloadEnv = new ID("{4691DEB5-9679-4897-9239-4CF5EFCBB051}");
            }
        }


        public static class GalleryModalDataItemTemplate
        {
            public static readonly ID TemplateID = new ID("{51DFF52E-6ACD-40AA-ADA7-73294A88C0FF}");
            public static class Fields
            {
                public static readonly ID url = new ID("{BC7A2494-3B97-447D-8C87-4424039F5BDA}");
                public static readonly ID rearid = new ID("{825CF7D8-C2AA-495A-8AAD-394A67FFD527}");
                public static readonly ID download = new ID("{520797B7-50C1-4519-91A6-616925767E72}");
                public static readonly ID downloadLink = new ID("{81D5FBAC-78B0-4665-97F0-3586A293B5DB}");
                public static readonly ID ReraSiteLink = new ID("{93290F48-C1F6-4CAD-AC3E-A85E967E3AF3}");
                public static readonly ID ReraTitle = new ID("{848B469A-DBD3-425D-B5D4-AA8A935D059A}");
                public static readonly ID qrCodeImage = new ID("{76B4C440-7B54-4065-A250-EFF0EBCDE4B7}");
            }
            public static readonly ID EnvTemplateID = new ID("{250E096D-B529-4743-A05C-7312D13908F1}");
            public static class envFields
            {
                public static readonly ID envurl = new ID("{4F33E842-F73B-4DC4-8262-75418B1D33D6}");
                public static readonly ID envMonth = new ID("{F64A31D2-42C4-4CFD-8D24-3C7EEAF4AF07}");
                public static readonly ID envdownload = new ID("{FB90B715-5A52-4F17-995C-6A020A17BD5C}");
                public static readonly ID envdownloadurl = new ID("{038AD515-DE62-41A9-955F-A00E6298824F}");
            }
        }


        public static class GalleryModalDataTemplate
        {
            public static readonly ID TemplateID = new ID("{C4057E3B-FCCE-4C3C-AB0A-2A88D3BD68E5}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{E3DB60A2-4E6B-4EBC-AEB0-D3461DFD6A16}");
                public static readonly ID CloseLink = new ID("{6397FB03-C898-48AA-8781-38B9FEB64CEF}");
                public static readonly ID ShareLink = new ID("{49E2B5D4-3D0C-4C3C-A2EC-8AD01AB3B691}");
                public static readonly ID Share = new ID("{951A733E-CB0D-4F4C-B45F-FFB0483FA43F}");
            }
        }

        public static class GalleryTabTemplate
        {
            public static readonly ID TemplateID = new ID("{A3767E3A-34CC-411F-92E2-5BCC9D96BC92}");
            public static class Fields
            {
                public static readonly ID Title = new ID("{721FCE73-E2B8-4684-ADCB-8FD30FDAC9CE}");
                public static readonly ID Link = new ID("{14583CE3-44DD-4186-A50B-99FB939C218E}");
            }
        }


        public static class MasterLayoutDataTemplate
        {
            public static readonly ID TemplateID = new ID("{84F9623A-CD1F-479F-826B-D5FA9B1C8AFD}");
            public static class Fields
            {
                public static readonly ID Heading = new ID("{15A5C581-C8A7-4237-A5E9-C06EB26F22D7}");
                public static readonly ID ComponentID = new ID("{C6B4333A-F198-4519-A10B-F328A2EB8487}");
            }
        }

        public static class VideoCarouselData
        {
            public static readonly ID TemplateID = new ID("{D0ACA36C-8E64-4A06-803B-E59034A7A1AE}");
            public static class Fields
            {
                public static readonly ID ModalSlidesData = new ID("{12E069EF-942C-467B-9F45-08C67F1BBB79}");
                public static readonly ID GalleryTabs = new ID("{72C92A45-BCF3-479A-89DF-EAA860947195}");
            }
        }


        public static class PointerDataTemplate
        {
            public static readonly ID TemplateID = new ID("{4F06BCED-3B5F-4636-9740-02AE271F790B}");
            public static class Fields
            {
                public static readonly ID Image = new ID("{429EC2E2-C647-4689-A60D-5AF3DFE7D79E}");
                public static readonly ID ImageAlt = new ID("{CB3E11A0-3BA6-4745-8D58-41BC657D06A6}");
                public static readonly ID Width = new ID("{C06840C2-D1DC-400B-AB63-679E6891611B}");
                public static readonly ID Height = new ID("{427A3B63-4F31-42AC-891C-EE02BCC6A3EA}");

                public static readonly ID Points = new ID("{B8BEFD67-805D-4D8E-AA96-CB83B889DC9E}");
            }
        }

        public static class Points
        {
            public static readonly ID TemplateID = new ID("{1BB423D7-E5D7-449C-989F-F5CB5063DF9C}");
            public static class Fields
            {
                public static readonly ID Left = new ID("{2D9D9033-136B-4254-A425-7A455EC27C71}");
                public static readonly ID Bottom = new ID("{3DB3C1F2-5E5B-4CC3-BEBD-B0DB942AEBC9}");
                public static readonly ID Title = new ID("{1B4C6685-439D-4415-8A95-F71CDE9C8E12}");
            }
        }

        public static class ProjectHighLightTemplate
        {
            public static readonly ID TemplateID = new ID("{66616FE2-977B-4FF9-A785-A0215CF78FB0}");
            public static class Fields
            {
                public static readonly ID GalleryIconData = new ID("{7A83372C-8145-457C-8D65-D2CAB66B95FC}");
                public static readonly ID ReraData = new ID("{D85F7FFD-8AB4-43AD-B707-76D5D26B2782}");
            }
        }

        public static class ModalSlidesDataTemplate
        {
            public static readonly ID TemplateID = new ID("{132E5DE6-39E0-4579-9BF7-E8C04062C146}");
            public static class Fields
            {
                public static readonly ID Id = new ID("{3CE0F1F0-9CD8-4751-9D5E-FBB523B4AC06}");
                public static readonly ID Poster = new ID("{2FAFF391-915E-4DA9-855A-63AF62CB66D7}");
                public static readonly ID Videomp4 = new ID("{24A60295-6B7E-4ADD-8388-72E8C1AA4D0A}");
                public static readonly ID Videoogg = new ID("{0010B203-938A-44C6-A82E-7F78B8DD2725}");
                public static readonly ID Thumbsrc = new ID("{78D03BEA-0AB1-4B99-A435-A2766E9B8304}");
                public static readonly ID Thumbalt = new ID("{3C989CC2-A38B-425F-B4C6-65B8D4CCFF40}");
                public static readonly ID Thumbtitle = new ID("{1E634805-C39A-4F95-920E-694C8EDDEF42}");
                public static readonly ID lable = new ID("{4D56DEDF-0B80-48B8-91F4-1C53523BAFCD}");
                public static readonly ID DataType = new ID("{7AC76487-A882-4C36-B313-CC9471F5A8F2}");
                public static readonly ID TabType = new ID("{3197044A-E094-4DD0-B846-3B2F4EEC5CBC}");
                public static readonly ID IframeUrl = new ID("{8A30E1AC-0CFF-4979-9E32-43A068C0209B}");
                public static readonly ID Thumbsrcmobile = new ID("{205A661F-EBE6-469C-9618-EBC5E9738F50}");
                public static readonly ID Postermobile = new ID("{CB90098E-E3DA-45E1-B506-624572F2F8D8}");
            }
        }


        public class FacilityItem
        {
            public static readonly ID TemplateID = new ID("{691A6784-2335-4D94-BB65-85328246A139}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{4A427D97-A397-4ACC-A0EB-482E46B5558C}");

                public static readonly ID Icon = new ID("{28A73658-9AF1-444C-BD80-0822D51619FA}");
            }
        }

        public class LocationDataItem
        {
            public static readonly ID TemplateID = new ID("{C52512C1-B7EF-4DDC-B6BB-6BCDDFE3FF84}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{D2BCBA42-8DB4-4BB4-B839-FA0A358E0092}");
                public static readonly ID ComponentId = new ID("{8FD4519C-BBC9-43CF-9ADE-A7FDD0397E38}");
                public static readonly ID Lattitude = new ID("{E06E3642-14E1-4174-A9EE-99AB8495A2C1}");
                public static readonly ID Longitude = new ID("{99F64744-87E2-430B-89F2-BF92C1130D81}");
                public static readonly ID Facilities = new ID("{24F23D29-8B17-4421-B635-18107257C73C}");
                public static readonly ID MapIFrameUrl = new ID("{FEFCB588-AEB5-4BC2-9436-8CE29740B3CC}");
            }
        }

        public class SimilarProjectDataItem
        {
            public static readonly ID TemplateID = new ID("{D06385E8-F2AC-4D7D-9F50-FE860E85076E}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{5BBCE8F8-B389-4C6C-803E-54B91761A854}");

                public static readonly ID Projects = new ID("{40851169-B5CD-4055-941A-B9925116A653}");
            }
        }

        public class SimilarProjctItem
        {
            public static readonly ID TemplateID = new ID("{A4C0BA6D-68F2-4518-860F-CBFC808F1622}");

            public static class Fields
            {
                public static readonly ID Src = new ID("{9876641B-73F1-4338-823B-1C6EDA2BD1E3}");

                public static readonly ID LogoSrc = new ID("{90E99AC3-DF87-47A9-AF6C-B94F7816EE84}");

                public static readonly ID Title = new ID("{0ED4B1E8-8FBF-410E-A424-CB1CB5F419FF}");

                public static readonly ID Type = new ID("{CF56E511-DB33-4A03-987E-5E58DED46E6D}");

                public static readonly ID Status = new ID("{7C1FE620-7F31-47D3-B69B-0E24CF81A4A8}");
                public static readonly ID Link = new ID("{03145397-C471-4E8C-BA2B-69885833A682}");
            }
        }


        public class ProjectFloorPlanDataItem
        {
            public static readonly ID TemplateID = new ID("{30A07946-440A-465D-A8AA-CD09C4F3AAC1}");

            public static class Fields
            {
                public static readonly ID FiledsID = new ID("{5D6A1933-6B74-4AA2-BE35-202F2F87E530}");
                public static readonly ID Heading = new ID("{85D1BCA8-CDEA-4336-9BB8-AEF6BB95B462}");

                public static readonly ID ComponentId = new ID("{67B9EC6E-4008-40D5-A283-15D0F0C766BA}");

                public static readonly ID tabHeading = new ID("{CBDF7F28-1FDE-4C20-B1FF-E7F7A369D5F0}");

                public static readonly ID Src = new ID("{AFC9973B-E007-41CD-BAF4-75ACDCF25F69}");

                public static readonly ID Points = new ID("{8D7E17E7-6A57-4499-99FC-E4A98409021A}");
            }
        }

        public class TypicalUnitPlan
        {
            public static readonly ID TemplateID = new ID("{6F86963F-88BF-44D2-BD26-FC3A0798A4E8}");

            public static class Fields
            {
                public static readonly ID TabList = new ID("{C0FAFEC0-4691-4B2B-8D72-AF05791074C4}");
            }
        }


        public class ProjectFloorPoint
        {
            public static readonly ID TemplateID = new ID("{1BB423D7-E5D7-449C-989F-F5CB5063DF9C}");

            public static class Fields
            {
                public static readonly ID Left = new ID("{2D9D9033-136B-4254-A425-7A455EC27C71}");

                public static readonly ID Bottom = new ID("{3DB3C1F2-5E5B-4CC3-BEBD-B0DB942AEBC9}");

                public static readonly ID Title = new ID("{1B4C6685-439D-4415-8A95-F71CDE9C8E12}");

            }
        }

        public class LivingTheGoodLifeItem
        {
            public static readonly ID TemplateID = new ID("{ADD777FA-AF1B-4703-8A22-806D92016CDB}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{1D989196-D319-4930-B09A-E31692346A89}");

                public static readonly ID Testimonials = new ID("{BF497EC4-57F6-415A-AEB9-5A495350AF3F}");
            }
        }

        public class TestimonialItem
        {
            public static readonly ID TemplateID = new ID("{25E782E3-2C0D-480D-8A57-FE2B9A763036}");

            public static class Fields
            {

                public static readonly ID isVideoTestimonial = new ID("{DBB46B1F-BAF1-4D94-B8F0-30873E4EB724}");

                public static readonly ID useravtar = new ID("{3EC86ED6-04CC-498F-B7FD-05C111FB266C}");

                public static readonly ID useravtaralt = new ID("{AF396EA6-FBD8-4732-9A76-7C3E5AF61BE8}");

                public static readonly ID useravtartitle = new ID("{DF9AF438-309E-4990-855D-927F6E3A149D}");
                public static readonly ID title = new ID("{9E38B9C5-F317-42A1-943F-0CCCF86CB6BE}");
                public static readonly ID Description = new ID("{C8C9F037-81FE-43CC-B653-D6E03533AA7C}");
                public static readonly ID Author = new ID("{CFD1B624-386A-4C86-85A9-FF36F6457BE6}");
                public static readonly ID propertylocation = new ID("{52616EA6-9D7D-40A0-81CA-19AE9414F5A5}");
                public static readonly ID iframesrc = new ID("{67F2D757-7CDD-4859-9741-0EED1227DCA5}");
                public static readonly ID SEOName = new ID("{7F2B0BF2-A5FD-4F12-B1CD-BE99EFD81077}");
                public static readonly ID SEODescription = new ID("{BFDC58B0-3704-42D7-AC12-EC3E086AF380}");
                public static readonly ID UploadDate = new ID("{9F743488-8572-49CA-AC0B-0824F019C04C}");


            }
        }


        public class ProjectUnitPlanDataTemplate
        {
            public static readonly ID TemplateID = new ID("{34003032-5437-4289-A5A9-6DA44E7074E2}");

            public static class Fields
            {

                public static readonly ID ComponentID = new ID("{2E37E521-2E25-4534-A95F-08BE8539D844}");

                public static readonly ID Heading = new ID("{95B8609D-A9DB-4A8C-A4BA-CF8BAF2CE3B9}");

                public static readonly ID TypicalUnitPlanData = new ID("{36088F4F-5EFD-4CB2-B9ED-B4A3DD49CBB3}");
            }
        }

        public class TypicalUnitPlanDataTemplate
        {
            public static readonly ID TemplateID = new ID("{A033C5D0-8988-46BE-871E-3D093256AC2C}");

            public static class Fields
            {
                public static readonly ID title = new ID("{33658A46-D00D-46B3-BCE4-85FC884BFBB8}");
                public static readonly ID desc = new ID("{70DAE5CB-EB31-4650-8BB8-2C968EAB37C7}");
                public static readonly ID Src = new ID("{BFAEFB71-965C-474C-BFF3-A30C4B72CE04}");
                public static readonly ID link = new ID("{417B4A1B-75EB-4DAF-8B0F-DB3B266D57A7}");
                public static readonly ID ButtonText = new ID("{4243516C-8035-4201-BA6B-95F6AE65CD49}");
                public static readonly ID TabHeading = new ID("{85D76D05-7A03-446E-9ADB-079D79729D82}");
                public static readonly ID FloorSpecifications = new ID("{52C10A65-15A4-45BA-8B30-1FEC257BAEF6}");
            }
        }

        public class ProjectUnitPlanDetailsTemplate
        {
            public static readonly ID TemplateID = new ID("{A033C5D0-8988-46BE-871E-3D093256AC2C}");

            public static class Fields
            {

                public static readonly ID SizeIn = new ID("{CF52B1BF-9D5E-4657-B119-D737AF47DF8A}");

                public static readonly ID Type = new ID("{0DF02D25-B039-4C12-970A-3A4A2177495F}");

                public static readonly ID Src = new ID("{F73F893C-0601-4C1F-BF40-D0ECCE83368B}");

                public static readonly ID ImgAlt = new ID("{9BC7D8E3-9D40-40FD-B20D-79BF65260B78}");

                public static readonly ID ImgTitle = new ID("{A7F3CEC7-3672-4943-BFF1-50BA690D33E4}");

                public static readonly ID Specifications = new ID("{BB93F9D0-36C6-4DE5-98C6-C9690F3B0E9A}");

                public static readonly ID AreaAsPerRera = new ID("{30D5F778-3B72-4F80-81E0-EB0D727D2052}");

                public static readonly ID ReraMeasurementScale = new ID("{C219023B-D9F6-4ED7-98E9-ED8BCFD7DD4D}");

                public static readonly ID ReraSpecifications = new ID("{E6405180-3074-4BF7-8C84-FF41F201B3D4}");

            }
        }


        public class SpecificationsTemplate
        {
            public static readonly ID TemplateID = new ID("{37C3F731-99C5-4F52-B8FA-D3C28138A7C8}");

            public static class Fields
            {

                public static readonly ID Place = new ID("{343DDD8E-2FC2-48CB-BA71-FEF58E70ACE5}");

                public static readonly ID SizeInFeet = new ID("{CB1498E0-01D7-4B30-AA09-476C69783D94}");

                public static readonly ID SizeInMetres = new ID("{52259D20-CE08-48F0-9FA7-AFB276304BCA}");

            }
        }

        public class ReraSpecifications
        {
            public static readonly ID TemplateID = new ID("{37C3F731-99C5-4F52-B8FA-D3C28138A7C8}");

            public static class Fields
            {

                public static readonly ID AreaType = new ID("{7AEE4D8B-363B-454A-9C0C-538BCB39ADFD}");

                public static readonly ID Size = new ID("{1F9EFBC4-72E8-4E89-ACB8-EA9F382F51DA}");

            }
        }

        public class ConfigurationKeyTemplate
        {
            public static readonly ID TemplateID = new ID("{D069C4BA-EA1D-4477-93B8-EE97728AE2A4}");

            public static class Fields
            {

                public static readonly ID Link = new ID("{BF82D285-9409-420A-9C98-16C80CAF5471}");

                public static readonly ID Keyword = new ID("{70D26E32-34F7-47A7-8E02-EED8A6A0A36B}");

            }
        }

        public class ConfigurationItemTeplate
        {
            public static readonly ID TemplateID = new ID("{A9E13CC0-C6FE-434A-B7F0-03DE32D7AFCB}");

            public static class Fields
            {

                public static readonly ID Title = new ID("{BB9E2688-3048-4719-AEC3-8ECC4334BDD2}");

                public static readonly ID Key = new ID("{43DEAC72-791E-40A4-A9FC-FF610C75C095}");

            }
        }

        public class ConfigurationDataTeplate
        {
            public static readonly ID TemplateID = new ID("{4CB67E17-C008-4AE3-A397-D69F20B172B6}");

            public static class Fields
            {

                public static readonly ID City = new ID("{A264FD40-D581-4C7C-BD64-B5FD244B1815}");

                public static readonly ID Items = new ID("{B2CDD98B-37AF-4186-99C8-0AE9F9046068}");

            }
        }

        public class LayoutDataTemplate
        {
            public static readonly ID TemplateID = new ID("{7C2773AA-DC8C-4FDB-835A-F68E64C61524}");

            public static class Fields
            {

                public static readonly ID ProjectTitle = new ID("{0C016AC5-6406-4CE7-B53D-07AACDC0370C}");

                public static readonly ID ProjectConfiguration = new ID("{BC141B1A-8083-4B3F-8271-58BD635C138C}");

                public static readonly ID ProjectPossessionInfo = new ID("{374455C4-E837-4253-B169-09F65EEB2FC3}");

                public static readonly ID PlanAVisitLabel = new ID("{18DCE585-5406-4045-A6C8-CA5C79BAE983}");

                public static readonly ID BookNowLabel = new ID("{8921063A-4FEA-471C-9881-1E5B29A1CCCE}");

                public static readonly ID EnquireNowLabel = new ID("{F904EE4E-2DC0-4F14-802F-47432EA63832}");

            }
        }

        public class ProjectData
        {
            public static readonly ID TemplateID = new ID("{C5A34A37-B528-4310-AE83-617A1D22D4B8}");

            public static class Fields
            {

                public static readonly ID DownloadBrochure = new ID("{FF92D4D9-A6AF-46BF-A2EC-50DE164C23B4}");

                public static readonly ID downloadModalTitle = new ID("{A65DD6E1-CB78-4EE0-A58A-1494670FD6DB}");

                public static readonly ID EnquireNow = new ID("{D8AA916F-F811-4183-B7D7-02D6E677485D}");

                public static readonly ID Share = new ID("{49DB74BC-78B5-4EE8-9A01-2295D0BBA76E}");

                public static readonly ID DownloadPDFLink = new ID("{6886227F-33F7-4AB5-9FD4-774BA5987A0C}");
            }
        }

        public class ProjectModelShareTemplate
        {
            public static readonly ID TemplateID = new ID("{4C2637AE-F97E-48BA-8BB9-2DD4DC8E6CCC}");

            public static class Fields
            {

                public static readonly ID Heading = new ID("{5AABC766-6BAA-45F3-850A-3CF48FB8A66F}");

                public static readonly ID Src = new ID("{A535F1C6-B6A6-4C77-82C8-1FD757FF82AC}");
                public static readonly string SrcFieldname = "Src";
                public static readonly ID ImgAlt = new ID("{8FA10318-2C6C-470C-B1FF-44D36E9D8452}");


                public static readonly ID Label = new ID("{FC715D96-D423-4667-B3AF-EE56098A5009}");

                public static readonly ID Location = new ID("{826A3AD7-AE01-4473-A171-C78C8220AEEB}");

                public static readonly ID Copylink = new ID("{887DD68F-0AB2-43D3-A362-E46B36F9A1CE}");

                public static readonly ID Email = new ID("{26473509-9DBD-4A21-8A16-A95FEBA44B3B}");

                public static readonly ID Twitter = new ID("{C462ADCE-9DDF-429C-84A5-78573FD07AE2}");

                public static readonly ID Facebook = new ID("{0852D3E3-C5B0-4337-B6C9-CDF2F4F0979D}");

                public static readonly ID Whatsapp = new ID("{6FD58A8F-2A36-4EEE-8861-2F89BE5EE87F}");
                public static readonly ID PageTitle = new ID("{907D116E-7D09-4A9F-A4A4-38B32E750AD7}");
            }
        }

        public class AdaniData
        {
            public static readonly ID TemplateID = new ID("{2CA3713A-0D27-40F5-8247-D34951DC16B6}");

            public static class Fields
            {
                public static readonly ID ComponentId = new ID("{4F30B2A7-2EDE-4628-AE8A-6DD822BD0790}");
                public static readonly ID AboutData = new ID("{733AA692-F859-46FF-803A-804952ED791A}");
                public static readonly ID ReadMore = new ID("{2302D0DC-4EE9-4E52-8A16-1088404C44D6}");
                public static readonly ID ReadLess = new ID("{AF6D3F43-94E0-43A6-AD29-CD9193F51A14}");
                public static readonly ID description = new ID("{779FE384-892C-4023-B94B-B34791D213D2}");
            }
        }

        public class AboutAdani
        {
            public static readonly ID TemplateID = new ID("{31A5D59D-44CA-4C90-81A3-9EF5E6966A41}");

            public static class Fields
            {
                public static readonly ID TitleHeading = new ID("{49D7BE55-ABBC-4A03-888A-04F7D0A7EBC8}");
                public static readonly ID Desc = new ID("{5516D382-C62D-44DD-8014-82D14A94CE9B}");
                public static readonly ID ReadMore = new ID("{5FF4E735-17D9-4F91-A7F0-9FA0D1980AF2}");
                public static readonly ID Terms = new ID("{5029E25F-E798-44D2-AD0F-9646D7EE42E5}");
            }
        }

        public class AboutNriTemplate
        {
            public static readonly ID TemplateID = new ID("{A4153F10-9E2A-4630-8D81-3EB632F0ECD6}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{084FCFBF-5BB7-437B-BD71-9A2C2783F500}");
                public static readonly ID About = new ID("{C6D7DF5A-E92D-41F5-BBD9-3EFDC2FE0BBC}");

                public static readonly ID ReadMore = new ID("{19D5C351-95B8-47CB-801D-AEAAA8019444}");
                public static readonly ID Terms = new ID("{E7D9279C-3218-4951-8D18-D58BC78EEF5B}");
                public static readonly ID DetailLink = new ID("{867E280C-32F6-49BA-97DD-58FEF879E6B4}");
                public static readonly ID ExtrCharges = new ID("{5206CD09-9542-4398-87EE-01794AE977D7}");
            }
        }

        public class ContentItem
        {
            public static readonly ID TemplateID = new ID("{1F553A17-7E10-44DA-A000-B3D33B5744D1}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{118827F1-EFA7-46DD-93EE-ADC8BD313192}");
                public static readonly ID PageData = new ID("{7513D6DE-78EA-409B-855C-9FDD181FCAC4}");
                public static readonly ID Heading = new ID("{B7879276-70CA-4090-8BF2-E10DE9FA7E55}");
            }
        }

        public class NRIBanner
        {
            public static readonly ID TemplateID = new ID("{A170A64B-4B10-46E0-BB9D-F0B247A85D04}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{98B2819E-939F-451E-84FC-677C9DE65FFA}");
                public static readonly ID Class1 = new ID("{A1E748CC-F52D-44D5-A924-5AEFE281B117}");
                public static readonly ID Class2 = new ID("{6575C4BB-E32C-496F-8CF8-D0A715BF51A7}");
                public static readonly ID Src = new ID("{BA670385-EF32-47CF-BDCE-380A1B6687A9}");
                public static readonly ID alt = new ID("{B534AC28-BD75-47FF-A005-40A61275BAFA}");

            }
        }

        public class ArticleDataItem
        {
            public static readonly ID TemplateID = new ID("{641A58E4-4EBA-42FE-9A4D-68FB00C3F820}");

            public static class Fields
            {
                public static readonly ID Src = new ID("{2656DD79-360A-4868-BC53-25921CB5E810}");
                public static readonly ID Title = new ID("{331D49B4-95BF-45A3-8737-875200920513}");
                public static readonly ID Date = new ID("{7CFA768D-9167-4CD5-A66D-F9222F8BEF43}");

            }
        }

        public class ArticleItem
        {
            public static readonly ID TemplateID = new ID("{5E9B3F1C-1D4A-47A1-80DF-B61BB5157F3D}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{302E40DB-956D-4928-973F-B95D5AA7DA9D}");
                public static readonly ID Data = new ID("{72B14343-A00B-480E-889A-6A2534C5B7E2}");

            }
        }

        public class InvestmentGuidelineItem
        {
            public static readonly ID TemplateID = new ID("{50C899D5-47A7-40D2-B4FF-71965030CB27}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{8155C80F-E882-4328-AEFD-51FD83F19410}");
                public static readonly ID Content = new ID("{DFD35AB3-B41A-468A-A8FA-D7ABD0229B04}");
                public static readonly ID readMore = new ID("{5D455FD8-B325-4092-8557-BF38EC43D4D4}");

            }
        }


        public class WhyInvestItem
        {
            public static readonly ID TemplateID = new ID("{9D105D5F-7D02-456B-AAC5-EB2732775C68}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{D4B163B6-77CB-4C43-B50E-E4F8CC2A2497}");
                public static readonly ID About = new ID("{A18FF1DD-C244-4F77-B2DE-0F9252312DE4}");
                public static readonly ID ReadMore = new ID("{7FC476A7-FBC6-4678-8525-2B6AADB1C9EC}");
                public static readonly ID Benefits = new ID("{863F332C-A2D3-40EB-B5E5-41556A6F6E19}");
            }
        }

        public class WhyInvestBenefitItem
        {
            public static readonly ID TemplateID = new ID("{1E8D03C1-B7EF-4AA6-AF22-F576144A34A4}");

            public static class Fields
            {
                public static readonly ID Icon = new ID("{2690440E-5136-4032-8C46-424FBD366AEC}");
                public static readonly ID Title = new ID("{ED9A6087-C397-402A-BC22-03099A4C4EC2}");
                public static readonly ID Desc = new ID("{2376B9B4-F4EE-4930-BE9B-9DA9996FA423}");
            }
        }

        public class NRITestimonialItem
        {
            public static readonly ID TemplateID = new ID("{CFA27A72-C41A-4791-9BDE-6380835F327E}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{779C033B-0FF6-45A8-988F-2E1F499DEFB9}");
                public static readonly ID TestimonialData = new ID("{A636D06A-6443-4FC4-9810-A3845FAF2B5F}");
            }
        }

        public class OurLocationItem
        {
            public static readonly ID TemplateID = new ID("{E46AE67B-F8C7-4E1E-842E-A0B4C598A28B}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{E666914E-7BC5-4FE4-A29B-BCF9FF7F0D0D}");
                public static readonly ID Data = new ID("{DF80C217-C512-4DA9-93ED-0C98CC14B272}");
            }
        }

        public class OurLocationDataItem
        {
            public static readonly ID TemplateID = new ID("{4F0AACF4-E846-4F01-968F-81C5C2A1E899}");

            public static class Fields
            {
                public static readonly ID CityName = new ID("{6CAE02B0-629C-416D-AA50-1EE921ECABE6}");
                public static readonly ID About = new ID("{496BFBBD-385B-4109-AEA9-760831F1BDBC}");
                public static readonly ID ReadMore = new ID("{8DB09BEC-B066-4635-966B-AC8470B14152}");
                public static readonly ID Feature = new ID("{7299A248-F05A-4DE1-A94C-15A6FEC2E6F6}");
            }
        }

        public class OurLocationFeatureItem
        {
            public static readonly ID TemplateID = new ID("{D52FCB7E-1F19-4254-B7D8-6F9BDB09DD58}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{7DA686C9-7B29-4FF2-B7AB-43A679E87445}");
            }
        }

        public class OurProjectItem
        {
            public static readonly ID TemplateID = new ID("{93D3609E-506F-4BD3-946D-4A726580B33D}");

            public static class Fields
            {
                public static readonly ID Heading = new ID("{A5700544-0B97-4B00-A96B-63BFC0A2FA6E}");
                public static readonly ID Data = new ID("{9A708972-1030-489F-9304-7C72BD187716}");
            }
        }

        public class PropertTypeItem
        {
            public static readonly ID TemplateID = new ID("{161F79B5-D5D9-40A8-8C36-8CEEDA60A539}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{AA18B73D-997E-4EE8-9018-9B964F26A435}");
            }
        }

        public class ProjectItem
        {
            public static readonly ID TemplateID = new ID("{B9F711F0-E192-42F5-9BAA-24839A3F1E3A}");

            public static class Fields
            {
                public static readonly ID Projecttitle = new ID("{2C33F676-E336-44ED-A1EE-FB48F96B2BBC}");
                public static readonly ID Projectprice = new ID("{571D912F-5A7A-4E0A-8C21-427533279BA0}");
                public static readonly ID Link = new ID("{9F42D4B1-91E1-4ECB-941E-B7FC65CCE1B6}");
                public static readonly ID propertyType = new ID("{5365E814-0AE8-47EF-AF32-350DDB73AF5A}");
            }
        }

        public class ProjectDataItem
        {
            public static readonly ID TemplateID = new ID("{3F4A860D-C5FC-4247-998D-82F1AEC8D945}");

            public static class Fields
            {
                public static readonly ID projectlist = new ID("{2F6B0ACF-244D-4DF4-B9B6-162C9F022B47}");
                public static readonly ID projectcity = new ID("{F2223464-A5DA-43EB-B559-7C7EBB2CB4D0}");
                public static readonly ID Link = new ID("{A349FAD8-CBBF-43AC-9FDB-1E678314C379}");
                public static readonly ID propertyType = new ID("{23F8E758-9BD4-4D81-BCEB-C02089C4F639}");
                public static readonly ID projectTitle = new ID("{85A8463A-191D-4B46-8452-AE28534844E7}");

                public static readonly ID src = new ID("{164C71C2-E0CE-4857-9F3C-A51648D92356}");
                public static readonly ID imgalt = new ID("{157689DB-1CDE-40B3-B27F-23236BF3DA97}");
                public static readonly ID imgtitle = new ID("{25E62103-81BF-43B2-9DF1-4275350A2860}");

                public static readonly ID title = new ID("{D3DFBEEC-923D-4663-8AF3-042EEB9BB928}");
                public static readonly ID SeeAllLink = new ID("{1F3C306A-6A8F-4696-97B0-419C43EC7351}");
            }
        }

        public static class MapLocationTemplate
        {
            public static readonly ID TemplateId = new ID("{F675AF83-C889-4851-8B25-0F7AB7C12BE6}");
            public static class Fields
            {
                public static readonly ID Latitude = new ID("{D04153A0-E7B0-43A8-911A-6BB9C6EC8326}");
                public static readonly ID Longitude = new ID("{E9E067E6-9B78-4BFC-B6AD-1D9791D8587A}");
                public static readonly ID MapUrl = new ID("{7DEB9B13-8A45-46EB-B2AC-83741938D775}");
            }
        }
        public static class AddressTemplate
        {
            public static readonly ID TemplateID = new ID("{75434EAA-4DBD-4DE4-A7C8-5F86BA73277E}");
            public static class Fields
            {
                public static readonly ID Address1 = new ID("{858CBBFC-6ABE-438D-A845-46F527679DE3}");
                public static readonly ID Address2 = new ID("{A2D902CB-EFEF-4F78-88D3-92A2CB1A79DD}");
                public static readonly ID City = new ID("{55ECAD35-5967-4473-A389-7D605722159D}");
                public static readonly ID State = new ID("{5A5F8386-71D7-4CAE-9E45-8C570F9EF697}");
                public static readonly ID Country = new ID("{AFE98AA5-A220-4A60-A9DF-A51A0822FFD9}");
                public static readonly ID Pincode = new ID("{940D7E07-402A-4F21-977E-9C342C6F415D}");
                public static readonly ID Contact = new ID("{289E1680-C3E8-43AF-869A-BA6AD7F52590}");

            }
        }
        public static class CityTabstemp
        {
            public static readonly ID TemplateID = new ID("{F1048961-A1CA-434C-B0DA-A98CBF989DE2}");
            public static class Fields
            {
                public static readonly ID title = new ID("{80878276-ABB6-48F9-9F5F-769524BD6981}");
                public static readonly ID link = new ID("{B7C4D60D-072F-47CA-8773-0F23D7EB5904}");
                public static readonly ID certificateID = new ID("{9915D670-3DED-45C9-B5CC-D398F4D2C3C2}");
                public static readonly ID ReracertificatesID = new ID("{7D38B88F-BE47-4403-AC53-9BADF835D240}");
                public static readonly ID EnvCertificateID = new ID("{EC97268F-C702-40DF-8426-A2425CA62FD5}");
            }
        }
        public static class PropertyTypeTemp
        {
            public static readonly ID TemplateID = new ID("{34C17A80-9AA1-456B-A5B9-CD89256BCABD}");
            public static class Fields
            {
                public static readonly ID heading = new ID("{844090E6-397F-42D7-BFF3-AEC1B05C7C06}");
                public static readonly ID Properties = new ID("{0AEC3472-C1B7-49D2-90BA-5884E7D9B942}");
            }
        }


        public static class AanganResultTemp
        {
            public static readonly ID TemplateID = new ID("{27339781-78A5-4EA5-A736-FE4DA121E6D5}");
            public static class Fields
            {
                public static readonly ID id = new ID("{B849A70C-6EC8-4979-81AD-2E4E75BDD17D}");
                public static readonly ID heading = new ID("{5318C442-4310-46EB-A4D8-A31AA6C6CA25}");
                public static readonly ID imageSource = new ID("{605A3148-85CA-4017-BDE6-9DC1B9D78196}");
                public static readonly ID imageSourceMobile = new ID("{2C8FC6F8-189D-4A50-91A6-210B9BD7D354}");
                public static readonly ID imageSourceTablet = new ID("{F91A4FAA-E271-4B93-A12B-EBBB233EF18B}");
                public static readonly ID imgAlt = new ID("{65885C44-8FCD-4A7F-94F1-EE63703FE193}");
            }
        }


        public class NRIRatingSchemaTemp
        {
            public static readonly ID TemplateID = new ID("{8B86AC2F-35EC-4C90-8B8C-D593EA5557EE}");

            public static class Fields
            {
                public static readonly ID ratingSchema = new ID("{7F00A90B-8BA7-419E-91E7-DFBD0FD48CEA}");
            }
        }
    }
}



