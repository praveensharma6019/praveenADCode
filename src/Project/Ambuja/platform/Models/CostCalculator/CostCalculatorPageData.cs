using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.CostCalculator
{
    public class CostCalculatorPageData
    {
        public Labels Labels { get; set; }
        public List<TabDatum> TabData { get; set; }
        public TextData TextData { get; set; }
    }
    
    public class Labels
    {
        public PDFLabels PdfLabels { get; set; }
        public string HeadingLabel { get; set; }
        public string HeadingIcon { get; set; }
        public string SubmitButtonLabel { get; set; }
        public string EditButtonLabel { get; set; }
        public string TotalAreaLabel { get; set; }
        public string AreaLabel { get; set; }
        public string DownloadEstimateLabel { get; set; }
        public string DownloadEstimateIcon { get; set; }
    }

    public class PDFLabels
    {
        public string PdfHeading { get; set; }
        public string PdfMaterialLabel { get; set; }
        public string PdfPricePerUnitLabel { get; set; }
        public string PdfQuantityLabel { get; set; }
        public string PdfTotalCostLabel { get; set; }
        public string PdfFileName { get; set; }
    }

    public class TextData : ImageModel
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }

    public class TabDatum
    {
        public string Type { get; set; }
        public string Label { get; set; }
        public string SubTitle { get; set; }
        public List<tooltipDataitems> tooltipData { get; set; }
        public Data Data { get; set; }
    }

    public class MaterialLabelData
    {
        public string SubmitButtonLabel { get; set; }
        public string HeadingLabel { get; set; }
        public string TotalCostLabel { get; set; }
        public string QuantitytLabel { get; set; }
        public string PricePerUnitLabel { get; set; }
        public string MaterialLabel { get; set; }
        public string PriceLabel { get; set; }
        public int DefaultActiveKey { get; set; }
    }

    public class Data
    {
        public List<ButtonTab> ButtonTabs { get; set; }
        public List<InputTab> InputTabs { get; set; }
        public MaterialLabelData Labels { get; set; }
        public SubmitButton SubmitButton { get; set; }
    }

    public class ButtonTab
    {
        public string Label { get; set; }
        public string Id { get; set; }
        public bool InitiallyChecked { get; set; }
    }
    public class InputTab
    {
        public string Placeholder { get; set; }
        public string Type { get; set; }
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
        public List<optionsList> options { get; set; }
    }

    public class optionsList
    {
        public string Label { get; set; }
        public int Id { get; set; }
    }
    public class SubmitButton
    {
        public string Link { get; set; }
        public string LinkTarget { get; set; }
        public string Type { get; set; }
    }
    public class tooltipDataitems
    {
        public string Title { get; set; }
        public string Description { get; set; }

    }
}