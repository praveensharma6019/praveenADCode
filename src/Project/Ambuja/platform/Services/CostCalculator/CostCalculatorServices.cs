using JSNLog.ValueInfos;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.CostCalculator;
using Project.AmbujaCement.Website.Models.Home;
using Project.AmbujaCement.Website.Templates;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using static Project.AmbujaCement.Website.Templates.CostCalculaterPageTemplate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using ButtonTab = Project.AmbujaCement.Website.Models.CostCalculator.ButtonTab;
using Data = Project.AmbujaCement.Website.Models.CostCalculator.Data;
using InputTab = Project.AmbujaCement.Website.Models.CostCalculator.InputTab;
using Item = Sitecore.Data.Items.Item;
using Labels = Project.AmbujaCement.Website.Models.CostCalculator.Labels;
using SubmitButton = Project.AmbujaCement.Website.Models.CostCalculator.SubmitButton;
using TabDatum = Project.AmbujaCement.Website.Models.CostCalculator.TabDatum;
using TextData = Project.AmbujaCement.Website.Models.CostCalculator.TextData;

namespace Project.AmbujaCement.Website.Services.CostCalculator
{
    public class CostCalculatorServices : ICostCalculatorServices
    {
        readonly Item _contextItem;
        public CostCalculatorServices()
        {
            _contextItem = Sitecore.Context.Item;
        }
        public CostCalculatorPageData GetCostCalculatorPageData(Rendering rendering)
        {
            CostCalculatorPageData costCalculatorPageData = new CostCalculatorPageData();
            Labels labelsdata = new Labels();
            
            TextData textData = new TextData();
        List<ButtonTab> buttonTabs = new List<ButtonTab>();
            List<InputTab> inputTabs = new List<InputTab>();


            int parsedValue;
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            var LabelContext = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "CostCalculatorlabels"));
            var TabContext = datasource.Children.FirstOrDefault(y => Utils.CompareIgnoreCase(y.Name, "CostCalculatorTab"));
            var TextContext = datasource.Children.FirstOrDefault(z => Utils.CompareIgnoreCase(z.Name, "CostCalculatorTextData"));
            try
            {
                #region LabelData
                labelsdata.HeadingIcon = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.HeadingIcon);
                    labelsdata.HeadingLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.HeadingLabel);
                    labelsdata.SubmitButtonLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.SubmitButtonLabel);
                    labelsdata.EditButtonLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.EditButtonLabel);
                    labelsdata.TotalAreaLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.TotalAreaLabel);
                    labelsdata.AreaLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.AreaLabel);
                    labelsdata.DownloadEstimateLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.DownloadEstimateLabel);
                    labelsdata.DownloadEstimateIcon = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.DownloadEstimateIcon);
                    PDFLabels pdf = new PDFLabels();
                    pdf.PdfHeading = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.PDFDataLabels.PdfHeading);
                    pdf.PdfMaterialLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.PDFDataLabels.PdfMaterialLabel);
                    pdf.PdfPricePerUnitLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.PDFDataLabels.PdfPricePerUnitLabel);
                    pdf.PdfQuantityLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.PDFDataLabels.PdfQuantityLabel);
                    pdf.PdfTotalCostLabel = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.PDFDataLabels.PdfTotalCostLabel);
                    pdf.PdfFileName = Utils.GetValue(LabelContext, CostCalculaterPageTemplate.CostCalculatorLabelsFields.PDFDataLabels.PdfFileName);
                    labelsdata.PdfLabels = pdf;
                
                costCalculatorPageData.Labels = labelsdata;
                #endregion
                #region TabData
                List<TabDatum> tabData = new List<TabDatum>();
                foreach (Item tabchild in TabContext.Children)
                {
                    TabDatum tabDataum = new TabDatum();
                    Data data = new Data();

                    tabDataum.Type = Utils.GetValue(tabchild, CostCalculaterPageTemplate.TabDataDetail.Type);
                    tabDataum.Label = Utils.GetValue(tabchild, CostCalculaterPageTemplate.TabDataDetail.Label);
                    tabDataum.SubTitle = Utils.GetValue(tabchild, CostCalculaterPageTemplate.TabDataDetail.SubTitle);
                    var TooltipContext = tabchild.Children.FirstOrDefault(t => Utils.CompareIgnoreCase(t.Name, "tooltipData"));
                    if (TooltipContext != null) 
                    {
                        List<tooltipDataitems> tooltipData = new List<tooltipDataitems>();
                        foreach (Item tooltipchild in TooltipContext.Children)
                        {
                            tooltipDataitems tooltip = new tooltipDataitems();
                            tooltip.Title = Utils.GetValue(tooltipchild, CostCalculaterPageTemplate.TabDataFields.Title);
                            tooltip.Description = Utils.GetValue(tooltipchild, CostCalculaterPageTemplate.TabDataFields.Description);
                            tooltipData.Add(tooltip);
                        }
                        tabDataum.tooltipData = tooltipData;
                    }
                    

                    var ButtonContext = tabchild.Children.FirstOrDefault(b => Utils.CompareIgnoreCase(b.Name, "buttonTabs"));
                    var InputContext = tabchild.Children.FirstOrDefault(p => Utils.CompareIgnoreCase(p.Name, "inputTabs"));
                    if (ButtonContext != null)
                    {
                        List<ButtonTab> ButtonTabs = new List<ButtonTab>();
                        foreach (Item child in ButtonContext.Children)
                        {
                            ButtonTab buttonTab = new ButtonTab();
                            buttonTab.Label = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataButtonTabs.Label);
                            buttonTab.Id = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataButtonTabs.Id);
                            buttonTab.InitiallyChecked = Utils.GetBoleanValue(child, CostCalculaterPageTemplate.TabDataButtonTabs.InitiallyChecked);
                            ButtonTabs.Add(buttonTab);
                        }
                        data.ButtonTabs = ButtonTabs;
                    }
                    

                    if (InputContext != null)
                    {
                        List<InputTab> InputTabs = new List<InputTab>();
                        foreach (Item child in InputContext.Children)
                        {
                            InputTab inputTab = new InputTab();
                            inputTab.Type = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataInputTabsFields.Label);
                            inputTab.Placeholder = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataInputTabsFields.Placeholder);
                            inputTab.FieldName = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataInputTabsFields.FieldName);
                            inputTab.ErrorMessage = Utils.GetValue(child, CostCalculaterPageTemplate.TabDataInputTabsFields.ErrorMessage);
                            //var DropdownContext = child.Children.FirstOrDefault(dr => Utils.CompareIgnoreCase(dr.Name, "dropdown"));
                            if (child.Children != null)
                            {
                                List<optionsList> optionsLists = new List<optionsList>();
                                foreach (Item optionschild in child.Children)
                                {
                                    optionsList optionsList = new optionsList();
                                    string optionsId = Utils.GetValue(optionschild, CostCalculaterPageTemplate.TabDataInputTabsOptionsFields.Id);
                                    if (optionsId != null && int.TryParse(optionsId, out parsedValue))
                                    { optionsList.Id = parsedValue; }
                                    //optionsList.Id = int.Parse(Utils.GetValue(optionschild, CostCalculaterPageTemplate.TabDataInputTabsOptionsFields.Id));
                                    optionsList.Label = Utils.GetValue(optionschild, CostCalculaterPageTemplate.TabDataInputTabsOptionsFields.Label);
                                    optionsLists.Add(optionsList);
                                }
                                inputTab.options = optionsLists;
                            }
                            InputTabs.Add(inputTab);
                        }
                        data.InputTabs = InputTabs;
                    }

                    var LabContext = tabchild.Children.FirstOrDefault(p => Utils.CompareIgnoreCase(p.Name, "TabDataLabel"));
                    MaterialLabelData matlabdata = new MaterialLabelData();
                    matlabdata.HeadingLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.HeadingLabel);
                    matlabdata.SubmitButtonLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.SubmitButtonLabel);
                    matlabdata.MaterialLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.MaterialLabel);
                    matlabdata.PricePerUnitLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.PricePerUnitLabel);
                    matlabdata.QuantitytLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.QuantitytLabel);
                    matlabdata.TotalCostLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.TotalCostLabel);
                    matlabdata.PriceLabel = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.PriceLabel);
                    string activekey = Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.DefaultActiveKey);
                    if (activekey != null && int.TryParse(activekey, out parsedValue))
                    { matlabdata.DefaultActiveKey = parsedValue; }
                    //matlabdata.DefaultActiveKey = int.Parse(Utils.GetValue(LabContext, CostCalculaterPageTemplate.TabDataLabelsFields.DefaultActiveKey));
                    data.Labels = matlabdata;
                    tabDataum.Data = data;
                    tabData.Add(tabDataum);
                }
                costCalculatorPageData.TabData = tabData;
                #endregion

                #region TextData
                textData.Heading = Utils.GetValue(TextContext, BaseTemplates.HeadingDetailsTemplate.Heading);
                    textData.Description = Utils.GetValue(TextContext, BaseTemplates.HeadingDetailsTemplate.Description);
                    textData.ImageSource = Utils.GetImageURLByFieldId(TextContext, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                    textData.ImageSourceMobile = Utils.GetImageURLByFieldId(TextContext, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                    textData.ImageSourceTablet = Utils.GetImageURLByFieldId(TextContext, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);
                    textData.ImageAlt = Utils.GetValue(TextContext, BaseTemplates.ImageSourceTemplate.ImageAltFieldId);
               
                costCalculatorPageData.TextData = textData;
                #endregion
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return costCalculatorPageData;
        }
    }
}