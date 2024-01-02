using Adani.SuperApp.Realty.Feature.Property.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Property.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Services
{
    public class ProperyDataBasicService : IProperyDataBasicService
    {
        private readonly ILogRepository _logRepository;

        public ProperyDataBasicService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End 
         *         */
        public AmenetiesData GetAmenities(Rendering rendering)
        {
            AmenetiesData amenetiesData = new AmenetiesData();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 

            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
           ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
           : null;

                amenetiesData.componentID = !string.IsNullOrEmpty(item.Fields[Templates.ProjectAmenites.Fields.ComponentId].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectAmenites.Fields.ComponentId].Value.Trim()) : "";
                amenetiesData.heading = !string.IsNullOrEmpty(item.Fields[Templates.ProjectAmenites.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectAmenites.Fields.Heading].Value.Trim()) : "";
                amenetiesData.disclaimer = !string.IsNullOrEmpty(item.Fields[Templates.ProjectAmenites.Fields.disclaimer].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectAmenites.Fields.disclaimer].Value.Trim()) : "";

                amenetiesData.projectAmeneties = new ProjectAmeneties();
                List<Amenities> amemeties = new List<Amenities>();
                Sitecore.Data.Fields.MultilistField amenites = item.Fields[Templates.ProjectAmenites.Fields.AmenitiesData];
                foreach (Item amenitiesdata in amenites.GetItems())
                {
                    Amenities amenities = new Amenities();
                    amenities.caption = !string.IsNullOrEmpty(amenitiesdata.Fields[Templates.Amenites.Fields.Caption].Value.ToString()) ? Convert.ToString(amenitiesdata.Fields[Templates.Amenites.Fields.Caption].Value.Trim()) : "";

                    //Changed started 
                    string str = Helper.GetImageURLbyField(amenitiesdata.Fields[Templates.Amenites.Fields.Src]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    amenities.src = str;

                    amenities.srcMobile = Helper.GetImageURLbyField(amenitiesdata.Fields[Templates.Amenites.Fields.SrcMobile]);

                    str = Helper.GetPropLinkURLbyField(amenitiesdata, amenitiesdata.Fields[Templates.Amenites.Fields.Link]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    amenities.link = str;
                    //Changed End

                    amenities.imgAlt = Helper.GetImageDetails(amenitiesdata, Amenites.Fields.Src.ToString()) != null ?
                                    Helper.GetImageDetails(amenitiesdata, Amenites.Fields.Src.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";

                    amenities.status = !string.IsNullOrEmpty(amenitiesdata.Fields[Templates.Amenites.Fields.status].Value.ToString()) ? Convert.ToString(amenitiesdata.Fields[Templates.Amenites.Fields.status].Value) : "";
                    amemeties.Add(amenities);
                }
                amenetiesData.projectAmeneties.data = amemeties;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return amenetiesData;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End 
         *         */
        public FeaturesData GetFeatures(Rendering rendering)
        {
            //Changed started
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            FeaturesData featureData = new FeaturesData();
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
           ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
           : null;

                featureData.componentID = !string.IsNullOrEmpty(item.Fields[Templates.ProjectFeatures.Fields.ComponentId].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectFeatures.Fields.ComponentId].Value.Trim()) : "";
                featureData.heading = !string.IsNullOrEmpty(item.Fields[Templates.ProjectFeatures.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectFeatures.Fields.Heading].Value.Trim()) : "";

                List<Featute> features = new List<Featute>();
                Sitecore.Data.Fields.MultilistField featureItems = item.Fields[Templates.ProjectFeatures.Fields.Features];
                foreach (Item amenitiesdata in featureItems.GetItems())
                {
                    Featute feature = new Featute();
                    feature.caption = !string.IsNullOrEmpty(amenitiesdata.Fields[Templates.FeatureItem.Fields.Caption].Value.ToString()) ? Convert.ToString(amenitiesdata.Fields[Templates.FeatureItem.Fields.Caption].Value.Trim()) : "";

                    //Changed started 
                    string str = Helper.GetImageURLbyField(amenitiesdata.Fields[Templates.FeatureItem.Fields.Src]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    feature.src = str;
                    //Changed End

                    feature.imgAlt = Helper.GetImageDetails(amenitiesdata, Templates.FeatureItem.Fields.Src.ToString()) != null ?
                                                Helper.GetImageDetails(amenitiesdata, Templates.FeatureItem.Fields.Src.ToString()).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                    feature.title = !string.IsNullOrEmpty(amenitiesdata.Fields[Templates.FeatureItem.Fields.Title].Value.ToString()) ? Convert.ToString(amenitiesdata.Fields[Templates.FeatureItem.Fields.Title].Value.Trim()) : "";
                    feature.desc = !string.IsNullOrEmpty(amenitiesdata.Fields[Templates.FeatureItem.Fields.Desc].Value.ToString()) ? Convert.ToString(amenitiesdata.Fields[Templates.FeatureItem.Fields.Desc].Value.Trim()) : "";
                    feature.labelTerms = !string.IsNullOrEmpty(amenitiesdata.Fields[Templates.FeatureItem.Fields.labelTerms].Value.ToString()) ? Convert.ToString(amenitiesdata.Fields[Templates.FeatureItem.Fields.labelTerms].Value.Trim()) : "";

                    feature.srcMobile = Helper.GetImageURLbyField(amenitiesdata.Fields[Templates.FeatureItem.Fields.srcMobile]);

                    features.Add(feature);
                }
                featureData.features = features;

            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return featureData;
        }


        public EmiCalculator GetEmiCalculator(Item item)
        {
            EmiCalculator emiCalculator1 = null;
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;

            try
            {
                List<BankingPartners> bankingPartners = new List<BankingPartners>();

                EmiCalculatorData emiCalculator = new EmiCalculatorData();
                emiCalculator.Heading = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.Heading].Value.Trim()) : "";
                emiCalculator.Rs = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.Rs].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.Rs].Value.Trim()) : "";
                emiCalculator.Lakhs = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.Lakhs].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.Lakhs].Value.Trim()) : "";
                emiCalculator.LoanAmountLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.LoanAmountLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.LoanAmountLabel].Value.Trim()) : "";
                emiCalculator.MinLoanAmountLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MinLoanAmountLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MinLoanAmountLabel].Value.Trim()) : "";
                emiCalculator.MaxLoanAmountLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MaxLoanAmountLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MaxLoanAmountLabel].Value.Trim()) : "";
                emiCalculator.MinLoanAmount = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MinLoanAmount].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MinLoanAmount].Value.Trim()) : "";
                emiCalculator.MaxLoanAmount = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MaxLoanAmount].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MaxLoanAmount].Value.Trim()) : "";
                emiCalculator.Percent = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.Percent].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.Percent].Value.Trim()) : "";
                emiCalculator.InterestRateLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.InterestRateLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.InterestRateLabel].Value.Trim()) : "";
                emiCalculator.MinInterestRateLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MinInterestRateLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MinInterestRateLabel].Value.Trim()) : "";

                emiCalculator.MaxInterestRateLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MaxInterestRateLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MaxInterestRateLabel].Value.Trim()) : "";
                emiCalculator.MinInterestRate = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MinInterestRate].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MinInterestRate].Value.Trim()) : "";
                emiCalculator.MaxInterestRate = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MaxInterestRate].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MaxInterestRate].Value.Trim()) : "";
                emiCalculator.LoanTenureLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.LoanTenureLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.LoanTenureLabel].Value.Trim()) : "";
                emiCalculator.Years = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.Years].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.Years].Value.Trim()) : "";
                emiCalculator.MinLoanTenureLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MinLoanTenureLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MinLoanTenureLabel].Value.Trim()) : "";
                emiCalculator.MaxLoanTenureLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MaxLoanTenureLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MaxLoanTenureLabel].Value.Trim()) : "";
                emiCalculator.MinLoanTenure = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MinLoanTenure].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MinLoanTenure].Value.Trim()) : "";
                emiCalculator.MaxLoanTenure = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.MaxLoanTenure].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.MaxLoanTenure].Value.Trim()) : "";
                emiCalculator.InterestAmountLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.InterestAmountLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.InterestAmountLabel].Value.Trim()) : "";
                emiCalculator.InterestAmountMobileLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.InterestAmountMobileLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.InterestAmountMobileLabel].Value.Trim()) : "";

                emiCalculator.PrincipalAmountLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.PrincipalAmountLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.PrincipalAmountLabel].Value.Trim()) : "";
                emiCalculator.PrincipalAmountMobileLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.PrincipalAmountMobileLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.PrincipalAmountMobileLabel].Value.Trim()) : "";
                emiCalculator.TotalPayableAmountLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.TotalPayableAmountLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.TotalPayableAmountLabel].Value.Trim()) : "";
                emiCalculator.TotalPayableAmountMobileLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.TotalPayableAmountMobileLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.TotalPayableAmountMobileLabel].Value.Trim()) : "";
                emiCalculator.DefaultLoanAmount = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.DefaultLoanAmount].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.DefaultLoanAmount].Value.Trim()) : "";
                emiCalculator.DefaultInterestRate = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.DefaultInterestRate].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.DefaultInterestRate].Value.Trim()) : "";
                emiCalculator.DefaultLoanTenure = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.DefaultLoanTenure].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.DefaultLoanTenure].Value.Trim()) : "";
                emiCalculator.OurBankingPartnersLabel = !string.IsNullOrEmpty(item.Fields[Templates.EmiCaluclator.Fields.OurBankingPartnersLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.EmiCaluclator.Fields.OurBankingPartnersLabel].Value.Trim()) : "";
                MultilistField multilistField = item.Fields[Templates.EmiCaluclator.Fields.BankingPartner];

                foreach (var partner in multilistField.GetItems())
                {
                    BankingPartners bankingPartners1 = null;
                    bankingPartners1 = new BankingPartners();

                    //Changed started 
                    string str = Helper.GetPropLinkURLbyField(partner, partner.Fields[Templates.BankingPartnerField.Fields.Src]);

                    //if (str.Contains(strSitedomain))
                    //{
                    //    str  = str.Replace(strSitedomain, "");
                    //}
                    bankingPartners1.Src = str;
                    //Changed EOD 

                    bankingPartners1.ImgAlt = !string.IsNullOrEmpty(partner.Fields[Templates.BankingPartnerField.Fields.ImgAlt].Value.ToString()) ? Convert.ToString(partner.Fields[Templates.BankingPartnerField.Fields.ImgAlt].Value.Trim()) : "";
                    bankingPartners1.ImgTitle = !string.IsNullOrEmpty(partner.Fields[Templates.BankingPartnerField.Fields.ImgTitle].Value.ToString()) ? Convert.ToString(partner.Fields[Templates.BankingPartnerField.Fields.ImgTitle].Value.Trim()) : "";
                    bankingPartners.Add(bankingPartners1);
                }
                emiCalculator.bankingPartnersData = bankingPartners;
                emiCalculator1 = new EmiCalculator()
                {
                    EmiCalculatorData = emiCalculator
                };

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return emiCalculator1;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End 
         *         */
        public List<Models.Highlights> GetHighlights(Item item)
        {
            List<Models.Highlights> highlights = new List<Models.Highlights>();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 

            try
            {

                Sitecore.Data.Fields.MultilistField highlits = item.Fields[Templates.Property.Fields.Highlights];
                foreach (Item highlightsData in item.Children)
                {
                    Models.Highlights hightlight = new Models.Highlights();
                    hightlight.Type = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.Type].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.Type].Value.Trim()) : "";
                    hightlight.Src = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.Src]);
                    hightlight.SrcMobile = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.SrcMobile]);


                    hightlight.ImgAlt = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.ImgAlt].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.ImgAlt].Value.Trim()) : "";
                    hightlight.ImgTitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.ImgTitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.ImgTitle].Value.Trim()) : "";

                    //Changed started
                    string str;
                    str = String.Empty;
                    str = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.LogoSrc]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    hightlight.LogoSrc = str;


                    hightlight.LogoAlt = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.LogoTitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.LogoTitle].Value.Trim()) : "";
                    hightlight.LogoTitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.LogoTitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.LogoTitle].Value.Trim()) : "";

                    str = String.Empty;
                    str = Helper.GetLinkTextbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.LogoSrc]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str  = str.Replace(strSitedomain, "");
                    //}
                    hightlight.AboutImg = str;

                    str = String.Empty;
                    str = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.Icon]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    hightlight.Icon = str;
                    //Changed End

                    hightlight.IconDesc = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.IconDesc].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.IconDesc].Value.Trim()) : "";

                    hightlight.Degree = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.Degree].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.Degree].Value.Trim()) : "";
                    hightlight.Tour = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.Tour].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.Tour].Value.Trim()) : "";
                    highlights.Add(hightlight);
                }

                if (string.IsNullOrEmpty(highlights.FirstOrDefault().IconDesc) || string.IsNullOrWhiteSpace(highlights.FirstOrDefault().IconDesc))
                {
                    highlights.FirstOrDefault().IconDesc = Convert.ToString(highlights.Count());
                }
            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }


            return highlights;
        }

        public PropertyAbout GetPropertyAbout(Item item)
        {
            PropertyAbout propertyAbout = new PropertyAbout();
            try
            {
                propertyAbout.ComponentID = !string.IsNullOrEmpty(item.Fields[Templates.PropertyAboutField.Fields.ComponentID].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PropertyAboutField.Fields.ComponentID].Value.Trim()) : "";
                propertyAbout.Heading = !string.IsNullOrEmpty(item.Fields[Templates.PropertyAboutField.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PropertyAboutField.Fields.Heading].Value.Trim()) : "";
                propertyAbout.Title = !string.IsNullOrEmpty(item.Fields[Templates.PropertyAboutField.Fields.Title].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PropertyAboutField.Fields.Title].Value.Trim()) : "";
                propertyAbout.Description = !string.IsNullOrEmpty(item.Fields[Templates.PropertyAboutField.Fields.Description].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PropertyAboutField.Fields.Description].Value.Trim()) : "";
                propertyAbout.readMore = !string.IsNullOrEmpty(item.Fields[Templates.PropertyAboutField.Fields.ReadMore].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PropertyAboutField.Fields.ReadMore].Value.Trim()) : "";
                propertyAbout.ReadLess = !string.IsNullOrEmpty(item.Fields[Templates.PropertyAboutField.Fields.ReadLess].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PropertyAboutField.Fields.ReadLess].Value.Trim()) : "";


            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return propertyAbout;
        }

        public PropertyBasicInfo GetPropertyBasicInfo(Item item)
        {
            PropertyBasicInfo propertyBasicInfo = new PropertyBasicInfo();

            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            try
            {
                propertyBasicInfo.Location = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.Location].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.Location].Value.Trim()) : "";
                propertyBasicInfo.ProjectStatus = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.ProjectStatus].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.ProjectStatus].Value.Trim()) : "";
                propertyBasicInfo.SiteStatus = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.SiteStatus].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.SiteStatus].Value.Trim()) : "";
                propertyBasicInfo.ProjectArea = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.ProjectArea].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.ProjectArea].Value.Trim()) : "";
                propertyBasicInfo.PropertyLogo = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.PropertyLogo].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.PropertyLogo].Value.Trim()) : "";
                propertyBasicInfo.SimilarProjectTitle = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.SimilarProjectTitle].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.SimilarProjectTitle].Value.Trim()) : "";
                propertyBasicInfo.SimilarProjects = new List<Models.Project>();


                MultilistField similarProjects = item.Fields[Templates.Property.Fields.SimilarProject];
                if (similarProjects != null)
                {
                    foreach (var projectItm in similarProjects.GetItems())
                    {
                        string str = Helper.GetPropLinkURLbyField(projectItm, projectItm.Fields[Templates.Project.Fields.Image]);
                        Models.Project proj = new Models.Project();

                        proj.Title = !string.IsNullOrEmpty(projectItm.Fields[Templates.Project.Fields.Title].Value.ToString()) ? Convert.ToString(projectItm.Fields[Templates.Project.Fields.Title].Value.Trim()) : "";
                        proj.SubTitle = !string.IsNullOrEmpty(projectItm.Fields[Templates.Project.Fields.SubTitle].Value.ToString()) ? Convert.ToString(projectItm.Fields[Templates.Project.Fields.SubTitle].Value.Trim()) : "";

                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        proj.Image = str;
                        propertyBasicInfo.SimilarProjects.Add(proj);

                    }
                }


                propertyBasicInfo.ProjectLayout = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.ProjectLayout].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.ProjectLayout].Value.Trim()) : "";
                propertyBasicInfo.MapLink = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.MapLink].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.MapLink].Value.Trim()) : "";
                propertyBasicInfo.UnitSize = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.UnitSize].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.UnitSize].Value.Trim()) : "";
                propertyBasicInfo.Possession = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.Possession].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.Possession].Value.Trim()) : "";
                propertyBasicInfo.Brochure = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.Location].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.Location].Value.Trim()) : "";
                propertyBasicInfo.PropertyType = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.PropertyType].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.PropertyType].Value.Trim()) : "";
                propertyBasicInfo.status = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.Status].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.Status].Value.Trim()) : "";
                propertyBasicInfo.Type = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.Type].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.Type].Value.Trim()) : "";
                propertyBasicInfo.MediaLibrary = new List<string>();
                propertyBasicInfo.PriceLabel = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.PriceLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.PriceLabel].Value.Trim()) : "";
                propertyBasicInfo.Onwards = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.Ownwards].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.Ownwards].Value.Trim()) : "";
                propertyBasicInfo.AreaLabel = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.AreaLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.AreaLabel].Value.Trim()) : "";
                propertyBasicInfo.rating = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.rating].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.rating].Value.Trim()) : "";
                propertyBasicInfo.ratingCount = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.ratingCount].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.ratingCount].Value.Trim()) : "";
                propertyBasicInfo.bestRating = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.bestRating].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.bestRating].Value.Trim()) : "";
                propertyBasicInfo.ratingName = !string.IsNullOrEmpty(item.Fields[Templates.Property.Fields.ratingName].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Property.Fields.ratingName].Value) : "";
                propertyBasicInfo.propertyDescription = item.Fields[Templates.Property.Fields.Possession] != null ? item.Fields[Templates.Property.Fields.Possession].Value : "";
                propertyBasicInfo.propertyImage = Helper.GetImageSource(item, Templates.Property.Fields.PropertyLogoImg.ToString()) != null ?
                                                    Helper.GetImageSource(item, Templates.Property.Fields.PropertyLogoImg.ToString()) : "";
                propertyBasicInfo.isProjectCompleted = item.Fields[Templates.Property.Fields.isProjectCompleted] != null ? Helper.GetCheckBoxSelection(item.Fields[Templates.Property.Fields.isProjectCompleted]) : false;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return propertyBasicInfo;

        }
        public List<PropertyList> ResidentialPropertyList(Rendering rendering)
        {

            List<PropertyList> listOfproperty = new List<PropertyList>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                   ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                   : null;
                var commonItem = Sitecore.Context.Database.GetItem(commonData.ItemID);

                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                var residentialProperty = datasource.Children.ToList();
                foreach (var item in residentialProperty)
                {
                    PropertyList propertyList = new PropertyList();
                    if (item.TemplateID == Templates.ResidentialPropertyData.LocationLandingTemplateID)
                    {
                        propertyList.city = !string.IsNullOrEmpty(item.Fields[Templates.ResidentialPropertyData.BaseFields.Title].Value.ToString()) ?
                                                                        item.Fields[Templates.ResidentialPropertyData.BaseFields.Title].Value.ToString() : "";
                        propertyList.headingLabel = !string.IsNullOrEmpty(item.Fields[Templates.ResidentialPropertyData.BaseFields.HeadingLabel].Value.ToString()) ?
                                                item.Fields[Templates.ResidentialPropertyData.BaseFields.HeadingLabel].Value.ToString() : "";
                        propertyList.SeeAllLink = Helper.GetLinkURLbyField(item, item.Fields[Templates.ResidentialPropertyData.Fields.PropertyLinkID]) != null ?
                                            Helper.GetLinkURLbyField(item, item.Fields[Templates.ResidentialPropertyData.Fields.PropertyLinkID]) : "";
                        propertyList.SeeAllKeyword = commonItem.Fields[commonData.Fields.SeeAllLabel] != null ? commonItem.Fields[commonData.Fields.SeeAllLabel].Value : "";
                        propertyList.subheadingLabel = !string.IsNullOrEmpty(item.Fields[Templates.ResidentialPropertyData.BaseFields.SubHeadingLabel].Value.ToString()) ?
                                                                        item.Fields[Templates.ResidentialPropertyData.BaseFields.SubHeadingLabel].Value.ToString() : "";
                        propertyList.property = GetResidentialItems(rendering, item);
                        listOfproperty.Add(propertyList);
                    }
                }
            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return listOfproperty;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End 
         *         */
        public List<PropertyItem> GetResidentialItems(Rendering rendering, Item item)
        {
            List<PropertyItem> listOfproperty = new List<PropertyItem>();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                List<Item> childList = new List<Item>();
                //var newcityList = item.Children.ToList();
                var childOfcities1 = item.Children.Where(x => x.Fields[Templates.Property.Fields.ProjectStatus].Value != null && x.Fields[Templates.Property.Fields.ProjectStatus].Value == "{9E9581BB-84C0-422A-81D4-10498CD90F38}");
                childList.AddRange(childOfcities1);
                var childOfcities2 = item.Children.Where(x => x.Fields[Templates.Property.Fields.ProjectStatus].Value != null && x.Fields[Templates.Property.Fields.ProjectStatus].Value != "{9E9581BB-84C0-422A-81D4-10498CD90F38}");
                childList.AddRange(childOfcities2);
                childList = childList.OrderBy(x => x.Fields[Templates.Property.Fields.SiteStatus].Value).Where(x => x.Fields[Templates.Property.Fields.SiteStatus].Value != "").ToList();
                foreach (var childItem in childList)
                {
                    PropertyItem propertyItem = new PropertyItem();
                    if (childItem.TemplateID == Templates.ResidentialPropertyData.ResidentialTemplateID || childItem.TemplateID == Templates.ResidentialPropertyData.CommercialTemplateID)
                    {
                        //Changed started 
                        string str = Helper.GetLinkURLbyField(childItem, childItem.Fields[Templates.ResidentialPropertyData.Fields.PropertyLinkID]) != null ?
                                            Helper.GetLinkURLbyField(childItem, childItem.Fields[Templates.ResidentialPropertyData.Fields.PropertyLinkID]) : "";

                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        propertyItem.link = str;
                        //Changed End 

                        propertyItem.linkTarget = Helper.GetLinkURLTargetSpace(childItem, Templates.ResidentialPropertyData.Fields.PropertyLinkID.ToString()) != null ?
                                            Helper.GetLinkURLTargetSpace(childItem, Templates.ResidentialPropertyData.Fields.PropertyLinkID.ToString()) : "";
                        propertyItem.logo = Helper.GetImageSource(childItem, Templates.ResidentialPropertyData.Fields.PropertyLogoFieldName) != null ?
                                                    Helper.GetImageSource(childItem, Templates.ResidentialPropertyData.Fields.PropertyLogoFieldName) : "";
                        propertyItem.logotitle = Helper.GetImageDetails(childItem, Templates.ResidentialPropertyData.Fields.PropertyLogoFieldName) != null ?
                                                Helper.GetImageDetails(childItem, Templates.ResidentialPropertyData.Fields.PropertyLogoFieldName).Fields[Templates.ImageFeilds.Fields.TitleFieldName].Value : "";
                        propertyItem.logoalt = Helper.GetImageDetails(childItem, Templates.ResidentialPropertyData.Fields.PropertyLogoFieldName) != null ?
                                                Helper.GetImageDetails(childItem, Templates.ResidentialPropertyData.Fields.PropertyLogoFieldName).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                        propertyItem.city = childItem.Parent.Name;

                        //Changed started 
                        str = Helper.GetImageSource(childItem, Templates.ResidentialPropertyData.Fields.ImageFieldName) != null ?
                                                 Helper.GetImageSource(childItem, Templates.ResidentialPropertyData.Fields.ImageFieldName) : "";

                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        propertyItem.src = str;
                        //Changed End 

                        propertyItem.imgalt = Helper.GetImageDetails(childItem, Templates.ResidentialPropertyData.Fields.ImageFieldName) != null ?
                                                Helper.GetImageDetails(childItem, Templates.ResidentialPropertyData.Fields.ImageFieldName).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                        propertyItem.title = !string.IsNullOrEmpty(childItem.Fields[Templates.ResidentialPropertyData.Fields.Title].Value.ToString()) ?
                                                 childItem.Fields[Templates.ResidentialPropertyData.Fields.Title].Value.ToString() : "";
                        propertyItem.imgtitle = Helper.GetImageDetails(childItem, Templates.ResidentialPropertyData.Fields.ImageFieldName) != null ?
                                                Helper.GetImageDetails(childItem, Templates.ResidentialPropertyData.Fields.ImageFieldName).Fields[Templates.ImageFeilds.Fields.TitleFieldName].Value : "";
                        propertyItem.location = !string.IsNullOrEmpty(childItem.Fields[Templates.ResidentialPropertyData.Fields.location].Value.ToString()) ?
                                                 childItem.Fields[Templates.ResidentialPropertyData.Fields.location].Value.ToString() : "";
                        propertyItem.subType = !string.IsNullOrEmpty(childItem.Fields[Templates.ResidentialPropertyData.Fields.subType].Value.ToString()) ?
                                                 childItem.Fields[Templates.ResidentialPropertyData.Fields.subType].Value.ToString() : "";
                        propertyItem.imgtype = !string.IsNullOrEmpty(childItem.Fields[Templates.ResidentialPropertyData.Fields.imgtype].Value.ToString()) ?
                                                 childItem.Fields[Templates.ResidentialPropertyData.Fields.imgtype].Value.ToString() : "";
                        propertyItem.status = !string.IsNullOrEmpty(childItem.Fields[Templates.ResidentialPropertyData.Fields.status].Value.ToString()) ?
                                                 childItem.Fields[Templates.ResidentialPropertyData.Fields.status].Value.ToString() : "";
                        propertyItem.category = !string.IsNullOrEmpty(childItem.Fields[Templates.ResidentialPropertyData.Fields.category].Value.ToString()) ?
                                                 childItem.Fields[Templates.ResidentialPropertyData.Fields.category].Value.ToString() : "";
                        listOfproperty.Add(propertyItem);
                    }
                }
            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return listOfproperty;
        }
        /* 09/06/2022
        * Changed started 
        * Changed End 
        *         */
        public TownshipSidebar GetTownshipSidebar(Rendering rendering)
        {
            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End

            TownshipSidebar townshipSidebar = new TownshipSidebar();
            int count = 1;
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                   ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                   : null;
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                Image image = new Image();
                Studio studio = new Studio();
                Service services = new Service();
                List<Service> listOfService = new List<Service>();

                //Changed started 
                string str = Helper.GetImageSource(datasource, Templates.TownshipSlideBar.Fields.ImageFieldName) != null ? Helper.GetImageSource(datasource, Templates.TownshipSlideBar.Fields.ImageFieldName) : "";
                //if (str.Contains(strSitedomain))
                //{
                //    str = str.Replace(strSitedomain, "");
                //}
                image.src = str;
                //Changed End 

                image.alt = Helper.GetImageDetails(datasource, Templates.TownshipSlideBar.Fields.ImageFieldName) != null ?
                                       Helper.GetImageDetails(datasource, Templates.TownshipSlideBar.Fields.ImageFieldName).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                townshipSidebar.image = image;
                townshipSidebar.description = !string.IsNullOrEmpty(datasource.Fields[Templates.TownshipSlideBar.Fields.Details].Value) ? datasource.Fields[Templates.TownshipSlideBar.Fields.Details].Value : "";
                studio.heading = !string.IsNullOrEmpty(datasource.Fields[Templates.TownshipSlideBar.Fields.Heading].Value) ? datasource.Fields[Templates.TownshipSlideBar.Fields.Heading].Value : "";
                var selectedServices = datasource.GetMultiListValueItem(Templates.TownshipSlideBar.Fields.TownshipServices);
                if (selectedServices != null)
                {
                    foreach (var childService in selectedServices)
                    {
                        Service service = new Service();
                        service.type = !string.IsNullOrEmpty(childService.Fields[Templates.TownshipSlideBar.Fields.TownshipServicesTitle].Value) ? childService.Fields[Templates.TownshipSlideBar.Fields.TownshipServicesTitle].Value : "";

                        service.key = count;
                        count = count + 1;
                        listOfService.Add(service);
                    }
                    studio.services = listOfService;
                    townshipSidebar.studio = studio;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return townshipSidebar;
        }
        /* 09/06/2022
       * Changed started 
       * Changed End 
       *         */
        public List<object> ExploreTownship(Rendering rendering)
        {
            List<object> obj = new List<object>();
            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                   ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                   : null;
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                var ExploretownshipMediaItem = datasource.TemplateID == Templates.ExploreTownshipMedia.TemplateID ? datasource.GetMultiListValueItem(Templates.ExploreTownshipMedia.Fields.MediaSelection) : null;
                if (ExploretownshipMediaItem != null)
                {
                    foreach (var item in ExploretownshipMediaItem)
                    {
                        ExploreTownship exploreTownship = new ExploreTownship();
                        exploreTownship.id = !string.IsNullOrEmpty(item.Fields[Templates.ExploreTownshipMedia.Fields.ID].Value) ? item.Fields[Templates.ExploreTownshipMedia.Fields.ID].Value : "";

                        //Changed started 
                        string str = Helper.GetImageSource(item, Templates.ExploreTownshipMedia.Fields.ImageFieldName) != null ? Helper.GetImageSource(item, Templates.ExploreTownshipMedia.Fields.ImageFieldName) : "";

                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        exploreTownship.src = str;
                        //Changed End

                        exploreTownship.imgAlt = Helper.GetImageDetails(item, Templates.ExploreTownshipMedia.Fields.ImageFieldName) != null ?
                                       Helper.GetImageDetails(item, Templates.ExploreTownshipMedia.Fields.ImageFieldName).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                        exploreTownship.dataCols = !string.IsNullOrEmpty(item.Fields[Templates.ExploreTownshipMedia.Fields.dataCols].Value) ? item.Fields[Templates.ExploreTownshipMedia.Fields.dataCols].Value : "";
                        exploreTownship.imgtitle = !string.IsNullOrEmpty(item.Fields[Templates.ExploreTownshipMedia.Fields.imgtypeID].Value) ? item.Fields[Templates.ExploreTownshipMedia.Fields.imgtypeID].Value : "";
                        obj.Add(exploreTownship);
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return obj;
        }
        public TownshipMasterLayout GetTownshipMasterLayout(Rendering rendering)
        {
            TownshipMasterLayout obj = new TownshipMasterLayout();
            List<TownshipPoints> listofPoints = new List<TownshipPoints>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                   ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                   : null;
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                var ExploretownshipMediaItem = datasource.TemplateID == Templates.TownshipMasterLayoutTemp.TemplateID ? datasource.Axes.GetDescendants().ToList() : null;

                if (ExploretownshipMediaItem != null)
                {
                    obj.image = Helper.GetImageSource(datasource, Templates.TownshipMasterLayoutTemp.Fields.ImageFieldName) != null ? Helper.GetImageSource(datasource, Templates.TownshipMasterLayoutTemp.Fields.ImageFieldName) : "";
                    foreach (var item in ExploretownshipMediaItem)
                    {
                        TownshipPoints townshipPoints = new TownshipPoints();
                        townshipPoints.left = !string.IsNullOrEmpty(item.Fields[Templates.TownshipMasterLayoutTemp.Fields.left].Value) ? item.Fields[Templates.TownshipMasterLayoutTemp.Fields.left].Value : "";
                        townshipPoints.bottom = !string.IsNullOrEmpty(item.Fields[Templates.TownshipMasterLayoutTemp.Fields.bottom].Value) ? item.Fields[Templates.TownshipMasterLayoutTemp.Fields.bottom].Value : "";
                        townshipPoints.title = !string.IsNullOrEmpty(item.Fields[Templates.TownshipMasterLayoutTemp.Fields.title].Value) ? item.Fields[Templates.TownshipMasterLayoutTemp.Fields.title].Value : "";
                        listofPoints.Add(townshipPoints);
                    }
                    obj.points = listofPoints;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return obj;
        }

        public ContactCtaDataModel GetContactCtaDataModel(Rendering rendering)
        {
            ContactCtaDataModel propertyBasicInfo = new ContactCtaDataModel();
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
                 ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                 : null;



                propertyBasicInfo.contactCtaData = new Models.ContactCtaData();
                propertyBasicInfo.contactCtaData.getInTouch = !string.IsNullOrEmpty(item.Fields[Templates.ContactCtaData.Fields.getInTouch].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ContactCtaData.Fields.getInTouch].Value.Trim()) : "";
                propertyBasicInfo.contactCtaData.heading = !string.IsNullOrEmpty(item.Fields[Templates.ContactCtaData.Fields.heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ContactCtaData.Fields.heading].Value.Trim()) : "";
                propertyBasicInfo.contactCtaData.desc = !string.IsNullOrEmpty(item.Fields[Templates.ContactCtaData.Fields.desc].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ContactCtaData.Fields.desc].Value.Trim()) : "";
                propertyBasicInfo.contactCtaData.button = !string.IsNullOrEmpty(item.Fields[Templates.ContactCtaData.Fields.button].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ContactCtaData.Fields.button].Value.Trim()) : "";
                propertyBasicInfo.contactCtaData.enquireNow = !string.IsNullOrEmpty(item.Fields[Templates.ContactCtaData.Fields.enquireNow].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ContactCtaData.Fields.enquireNow].Value.Trim()) : "";
                propertyBasicInfo.contactCtaData.propertyThanks = new Models.Clubthanks();

                propertyBasicInfo.contactCtaData.propertyThanks.heading = !string.IsNullOrEmpty(item.Fields[Templates.Clubthanks.Fields.heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Clubthanks.Fields.heading].Value.Trim()) : "";
                propertyBasicInfo.contactCtaData.propertyThanks.para = !string.IsNullOrEmpty(item.Fields[Templates.Clubthanks.Fields.para].Value.ToString()) ? Convert.ToString(item.Fields[Templates.Clubthanks.Fields.para].Value.Trim()) : "";
                propertyBasicInfo.contactCtaData.propertyThanks.data = new List<Datum>();


                MultilistField multilistField = item.Fields[Templates.Clubthanks.Fields.data];
                if (multilistField != null)
                {
                    foreach (var projectItm in multilistField.GetItems())
                    {
                        propertyBasicInfo.contactCtaData.propertyThanks.data.Add(new Datum()
                        {
                            labelheading = !string.IsNullOrEmpty(projectItm.Fields[Templates.DatumItem.Fields.labelheading].Value.ToString()) ? Convert.ToString(projectItm.Fields[Templates.DatumItem.Fields.labelheading].Value.Trim()) : "",
                            labeldata = !string.IsNullOrEmpty(projectItm.Fields[Templates.DatumItem.Fields.labeldata].Value.ToString()) ? Convert.ToString(projectItm.Fields[Templates.DatumItem.Fields.labeldata].Value.Trim()) : "",
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return propertyBasicInfo;
        }


        public dynamic GetProjectNameModel(Rendering rendering)
        {
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
                 ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                 : null;

                ProjectName propertyBasicInfo = new ProjectName();

                propertyBasicInfo.title = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.title].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.title].Value.Trim()) : "";
                propertyBasicInfo.projectLink = Helper.GetLinkURL(item, ProjectNameItem.Fields.linkText) != null ?
                                            Helper.GetLinkURL(item, ProjectNameItem.Fields.linkText) : "";
                propertyBasicInfo.location = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.location].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.location].Value.Trim()) : "";
                propertyBasicInfo.price = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.price].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.price].Value.Trim()) : "";
                if (!string.IsNullOrWhiteSpace(propertyBasicInfo.price))
                {
                    propertyBasicInfo.discount = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.discount].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.discount].Value.Trim()) : "";
                    propertyBasicInfo.discountLabel = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.discountLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.discountLabel].Value.Trim()) : "";
                    propertyBasicInfo.priceLabel = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.priceLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.priceLabel].Value.Trim()) : "";
                    propertyBasicInfo.priceStartingAt = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.priceStartingAt].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.priceStartingAt].Value.Trim()) : "";
                    propertyBasicInfo.Rs = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.Rs].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.Rs].Value.Trim()) : "";
                    propertyBasicInfo.sup = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.sup].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.sup].Value.Trim()) : "";
                    propertyBasicInfo.allInclusive = !string.IsNullOrEmpty(item.Fields[Templates.ProjectNameItem.Fields.allInclusive].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectNameItem.Fields.allInclusive].Value.Trim()) : "";
                }

                dynamic projectName = new
                {
                    projectName = propertyBasicInfo
                };
                return projectName;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
                return null;
            }


        }

        public dynamic GetNavbarTabsModel(Rendering rendering)
        {
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
                 ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                 : null;

                NavbarTabs navbarTabs = new NavbarTabs();

                navbarTabs.about = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.About].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.About].Value.Trim()) : "";
                navbarTabs.ameneties = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.Ameneties].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.Ameneties].Value.Trim()) : "";
                navbarTabs.projects = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.Projects].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.Projects].Value.Trim()) : "";
                navbarTabs.masterLayout = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.MasterLayout].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.MasterLayout].Value.Trim()) : "";
                navbarTabs.typicalFloorPlan = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.TypicalFloorPlan].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.TypicalFloorPlan].Value.Trim()) : "";
                navbarTabs.typicalUnitPlan = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.TypicalUnitPlan].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.TypicalUnitPlan].Value.Trim()) : "";
                navbarTabs.exploreTownship = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.ExploreTownship].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.ExploreTownship].Value.Trim()) : "";
                navbarTabs.locationMap = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.LocationMap].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.LocationMap].Value.Trim()) : "";
                navbarTabs.video = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.video].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.video].Value.Trim()) : "";
                navbarTabs.projectHighlights = !string.IsNullOrEmpty(item.Fields[Templates.NavbarTabsItem.Fields.projectHighlights].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NavbarTabsItem.Fields.projectHighlights].Value.Trim()) : "";
                dynamic navbarTabModel = new
                {
                    navbarTabs = navbarTabs
                };
                return navbarTabModel;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
                return null;
            }

        }
        /* 09/06/2022
       * Changed started 
       * Changed End 
       *         */
        public PropertyHighLight GetProjectHightight(Item item)
        {
            PropertyHighLight projectHighLight = null;

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            string str = string.Empty;
            //Changed End 
            try
            {
                ProjectHighlights projectHighlights = new ProjectHighlights();
                var galleryIconData = item.GetMultiListValueItem(ProjectHighLightTemplate.Fields.GalleryIconData);
                var reraData = item.GetMultiListValueItem(ProjectHighLightTemplate.Fields.ReraData);
                List<GalleryIconsData> galleryIconsDatas = new List<GalleryIconsData>();
                List<ReraData> reraIconData = new List<ReraData>();
                foreach (var galleryicon in galleryIconData)
                {
                    GalleryIconsData galleryIconsData = new GalleryIconsData();

                    //Changed started 
                    str = !string.IsNullOrEmpty(galleryicon.Fields[Templates.GalleryIconDataTemplate.Fields.Icon].Value.ToString()) ? Convert.ToString(galleryicon.Fields[Templates.GalleryIconDataTemplate.Fields.Icon].Value.Trim()) : "";
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = string.Empty;
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    galleryIconsData.Icon = str;
                    //Changed End 

                    galleryIconsData.Label = !string.IsNullOrEmpty(galleryicon.Fields[Templates.GalleryIconDataTemplate.Fields.Label].Value.ToString()) ? Convert.ToString(galleryicon.Fields[Templates.GalleryIconDataTemplate.Fields.Label].Value.Trim()) : "";
                    galleryIconsData.Type = !string.IsNullOrEmpty(galleryicon.Fields[Templates.GalleryIconDataTemplate.Fields.Type].Value.ToString()) ? Convert.ToString(galleryicon.Fields[Templates.GalleryIconDataTemplate.Fields.Type].Value.Trim()) : "";
                    galleryIconsDatas.Add(galleryIconsData);
                }

                foreach (var rera in reraData)
                {
                    ReraData reraData1 = new ReraData();
                    //Changed started 
                    str = !string.IsNullOrEmpty(rera.Fields[Templates.ReraDataTemplate.Fields.Icon].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraDataTemplate.Fields.Icon].Value.Trim()) : "";
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = string.Empty;
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    reraData1.Icon = str;

                    reraData1.As = !string.IsNullOrEmpty(rera.Fields[Templates.ReraDataTemplate.Fields.As].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraDataTemplate.Fields.As].Value.Trim()) : "";

                    str = Helper.GetLinkURLbyField(rera, rera.Fields[Templates.ReraDataTemplate.Fields.DownloadLink]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = string.Empty;
                    //    str = str.Replace(strSitedomain, "");
                    //}


                    reraData1.downloadLink = str;

                    //Changed End 
                    reraData1.ProjectListedOn = !string.IsNullOrEmpty(rera.Fields[Templates.ReraDataTemplate.Fields.ProjectListedOn].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ReraDataTemplate.Fields.ProjectListedOn].Value.Trim()) : "";
                    reraData1.Rera = !string.IsNullOrEmpty(rera.Fields[Templates.ReraDataTemplate.Fields.Rera].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraDataTemplate.Fields.Rera].Value.Trim()) : "";
                    reraData1.ReraNumber = !string.IsNullOrEmpty(rera.Fields[Templates.ReraDataTemplate.Fields.ReraNumber].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraDataTemplate.Fields.ReraNumber].Value.Trim()) : "";
                    reraData1.ReraWebsite = !string.IsNullOrEmpty(rera.Fields[Templates.ReraDataTemplate.Fields.ReraWebsite].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraDataTemplate.Fields.ReraWebsite].Value.Trim()) : "";

                    //Changed started 
                    str = Helper.GetLinkURLbyField(rera, rera.Fields[Templates.ReraDataTemplate.Fields.ReraWebsiteLink]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = string.Empty;
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    reraData1.ReraWebsitelink = str;
                    //Changed End

                    reraData1.ReraWebsiteLinkTarget = Helper.GetLinkURLTargetSpace(rera, Templates.ReraDataTemplate.Fields.ReraWebsiteLink.ToString());
                    reraData1.reranumber = !string.IsNullOrEmpty(rera.Fields[Templates.ReraDataTemplate.Fields.ReraNumberlabel].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraDataTemplate.Fields.ReraNumberlabel].Value.Trim()) : "";
                    reraData1.ModalTitle = !string.IsNullOrEmpty(rera.Fields[Templates.ReraDataTemplate.Fields.ModalTitle].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraDataTemplate.Fields.ModalTitle].Value.Trim()) : "";
                    reraData1.download = Helper.GetPropLinkURLbyField(rera, rera.Fields[Templates.ReraDataTemplate.Fields.download]);
                    reraData1.reraModal = new List<GalleryIconModelsData>();
                    Sitecore.Data.Fields.MultilistField slides = rera.Fields[Templates.ReraDataTemplate.Fields.reraModal];
                    foreach (Item highlightsData in slides.GetItems())
                    {
                        Models.GalleryIconModelsData point = new Models.GalleryIconModelsData();
                        point.url = !string.IsNullOrEmpty(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.url].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.url].Value.Trim()) : "";
                        point.reraid = !string.IsNullOrEmpty(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.rearid].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.rearid].Value.Trim()) : "";
                        point.download = !string.IsNullOrEmpty(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.download].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.download].Value.Trim()) : "";
                        point.qrCodeImage = Helper.GetImageURLbyField(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.qrCodeImage]);
                        
                            
                        //Changed started 
                        str = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.downloadLink]);
                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = string.Empty;
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        point.downloadLink = str;
                        //Changed End 

                        //Changed started 
                        str = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.ReraSiteLink]);
                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = string.Empty;
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        point.ReraWebsitelink = str;
                        //Changed End

                        point.reraWebsiteLinkTarget = Helper.GetLinkURLTargetSpace(highlightsData, Templates.GalleryModalDataItemTemplate.Fields.ReraSiteLink.ToString());
                        point.reraTitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.ReraTitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.GalleryModalDataItemTemplate.Fields.ReraTitle].Value.Trim()) : "";
                        reraData1.reraModal.Add(point);
                    }

                    reraIconData.Add(reraData1);
                }

                projectHighlights.GalleryIconsData = galleryIconsDatas;
                projectHighlights.reraData = reraIconData;
                projectHighLight = new PropertyHighLight()
                {
                    ProjectHighlights = projectHighlights
                };

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }


            return projectHighLight;

        }
        /* 09/06/2022
       * Changed started 
       * Changed End 
       *         */
        public dynamic GetGalleryHighlightsModel(Rendering rendering)
        {
            GalleryHighlight galleryHighlight = new GalleryHighlight();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {


                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                List<Models.Highlights> highlights = new List<Models.Highlights>();
                Sitecore.Data.Fields.MultilistField highlits = item.Fields[Templates.Property.Fields.Highlights];
                foreach (Item highlightsData in item.Children)
                {
                    Models.Highlights hightlight = new Models.Highlights();
                    hightlight.Type = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.Type].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.Type].Value.Trim()) : "";

                    hightlight.Src = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.Src]);
                    hightlight.SrcMobile = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.SrcMobile]);
                    hightlight.ImgAlt = Helper.GetLinkDetails(highlightsData, Templates.Highlights.Fields.Src.ToString()) != null ? Helper.GetLinkDetails(highlightsData, Templates.Highlights.Fields.Src.ToString()).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                    hightlight.ImgTitle = Helper.GetLinkDetails(highlightsData, Templates.Highlights.Fields.LogoTitle.ToString()) != null ? Helper.GetLinkDetails(highlightsData, Templates.Highlights.Fields.LogoTitle.ToString()).Fields[Templates.ImageFeilds.Fields.TitleFieldName].Value : "";

                    //Changed started
                    hightlight.LogoSrc = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.LogoSrc]);
                    //Changed End

                    hightlight.LogoAlt = Helper.GetLinkDetails(highlightsData, Templates.Highlights.Fields.LogoSrc.ToString()) != null ? Helper.GetLinkDetails(highlightsData, Templates.Highlights.Fields.LogoSrc.ToString()).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                    hightlight.LogoTitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.LogoTitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.LogoTitle].Value.Trim()) : "";

                    //Changed started
                    string str = string.Empty;
                    str = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.Highlights.Fields.LogoSrc]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str  = str.Replace(strSitedomain, "");
                    //}
                    hightlight.AboutImg = str;

                    str = string.Empty;
                    str = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.Icon].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.Icon].Value.Trim()) : "";

                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    hightlight.Icon = str;
                    //Changed End
                    hightlight.ImgCount = HttpContext.Current.Session["ImageCount"] != null ? HttpContext.Current.Session["ImageCount"].ToString() : "";
                    hightlight.IconDesc = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.IconDesc].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.IconDesc].Value.Trim()) : "";

                    hightlight.Degree = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.Degree].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.Degree].Value.Trim()) : "";
                    hightlight.Tour = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.Tour].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.Tour].Value.Trim()) : "";
                    hightlight.tabType = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Highlights.Fields.tabType].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Highlights.Fields.tabType].Value.Trim()) : "";
                    highlights.Add(hightlight);
                }
                //if (string.IsNullOrEmpty(highlights.FirstOrDefault().IconDesc) || string.IsNullOrWhiteSpace(highlights.FirstOrDefault().IconDesc))
                //{
                //    highlights.FirstOrDefault().IconDesc = Convert.ToString(highlights.Count());
                //}

                galleryHighlight.GalleryHighlights = highlights;
            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return galleryHighlight;
        }
        /* 09/06/2022
      * Changed started 
      * Changed End 
      *         */
        public dynamic GetGalleryModalDataModel(Rendering rendering)
        {
            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                GalleryModalData galleryModalData = new GalleryModalData();
                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                int count = 0;

                galleryModalData.title = !string.IsNullOrEmpty(item.Fields[Templates.GalleryModalDataTemplate.Fields.Title].Value.ToString()) ? Convert.ToString(item.Fields[Templates.GalleryModalDataTemplate.Fields.Title].Value.Trim()) : "";
                //Changed started 
                galleryModalData.closelink = Helper.GetLinkURLbyField(item, item.Fields[Templates.GalleryModalDataTemplate.Fields.CloseLink]);

                galleryModalData.sharelink = Helper.GetLinkURLbyField(item, item.Fields[Templates.GalleryModalDataTemplate.Fields.ShareLink]);
                galleryModalData.share = !string.IsNullOrEmpty(item.Fields[Templates.GalleryModalDataTemplate.Fields.Share].Value.ToString()) ? Convert.ToString(item.Fields[Templates.GalleryModalDataTemplate.Fields.Share].Value.Trim()) : "";

                galleryModalData.videoCarouselData = new Models.VideoCarouselData();
                galleryModalData.videoCarouselData.GalleryTabs = new Models.GalleryTabs();
                galleryModalData.videoCarouselData.GalleryTabs.data = new List<GalleryTabDatum>();



                galleryModalData.videoCarouselData.ModalSlidesData = new Models.ModalSlidesData();
                galleryModalData.videoCarouselData.ModalSlidesData.Gallerydata = new List<GalleryDatum>();

                string str;
                Sitecore.Data.Fields.MultilistField slides = item.Fields[Templates.VideoCarouselData.Fields.ModalSlidesData];
                foreach (Item highlightsData in slides.GetItems())
                {
                    Models.GalleryDatum point = new Models.GalleryDatum();
                    point.id = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Id].Value.ToString()) ? Convert.ToInt32(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Id].Value.Trim()) : 0;

                    //Changed started 
                    str = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Poster]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str  = str.Replace(strSitedomain, "");
                    //}
                    point.poster = str;
                    //Changed End

                    point.posterAlt = Helper.GetLinkDetails(highlightsData, ModalSlidesDataTemplate.Fields.Poster.ToString()) != null ?
                                Helper.GetLinkDetails(highlightsData, ModalSlidesDataTemplate.Fields.Poster.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";

                    //Changed started 
                    str = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Videomp4]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str  = str.Replace(strSitedomain, "");
                    //}
                    point.videomp4 = str;

                    str = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Videoogg]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    point.videoogg = str;

                    str = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.IframeUrl]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    point.iframeurl = str;
                    //Changed End

                    point.thumbsrc = Helper.GetPropLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Thumbsrc]);
                    point.thumbsrcMobile = Helper.GetImageURLbyField(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Thumbsrcmobile]);
                    point.posterMobile = Helper.GetImageURLbyField(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Postermobile]);

                    point.thumbalt = Helper.GetLinkDetails(highlightsData, Templates.ModalSlidesDataTemplate.Fields.Thumbsrc.ToString()) != null ?
                                    Helper.GetLinkDetails(highlightsData, Templates.ModalSlidesDataTemplate.Fields.Thumbsrc.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    point.thumbtitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Thumbtitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.Thumbtitle].Value.Trim()) : "";
                    point.label = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.lable].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.lable].Value) : "";
                    point.datatype = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.DataType].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.DataType].Value.Trim()) : "";
                    if (point.datatype != null && point.datatype.ToLower().Contains("video") == false)
                    {
                        count = count + 1;
                    }
                    else if (point.datatype != null && point.datatype.ToLower() == "gallery")
                    {
                        if (!string.IsNullOrEmpty(point.iframeurl))
                        {
                            count = count + 1;
                        }
                    }
                    point.tabType = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.TabType].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ModalSlidesDataTemplate.Fields.TabType].Value.Trim()) : "";
                    galleryModalData.videoCarouselData.ModalSlidesData.Gallerydata.Add(point);
                }
                HttpContext.Current.Session["ImageCount"] = count;

                Sitecore.Data.Fields.MultilistField tabs = item.Fields[Templates.VideoCarouselData.Fields.GalleryTabs];

                var groupedTabType = galleryModalData.videoCarouselData.ModalSlidesData.Gallerydata.GroupBy(gd => gd.tabType);

                foreach (Item highlightsData in tabs.GetItems())
                {
                    string tablink = !string.IsNullOrEmpty(highlightsData.Fields[Templates.GalleryTabTemplate.Fields.Link].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.GalleryTabTemplate.Fields.Link].Value.Trim()) : "";
                    var tabData = groupedTabType.ToList().FirstOrDefault(gt => gt.Key.Equals(tablink, StringComparison.InvariantCultureIgnoreCase));
                    if (tabData != null)
                    {
                        Models.GalleryTabDatum galleryTabDatum = new Models.GalleryTabDatum();

                        //Changed started 
                        str = tablink;
                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        galleryTabDatum.link = str;
                        //Changed End 

                        galleryTabDatum.tabtypecount = tabData.Key.Equals("enquirenow", StringComparison.InvariantCultureIgnoreCase) ? "" : tabData.Count().ToString();
                        galleryTabDatum.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.GalleryTabTemplate.Fields.Title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.GalleryTabTemplate.Fields.Title].Value.Trim()) : "";
                        galleryModalData.videoCarouselData.GalleryTabs.data.Add(galleryTabDatum);
                    }
                }


                ModalShare modalShare = new ModalShare();
                Models.ProjectData projectData = new Models.ProjectData();

                modalShare.titleHeading = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Heading].Value.Trim()) : "";

                //Changed started 
                str = Helper.GetImageSource(item, ProjectModelShareTemplate.Fields.SrcFieldname) != null ?
                                    Helper.GetImageSource(item, ProjectModelShareTemplate.Fields.SrcFieldname) : "";
                //if (str.Contains(strSitedomain))
                //{
                //    str = str.Replace(strSitedomain, "");
                //}
                modalShare.src = str;
                //Changed End 



                modalShare.imgAlt = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.ImgAlt].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Heading].Value.Trim()) : "";
                modalShare.label = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Label].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Label].Value.Trim()) : "";
                modalShare.location = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Location].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Location].Value.Trim()) : "";
                modalShare.copylink = modalShare.copylink = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Copylink].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Copylink].Value) : "";
                modalShare.email = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Email].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Email].Value.Trim()) : "";
                modalShare.twitter = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Twitter].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Twitter].Value.Trim()) : "";
                modalShare.facebook = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Facebook].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Facebook].Value.Trim()) : "";
                modalShare.whatsapp = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Whatsapp].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Whatsapp].Value.Trim()) : "";

                galleryModalData.modalShare = modalShare;

                dynamic galleryModalDataModel = new { galleryModalData = galleryModalData };
                return galleryModalDataModel;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
                return null;
            }
        }

        public MasterLayoutDataModel GetMasterLayoutDataModel(Rendering rendering)
        {
            MasterLayoutDataModel masterLayoutDataModel = new MasterLayoutDataModel();
            try
            {


                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                MasterLayoutData masterLayoutData = new MasterLayoutData();
                masterLayoutData.componentID = !string.IsNullOrEmpty(item.Fields[Templates.MasterLayoutDataTemplate.Fields.ComponentID].Value.ToString()) ? Convert.ToString(item.Fields[Templates.MasterLayoutDataTemplate.Fields.ComponentID].Value.Trim()) : "";
                masterLayoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.MasterLayoutDataTemplate.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.MasterLayoutDataTemplate.Fields.Heading].Value.Trim()) : "";

                masterLayoutData.pointerData = new PointerData();

                masterLayoutData.pointerData.height = !string.IsNullOrEmpty(item.Fields[Templates.PointerDataTemplate.Fields.Height].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PointerDataTemplate.Fields.Height].Value.Trim()) : "";
                masterLayoutData.pointerData.image = Helper.GetPropLinkURLbyField(item, item.Fields[Templates.PointerDataTemplate.Fields.Image]);
                masterLayoutData.pointerData.imgAlt = !string.IsNullOrEmpty(item.Fields[Templates.PointerDataTemplate.Fields.ImageAlt].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PointerDataTemplate.Fields.ImageAlt].Value.Trim()) : "";
                masterLayoutData.pointerData.width = !string.IsNullOrEmpty(item.Fields[Templates.PointerDataTemplate.Fields.Width].Value.ToString()) ? Convert.ToString(item.Fields[Templates.PointerDataTemplate.Fields.Width].Value.Trim()) : "";
                masterLayoutData.pointerData.points = new List<PointData>();

                Sitecore.Data.Fields.MultilistField points = item.Fields[Templates.PointerDataTemplate.Fields.Points];
                foreach (Item highlightsData in points.GetItems())
                {
                    Models.PointData point = new Models.PointData();
                    point.left = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Points.Fields.Left].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Points.Fields.Left].Value.Trim()) : "";
                    point.bottom = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Points.Fields.Bottom].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Points.Fields.Bottom].Value.Trim()) : "";
                    point.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.Points.Fields.Title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.Points.Fields.Title].Value.Trim()) : "";

                    masterLayoutData.pointerData.points.Add(point);
                }
                masterLayoutDataModel.masterLayoutData = masterLayoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return masterLayoutDataModel;
        }
        /* 09/06/2022
      * Changed started 
      * Changed End 
      *         */
        public LocationDataModel GetLocationDataModel(Rendering rendering)
        {
            LocationDataModel locationDataModel = new LocationDataModel();
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
              ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
              : null;
                var commonItem = Sitecore.Context.Database.GetItem(commonData.ItemID);

                //Changed started
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                //Changed End

                LocationData locationData = new LocationData();

                locationData.componentID = !string.IsNullOrEmpty(item.Fields[Templates.LocationDataItem.Fields.ComponentId].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LocationDataItem.Fields.ComponentId].Value.Trim()) : "";
                locationData.heading = !string.IsNullOrEmpty(item.Fields[Templates.LocationDataItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LocationDataItem.Fields.Heading].Value.Trim()) : "";
                locationData.longitude = item?.Fields[Templates.LocationDataItem.Fields.Longitude]?.Value;
                locationData.latitude = item?.Fields[Templates.LocationDataItem.Fields.Lattitude]?.Value;
                locationData.MapIframeUrl = item?.Fields[Templates.LocationDataItem.Fields.MapIFrameUrl]?.Value;
                locationData.contactUsLabel = commonItem?.Fields[commonData.Fields.ContactUSLabel]?.Value;
                locationData.ImageLogo = Helper.GetImageURLbyField(Sitecore.Context.Item?.Fields[Templates.Property.Fields.PropertyLogoImg]);
                locationData.ProjectLocation = GetProjectLocation(Sitecore.Context.Item);
                locationData.MarkerTitle = Sitecore.Context.Item?.Fields[Templates.ResidentialPropertyData.Fields.Title]?.Value;

                locationData.facilitiesData = new List<FacilitiesDatum>();

                Sitecore.Data.Fields.MultilistField facilities = item.Fields[Templates.LocationDataItem.Fields.Facilities];
                foreach (Item highlightsData in facilities.GetItems())
                {
                    Models.FacilitiesDatum facilitiesDatum = new Models.FacilitiesDatum();

                    facilitiesDatum.label = !string.IsNullOrEmpty(highlightsData.Fields[Templates.FacilityItem.Fields.Title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.FacilityItem.Fields.Title].Value.Trim()) : "";
                    //Changed started
                    string str = !string.IsNullOrEmpty(highlightsData.Fields[Templates.FacilityItem.Fields.Icon].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.FacilityItem.Fields.Icon].Value.Trim()) : "";

                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    facilitiesDatum.icon = str;
                    //Changed End

                    locationData.facilitiesData.Add(facilitiesDatum);
                }
                locationDataModel.locationData = locationData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return locationDataModel;
        }

        private ProjectLocation GetProjectLocation(Item item)
        {
            ProjectLocation projectLocation = new ProjectLocation();
            try
            {
                if (item != null)
                {
                    var commonItem = Sitecore.Context.Database.GetItem(commonData.ItemID);
                    projectLocation.contactUsLabel = commonItem?.Fields[commonData.Fields.ContactUSLabel]?.Value;
                    projectLocation.Address1 = item?.Fields[AddressTemplate.Fields.Address1]?.Value;
                    projectLocation.Address2 = item?.Fields[AddressTemplate.Fields.Address2]?.Value;
                    projectLocation.City = item?.Fields[AddressTemplate.Fields.City]?.Value;
                    projectLocation.State = item?.Fields[AddressTemplate.Fields.State]?.Value;
                    projectLocation.Country = item?.Fields[AddressTemplate.Fields.Country]?.Value;
                    projectLocation.Pincode = item?.Fields[AddressTemplate.Fields.Pincode]?.Value;
                    projectLocation.Contact = item?.Fields[AddressTemplate.Fields.Contact]?.Value;
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return projectLocation;
        }
        /* 09/06/2022
      * Changed started 
      * Changed End 
      *         */
        public SimilarProjectsDataModel GetSimilarProjectsDataModel(Rendering rendering)
        {
            SimilarProjectsDataModel similarProjectsDataModel = new SimilarProjectsDataModel();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
              ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
              : null;

                SimilarProjectsData locationData = new SimilarProjectsData();

                locationData.heading = !string.IsNullOrEmpty(item.Fields[Templates.SimilarProjectDataItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.SimilarProjectDataItem.Fields.Heading].Value.Trim()) : "";
                locationData.data = new List<SimilarProject>();

                Sitecore.Data.Fields.MultilistField projects = item.Fields[Templates.SimilarProjectDataItem.Fields.Projects];
                foreach (Item highlightsData in projects.GetItems())
                {

                    Models.SimilarProject facilitiesDatum = new Models.SimilarProject();
                    facilitiesDatum.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.SimilarProjctItem.Fields.Title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.SimilarProjctItem.Fields.Title].Value.Trim()) : "";
                    facilitiesDatum.type = !string.IsNullOrEmpty(highlightsData.Fields[Templates.SimilarProjctItem.Fields.Type].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.SimilarProjctItem.Fields.Type].Value.Trim()) : "";
                    facilitiesDatum.status = !string.IsNullOrEmpty(highlightsData.Fields[Templates.SimilarProjctItem.Fields.Status].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.SimilarProjctItem.Fields.Status].Value.Trim()) : "";

                    facilitiesDatum.link = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.SimilarProjctItem.Fields.Link]);

                    //Changed started 
                    string str = Helper.GetImageURLbyField(highlightsData.Fields[Templates.SimilarProjctItem.Fields.Src]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    facilitiesDatum.src = str;
                    //Changed End

                    facilitiesDatum.srcAlt = Helper.GetImageDetails(highlightsData, Templates.SimilarProjctItem.Fields.Src.ToString()) != null ?
                                                Helper.GetImageDetails(highlightsData, Templates.SimilarProjctItem.Fields.Src.ToString()).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : ""; ;

                    //Changed started 
                    str = string.Empty;
                    str = Helper.GetImageURLbyField(highlightsData.Fields[Templates.SimilarProjctItem.Fields.LogoSrc]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    facilitiesDatum.logosrc = str;
                    //Changed started 
                    facilitiesDatum.linkTarget = Helper.GetLinkURLTargetSpace(highlightsData, Templates.SimilarProjctItem.Fields.Link.ToString()) != null ?
                                            Helper.GetLinkURLTargetSpace(highlightsData, Templates.SimilarProjctItem.Fields.Link.ToString()) : "";
                    facilitiesDatum.logoAlt = Helper.GetImageDetails(highlightsData, Templates.SimilarProjctItem.Fields.LogoSrc.ToString()) != null ?
                                                Helper.GetImageDetails(highlightsData, Templates.SimilarProjctItem.Fields.LogoSrc.ToString()).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : ""; ;

                    locationData.data.Add(facilitiesDatum);
                }

                similarProjectsDataModel.similarProjectsData = locationData;
            }

            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return similarProjectsDataModel;
        }
        /* 09/06/2022
      * Changed started 
      * Changed End 
      *         */
        public ProjectFloorPlanData GetProjectFloorPlanData(Rendering rendering)
        {
            ProjectFloorPlanData projectFloorPlanData = new ProjectFloorPlanData();
            List<PlanData> planDatas = new List<PlanData>();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
              ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
              : null;


                var floorplandata = item.GetMultiListValueItem(ProjectFloorPlanDataItem.Fields.FiledsID);
                PlanData planData = new PlanData();


                if (floorplandata != null)
                {
                    planData.floorPlanData = new List<FloorPlanData>();
                    foreach (var floorplandataItem in floorplandata)
                    {

                        FloorPlanData FloorPlanData = new FloorPlanData();
                        planData.componentID = !string.IsNullOrEmpty(floorplandataItem.Fields[Templates.ProjectFloorPlanDataItem.Fields.ComponentId].Value.ToString()) ? Convert.ToString(floorplandataItem.Fields[Templates.ProjectFloorPlanDataItem.Fields.ComponentId].Value.Trim()) : "";
                        planData.heading = !string.IsNullOrEmpty(floorplandataItem.Fields[Templates.ProjectFloorPlanDataItem.Fields.Heading].Value.ToString()) ? Convert.ToString(floorplandataItem.Fields[Templates.ProjectFloorPlanDataItem.Fields.Heading].Value.Trim()) : "";

                        //Changed started
                        string str = Helper.GetPropLinkURLbyField(floorplandataItem, floorplandataItem.Fields[Templates.ProjectFloorPlanDataItem.Fields.Src]);
                        //if (str.Contains(strSitedomain))
                        //{
                        //    str  = str.Replace(strSitedomain, "");
                        //}
                        FloorPlanData.src = str;
                        //Changed End 

                        FloorPlanData.imgAlt = Helper.GetImageDetails(floorplandataItem, Templates.ProjectFloorPlanDataItem.Fields.Src.ToString()) != null ? Helper.GetImageDetails(floorplandataItem, Templates.ProjectFloorPlanDataItem.Fields.Src.ToString()).Fields[Templates.ImageFeilds.Fields.AltFieldName].Value : "";
                        FloorPlanData.tabHeading = !string.IsNullOrEmpty(floorplandataItem.Fields[Templates.ProjectFloorPlanDataItem.Fields.tabHeading].Value.ToString()) ? Convert.ToString(floorplandataItem.Fields[Templates.ProjectFloorPlanDataItem.Fields.tabHeading].Value.Trim()) : "";
                        FloorPlanData.points = new List<Point>();
                        Sitecore.Data.Fields.MultilistField points = floorplandataItem.Fields[Templates.ProjectFloorPlanDataItem.Fields.Points];
                        foreach (Item highlightsData in points.GetItems())
                        {
                            Models.Point point = new Models.Point();
                            point.left = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ProjectFloorPoint.Fields.Left].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ProjectFloorPoint.Fields.Left].Value.Trim()) : "";
                            point.bottom = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ProjectFloorPoint.Fields.Bottom].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ProjectFloorPoint.Fields.Bottom].Value.Trim()) : "";
                            point.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ProjectFloorPoint.Fields.Title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ProjectFloorPoint.Fields.Title].Value.Trim()) : "";
                            FloorPlanData.points.Add(point);
                        }
                        planData.floorPlanData.Add(FloorPlanData);
                    }
                    projectFloorPlanData.projectFloorPlanData = planData;

                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return projectFloorPlanData;
        }

        public LivingTheGoodLifeDataModel GetLivingTheGoodLifeDataModel(Rendering rendering)
        {

            LivingTheGoodLifeDataModel projectFloorPlanData = new LivingTheGoodLifeDataModel();
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
              ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
              : null;

                LivingTheGoodLifeData planData = new LivingTheGoodLifeData();

                planData.heading = !string.IsNullOrEmpty(item.Fields[Templates.LivingTheGoodLifeItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LivingTheGoodLifeItem.Fields.Heading].Value.Trim()) : "";
                planData.testimonialData = new List<TestimonialDatum>();

                Sitecore.Data.Fields.MultilistField points = item.Fields[Templates.LivingTheGoodLifeItem.Fields.Testimonials];
                foreach (Item highlightsData in points.GetItems())
                {
                    Models.TestimonialDatum point = new Models.TestimonialDatum();
                    point.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.title].Value.Trim()) : "";
                    point.description = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.Description].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.Description].Value.Trim()) : "";
                    point.author = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.Author].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.Author].Value.Trim()) : "";
                    point.useravtar = Helper.GetImageURLbyField(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtar]);
                    point.useravtaralt = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtaralt].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtaralt].Value.Trim()) : "";
                    point.useravtartitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtartitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtartitle].Value.Trim()) : "";
                    point.isVideoTestimonial = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.isVideoTestimonial].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.isVideoTestimonial].Value.Trim()) : "";
                    point.propertylocation = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.propertylocation].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.propertylocation].Value.Trim()) : "";
                    point.iframesrc = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.TestimonialItem.Fields.iframesrc]);
                    point.SEOName = highlightsData.Fields[Templates.TestimonialItem.Fields.SEOName].Value != null ? highlightsData.Fields[Templates.TestimonialItem.Fields.SEOName].Value : "";
                    point.SEODescription = highlightsData.Fields[Templates.TestimonialItem.Fields.SEODescription].Value != null ? highlightsData.Fields[Templates.TestimonialItem.Fields.SEODescription].Value : "";
                    point.UploadDate = highlightsData.Fields[Templates.TestimonialItem.Fields.UploadDate].Value != null ? highlightsData.Fields[Templates.TestimonialItem.Fields.UploadDate].Value : "";

                    planData.testimonialData.Add(point);
                }
                projectFloorPlanData.livingTheGoodLifeData = planData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return projectFloorPlanData;
        }
        /* 09/06/2022
         * Line 1493 to 1496 Added
         * Line 1514 to 1522 Added
         *         */
        public ProjectUnitPlanData GetProjectUnitPlanData(Rendering rendering)
        {
            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                ProjectUnitPlanData projectUnitPlanData = new ProjectUnitPlanData();
                var item = rendering?.Item;
                List<Models.TypicalUnitPlanData> typicalUnitPlanList = new List<Models.TypicalUnitPlanData>();
                if (item != null)
                {
                    projectUnitPlanData.ComponentID = item?.Fields[ProjectUnitPlanDataTemplate.Fields.ComponentID]?.Value;
                    projectUnitPlanData.Heading = item?.Fields[ProjectUnitPlanDataTemplate.Fields.Heading]?.Value;
                    if (item?.GetChildren()?.Count() > 0)
                    {
                        foreach (Item specItem in item?.GetChildren())
                        {
                            Models.TypicalUnitPlanData typicalUnitPlanData = new Models.TypicalUnitPlanData();
                            typicalUnitPlanData.Title = specItem?.Fields[TypicalUnitPlanDataTemplate.Fields.title]?.Value;
                            typicalUnitPlanData.Desc = specItem?.Fields[TypicalUnitPlanDataTemplate.Fields.desc]?.Value;

                            //Changed started  
                            string str = Helper.GetPropLinkURLbyField(specItem, specItem.Fields[TypicalUnitPlanDataTemplate.Fields.Src]);
                            //if (str.Contains(strSitedomain))
                            //{
                            //    str  = str.Replace(strSitedomain, "");
                            //}
                            typicalUnitPlanData.Src = str;
                            //Changed End 

                            typicalUnitPlanData.imgAlt = Helper.GetLinkDetails(specItem, TypicalUnitPlanDataTemplate.Fields.Src.ToString()) != null ?
Helper.GetLinkDetails(specItem, TypicalUnitPlanDataTemplate.Fields.Src.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";

                            typicalUnitPlanData.Link = Helper.GetLinkURLbyField(specItem, specItem.Fields[TypicalUnitPlanDataTemplate.Fields.link]);

                            typicalUnitPlanData.ButtonText = specItem?.Fields[TypicalUnitPlanDataTemplate.Fields.ButtonText]?.Value;
                            MultilistField floorSpecField = specItem?.Fields[TypicalUnitPlanDataTemplate.Fields.FloorSpecifications];
                            typicalUnitPlanData.ProjectDetails = new List<ProjectDetails>();
                            if (floorSpecField != null && floorSpecField?.GetItems()?.Count() > 0)
                            {
                                foreach (Item floorItem in floorSpecField?.GetItems())
                                {

                                    ProjectDetails projectDetails = new ProjectDetails();
                                    projectDetails.SizeIn = !string.IsNullOrEmpty(floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.SizeIn].Value.ToString()) ? Convert.ToString(floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.SizeIn].Value.Trim()) : "";
                                    projectDetails.Type = !string.IsNullOrEmpty(floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.Type].Value.ToString()) ? Convert.ToString(floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.Type].Value.Trim()) : "";
                                    projectDetails.Src = Helper.GetPropLinkURLbyField(floorItem, floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.Src]);
                                    projectDetails.ImageAlt = Helper.GetLinkDetails(floorItem, ProjectUnitPlanDetailsTemplate.Fields.Src.ToString()) != null ? Helper.GetLinkDetails(floorItem, ProjectUnitPlanDetailsTemplate.Fields.Src.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                                    projectDetails.ImageTitle = !string.IsNullOrEmpty(floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.ImgTitle].Value.ToString()) ? Convert.ToString(floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.ImgTitle].Value.Trim()) : "";
                                    projectDetails.TabHeading = floorItem?.Fields[TypicalUnitPlanDataTemplate.Fields.TabHeading]?.Value;
                                    Sitecore.Data.Fields.MultilistField specifications = floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.Specifications];
                                    if (specifications != null && specifications?.GetItems().Count() > 0)
                                    {
                                        projectDetails.Specifications = new List<Specification>();
                                        foreach (Item specification in specifications?.GetItems())
                                        {
                                            Models.Specification specificationItem = new Models.Specification();
                                            specificationItem.Place = !string.IsNullOrEmpty(specification.Fields[Templates.SpecificationsTemplate.Fields.Place].Value.ToString()) ? Convert.ToString(specification.Fields[Templates.SpecificationsTemplate.Fields.Place].Value.Trim()) : "";
                                            specificationItem.SizeInFeet = !string.IsNullOrEmpty(specification.Fields[Templates.SpecificationsTemplate.Fields.SizeInFeet].Value.ToString()) ? Convert.ToString(specification.Fields[Templates.SpecificationsTemplate.Fields.SizeInFeet].Value.Trim()) : "";
                                            specificationItem.SizeInMetres = !string.IsNullOrEmpty(specification.Fields[Templates.SpecificationsTemplate.Fields.SizeInMetres].Value.ToString()) ? Convert.ToString(specification.Fields[Templates.SpecificationsTemplate.Fields.SizeInMetres].Value.Trim()) : "";
                                            projectDetails.Specifications.Add(specificationItem);
                                        }
                                    }
                                    projectDetails.ReraSpecifications = new List<ReraSpecification>();
                                    Sitecore.Data.Fields.MultilistField reraSpecifications = floorItem.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.ReraSpecifications];
                                    projectDetails.AreaAsPerRera = floorItem?.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.AreaAsPerRera]?.Value;
                                    projectDetails.ReraMeasurementScale = floorItem?.Fields[Templates.ProjectUnitPlanDetailsTemplate.Fields.ReraMeasurementScale]?.Value;
                                    if (reraSpecifications != null && reraSpecifications.GetItems().Count() > 0)
                                    {
                                        foreach (Item rera in reraSpecifications.GetItems())
                                        {
                                            Models.ReraSpecification reraSpecification = new Models.ReraSpecification();
                                            reraSpecification.AreaType = !string.IsNullOrEmpty(rera.Fields[Templates.ReraSpecifications.Fields.AreaType].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraSpecifications.Fields.AreaType].Value.Trim()) : "";
                                            reraSpecification.Size = !string.IsNullOrEmpty(rera.Fields[Templates.ReraSpecifications.Fields.Size].Value.ToString()) ? Convert.ToString(rera.Fields[Templates.ReraSpecifications.Fields.Size].Value.Trim()) : "";

                                            projectDetails.ReraSpecifications.Add(reraSpecification);
                                        }
                                    }
                                    typicalUnitPlanData.ProjectDetails.Add(projectDetails);
                                }
                            }
                            typicalUnitPlanList.Add(typicalUnitPlanData);
                        }
                        projectUnitPlanData.TypicalUnitPlanList = typicalUnitPlanList;
                    }
                }
                return projectUnitPlanData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return null;
        }

        public dynamic GetConfigurationData(Rendering rendering)
        {
            try
            {
                ConfigurationDataModel projectUnitPlanData = new ConfigurationDataModel();
                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                projectUnitPlanData.city = !string.IsNullOrEmpty(item.Fields[Templates.ConfigurationDataTeplate.Fields.City].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ConfigurationDataTeplate.Fields.City].Value.Trim()) : "";
                projectUnitPlanData.items = new List<ConfigurationData>();

                Sitecore.Data.Fields.MultilistField typicalUnitPlanData = item.Fields[Templates.ConfigurationDataTeplate.Fields.Items];
                foreach (Item highlightsData in typicalUnitPlanData.GetItems())
                {
                    Models.ConfigurationData point = new Models.ConfigurationData();
                    point.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ConfigurationItemTeplate.Fields.Title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ConfigurationItemTeplate.Fields.Title].Value.Trim()) : "";
                    point.keys = new List<Key>();

                    Sitecore.Data.Fields.MultilistField specifications = highlightsData.Fields[Templates.ConfigurationItemTeplate.Fields.Key];
                    foreach (Item specification in specifications.GetItems())
                    {
                        Models.Key specificationItem = new Models.Key();
                        specificationItem.link = Helper.GetLinkURLbyField(specification, specification.Fields[Templates.ConfigurationKeyTemplate.Fields.Link]);

                        specificationItem.keyword = !string.IsNullOrEmpty(specification.Fields[Templates.ConfigurationKeyTemplate.Fields.Keyword].Value.ToString()) ? Convert.ToString(specification.Fields[Templates.ConfigurationKeyTemplate.Fields.Keyword].Value.Trim()) : "";

                        point.keys.Add(specificationItem);
                    }

                    projectUnitPlanData.items.Add(point);
                }

                dynamic configurationDataModel = new { configurationData = projectUnitPlanData };


                return configurationDataModel;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return null;
        }

        public LayoutDataModel GetLayoutDataModel(Rendering rendering)
        {
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
            ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
            : null;

                LayoutData layoutData = new LayoutData();

                layoutData.projectTitle = !string.IsNullOrEmpty(item.Fields[Templates.LayoutDataTemplate.Fields.ProjectTitle].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LayoutDataTemplate.Fields.ProjectTitle].Value.Trim()) : "";
                layoutData.projectConfiguration = !string.IsNullOrEmpty(item.Fields[Templates.LayoutDataTemplate.Fields.ProjectConfiguration].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LayoutDataTemplate.Fields.ProjectConfiguration].Value.Trim()) : "";
                layoutData.projectPossesionInfo = !string.IsNullOrEmpty(item.Fields[Templates.LayoutDataTemplate.Fields.ProjectPossessionInfo].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LayoutDataTemplate.Fields.ProjectPossessionInfo].Value.Trim()) : "";
                layoutData.planAVisitLabel = !string.IsNullOrEmpty(item.Fields[Templates.LayoutDataTemplate.Fields.PlanAVisitLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LayoutDataTemplate.Fields.PlanAVisitLabel].Value.Trim()) : "";
                layoutData.bookNowLabel = !string.IsNullOrEmpty(item.Fields[Templates.LayoutDataTemplate.Fields.BookNowLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LayoutDataTemplate.Fields.BookNowLabel].Value.Trim()) : "";
                layoutData.enquireNowLabel = !string.IsNullOrEmpty(item.Fields[Templates.LayoutDataTemplate.Fields.EnquireNowLabel].Value.ToString()) ? Convert.ToString(item.Fields[Templates.LayoutDataTemplate.Fields.EnquireNowLabel].Value.Trim()) : "";

                LayoutDataModel navbarTabModel = new LayoutDataModel()
                {
                    layoutData = layoutData
                };

                return navbarTabModel;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return null;
        }
        /* 09/06/2022
         * Line 1670 to 1597 1673
         * Line 1683 to 1692 Added
         *         */
        public PropertyFaq GetPropertyFaqData(Rendering rendering)
        {
            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
          ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
          : null;
                FaqData faqData = new FaqData();
                List<Faq> faqs = new List<Faq>();
                var renderingParams = rendering?.Parameters;

                //Changed started

                string str = renderingParams != null ? renderingParams["SeeAllLink"] : string.Empty;
                if (str != null)
                {
                    str = strSitedomain + str;
                }
                faqData.faqLink = str;
                //Changed End 
                faqData.id = !string.IsNullOrEmpty(item.Fields[Templates.FaqDataTemplate.Fields.Id].Value.ToString()) ? Convert.ToString(item.Fields[Templates.FaqDataTemplate.Fields.Id].Value.Trim()) : "";
                faqData.heading = !string.IsNullOrEmpty(item.Fields[Templates.FaqDataTemplate.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.FaqDataTemplate.Fields.Heading].Value.Trim()) : "";
                faqData.seeAll = !string.IsNullOrEmpty(item.Fields[Templates.FaqDataTemplate.Fields.seeAll].Value.ToString()) ? Convert.ToString(item.Fields[Templates.FaqDataTemplate.Fields.seeAll].Value.Trim()) : "";
                MultilistField faqvalue = item.Fields[Templates.FaqDataTemplate.Fields.faqs];
                foreach (var fvalue in faqvalue.GetItems())
                {
                    Faq faq = new Faq()
                    {
                        body = !string.IsNullOrEmpty(fvalue.Fields[Templates.FaqTemplate.Fields.Body].Value.ToString()) ? Convert.ToString(fvalue.Fields[Templates.FaqTemplate.Fields.Body].Value.Trim()) : "",
                        title = !string.IsNullOrEmpty(fvalue.Fields[Templates.FaqTemplate.Fields.Title].Value.ToString()) ? Convert.ToString(fvalue.Fields[Templates.FaqTemplate.Fields.Title].Value.Trim()) : ""
                    };

                    faqs.Add(faq);
                }

                faqData.faqs = faqs;
                PropertyFaq propertyFaq = new PropertyFaq()
                {
                    faqData = faqData
                };

                return propertyFaq;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return null;

        }
        /* 09/06/2022
         * Changed started
         * Changed End 
         *         */
        public ProjectActions GetProjectActions(Rendering rendering)
        {
            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 

            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
        ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
        : null;
                ModalShare modalShare = new ModalShare();
                Models.ProjectData projectData = new Models.ProjectData();

                modalShare.titleHeading = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Heading].Value.Trim()) : "";

                //Changed started 

                string str = Helper.GetImageSource(item, ProjectModelShareTemplate.Fields.SrcFieldname) != null ?
                                  Helper.GetImageSource(item, ProjectModelShareTemplate.Fields.SrcFieldname) : "";
                //if (str.Contains(strSitedomain))
                //{
                //    str = str.Replace(strSitedomain, "");
                //}
                modalShare.src = str;

                //Changed End 

                modalShare.imgAlt = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.ImgAlt].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Heading].Value.Trim()) : "";
                modalShare.label = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Label].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Label].Value.Trim()) : "";
                modalShare.location = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Location].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Location].Value.Trim()) : "";
                modalShare.copylink = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Copylink].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Copylink].Value) : "";
                modalShare.email = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Email].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Email].Value.Trim()) : "";
                modalShare.twitter = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Twitter].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Twitter].Value.Trim()) : "";
                modalShare.facebook = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Facebook].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Facebook].Value.Trim()) : "";
                modalShare.whatsapp = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.Whatsapp].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.Whatsapp].Value.Trim()) : "";
                modalShare.PageTitle = !string.IsNullOrEmpty(item.Fields[Templates.ProjectModelShareTemplate.Fields.PageTitle].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectModelShareTemplate.Fields.PageTitle].Value.Trim()) : "";

                projectData.downloadBrochure = !string.IsNullOrEmpty(item.Fields[Templates.ProjectData.Fields.DownloadBrochure].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectData.Fields.DownloadBrochure].Value.Trim()) : "";
                projectData.downloadModalTitle = !string.IsNullOrEmpty(item.Fields[Templates.ProjectData.Fields.downloadModalTitle].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectData.Fields.downloadModalTitle].Value.Trim()) : "";
                projectData.enquireNow = !string.IsNullOrEmpty(item.Fields[Templates.ProjectData.Fields.EnquireNow].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectData.Fields.EnquireNow].Value.Trim()) : "";
                projectData.share = !string.IsNullOrEmpty(item.Fields[Templates.ProjectData.Fields.Share].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ProjectData.Fields.Share].Value.Trim()) : "";
                projectData.downloadPDFLink = Helper.GetPropLinkURLbyField(item, item.Fields[Templates.ProjectData.Fields.DownloadPDFLink]);
                projectData.modalShare = modalShare;
                ProjectActions projectActions = new ProjectActions()
                {
                    projectActions = projectData
                };

                return projectActions;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return null;
        }

        public AboutAdaniData GetAdaniData(Rendering rendering)
        {
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
: null;
                AboutData aboutData = new AboutData();
                Models.AdaniData adaniData = new Models.AdaniData();

                List<Faq> faqs = new List<Faq>();
                aboutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.AboutAdani.Fields.TitleHeading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutAdani.Fields.TitleHeading].Value.Trim()) : "";
                aboutData.desc = !string.IsNullOrEmpty(item.Fields[Templates.AboutAdani.Fields.Desc].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutAdani.Fields.Desc].Value.Trim()) : "";
                aboutData.readMore = !string.IsNullOrEmpty(item.Fields[Templates.AboutAdani.Fields.ReadMore].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutAdani.Fields.ReadMore].Value.Trim()) : "";
                aboutData.terms = !string.IsNullOrEmpty(item.Fields[Templates.AboutAdani.Fields.Terms].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutAdani.Fields.Terms].Value.Trim()) : "";

                adaniData.componentID = !string.IsNullOrEmpty(item.Fields[Templates.AdaniData.Fields.ComponentId].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AdaniData.Fields.ComponentId].Value.Trim()) : "";
                adaniData.readMore = !string.IsNullOrEmpty(item.Fields[Templates.AdaniData.Fields.ReadMore].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AdaniData.Fields.ReadMore].Value.Trim()) : "";
                adaniData.readLess = !string.IsNullOrEmpty(item.Fields[Templates.AdaniData.Fields.ReadLess].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AdaniData.Fields.ReadLess].Value.Trim()) : "";
                adaniData.description = !string.IsNullOrEmpty(item.Fields[Templates.AdaniData.Fields.description].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AdaniData.Fields.description].Value.Trim()) : "";

                adaniData.aboutData = aboutData;

                AboutAdaniData aboutAdaniData = new AboutAdaniData()
                {
                    aboutAdaniData = adaniData
                };

                return aboutAdaniData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return null;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End
         *         */
        public NRIBannerModel GetNRIBannerModel(Rendering rendering)
        {
            NRIBannerModel nRIBannerModel = new NRIBannerModel();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 

            try
            {

                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                NriBanner layoutData = new NriBanner();

                layoutData.title = !string.IsNullOrEmpty(item.Fields[Templates.NRIBanner.Fields.Title].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NRIBanner.Fields.Title].Value.Trim()) : "";
                layoutData.class1 = !string.IsNullOrEmpty(item.Fields[Templates.NRIBanner.Fields.Class1].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NRIBanner.Fields.Class1].Value.Trim()) : "";
                layoutData.class2 = !string.IsNullOrEmpty(item.Fields[Templates.NRIBanner.Fields.Class2].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NRIBanner.Fields.Class2].Value.Trim()) : "";
                layoutData.alt = !string.IsNullOrEmpty(item.Fields[Templates.NRIBanner.Fields.alt].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NRIBanner.Fields.alt].Value.Trim()) : "";

                //Changed started 
                string str = Helper.GetLinkURLbyField(item, item.Fields[Templates.NRIBanner.Fields.Src]);
                //if (str.Contains(strSitedomain))
                //{
                //    str = str.Replace(strSitedomain, "");
                //}
                layoutData.src = str;
                //Changed End 

                nRIBannerModel.nriBanner = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return nRIBannerModel;
        }

        public ContentModel GetContentModel(Rendering rendering)
        {
            ContentModel contentModel = new ContentModel();
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
           ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
           : null;

                Content layoutData = new Content();

                layoutData.title = !string.IsNullOrEmpty(item.Fields[Templates.ContentItem.Fields.Title].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ContentItem.Fields.Title].Value.Trim()) : "";
                layoutData.pageData = !string.IsNullOrEmpty(item.Fields[Templates.ContentItem.Fields.PageData].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ContentItem.Fields.PageData].Value.Trim()) : "";
                layoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.ContentItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ContentItem.Fields.Heading].Value.Trim()) : "";

                contentModel.Content = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }


            return contentModel;
        }

        public AboutNRIModel GetAboutNRIModel(Rendering rendering)
        {
            AboutNRIModel aboutNRIModel = new AboutNRIModel();
            try
            {

                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                AboutNri layoutData = new AboutNri();

                layoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.AboutNriTemplate.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutNriTemplate.Fields.Heading].Value.Trim()) : "";
                layoutData.about = !string.IsNullOrEmpty(item.Fields[Templates.AboutNriTemplate.Fields.About].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutNriTemplate.Fields.About].Value.Trim()) : "";
                layoutData.readMore = !string.IsNullOrEmpty(item.Fields[Templates.AboutNriTemplate.Fields.ReadMore].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutNriTemplate.Fields.ReadMore].Value.Trim()) : "";
                layoutData.terms = !string.IsNullOrEmpty(item.Fields[Templates.AboutNriTemplate.Fields.Terms].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutNriTemplate.Fields.Terms].Value.Trim()) : "";
                layoutData.detailLink = !string.IsNullOrEmpty(item.Fields[Templates.AboutNriTemplate.Fields.DetailLink].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutNriTemplate.Fields.DetailLink].Value.Trim()) : "";
                layoutData.extrCharges = !string.IsNullOrEmpty(item.Fields[Templates.AboutNriTemplate.Fields.ExtrCharges].Value.ToString()) ? Convert.ToString(item.Fields[Templates.AboutNriTemplate.Fields.ExtrCharges].Value.Trim()) : "";


                aboutNRIModel.AboutNri = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutNRIModel;
        }

        public TestimonialModel GetTestimonialModel(Rendering rendering)
        {
            TestimonialModel testimonialModel = new TestimonialModel();
            try
            {

                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                Testimonial layoutData = new Testimonial();

                layoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.NRITestimonialItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.NRITestimonialItem.Fields.Heading].Value.Trim()) : "";

                layoutData.testimonialData = new List<TestimonialDatum>();

                Sitecore.Data.Fields.MultilistField points = item.Fields[Templates.NRITestimonialItem.Fields.TestimonialData];
                foreach (Item highlightsData in points.GetItems())
                {
                    Models.TestimonialDatum point = new Models.TestimonialDatum();
                    point.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.title].Value.Trim()) : "";
                    point.description = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.Description].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.Description].Value.Trim()) : "";
                    point.author = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.Author].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.Author].Value.Trim()) : "";
                    point.useravtar = Helper.GetImageURLbyField(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtar]);
                    point.useravtaralt = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtaralt].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtaralt].Value.Trim()) : "";
                    point.useravtartitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtartitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.useravtartitle].Value.Trim()) : "";
                    point.isVideoTestimonial = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.isVideoTestimonial].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.isVideoTestimonial].Value.Trim()) : "";
                    point.propertylocation = !string.IsNullOrEmpty(highlightsData.Fields[Templates.TestimonialItem.Fields.propertylocation].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.TestimonialItem.Fields.propertylocation].Value.Trim()) : "";
                    point.iframesrc = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.TestimonialItem.Fields.iframesrc]);
                    point.SEOName = highlightsData.Fields[Templates.TestimonialItem.Fields.SEOName].Value != null ? highlightsData.Fields[Templates.TestimonialItem.Fields.SEOName].Value : "";
                    point.SEODescription = highlightsData.Fields[Templates.TestimonialItem.Fields.SEODescription].Value != null ? highlightsData.Fields[Templates.TestimonialItem.Fields.SEODescription].Value : "";
                    point.UploadDate = highlightsData.Fields[Templates.TestimonialItem.Fields.UploadDate].Value != null ? highlightsData.Fields[Templates.TestimonialItem.Fields.UploadDate].Value : "";

                    layoutData.testimonialData.Add(point);
                }


                testimonialModel.testimonial = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return testimonialModel;
        }

        public InvestmentGuidelineModel GetInvestmentGuidelineModel(Rendering rendering)
        {
            InvestmentGuidelineModel investmentGuidelineModel = new InvestmentGuidelineModel();
            try
            {

                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                InvestmentGuidelines layoutData = new InvestmentGuidelines();

                layoutData.content = !string.IsNullOrEmpty(item.Fields[Templates.InvestmentGuidelineItem.Fields.Content].Value.ToString()) ? Convert.ToString(item.Fields[Templates.InvestmentGuidelineItem.Fields.Content].Value.Trim()) : "";
                layoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.InvestmentGuidelineItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.InvestmentGuidelineItem.Fields.Heading].Value.Trim()) : "";
                layoutData.readMore = !string.IsNullOrEmpty(item.Fields[Templates.InvestmentGuidelineItem.Fields.readMore].Value.ToString()) ? Convert.ToString(item.Fields[Templates.InvestmentGuidelineItem.Fields.readMore].Value.Trim()) : "";

                investmentGuidelineModel.investmentGuidelines = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return investmentGuidelineModel;
        }
        /* 09/06/2022
                 * Changed started 
                 * Changed End 
                 *         */
        public WhyInvestModel GetWhyInvestModel(Rendering rendering)
        {
            WhyInvestModel whyInvestModel = new WhyInvestModel();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
            ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
            : null;

                WhyInvest layoutData = new WhyInvest();

                layoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.WhyInvestItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.WhyInvestItem.Fields.Heading].Value.Trim()) : "";
                layoutData.about = !string.IsNullOrEmpty(item.Fields[Templates.WhyInvestItem.Fields.About].Value.ToString()) ? Convert.ToString(item.Fields[Templates.WhyInvestItem.Fields.About].Value.Trim()) : "";
                layoutData.readMore = !string.IsNullOrEmpty(item.Fields[Templates.WhyInvestItem.Fields.ReadMore].Value.ToString()) ? Convert.ToString(item.Fields[Templates.WhyInvestItem.Fields.ReadMore].Value.Trim()) : "";

                layoutData.benefits = new List<Benefit>();

                Sitecore.Data.Fields.MultilistField points = item.Fields[Templates.WhyInvestItem.Fields.Benefits];
                foreach (Item highlightsData in points.GetItems())
                {
                    Models.Benefit point = new Models.Benefit();
                    point.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.WhyInvestBenefitItem.Fields.Title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.WhyInvestBenefitItem.Fields.Title].Value.Trim()) : "";

                    //Changed started
                    string str = !string.IsNullOrEmpty(highlightsData.Fields[Templates.WhyInvestBenefitItem.Fields.Icon].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.WhyInvestBenefitItem.Fields.Icon].Value.Trim()) : "";
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    point.icon = str;
                    //Changed End 
                    point.desc = !string.IsNullOrEmpty(highlightsData.Fields[Templates.WhyInvestBenefitItem.Fields.Desc].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.WhyInvestBenefitItem.Fields.Desc].Value.Trim()) : "";

                    layoutData.benefits.Add(point);
                }


                whyInvestModel.whyInvest = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }


            return whyInvestModel;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End 
         *         */
        public ArticleModel GetArticleModel(Rendering rendering)
        {
            ArticleModel articleModel = new ArticleModel();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
          ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
          : null;

                Articles layoutData = new Articles();

                layoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.ArticleItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.ArticleItem.Fields.Heading].Value.Trim()) : "";

                layoutData.data = new List<ArticleData>();

                Sitecore.Data.Fields.MultilistField points = item.Fields[Templates.ArticleItem.Fields.Data];
                foreach (Item highlightsData in points.GetItems())
                {
                    Models.ArticleData point = new Models.ArticleData();
                    point.title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ArticleDataItem.Fields.Title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ArticleDataItem.Fields.Title].Value.Trim()) : "";
                    point.date = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ArticleDataItem.Fields.Date].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ArticleDataItem.Fields.Date].Value.Trim()) : "";

                    //Changed started
                    string str = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ArticleDataItem.Fields.Src]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    point.src = str;
                    //Changed End 

                    layoutData.data.Add(point);
                }


                articleModel.articles = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }


            return articleModel;
        }

        public OurLocationModel GetOurLocationModel(Rendering rendering)
        {
            OurLocationModel ourLocationModel = new OurLocationModel();
            try
            {
                var item = !string.IsNullOrEmpty(rendering.DataSource)
           ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
           : null;

                OurLocations layoutData = new OurLocations();

                layoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.OurLocationItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.OurLocationItem.Fields.Heading].Value.Trim()) : "";

                layoutData.data = new List<OurLocationData>();

                Sitecore.Data.Fields.MultilistField points = item.Fields[Templates.OurLocationItem.Fields.Data];
                foreach (Item highlightsData in points.GetItems())
                {
                    Models.OurLocationData point = new Models.OurLocationData();
                    point.cityName = !string.IsNullOrEmpty(highlightsData.Fields[Templates.OurLocationDataItem.Fields.CityName].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.OurLocationDataItem.Fields.CityName].Value.Trim()) : "";
                    point.about = !string.IsNullOrEmpty(highlightsData.Fields[Templates.OurLocationDataItem.Fields.About].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.OurLocationDataItem.Fields.About].Value.Trim()) : "";
                    point.readMore = !string.IsNullOrEmpty(highlightsData.Fields[Templates.OurLocationDataItem.Fields.ReadMore].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.OurLocationDataItem.Fields.ReadMore].Value.Trim()) : "";

                    point.features = new List<string>();
                    Sitecore.Data.Fields.MultilistField features = highlightsData.Fields[Templates.OurLocationDataItem.Fields.Feature];
                    foreach (Item feature in features.GetItems())
                    {
                        point.features.Add(!string.IsNullOrEmpty(feature.Fields[Templates.OurLocationFeatureItem.Fields.Title].Value.ToString()) ? Convert.ToString(feature.Fields[Templates.OurLocationFeatureItem.Fields.Title].Value.Trim()) : "");
                    }

                    layoutData.data.Add(point);
                }


                ourLocationModel.ourLocations = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }


            return ourLocationModel;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End 
         *         */
        public OurProjectModel GetOurProjectModel(Rendering rendering)
        {
            OurProjectModel ourProjectModel = new OurProjectModel();
            try
            {

                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                //Changed started 
                var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                //Changed End 

                OurProjects layoutData = new OurProjects();

                layoutData.heading = !string.IsNullOrEmpty(item.Fields[Templates.OurProjectItem.Fields.Heading].Value.ToString()) ? Convert.ToString(item.Fields[Templates.OurProjectItem.Fields.Heading].Value.Trim()) : "";

                layoutData.data = new List<OurProjectData>();

                Sitecore.Data.Fields.MultilistField points = item.Fields[Templates.OurProjectItem.Fields.Data];
                foreach (Item highlightsData in points.GetItems())
                {
                    Models.OurProjectData point = new Models.OurProjectData();
                    point.imgalt = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ProjectDataItem.Fields.imgalt].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ProjectDataItem.Fields.imgalt].Value.Trim()) : "";
                    point.imgtitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ProjectDataItem.Fields.imgtitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ProjectDataItem.Fields.imgtitle].Value.Trim()) : "";
                    point.projectcity = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ProjectDataItem.Fields.projectcity].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ProjectDataItem.Fields.projectcity].Value.Trim()) : "";
                    point.projecttitle = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ProjectDataItem.Fields.projectTitle].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ProjectDataItem.Fields.projectTitle].Value.Trim()) : "";

                    //Changed started 
                    string str = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ProjectDataItem.Fields.src]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    point.src = str;

                    str = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ProjectDataItem.Fields.Link]);
                    //if (str.Contains(strSitedomain))
                    //{
                    //    str = str.Replace(strSitedomain, "");
                    //}
                    point.link = str;
                    //Changed End 

                    point.seeAll = new SeeAll()
                    {
                        link = Helper.GetLinkURLbyField(highlightsData, highlightsData.Fields[Templates.ProjectDataItem.Fields.SeeAllLink]),
                        title = !string.IsNullOrEmpty(highlightsData.Fields[Templates.ProjectDataItem.Fields.title].Value.ToString()) ? Convert.ToString(highlightsData.Fields[Templates.ProjectDataItem.Fields.title].Value.Trim()) : "",
                    };

                    point.projectlist = new List<Projectlist>();
                    Sitecore.Data.Fields.MultilistField projectlist = highlightsData.Fields[Templates.ProjectDataItem.Fields.projectlist];
                    foreach (Item feature in projectlist.GetItems())
                    {
                        Models.Projectlist project = new Models.Projectlist();

                        str = Helper.GetLinkURLbyField(feature, feature.Fields[Templates.ProjectItem.Fields.Link]);
                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        project.link = str;

                        project.projecttitle = !string.IsNullOrEmpty(feature.Fields[Templates.ProjectItem.Fields.Projecttitle].Value.ToString()) ? Convert.ToString(feature.Fields[Templates.ProjectItem.Fields.Projecttitle].Value.Trim()) : "";
                        project.projectprice = !string.IsNullOrEmpty(feature.Fields[Templates.ProjectItem.Fields.Projectprice].Value.ToString()) ? Convert.ToString(feature.Fields[Templates.ProjectItem.Fields.Projectprice].Value.Trim()) : "";
                        project.propertyType = !string.IsNullOrEmpty(feature.Fields[Templates.ProjectItem.Fields.propertyType].Value.ToString()) ? Convert.ToString(feature.Fields[Templates.ProjectItem.Fields.propertyType].Value.Trim()) : "";

                        point.projectlist.Add(project);
                    }


                    point.propertyType = new List<string>();
                    Sitecore.Data.Fields.MultilistField features = highlightsData.Fields[Templates.ProjectDataItem.Fields.propertyType];
                    foreach (Item feature in features.GetItems())
                    {
                        point.propertyType.Add(!string.IsNullOrEmpty(feature.Fields[Templates.PropertTypeItem.Fields.Title].Value.ToString()) ? Convert.ToString(feature.Fields[Templates.PropertTypeItem.Fields.Title].Value.Trim()) : "");
                    }

                    layoutData.data.Add(point);
                }


                ourProjectModel.ourProjects = layoutData;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return ourProjectModel;
        }

        public MapLocationModel GetMapLocationData(Rendering rendering)
        {
            MapLocationModel mapLocationModel = new MapLocationModel();
            try
            {
                Item renderingItem = rendering?.Item;
                if (renderingItem != null)
                {
                    mapLocationModel.Latitude = renderingItem?.Fields[MapLocationTemplate.Fields.Latitude]?.Value;
                    mapLocationModel.Longitude = renderingItem?.Fields[MapLocationTemplate.Fields.Longitude]?.Value;
                    mapLocationModel.MapUrl = Helper.GetLinkURLbyField(renderingItem, renderingItem?.Fields[MapLocationTemplate.Fields.MapUrl]);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return mapLocationModel;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End 
         *         */
        public List<CityTabs> GetCityData(Rendering rendering)
        {
            List<CityTabs> listOfCityTabs = new List<CityTabs>();

            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            // Changed End 
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                foreach (Item item in datasource.Children)
                {
                    if (item.TemplateID == CityTabstemp.TemplateID)
                    {
                        CityTabs cityTabs = new CityTabs();
                        cityTabs.title = !string.IsNullOrEmpty(item.Fields[CityTabstemp.Fields.title].Value.ToString()) ? item.Fields[CityTabstemp.Fields.title].Value.ToString() : "";

                        //Changed started 
                        string str = Helper.GetLinkURL(item, CityTabstemp.Fields.link.ToString()) != null ?
                                    Helper.GetLinkURL(item, CityTabstemp.Fields.link.ToString()) : "";
                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        cityTabs.link = str;
                        //Changed End 

                        listOfCityTabs.Add(cityTabs);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetCityData gives -> " + ex.Message);
            }
            return listOfCityTabs;
        }
        public List<CertificatesModel> GetCertificatesDetails(Rendering rendering)
        {
            List<CertificatesModel> listOfCertificates = new List<CertificatesModel>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                foreach (Item item in datasource.Children)
                {
                    if (item.TemplateID == CityTabstemp.TemplateID)
                    {
                        CertificatesModel certificatesModel = new CertificatesModel();
                        certificatesModel.certificateID = !string.IsNullOrEmpty(item.Fields[CityTabstemp.Fields.certificateID].Value.ToString()) ? item.Fields[CityTabstemp.Fields.certificateID].Value.ToString() : "";
                        certificatesModel.data = GetPropertyTypeData(item);
                        listOfCertificates.Add(certificatesModel);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetCertificatesDetails gives -> " + ex.Message);
            }
            return listOfCertificates;
        }
        public List<PropertyType> GetPropertyTypeData(Item item)
        {
            List<PropertyType> listOftypes = new List<PropertyType>();
            try
            {
                var childList = item.Children.ToList();
                foreach (var childItem in childList)
                {
                    if (childItem.TemplateID == PropertyTypeTemp.TemplateID)
                    {
                        PropertyType propertyType = new PropertyType();
                        propertyType.heading = !string.IsNullOrEmpty(childItem.Fields[PropertyTypeTemp.Fields.heading].Value.ToString()) ? childItem.Fields[PropertyTypeTemp.Fields.heading].Value.ToString() : "";
                        var multilist = childItem.GetMultiListValueItem(PropertyTypeTemp.Fields.Properties);
                        if (multilist.Count() != 0 && multilist != null)
                        {
                            propertyType.data = GetPropertyData(multilist);
                        }
                        listOftypes.Add(propertyType);
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetPropertyData gives -> " + ex.Message);
            }
            return listOftypes;
        }
        /* 09/06/2022
         * Changed started 
         * Changed End 
         *         */
        public List<Details> GetPropertyData(IEnumerable<Item> multilist)
        {
            List<Details> listOfDetails = new List<Details>();
            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                foreach (var property in multilist)
                {
                    if (property.TemplateID == ResidentialPropertyData.ResidentialTemplateID || property.TemplateID == ResidentialPropertyData.CommercialTemplateID)
                    {
                        Details details = new Details();
                        //Changed started
                        string str = Helper.GetImageSource(property, Templates.Property.Fields.PropertyLogoImg.ToString()) != null ?
                                  Helper.GetImageSource(property, Templates.Property.Fields.PropertyLogoImg.ToString()) : "";
                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        details.src = str;
                        //Changed End 

                        details.alt = Helper.GetImageDetails(property, Templates.Property.Fields.PropertyLogoImg.ToString()) != null ?
                                        Helper.GetImageDetails(property, Templates.Property.Fields.PropertyLogoImg.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";

                        //Changed started
                        str = Helper.GetLinkURL(property, ResidentialPropertyData.Fields.PropertyLinkID.ToString()) != "" ? Helper.GetLinkURL(property, ResidentialPropertyData.Fields.PropertyLinkID.ToString()) : "";

                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        details.link = str;
                        //Changed End 

                        details.target = Helper.GetLinkURLTargetSpace(property, ResidentialPropertyData.Fields.PropertyLinkID.ToString()) != "" ? Helper.GetLinkURLTargetSpace(property, ResidentialPropertyData.Fields.PropertyLinkID.ToString()) : "";
                        details.propertyName = !string.IsNullOrEmpty(property.Fields[ResidentialPropertyData.Fields.Title].Value.ToString()) ? property.Fields[ResidentialPropertyData.Fields.Title].Value.ToString() : "";
                        details.reraHeading = !string.IsNullOrEmpty(commonItem.Fields[ReraDataTemplate.Fields.reraHeading].Value.ToString()) ? commonItem.Fields[ReraDataTemplate.Fields.reraHeading].Value.ToString() : "";
                        details.downloadRera = !string.IsNullOrEmpty(commonItem.Fields[ReraDataTemplate.Fields.downloadRera].Value.ToString()) ? commonItem.Fields[ReraDataTemplate.Fields.downloadRera].Value.ToString() : "";
                        details.rera = GetReraCerificateDetail(property);
                        details.envHeading = !string.IsNullOrEmpty(commonItem.Fields[ReraDataTemplate.Fields.envHeading].Value.ToString()) ? commonItem.Fields[ReraDataTemplate.Fields.envHeading].Value.ToString() : "";
                        details.downloadEnv = !string.IsNullOrEmpty(commonItem.Fields[ReraDataTemplate.Fields.downloadEnv].Value.ToString()) ? commonItem.Fields[ReraDataTemplate.Fields.downloadEnv].Value.ToString() : "";
                        details.envModal = GetenvCerificateDetail(property);
                        listOfDetails.Add(details);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetPropertyData gives -> " + ex.Message);
            }
            return listOfDetails;
        }
        public List<Rera> GetReraCerificateDetail(Item property)
        {
            List<Rera> listOfRera = new List<Rera>();
            try
            {
                var multiListOfRera = property.GetMultiListValueItem(CityTabstemp.Fields.ReracertificatesID);
                if (multiListOfRera != null && multiListOfRera.Count() != 0)
                {
                    foreach (var reraItem in multiListOfRera)
                    {
                        if (reraItem.TemplateID == GalleryModalDataItemTemplate.TemplateID)
                        {
                            Rera rera = new Rera();
                            rera.reraTitle = !string.IsNullOrEmpty(reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.ReraTitle].Value.ToString()) ? Convert.ToString(reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.ReraTitle].Value.Trim()) : "";
                            rera.title = !string.IsNullOrEmpty(reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.url].Value.ToString()) ? Convert.ToString(reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.url].Value.Trim()) : "";
                            rera.titleLink = Helper.GetLinkURLbyField(reraItem, reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.ReraSiteLink]);
                            rera.titleLinkTarget = Helper.GetLinkURLTargetSpace(reraItem, Templates.GalleryModalDataItemTemplate.Fields.ReraSiteLink.ToString());
                            rera.reraNumber = !string.IsNullOrEmpty(reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.rearid].Value.ToString()) ? Convert.ToString(reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.rearid].Value.Trim()) : "";
                            rera.download = !string.IsNullOrEmpty(reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.download].Value.ToString()) ? Convert.ToString(reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.download].Value.Trim()) : "";
                            rera.downloadurl = Helper.GetPropLinkURLbyField(reraItem, reraItem.Fields[Templates.GalleryModalDataItemTemplate.Fields.downloadLink]);
                            rera.qrCodeImage = Helper.GetImageSource(reraItem, GalleryModalDataItemTemplate.Fields.qrCodeImage.ToString()) != null ?
                                  Helper.GetImageSource(reraItem, GalleryModalDataItemTemplate.Fields.qrCodeImage.ToString()) : "";
                            listOfRera.Add(rera);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetReraCerificateDetail gives -> " + ex.Message);
            }
            return listOfRera;
        }
        public List<envModal> GetenvCerificateDetail(Item property)
        {
            List<envModal> listOfenv = new List<envModal>();
            try
            {
                var multiListOfRera = property.GetMultiListValueItem(CityTabstemp.Fields.EnvCertificateID);
                if (multiListOfRera != null && multiListOfRera.Count() != 0)
                {
                    foreach (var reraItem in multiListOfRera)
                    {
                        if (reraItem.TemplateID == GalleryModalDataItemTemplate.EnvTemplateID)
                        {
                            envModal env = new envModal();
                            env.url = Helper.GetPropLinkURLbyField(reraItem, reraItem.Fields[Templates.GalleryModalDataItemTemplate.envFields.envurl]);
                            env.envMonth = !string.IsNullOrEmpty(reraItem.Fields[Templates.GalleryModalDataItemTemplate.envFields.envMonth].Value.ToString()) ? Convert.ToString(reraItem.Fields[Templates.GalleryModalDataItemTemplate.envFields.envMonth].Value.Trim()) : "";
                            env.download = !string.IsNullOrEmpty(reraItem.Fields[Templates.GalleryModalDataItemTemplate.envFields.envdownload].Value.ToString()) ? Convert.ToString(reraItem.Fields[Templates.GalleryModalDataItemTemplate.envFields.envdownload].Value.Trim()) : "";
                            env.downloadurl = Helper.GetPropLinkURLbyField(reraItem, reraItem.Fields[Templates.GalleryModalDataItemTemplate.envFields.envdownloadurl]);
                            env.envMonthTarget = Helper.GetLinkURLTargetSpace(reraItem, Templates.GalleryModalDataItemTemplate.envFields.envdownloadurl.ToString()) != null ? Helper.GetLinkURLTargetSpace(reraItem, Templates.GalleryModalDataItemTemplate.envFields.envdownloadurl.ToString()) : "";
                            listOfenv.Add(env);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetenvCerificateDetail gives -> " + ex.Message);
            }
            return listOfenv;
        }
        public List<CompletedProjectsModel> GetCompletedProjectList(Rendering rendering)
        {
            List<CompletedProjectsModel> listOfCompletedProjects = new List<CompletedProjectsModel>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                foreach (Item item in datasource.Children)
                {
                    if (item.TemplateID == CityTabstemp.TemplateID)
                    {
                        CompletedProjectsModel certificatesModel = new CompletedProjectsModel();
                        certificatesModel.projectID = !string.IsNullOrEmpty(item.Fields[CityTabstemp.Fields.certificateID].Value.ToString()) ? item.Fields[CityTabstemp.Fields.certificateID].Value.ToString() : "";
                        certificatesModel.data = GetCompletedPropertyTypeData(item);
                        listOfCompletedProjects.Add(certificatesModel);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetCertificatesDetails gives -> " + ex.Message);
            }
            return listOfCompletedProjects;
        }
        public List<CompletedPropertyType> GetCompletedPropertyTypeData(Item item)
        {
            List<CompletedPropertyType> listOftypes = new List<CompletedPropertyType>();
            try
            {
                var childList = item.Children.ToList();
                foreach (var childItem in childList)
                {
                    if (childItem.TemplateID == PropertyTypeTemp.TemplateID)
                    {
                        CompletedPropertyType propertyType = new CompletedPropertyType();
                        propertyType.heading = !string.IsNullOrEmpty(childItem.Fields[PropertyTypeTemp.Fields.heading].Value.ToString()) ? childItem.Fields[PropertyTypeTemp.Fields.heading].Value.ToString() : "";
                        var multilist = childItem.GetMultiListValueItem(PropertyTypeTemp.Fields.Properties);
                        if (multilist.Count() != 0 && multilist != null)
                        {
                            propertyType.data = GetCompletedPropertyData(multilist);
                        }
                        listOftypes.Add(propertyType);
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetPropertyData gives -> " + ex.Message);
            }
            return listOftypes;
        }
        public List<CompletedProjectDetails> GetCompletedPropertyData(IEnumerable<Item> multilist)
        {
            List<CompletedProjectDetails> listOfDetails = new List<CompletedProjectDetails>();
            //Changed started 
            var commonItem = Sitecore.Context.Database.GetItem(Commondata.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            //Changed End 
            try
            {
                foreach (var property in multilist)
                {
                    if (property.TemplateID == ResidentialPropertyData.ResidentialTemplateID || property.TemplateID == ResidentialPropertyData.CommercialTemplateID)
                    {
                        CompletedProjectDetails details = new CompletedProjectDetails();
                        //Changed started
                        string str = Helper.GetImageSource(property, Templates.Property.Fields.PropertyLogoImg.ToString()) != null ?
                                  Helper.GetImageSource(property, Templates.Property.Fields.PropertyLogoImg.ToString()) : "";
                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        details.imageSource = str;
                        //Changed End 

                        details.imageAlt = Helper.GetImageDetails(property, Templates.Property.Fields.PropertyLogoImg.ToString()) != null ?
                                        Helper.GetImageDetails(property, Templates.Property.Fields.PropertyLogoImg.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";

                        //Changed started
                        str = Helper.GetLinkURL(property, ResidentialPropertyData.Fields.PropertyLinkID.ToString()) != "" ? Helper.GetLinkURL(property, ResidentialPropertyData.Fields.PropertyLinkID.ToString()) : "";

                        //if (str.Contains(strSitedomain))
                        //{
                        //    str = str.Replace(strSitedomain, "");
                        //}
                        details.link = str;
                        //Changed End 

                        details.target = Helper.GetLinkURLTargetSpace(property, ResidentialPropertyData.Fields.PropertyLinkID.ToString()) != "" ? Helper.GetLinkURLTargetSpace(property, ResidentialPropertyData.Fields.PropertyLinkID.ToString()) : "";
                        details.projectName = !string.IsNullOrEmpty(property.Fields[ResidentialPropertyData.Fields.Title].Value.ToString()) ? property.Fields[ResidentialPropertyData.Fields.Title].Value.ToString() : "";
                        details.projectArea = !string.IsNullOrEmpty(property.Fields[ResidentialPropertyData.Fields.projectArea].Value.ToString()) ? property.Fields[ResidentialPropertyData.Fields.projectArea].Value.ToString() : "";
                        details.areaTitle = commonItem != null ? commonItem.Fields[ResidentialPropertyData.Fields.areaTitle].Value.ToString() : string.Empty;
                        details.areaDesc = !string.IsNullOrEmpty(property.Fields[ResidentialPropertyData.Fields.areaDesc].Value.ToString()) ? property.Fields[ResidentialPropertyData.Fields.areaDesc].Value.ToString() : "";
                        listOfDetails.Add(details);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver GetPropertyData gives -> " + ex.Message);
            }
            return listOfDetails;
        }
        public AanganDrawResultModel GetDrawResultModel(Rendering rendering)
        {
            AanganDrawResultModel drawResultModel = new AanganDrawResultModel();
            try
            {
                Item renderingItem = rendering?.Item;
                if (renderingItem != null)
                {
                    drawResultModel.heading = renderingItem?.Fields[AanganResultTemp.Fields.heading]?.Value;
                    drawResultModel.Id = renderingItem?.Fields[AanganResultTemp.Fields.id]?.Value;
                    drawResultModel.imageSource = Helper.GetImageURLbyField(renderingItem.Fields[Templates.AanganResultTemp.Fields.imageSource]);
                    drawResultModel.imageSourceMobile = Helper.GetImageURLbyField(renderingItem.Fields[Templates.AanganResultTemp.Fields.imageSourceMobile]);
                    drawResultModel.ImageSourceTablet = Helper.GetImageURLbyField(renderingItem.Fields[Templates.AanganResultTemp.Fields.imageSourceTablet]);
                    drawResultModel.ImgAlt = Helper.GetImageDetails(renderingItem, Templates.AanganResultTemp.Fields.imageSource.ToString()).Fields[Templates.AanganResultTemp.Fields.imgAlt].Value;
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return drawResultModel;
        }

        public NRIRatingSchemaModel GetNRIRatingSchemaModel(Rendering rendering)
        {
            NRIRatingSchemaModel nRIRatingSchemaModel = new NRIRatingSchemaModel();

            try
            {

                var item = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;

                NRIRatingSchemaModel layoutData = new NRIRatingSchemaModel();

                layoutData.ratingSchema = !string.IsNullOrEmpty(item.Fields[Templates.NRIRatingSchemaTemp.Fields.ratingSchema].Value.ToString()) ?
                Convert.ToString(item.Fields[Templates.NRIRatingSchemaTemp.Fields.ratingSchema].Value.Trim()) : "";

                nRIRatingSchemaModel.ratingSchema = layoutData.ratingSchema;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return nRIRatingSchemaModel;
        }
    }

}

