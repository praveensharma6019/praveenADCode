using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Configuration.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public class WhyAdaniService : IWhyAdaniService
    {
        private readonly ILogRepository _logRepository;
        public WhyAdaniService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public WhyAdaniModel GetWhyAdaniData(Rendering rendering)
        {
            WhyAdaniModel whyAdaniModel = new WhyAdaniModel();
            try
            {
                Item renderingItem = rendering?.Item;
                if (renderingItem != null)
                {
                    whyAdaniModel.WhyUsHighlights = new WhyUsHighlights();
                    WhyUsHighlights whyUsHighlights = new WhyUsHighlights();
                    whyUsHighlights.Data = GetWhyUsHighLightsData(renderingItem?.Fields[WhyAdaniTemplate.WhyAdaniFields.WhyUsHighlights]);
                    whyAdaniModel.WhyUsHighlights = whyUsHighlights;
                    whyAdaniModel.WhyAdani = new WhyAdani();
                    WhyAdani whyAdani = new WhyAdani();
                    whyAdani.Data = GetWhyAdaniInnerData(renderingItem?.Fields[WhyAdaniTemplate.WhyAdaniFields.WhyAdani]);
                    whyAdaniModel.WhyAdani = whyAdani;
                    whyAdaniModel.AboutAdani = new AboutAdani();
                    AboutAdani aboutAdani = new AboutAdani();
                    aboutAdani.Heading = renderingItem?.Fields[WhyAdaniTemplate.WhyAdaniFields.Heading]?.Value;
                    aboutAdani.About = renderingItem?.Fields[WhyAdaniTemplate.WhyAdaniFields.About]?.Value;                   
                    aboutAdani.Readmore = renderingItem?.Fields[WhyAdaniTemplate.WhyAdaniFields.ReadMore]?.Value;
                    aboutAdani.BlockHeading = GetBlockHeadingData(renderingItem?.Fields[WhyAdaniTemplate.WhyAdaniFields.BlockHeading]);
                    whyAdaniModel.AboutAdani = aboutAdani;
                    whyAdaniModel.BottomQuote = new BottomQuote();
                    BottomQuote bottomQuote = new BottomQuote();
                    bottomQuote.Data = GetBottomQuoteData(renderingItem?.Fields[WhyAdaniTemplate.WhyAdaniFields.BottmQuote]);
                    whyAdaniModel.BottomQuote = bottomQuote;
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return whyAdaniModel;
        }

        private List<BottomQuoteData> GetBottomQuoteData(Field field)
        {
            List<BottomQuoteData> bottomQuoteList = new List<BottomQuoteData>();
            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    BottomQuoteData bottomQuote = new BottomQuoteData();
                    bottomQuote.SectionHeading = item?.Fields[WhyAdaniTemplate.BottomQuoteFields.SectionHeading]?.Value;
                    bottomQuote.IconPrimary = Helper.GetImageAltbyField(item?.Fields[WhyAdaniTemplate.BottomQuoteFields.IconPrimary]);
                    bottomQuote.IconPrimaryAlt = Helper.GetImageURLbyField(item?.Fields[WhyAdaniTemplate.BottomQuoteFields.IconPrimary]);
                    bottomQuote.IconSecondary = Helper.GetImageAltbyField(item?.Fields[WhyAdaniTemplate.BottomQuoteFields.IconSecondary]);
                    bottomQuote.IconSecondaryAlt = Helper.GetImageURLbyField(item?.Fields[WhyAdaniTemplate.BottomQuoteFields.IconSecondary]);
                    bottomQuoteList?.Add(bottomQuote);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return bottomQuoteList;
        }

        private List<BlockHeadingData> GetBlockHeadingData(Field field)
        {
            List<BlockHeadingData> blockHeadingList = new List<BlockHeadingData>();

            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    BlockHeadingData blockHeadingData = new BlockHeadingData();
                    blockHeadingData.Heading = item?.Fields[WhyAdaniTemplate.BlockHeadingFields.Heading]?.Value;
                    blockHeadingList?.Add(blockHeadingData);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return blockHeadingList;
        }

        private List<WhyAdaniData> GetWhyAdaniInnerData(Field field)
        {
            List<WhyAdaniData> whyAdaniDataList = new List<WhyAdaniData>();
            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    WhyAdaniData whyAdaniData = new WhyAdaniData();
                    whyAdaniData.AlignCol = item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.AlignCol]?.Value;
                    whyAdaniData.SectionHeading = item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.SectionHeading]?.Value;
                    whyAdaniData.IconPrimary = Helper.GetImageAltbyField(item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.IconPrimary]);
                    whyAdaniData.IconPrimaryAlt = Helper.GetImageURLbyField(item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.IconPrimary]);
                    whyAdaniData.IconSecondary = Helper.GetImageAltbyField(item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.IconSecondary]);
                    whyAdaniData.IconSecondaryAlt = Helper.GetImageURLbyField(item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.IconSecondary]);
                    whyAdaniData.Heading = item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.Heading]?.Value;
                    whyAdaniData.SubHeading = item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.SubHeading]?.Value;
                    whyAdaniData.Description = item?.Fields[WhyAdaniTemplate.WhyAdaniInnerFields.Description]?.Value;
                    whyAdaniDataList?.Add(whyAdaniData);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return whyAdaniDataList;
        }

        private List<WhyusHighlightsData> GetWhyUsHighLightsData(Field field)
        {
            List<WhyusHighlightsData> whyusHighLightsDataList = new List<WhyusHighlightsData>();
            try
            {
                MultilistField mulField = field;
                foreach (Item item in mulField.GetItems())
                {
                    WhyusHighlightsData whyusHighLightData = new WhyusHighlightsData();
                    whyusHighLightData.SectionHeading = item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.SectionHeading]?.Value;
                    whyusHighLightData.HeadingAsset = Helper.GetImageURLbyField(item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.HeadingAsset]);
                    whyusHighLightData.HeadingAssetAlt = Helper.GetImageAltbyField(item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.HeadingAsset]);
                    whyusHighLightData.Src = Helper.GetImageURLbyField(item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.Src]);
                    whyusHighLightData.ImageAlt = Helper.GetImageAltbyField(item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.Src]);
                    whyusHighLightData.ImageTitle = item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.ImageTitle]?.Value;
                    whyusHighLightData.Title = item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.Title]?.Value;
                    whyusHighLightData.SubHeading = item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.SubHeading]?.Value;
                    whyusHighLightData.Description = item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.Description]?.Value;
                    whyusHighLightData.DataAligh = item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.DataAlign]?.Value;
                    whyusHighLightData.imgType = item?.Fields[WhyAdaniTemplate.WhyUsHighlightsFields.imgType]?.Value;
                    whyusHighLightsDataList?.Add(whyusHighLightData);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return whyusHighLightsDataList;
        }



        public GoodnessBannerModel GetGoodnessBanner(Rendering rendering)
        {
           
                GoodnessBannerModel goodnessBanner= new GoodnessBannerModel();
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)? rendering.RenderingItem?.Database.GetItem(rendering.DataSource): null;
            if (datasource == null)
            {
                throw new NullReferenceException();
            }

            try
            {                
                goodnessBanner.srcMobile = Helper.GetImageURL(datasource, Templates.GoodnessBannerTemplate.Fields.srcMobile.ToString()) != null ? 
                                                           Helper.GetImageURL(datasource, Templates.GoodnessBannerTemplate.Fields.srcMobile.ToString()) : "";
                goodnessBanner.src = Helper.GetImageURL(datasource, GoodnessBannerTemplate.Fields.src.ToString()) != null ?
                                Helper.GetImageURL(datasource, GoodnessBannerTemplate.Fields.src.ToString()) : "";
                goodnessBanner.alt = Helper.GetImageDetails(datasource, GoodnessBannerTemplate.Fields.src.ToString()) != null ?
                                        Helper.GetImageDetails(datasource, GoodnessBannerTemplate.Fields.src.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                goodnessBanner.heading = !string.IsNullOrEmpty(datasource.Fields[GoodnessBannerTemplate.Fields.heading].Value.ToString()) ? datasource.Fields[GoodnessBannerTemplate.Fields.heading].Value.ToString() : "";
                goodnessBanner.description = !string.IsNullOrEmpty(datasource.Fields[GoodnessBannerTemplate.Fields.description].Value.ToString()) ? datasource.Fields[GoodnessBannerTemplate.Fields.description].Value.ToString() : "";
                goodnessBanner.link = Helper.GetLinkURL(datasource, GoodnessBannerTemplate.Fields.link.ToString()) != null ?
                            Helper.GetLinkURL(datasource, GoodnessBannerTemplate.Fields.link.ToString()) : "";
                goodnessBanner.target = Helper.GetLinkURLTargetSpace(datasource, GoodnessBannerTemplate.Fields.link.ToString());                
                goodnessBanner.linkTitle = Helper.GetLinkDescriptionByField(datasource?.Fields[GoodnessBannerTemplate.Fields.link]);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return goodnessBanner;
        }
    }
}