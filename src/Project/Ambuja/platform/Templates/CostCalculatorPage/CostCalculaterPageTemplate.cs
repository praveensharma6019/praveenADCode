using Sitecore.Data;

namespace Project.AmbujaCement.Website.Templates
{
    public class CostCalculaterPageTemplate
    {
        public class CostCalculatorLabelsFields
        {
            public static readonly ID HeadingLabel = new ID("{53A0EC37-8EB5-495B-8E95-61467D6FCC0E}");
            public static readonly ID HeadingIcon = new ID("{1BDB56AD-D48B-4447-B2AC-B9D2330D9463}");
            public static readonly ID SubmitButtonLabel = new ID("{5B50B445-A2B4-462E-B624-F46C5E7CB62A}");
            public static readonly ID EditButtonLabel = new ID("{8E250C9A-CC19-4735-B451-185C5CB13B5B}");
            public static readonly ID TotalAreaLabel = new ID("{C4BE619C-D1EC-4779-BFA4-EA1DB0E8E9A5}");
            public static readonly ID AreaLabel = new ID("{6E681E9E-E09A-4FF7-8A56-57F41B3B9EDE}");
            public static readonly ID DownloadEstimateLabel = new ID("{A01084B2-4E6D-4D25-90D5-012530D5DA0F}");
            public static readonly ID DownloadEstimateIcon = new ID("{695FD6CE-4872-40CA-B699-3BB0B8038124}");
            public static readonly ID GroundFloorLabel = new ID("{1141BF9F-174F-43F2-B580-3F72AAB25F9C}");
            public static readonly ID FirstFloorLabel = new ID("{97F40971-74D8-4FC0-B2E0-F785353E164F}");
            public static readonly ID SecondFloorLabel = new ID("{E57A57C9-2FDA-4521-A51C-BABE4884ADAB}");
            public static readonly ID ThirdFloorLabel = new ID("{9A6CD9B7-9C3B-42BD-82CE-ACDCBC202228}");
            public static readonly ID HomeHeadingLabel = new ID("{466EEEA8-D209-45DA-82FE-232F882A4F87}");
            public class PDFDataLabels
            {
                public static readonly ID PdfHeading = new ID("{31456CB3-2AEF-449D-8DC6-F9ADE11036D7}");
                public static readonly ID PdfMaterialLabel = new ID("{1EAFC7FC-60A2-4BED-8DBF-CADB42E9B874}");
                public static readonly ID PdfPricePerUnitLabel = new ID("{0640225C-18BB-4B2C-82DC-03A7BAAD016F}");
                public static readonly ID PdfQuantityLabel = new ID("{FE2607B5-D144-4383-B261-AD65D5F04EF8}");
                public static readonly ID PdfTotalCostLabel = new ID("{E6A2C2AF-B617-4134-8397-EFC861CA6F8B}");
                public static readonly ID PdfFileName = new ID("{D800292E-1171-441C-B24A-D77146790510}");
            }
        }

        public class TabDataButtonTabs
        {
            public static readonly ID Label = new ID("{BB81E884-8E0D-4FC9-B077-A0F21D3A664A}");
            public static readonly ID Id = new ID("{1659E8A1-69DA-4261-89CF-1214E5BCAE9C}");
            public static readonly ID InitiallyChecked = new ID("{1D493986-C07A-4E27-9F77-771915F1F5E8}");
        }
        

        public class TabDataDetail
        {
            public static readonly ID Type = new ID("{5B427951-D05C-4B3F-86B6-3AE4BC61AC1B}");
            public static readonly ID Label = new ID("{AAADBE22-CE87-4B02-BB8B-26DD8953EBCF}");
            public static readonly ID SubTitle = new ID("{848558D8-8C1B-46A2-B489-9B198DE71A64}");
        }

        public class TabDataFields
        {
            public static readonly ID Title = new ID("{B7F8C0BE-39EA-41F5-90C4-A88B3C6C6DF8}");
            public static readonly ID Description = new ID("{EA82288C-5154-4095-A205-0E89A6F2AD55}");
        }

        public class TabDataInputTabsFields
        {
            public static readonly ID Placeholder = new ID("{7F71F3A6-B318-4301-8617-D7C4EEC6DE36}");
            public static readonly ID Label = new ID("{982F3B55-22BE-4197-8CD1-FD7F5CF644B9}");
            public static readonly ID FieldName = new ID("{5BDF9C87-7A50-434B-9BC1-ADC0B3B82FD4}");
            public static readonly ID ErrorMessage = new ID("{53E36CF9-113F-43A8-8C22-014C9D56E4B6}");
            public static readonly ID Type = new ID("{982F3B55-22BE-4197-8CD1-FD7F5CF644B9}");
        }

        public class TabDataInputTabsOptionsFields
        {
            public static readonly ID Id = new ID("{DD1561B4-0139-4BBE-ACB7-17B274F08DD4}");
            public static readonly ID Label = new ID("{F3C8BD75-5388-4782-8B3F-955924FE70E8}");
        }

        public class TabDataLabelsFields
        {
            public static readonly ID SubmitButtonLabel = new ID("{DBD2D3D5-48F0-4A95-AD38-DD9C32ED4C8C}");
            public static readonly ID HeadingLabel = new ID("{394BD326-2FAB-42C3-A813-709B6EC846DB}");
            public static readonly ID MaterialLabel = new ID("{CCE989E1-5199-4FFB-8212-DFE06ADD6F6D}");
            public static readonly ID PricePerUnitLabel = new ID("{8FE9F7CB-FDED-4E11-8774-796C6C265CB5}");
            public static readonly ID QuantitytLabel = new ID("{232A4962-8E81-481A-9D75-24034E7509BE}");
            public static readonly ID TotalCostLabel = new ID("{560883A0-D0AD-4519-9764-6A8FA7004DC4}");
            public static readonly ID DefaultActiveKey = new ID("{46007E04-E24B-4148-8B67-A648D82251A4}");
            public static readonly ID PriceLabel = new ID("{C98129E7-3137-49AF-8360-CB4B9414DE60}");
            public static readonly ID HomeSubmitButtonLabel = new ID("{85720853-D94A-495D-B6B4-AFA99277C5C6}");
            public static readonly ID HomeHeadingLabel = new ID("{B9BDA219-D776-42F1-B5E5-D8AE9E962D50}");
            public static readonly ID HomeDefaultActiveKey = new ID("{9A562707-4ED9-454D-9687-9A9CA1F88D54}");
        }
        public class CostCalulatorStaticFields
        {
            public static readonly ID TabDataFields = new ID("{014F4CDB-5761-445B-AC7F-E3E5AF7EF5C5}");
            public static readonly ID TextDataItem = new ID("{370667D2-0F07-48BF-A577-02C9831A085F}");
            public static readonly ID TabDataInputTabs = new ID("{2236EAA4-0F28-4CC5-B155-A0F2E44E1C26}");
            public static readonly ID CostCalculatorlabels = new ID("{AE182166-92B0-4623-8E08-4EFFAA91C307}");
            public static readonly ID TabDataSubmitButton = new ID("{AC860757-7952-4C4A-A062-6471A73CF647}");
            public static readonly ID DataSubmitButton = new ID("{C134F8EE-138B-48EC-BFC3-D6E42CBDB6AD}");
        }
        public class TabDataInputTabsActiveTabIdsFields
        {
            public static readonly ID IdField = new ID("{D7C9559A-919A-49A4-A5C0-54519A04C8C0}");
        }
    }
}