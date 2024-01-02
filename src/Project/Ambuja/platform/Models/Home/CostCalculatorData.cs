using Project.AmbujaCement.Website.Models.CostCalculator;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.Home
{
    public class CostCalculatorData
    {
        public HomeLabels Labels { get; set; }
        public List<HomeTabDatum> TabData { get; set; }
        public HomeTextData TextData { get; set; }
    }

    public class HomeLabels
    {
        public string HeadingLabel { get; set; }
        public string SubmitButtonLabel { get; set; }
        public int DefaultActiveKey { get; set; }
    }

    public class HomeTextData :ImageModel
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }

    public class HomeTabDatum
    {
        public string Type { get; set; }
        public string Label { get; set; }
        public string SubTitle { get; set; }
        public List<HometooltipDataitems> tooltipData { get; set; }
        public HomeData Data { get; set; }
    }


    public class HomeData
    {
        public List<HomeButtonTab> ButtonTabs { get; set; }
        public List<HomeInputTab> InputTabs { get; set; }
        public HomeLabels Labels { get; set; }
        public HomeSubmitButton SubmitButton { get; set; }
    }

    public class HomeButtonTab
    {
        public string Label { get; set; }
        public string Id { get; set; }
        public bool InitiallyChecked { get; set; }
    }
    public class HomeInputTab
    {
        public string Placeholder { get; set; }
        public string Type { get; set; }
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
        public List<HomeoptionsList> options { get; set; }
    }
    public class HomeoptionsList
    {
        public string Label { get; set; }
        public int Id { get; set; }
    }
    public class HomeSubmitButton
    {
        public string Link { get; set; }
        public string LinkTarget { get; set; }
        public string Type { get; set; }
    }
    public class HometooltipDataitems
    {
        public string Title { get; set; }
        public string Description { get; set; }

    }

}