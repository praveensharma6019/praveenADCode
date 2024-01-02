using Sitecore.Data;

namespace Project.AAHL.Website.Templates
{
    public static class BaseTemplate
    {
        public static class Fields
        {
            public static readonly ID CTAText = ID.Parse("{4A0FA205-C61D-4B42-B781-484EF160B934}");
            public static readonly ID CTALink = ID.Parse("{CD11DC88-B124-4305-A3FF-13A525548557}");
            public static readonly ID IconImage = ID.Parse("{44F2FCA1-A25F-4BDC-848A-6E9C478BA2E3}");
            public static readonly ID IconAltText = ID.Parse("{935A3916-C56F-4B25-ABA3-6201B3EB9297}");
            public static readonly ID IconClass = ID.Parse("{21C1F066-CB51-47E5-ADEC-9361199BD1F4}");
            public static readonly ID Target = ID.Parse("{FF49D12F-CABB-49C7-B69A-56C771396792}");
            public static readonly ID Heading = ID.Parse("{A622E6DD-1E44-425B-B083-0902BA23E0A4}");

            public static readonly ID Title = ID.Parse("{348AE1AD-E697-4593-B7A3-EA80B998F693}");
            public static readonly ID SubTitle = ID.Parse("{D4EAC37B-193B-47CA-B9DD-37A8751A8034}");
            public static readonly ID Description = ID.Parse("{2750E46C-2235-49A5-908F-D7249141C798}");
            public static readonly ID Image = ID.Parse("{AF4926AD-13C5-49FF-87E0-BBD96E396FA1}");
            public static readonly ID MobileImage = ID.Parse("{AF564FBF-96F3-4562-B60D-1735C1AB307F}");
            public static readonly ID TabletImage = ID.Parse("{BD20A019-C432-463B-861D-8D994549E95C}");
            public static readonly ID Imgalttext = ID.Parse("{F95A073D-651F-45E9-8261-E01363F523F6}");
            public static readonly ID Link = ID.Parse("{CC6B9480-FE94-4853-AFE7-541E760BDB0B}");
            public static readonly ID Date = ID.Parse("{1E31B540-D90D-4039-95C2-3B6BF20D0717}");
            public static readonly ID BtnText = ID.Parse("{705906A3-C929-4E22-AB23-AA1417BBFC56}");
            public static readonly ID Direction = ID.Parse("{2D4E233F-6A39-4DA2-A735-60BC1B237478}");
            public static readonly ID GridSize = ID.Parse("{F0215DDD-9A18-4AC6-8B13-C4B7068427E4}");
            public static readonly ID WidgetType = ID.Parse("{1E297F52-2E2B-4857-9B5C-18DB1EC6EDBF}");
            public static readonly ID WidgetSubType = ID.Parse("{D77E4484-B068-498B-9ACB-A04186C8055C}");
            public static readonly ID Theme = ID.Parse("{336831EA-F953-4FCE-836E-FA8AF1957846}");


        }

        public static class HomeMetaData
        {
            public static readonly ID Keywords = ID.Parse("{8A39488B-586D-41A0-9078-3C6DB5A8B5C1}");
            public static readonly ID Canonical = ID.Parse("{FF88D55B-B2AF-466A-9D23-61A338AC8827}");
            public static readonly ID Robots = ID.Parse("{4710842A-443F-4679-9557-8058D701DBCC}");
            public static readonly ID ImageAltTag = ID.Parse("{32C4C554-7451-475A-8BB1-210A90706E82}");
        }
        public static class HomeAirportStats
        {
            public static readonly ID Value = ID.Parse("{ECB78285-EAC2-4DC4-8037-4DE53BD1B731}");
            public static readonly ID Sign = ID.Parse("{C8314E12-C6CE-4DAF-8ED2-4AE24C73429D}");
        }

        public static class DataSourceID
        {
            public static readonly ID SocialIconSourceID = ID.Parse("{D6C56137-389A-4F92-A11A-AD3F14B8152A}");
            public static readonly ID FooterMainNavigationSourceID = ID.Parse("{753D40DD-5043-4331-ACBF-5FD42CADB01C}");
            public static readonly ID CopyRightSourceID = ID.Parse("{14562308-23CB-40C3-87A4-385399BDA7E9}");
        }

        public class ImageSourceTemplate
        {
            public static readonly ID ImageSourceFieldId = new ID("{AF4926AD-13C5-49FF-87E0-BBD96E396FA1}");
            public static readonly ID ImageSourceMobileFieldId = new ID("{AF564FBF-96F3-4562-B60D-1735C1AB307F}");
            public static readonly ID ImageSourceTabletFieldId = new ID("{BD20A019-C432-463B-861D-8D994549E95C}");
            public static readonly ID ImageAltFieldId = new ID("{F95A073D-651F-45E9-8261-E01363F523F6}");
            public static readonly ID ImageTitleFieldId = new ID("{2AF85C78-3BCF-41AD-B152-53324DC0A744}");
        }
        public class HeadingTemplate
        {
            public static readonly ID HeadingFieldId = new ID("{A622E6DD-1E44-425B-B083-0902BA23E0A4}");
            public static readonly ID SubHeadingFieldId = new ID("{5763BFFD-9612-423C-9AAF-AB782D0F03DC}");
        }
        public class TextTemplate
        {
            public static readonly ID TextFieldId = new ID("{4A3C1440-ED77-488C-89AB-33ACF5506CBD}");
        }
        public class IsactiveTemplate
        {
            public static readonly ID IsactiveFieldId = new ID("{AB24DD1E-015B-4AF9-8B31-A235C0BE6F32}");
        }

        public class DescriptionTemplate
        {
            public static readonly ID DescriptionFieldId = new ID("{2750E46C-2235-49A5-908F-D7249141C798}");
            public static readonly ID SubDescriptionFieldId = new ID("{7BBB56C3-8FC5-4B9C-BF90-A1DBD3C99B3F}");
        }

        public class LinkTemplate
        {
            public static readonly ID LinkTextFieldId = new ID("{3E03BEEB-7360-4F74-903B-9227816683D3}");
            public static readonly ID LinkUrlFieldId = new ID("{CC6B9480-FE94-4853-AFE7-541E760BDB0B}");
            public static readonly ID TargetFieldId = new ID("{FF49D12F-CABB-49C7-B69A-56C771396792}");
        }
        public class TitleTemplate
        {
            public static readonly ID TitleFieldId = new ID("{348AE1AD-E697-4593-B7A3-EA80B998F693}");
            public static readonly ID SubTitleFieldID = new ID("{D4EAC37B-193B-47CA-B9DD-37A8751A8034}");
            public static class BannerAdsTemplate
            {
                public static readonly ID ItemAlignment = ID.Parse("{8D0F7406-C835-4A24-A960-09312A28C4E5}");
            }
        }

        public class HeaderTemplate
        {
            public static readonly ID ColoredLogo = new ID("{96ED374F-A3AE-48A5-9A65-CE23D7371128}");
            public static readonly ID DefaultLogo = new ID("{C19FBD49-92D6-4AC7-9BDA-C5FF786F1A65}");
            public static readonly ID ColoredLogoIconClass = new ID("{3745648F-0BCD-4B5B-A245-BEA22A765BBD}");
            public static readonly ID DefaultLogoIconClass = new ID("{F19A5F60-F24C-4B64-890C-011E89D8689E}");

            public static readonly ID BusinessesMenuList = new ID("{3920657B-532C-45EF-8F97-6928E7F76F1E}");
            public static readonly ID TopNavigationList = new ID("{366346D5-03DC-42DD-903F-FA01CC729892}");
            public static readonly ID Brand = new ID("{19F6DA4C-38D8-47DF-9324-242F79783D45}");
            public static readonly ID Search = new ID("{4E6A93DD-BA32-413D-B8C1-76BD73877218}");

            public static readonly ID PrimaryHeaderMenu = new ID("{B20AEF15-82A9-4876-83D4-5F51C7743A1E}");
            public static readonly ID PrimaryHeaderMenus2 = new ID("{6E09098E-A2A3-4E91-B7F1-1270330EF315}");

            public static readonly ID PrimaryHeaderMenus = new ID("{EF3A7E31-9F2E-497F-B80C-011669933C10}");
            public static readonly ID PrimaryHeaderMenusCard = new ID("{3626CE78-E9F1-4DD2-8B5E-DB58B9F325CB}");
            public static readonly ID PrimaryHeaderMenusitems = new ID("{DD49786C-D46F-422E-A231-D74E3C1F2FFC}");
            public static readonly ID PrimaryHeaderMenusitemsitems = new ID("{31D539D6-0A59-41CA-84FF-EF16D63D04E2}");
            public static readonly ID PrimaryHeaderMenumessages = new ID("{3DBBDB8B-56AC-4EEE-BAEB-D684124937CD}");

        }

        public static class WriteToUsFormTemplate
        {

            public class TemplatesSection
            {
                public static readonly ID NameField = new ID("{524F2B96-7D61-4DD4-BCCE-4020588BFD48}");
                public static readonly ID Email = new ID("{83F090FB-AA85-46C8-AF17-390A664BAB0F}");
                public static readonly ID CountryCodeDropDown = new ID("{77B1BB90-2F79-4532-8F98-314D402919BB}");
                public static readonly ID MobileNumber = new ID("{6DF9E672-C4EF-4BF7-B85F-A96BC60EA306}");
                public static readonly ID EnquiryType = new ID("{E03E53F3-88EE-469A-BB33-F4A0EFC63FB4}");
                public static readonly ID Message = new ID("{0A4188AB-D6FB-4B3E-90E8-77BD7D59184E}");
                public static readonly ID GoogleRecaptchaV3 = new ID("{60D519C6-D318-4725-A5FE-8D45E4B3AAD8}");
                public static readonly ID Submit = new ID("{B0121BDA-AB00-45E9-9274-34C75C830532}");

                public static readonly ID ErrMsgfieldItem = new ID("{1F305C44-CA38-40BE-83A9-F0719F57271B}");
                public static readonly ID ThankYouDatafieldItem = new ID("{01C0DEDA-2EC2-47FB-9DAA-9172EA55C21F}");
                public static readonly ID ReCaptchaFieldfieldItem = new ID("{DBFEBA05-76DF-468E-B4C9-085F7C512626}");
                public static readonly ID FormGTMDatafieldItem = new ID("{FA9589EF-B634-402A-8DFD-BCD06C3FCCBC}");
                public static readonly ID ProgressDatafieldItem = new ID("{9E270F27-7769-4F47-AB20-A45AEE1C17E8}");
                public static readonly ID FormFailDatafieldItem = new ID("{BCA20D9B-5C43-4F13-97EB-0274BD8478A7}");
                public static readonly ID ThemeDatafieldItem = new ID("{D72CDD1B-98F8-4E64-B183-3DB25986CB94}");

                public static readonly ID EnquiryTypeItem = new ID("{274AB286-53BE-4322-AC72-7B7B36383AAC}");
                public static readonly ID CountryCodeDropdownFolderItem = new ID("{4591A302-2601-4B5E-B9CA-1723CC0AE107}");
            }

            public class FormFieldsSection
            {
                public static readonly ID PlaceholderText = new ID("{A6972F08-CBB5-4830-A8C9-8A0AE34650D1}");
                public static readonly ID minRequiredLength = new ID("{9F7DA002-7B86-436B-8DDA-7EFE5837B88E}");
                public static readonly ID maxAllowedLength = new ID("{71061C01-A495-4BF3-A52C-D378346BFAE2}");
                public static readonly ID DefaultValue = new ID("{B775BCA9-2605-4D60-B367-A0C354BE2504}");
                public static readonly ID Required = new ID("{8A43AF61-0812-4B37-A343-96CDE3F12BB1}");
                public static readonly ID Title = new ID("{71FFD7B2-8B09-4F7B-8A66-1E4CEF653E8D}");
                public static readonly ID DefaultSelection = new ID("{21ED172D-97E2-4AC4-8EAF-0A8860B891F4}");
                public static readonly ID fieldOptionslabel = new ID("{3A07C171-9BCA-464D-8670-C5703C6D3F11}");

                public static readonly ID MaxFileSize = new ID("{EF7B8D9E-2C05-4EA3-B0A6-8A0B620B7C29}");
                public static readonly ID FileSizeUnit = new ID("{76A13EA6-D06D-4CFD-AB5B-8252DD716E17}");
                public static readonly ID MaxFileCount = new ID("{AF4F2BA2-1284-40E4-8572-5ADADAECC393}");
                public static readonly ID AllowedContentTypes = new ID("{41FE1C8B-64AD-423B-BA86-471389588968}");


                public static readonly ID CheckboxDefaultValue = new ID("{3C05A626-A3B1-4CBC-BF67-198537A13E20}");
                public static readonly ID ValueProviderParameters = new ID("{F6696439-2DAC-4183-8F3D-1979188D8336}");

            }


            public class ErrorMessagesSection
            {
                public static readonly ID MaxLengthErrorMessage = new ID("{2FA9A86C-9AE8-48E8-9536-92CC91A3364B}");
                public static readonly ID MinLengthErrorMessage = new ID("{7E4D1EDA-E5DC-4ACC-BB92-F51D9CFB7930}");
                public static readonly ID RegexErrorMessage = new ID("{B556222E-A1D2-4BED-9262-2230B8EC149A}");
                public static readonly ID RequiredFieldErrorEmail = new ID("{C5CE3098-7D82-47FA-A02A-5FDCCDA5E83E}");
                public static readonly ID RequiredFieldErrorEnquiryType = new ID("{06D914CA-63C9-4FB3-B499-5542598534BF}");
                public static readonly ID RequiredFieldErrorCountryCode = new ID("{83744CAA-9B86-409C-86BC-44715FB0E092}");
                public static readonly ID RequiredFieldErrorMessageField = new ID("{96429CF6-15F8-423B-B666-D2B0FF6E669C}");
                public static readonly ID RequiredFieldErrorMobileNumber = new ID("{7F1304DF-0511-4777-89BC-1BEB21F0A815}");
                public static readonly ID RequiredFieldErrorName = new ID("{1CB0E9EA-3835-46AA-AAD5-115238D50949}");
                public static readonly ID RequiredFieldErrorMessagereCaptcha = new ID("{07ACA679-7654-4BEE-871B-0ECEBCD4FD78}");
            }

            public class ThankYouDataSection
            {
                public static readonly ID HeadingFieldId = new ID("{A622E6DD-1E44-425B-B083-0902BA23E0A4}");
                public static readonly ID DescriptionFieldId = new ID("{2750E46C-2235-49A5-908F-D7249141C798}");
            }

            public class FormGTMDataSection
            {

                public static readonly ID GtmCategory = new ID("{CFD40328-E8C1-4A42-8920-18115EDA83D6}");
                public static readonly ID GtmSubCategory = new ID("{46C9D898-6205-4151-B4CF-CE2D0A63E6BB}");
                public static readonly ID GtmpageType = new ID("{980A512F-54D9-47B8-A1B1-0375B55E7356}");
                public static readonly ID GtmEvent = new ID("{7A25BFA8-92C3-4A1C-B833-9D2F13E01FAE}");
                public static readonly ID GtmTitle = new ID("{378637CF-76D7-4B18-89B0-ED1F803D3D43}");
            }

            public class ReCaptchaFieldSection
            {
                public static readonly ID Title = new ID("{348AE1AD-E697-4593-B7A3-EA80B998F693}");
                public static readonly ID SubTitle = new ID("{D4EAC37B-193B-47CA-B9DD-37A8751A8034}");
                public static readonly ID Heading = new ID("{A622E6DD-1E44-425B-B083-0902BA23E0A4}");
                public static readonly ID errorMessages = new ID("{15B2D475-5393-4421-9B40-F2F43E1EB2F8}");
                public static readonly ID Active = new ID("{AB24DD1E-015B-4AF9-8B31-A235C0BE6F32}");
            }

            public class CountryCodeOptionDetails
            {
                public static readonly ID CountryNameFieldId = new ID("{09B1E4DB-F011-47F2-B1D4-351E695DDAA2}");
                public static readonly ID DialCodeFieldId = new ID("{D95CFB5C-7589-422F-8977-1C97D6116DCD}");
                public static readonly ID IsO3FieldId = new ID("{87675EE4-544E-4AC0-AC73-93E9952E1E7B}");
                public static readonly ID IsO2FieldId = new ID("{FAD24220-488D-4968-986D-E26EF9B83879}");
                public static readonly ID CountryFlagFieldId = new ID("{FBBE6D17-A1C3-4EC4-B964-47B57AD4CD87}");
            }

        }
        public class OurMissionSection
        {

            public static readonly ID ImageList = new ID("{B2A7B251-7605-4761-840D-98627FA30B54}");
            public static readonly ID ItemsList = new ID("{1F9F82D6-EBF3-4130-BEFA-7F06D611D4B7}");
        }

        public class Sustainability
        {
            public static readonly ID Sustainabilitys = new ID("{61B0C557-258D-4D64-B8BD-E3D8AC66AEE7}");
            public static readonly ID BannerDetails = new ID("{56D207C5-206D-48F3-A48F-C16AA551985C}");
            public static readonly ID Common = new ID("{BC45717E-0734-40F4-82A4-11E05BF1F260}");
            //   public static readonly ID WasteManagement = new ID("{79D30E64-1DC5-411D-8D85-C1FBD65D67CF}");
            public static readonly ID DecarbonisationCard = new ID("{272D1EF5-4F9A-47F7-8A26-1D7D54B362D7}");
            public static readonly ID SustainabilityStories = new ID("{44129508-417F-4B46-9496-41DB22351922}");
            //  public static readonly ID SustainabilityStoriesItem = new ID("{C664A1AC-3783-4B09-9A28-06D5A7415144}");
        }
    }
}
