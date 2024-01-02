using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Models.EnrollNow;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services.EnrollNow
{
    public class EnrollNowService : IEnrollNowService
    {
        public EnrollNowOverviewModel GetOverviewModel(Rendering rendering)
        {
            EnrollNowOverviewModel overviewModel = new EnrollNowOverviewModel();
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                overviewModel.Heading = datasource.Fields[BaseTemplates.TitleTemplate.TitleFieldId].Value;
                overviewModel.Description = datasource.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value;

                overviewModel.Button = new List<OverviewButton>();

                foreach (Item overViewButtonItem in datasource.Children)
                {
                    OverviewButton overviewButton = new OverviewButton();
                    overviewButton.URL = Utils.GetLinkURL(overViewButtonItem?.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId]);
                    overviewButton.Label = overViewButtonItem.Fields[BaseTemplates.CtaTemplate.CtaTextFieldId].Value;
                    overviewButton.Variant = overViewButtonItem.Fields[BaseTemplates.VariantTemplate.VariantFieldId].Value;
                    overviewButton.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(overViewButtonItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(overViewButtonItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(overViewButtonItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Banner_category = Utils.GetValue(overViewButtonItem, BaseTemplates.GTMTemplate.GtmBannerCategoryFieldId),
                        Label = Utils.GetValue(overViewButtonItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    overviewModel.Button.Add(overviewButton);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return overviewModel;
        }
        public BaseHeadingModel<AdmissionStage> GetAdmissionDocuments(Rendering rendering)
        {
            var admissionDocumentsModel = new BaseHeadingModel<AdmissionStage>();
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                admissionDocumentsModel.Heading = datasource.Fields[BaseTemplates.TitleTemplate.TitleFieldId].Value;


                foreach (Item admissionStageItem in datasource.Children)
                {
                    AdmissionStage admissionStage = new AdmissionStage();
                    admissionStage.SubHeading = admissionStageItem.Fields[BaseTemplates.SubHeadingTemplate.SubHeadingFieldId].Value;

                    admissionStage.Description = new List<AdmissionDocument>();

                    foreach (Item admissionDocumentItem in admissionStageItem.Children)
                    {
                        AdmissionDocument admissionDocument = new AdmissionDocument();
                        admissionDocument.Label = admissionDocumentItem.Fields[BaseTemplates.LabelTemplate.LabelFieldId].Value;

                        admissionDocument.SubDescription = new List<AdmissionSubDocument>();

                        foreach (Item admissionSubDocumentItem in admissionDocumentItem.Children)
                        {
                            AdmissionSubDocument admissionSubDocument = new AdmissionSubDocument();
                            admissionSubDocument.Label = admissionSubDocumentItem.Fields[BaseTemplates.LabelTemplate.LabelFieldId].Value;

                            admissionDocument.SubDescription.Add(admissionSubDocument);
                        }
                        admissionStage.Description.Add(admissionDocument);
                    }
                    admissionDocumentsModel.Data.Add(admissionStage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return admissionDocumentsModel;
        }

        public AgeCriteriaModel GetAgeCriteriaModel(Rendering rendering)
        {
            AgeCriteriaModel ageCriteriaModel = new AgeCriteriaModel();
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                ageCriteriaModel.Heading = datasource.Fields[BaseTemplates.TitleTemplate.TitleFieldId].Value;

                AgeCriteria ageCriteria = new AgeCriteria();
                ageCriteria.Th = new List<TH>();

                var th = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Th"));
                foreach (Item thItem in th.Children)
                {
                    TH tH = new TH();
                    tH.Label = thItem.Fields[BaseTemplates.LabelTemplate.LabelFieldId].Value;
                    ageCriteria.Th.Add(tH);
                }

                ageCriteria.Td = new List<List<TH>>();
                var tr = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Td"));

                foreach (Item trItem in tr.Children)
                {
                    var tableDataList = new List<TH>();

                    foreach (Item tdItem in trItem.Children)
                    {
                        TH tH = new TH();
                        tH.Label = tdItem.Fields[BaseTemplates.LabelTemplate.LabelFieldId].Value;
                        tableDataList.Add(tH);
                    }

                    ageCriteria.Td.Add(tableDataList);
                }
                ageCriteriaModel.Data = ageCriteria;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ageCriteriaModel;
        }
    }
}