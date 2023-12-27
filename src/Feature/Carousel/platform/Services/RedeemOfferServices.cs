using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class RedeemOfferServices : IRedeemOffer
    {
        //Ticket NO  18854
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public RedeemOfferServices(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel RedeemOfferSteps(Rendering rendering)
        {
            WidgetModel RedeemStepList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                RedeemStepList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                RedeemStepList.widget.widgetItems = GetRedeemOfferSteps(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" RedeemOfferService gives -> " + ex.Message);
            }
            return RedeemStepList;
        }

        private List<object> GetRedeemOfferSteps(Rendering rendering)
        {
            List<Object> RedeemStepList = new List<Object>();
            try
            {
                var datasourceItem = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasourceItem != null && datasourceItem.GetChildren()!=null && datasourceItem.GetChildren().Count()>0)
                {
                    RedeemSteps redeemSteps = null;
                    foreach (Sitecore.Data.Items.Item step in datasourceItem.GetChildren())
                    {
                        redeemSteps = new RedeemSteps();
                        redeemSteps.ComponentTitle = !string.IsNullOrEmpty(step.Fields[Constant.HowToRedeemDS.ComponentTitle].Value) ?
                                                        step.Fields[Constant.HowToRedeemDS.ComponentTitle].Value :
                                                        string.Empty;
                        Sitecore.Data.Fields.MultilistField howtoredeemsteps = step.Fields[Constant.HowToRedeemDS.RedeemSteps];
                        redeemSteps.howtoredeemSteps = getReedeemStepsContent(howtoredeemsteps);
                        RedeemStepList.Add(redeemSteps);
                    }

                }
            }
            catch(Exception ex)
            {
                _logRepository.Error(" GetRedeemOfferSteps gives -> " + ex.Message);
            }
            return RedeemStepList;
        }

        private List<howtoredeemStep> getReedeemStepsContent(MultilistField howtoredeemsteps)
        {
            List<howtoredeemStep> howtoredeemslst = new List<howtoredeemStep>();
            howtoredeemStep howtoredeemStep = null;
            try
            {
                if(howtoredeemsteps!=null && howtoredeemsteps.GetItems()!=null && howtoredeemsteps.GetItems().Count()>0)
                {
                    foreach(var RedeemItem in howtoredeemsteps.GetItems())
                    {
                        howtoredeemStep = new howtoredeemStep();
                        howtoredeemStep.redeemTitle = !string.IsNullOrEmpty(RedeemItem.Fields[Constant.HowToRedeemSection.RedeemTitle].Value) ? RedeemItem.Fields[Constant.HowToRedeemSection.RedeemTitle].Value : string.Empty;
                        howtoredeemStep.redeemDescription = !string.IsNullOrEmpty(RedeemItem.Fields[Constant.HowToRedeemSection.RedeemDescription].Value) ? RedeemItem.Fields[Constant.HowToRedeemSection.RedeemDescription].Value : string.Empty;
                        howtoredeemStep.redeemDesktopImage = !string.IsNullOrEmpty(RedeemItem.Fields[Constant.HowToRedeemSection.RedeemDesktopImage].Value) ? _helper.GetImageURLByFieldId(RedeemItem, Constant.HowToRedeemSection.RedeemDesktopImage.ToString()) : string.Empty;
                        howtoredeemStep.redeemMobileImage = !string.IsNullOrEmpty(RedeemItem.Fields[Constant.HowToRedeemSection.RedeemMobileImage].Value) ? _helper.GetImageURLByFieldId(RedeemItem, Constant.HowToRedeemSection.RedeemMobileImage.ToString()) : string.Empty;
                        howtoredeemStep.redeemlinkText = !string.IsNullOrEmpty(RedeemItem.Fields[Constant.HowToRedeemSection.RedeemLink].Value) ? _helper.GetLinkText(RedeemItem, Constant.HowToRedeemSection.RedeemCTA) : string.Empty;
                        howtoredeemStep.redeemlinkUrl = !string.IsNullOrEmpty(RedeemItem.Fields[Constant.HowToRedeemSection.RedeemLink].Value) ? _helper.GetLinkURL(RedeemItem, Constant.HowToRedeemSection.RedeemCTA) : string.Empty;
                        howtoredeemStep.redeemDescriptionApp = !string.IsNullOrEmpty(RedeemItem.Fields[Constant.HowToRedeemSection.RedeemDescriptionApp].Value) ? RedeemItem.Fields[Constant.HowToRedeemSection.RedeemDescriptionApp].Value : string.Empty;

                        howtoredeemslst.Add(howtoredeemStep);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" getReedeemStepsContent gives -> " + ex.Message);
            }
            return howtoredeemslst;
        }
    }
}