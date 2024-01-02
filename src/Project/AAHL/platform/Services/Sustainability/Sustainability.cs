using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.LayoutServiceContentResolvers;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.Our_Expertise;
using Project.AAHL.Website.Models.OurAirports;
using Project.AAHL.Website.Models.OurBelief;
using Project.AAHL.Website.Models.OurExpertise;
using Project.AAHL.Website.Models.OurLeadership;
using Project.AAHL.Website.Models.Sustainability;
using Project.AAHL.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Project.AAHL.Website.Services.Common
{
    public class Sustainability : ISustainability
    {

        private readonly IMvcContext _mvcContext;

        public Sustainability(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }



        public SustainabilityStoriesModel GetSustainabilityStories(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<SustainabilityStoriesModel>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public SustainabilityModel GetSustainability(Rendering rendering)
        {

            SustainabilityModel sustainabilityData = new SustainabilityModel();
            List<SustainabilityItem> SustainabilityDetails = new List<SustainabilityItem>();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {
                var Sustainabilitys = BaseTemplate.Sustainability.Sustainabilitys;
                var BannerDetails = BaseTemplate.Sustainability.BannerDetails;
                var Common = BaseTemplate.Sustainability.Common;
                var DecarbonisationCard = BaseTemplate.Sustainability.DecarbonisationCard;
                var SustainabilityStories = BaseTemplate.Sustainability.SustainabilityStories;

               

                foreach (Item Item in datasource.Children)
                {
                    SustainabilityItem sustainabilityItem = new SustainabilityItem();
                    foreach (Item SustainabilityItem in Item.Children)
                    {
                        if (SustainabilityItem.TemplateID == Sustainabilitys)
                        {
                            sustainabilityItem.Heading = SustainabilityItem.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                            sustainabilityItem.Isactive = Utils.GetBoleanValue(SustainabilityItem, BaseTemplate.IsactiveTemplate.IsactiveFieldId);
                        }
                      
                        if (SustainabilityItem.TemplateID == BannerDetails)
                        {
                            SustainabilityItemData sustainabilityItemData = new SustainabilityItemData();
                            sustainabilityItemData.Heading = SustainabilityItem.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                            sustainabilityItemData.Description = SustainabilityItem.Fields[BaseTemplate.DescriptionTemplate.DescriptionFieldId].Value;
                            sustainabilityItemData.ImagePath = Utils.GetImageURLByFieldId(SustainabilityItem, BaseTemplate.ImageSourceTemplate.ImageSourceFieldId);
                            sustainabilityItemData.MImagePath = Utils.GetImageURLByFieldId(SustainabilityItem, BaseTemplate.ImageSourceTemplate.ImageSourceMobileFieldId);
                            sustainabilityItemData.TImagePath = Utils.GetImageURLByFieldId(SustainabilityItem, BaseTemplate.ImageSourceTemplate.ImageSourceTabletFieldId);
                            sustainabilityItemData.Imgalttext = SustainabilityItem.Fields[BaseTemplate.ImageSourceTemplate.ImageAltFieldId].Value;
                            sustainabilityItem.SustainabilityData = sustainabilityItemData;
                        }
                       
                        if (SustainabilityItem.TemplateID == SustainabilityStories)
                        {
                            StoriesReports storiesReports = new StoriesReports();
                            storiesReports.Heading = SustainabilityItem.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                            List<StoriesReportsitem> storiesReportsitems = new List<StoriesReportsitem>();
                            foreach (Item childitem in SustainabilityItem.Children)
                            {
                                StoriesReportsitem storiesReportsitem = new StoriesReportsitem();
                                storiesReportsitem.Heading = childitem.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                                storiesReportsitem.Description = childitem.Fields[BaseTemplate.DescriptionTemplate.DescriptionFieldId].Value;
                                storiesReportsitem.LinkUrl = Utils.GetLinkURL(childitem.Fields[BaseTemplate.LinkTemplate.LinkUrlFieldId]);
                                storiesReportsitem.LinkText = childitem.Fields[BaseTemplate.LinkTemplate.LinkTextFieldId].Value;
                                storiesReportsitem.ImagePath = Utils.GetImageURLByFieldId(childitem, BaseTemplate.ImageSourceTemplate.ImageSourceFieldId);
                                storiesReportsitem.MImagePath = Utils.GetImageURLByFieldId(childitem, BaseTemplate.ImageSourceTemplate.ImageSourceMobileFieldId);
                                storiesReportsitem.TImagePath = Utils.GetImageURLByFieldId(childitem, BaseTemplate.ImageSourceTemplate.ImageSourceTabletFieldId);
                                storiesReportsitem.Imgalttext = childitem.Fields[BaseTemplate.ImageSourceTemplate.ImageAltFieldId].Value;
                                storiesReportsitems.Add(storiesReportsitem);
                            }
                            storiesReports.StoriesReportsitem = storiesReportsitems;
                            sustainabilityItem.StoriesReports = storiesReports;
                        }
                       
                        if (SustainabilityItem.TemplateID == Common)
                        {
                            CommonData commonData = new CommonData();
                            commonData.Heading = SustainabilityItem.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                            commonData.Description = SustainabilityItem.Fields[BaseTemplate.DescriptionTemplate.DescriptionFieldId].Value;
                            commonData.SubDescription = SustainabilityItem.Fields[BaseTemplate.DescriptionTemplate.SubDescriptionFieldId].Value;
                            sustainabilityItem.DetailsData = commonData;
                        }
                       
                        if (SustainabilityItem.TemplateID == DecarbonisationCard)
                        {
                            List<Listitem> listitems = new List<Listitem>();
                            foreach (Item SustainabilityItems in SustainabilityItem.Children)
                            {
                                Listitem listitem = new Listitem();
                                listitem.Heading = SustainabilityItems.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                                listitem.Description = SustainabilityItems.Fields[BaseTemplate.DescriptionTemplate.DescriptionFieldId].Value;
                                listitem.ImagePath = Utils.GetImageURLByFieldId(SustainabilityItems, BaseTemplate.ImageSourceTemplate.ImageSourceFieldId);
                                listitem.MImagePath = Utils.GetImageURLByFieldId(SustainabilityItems, BaseTemplate.ImageSourceTemplate.ImageSourceMobileFieldId);
                                listitem.TImagePath = Utils.GetImageURLByFieldId(SustainabilityItems, BaseTemplate.ImageSourceTemplate.ImageSourceTabletFieldId);
                                listitem.Imgalttext = SustainabilityItems.Fields[BaseTemplate.ImageSourceTemplate.ImageAltFieldId].Value;
                                listitems.Add(listitem);
                            }
                            sustainabilityItem.CardListitem = listitems;
                        }
                    }
                    SustainabilityDetails.Add(sustainabilityItem);
                }
                sustainabilityData.Items = SustainabilityDetails;
                return sustainabilityData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}