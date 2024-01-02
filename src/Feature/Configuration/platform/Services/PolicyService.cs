using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
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
    public class PolicyService : IPolicyService
    {
        private readonly ILogRepository _logRepository;
        public PolicyService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }

        public PolicyModel GetPolicyData(Rendering rendering)
        {
            PolicyModel policyModel = new PolicyModel();
            try
            {
                Item item = rendering?.Item;
                policyModel.Disclaimber = GetDisclaimerData(item);
                policyModel.CookiePolicy = GetCookiePolicyData(item);

            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return policyModel;
        }

        private Cookie GetCookiePolicyData(Item item)
        {
            Cookie policy = new Cookie();
            try
            {
                policy.Heading = item?.Fields[PolicyTemplate.CookieFields.Heading]?.Value;
                policy.Content = item?.Fields[PolicyTemplate.CookieFields.Content]?.Value;
                policy.ButtonUrl = Helper.GetLinkURLbyField(item, item?.Fields[PolicyTemplate.CookieFields.AcceptBtn]);
                policy.ButtonText = Helper.GetLinkDescriptionByField(item?.Fields[PolicyTemplate.CookieFields.AcceptBtn]);
                policy.CancelButtonText = item?.Fields[PolicyTemplate.CookieFields.CancelBtn]?.Value;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return policy;
        }

        private Disclaimer GetDisclaimerData(Item item)
        {

            Disclaimer policy = new Disclaimer();
            try
            {
                policy.DisclaimerLogo = Helper.GetImageURLbyField(item?.Fields[PolicyTemplate.DisclaimerFields.DisclaimerLogo]);
                policy.LogoAlt = item?.Fields[PolicyTemplate.DisclaimerFields.LogoAlt]?.Value;
                policy.Heading = item?.Fields[PolicyTemplate.DisclaimerFields.Heading]?.Value;
                policy.Content = item?.Fields[PolicyTemplate.DisclaimerFields.Content]?.Value;
                policy.ButtonUrl = Helper.GetLinkURLbyField(item, item?.Fields[PolicyTemplate.DisclaimerFields.AcceptBtn]);
                policy.ButtonText = Helper.GetLinkDescriptionByField(item?.Fields[PolicyTemplate.DisclaimerFields.AcceptBtn]);
                policy.CancelButtonText = item?.Fields[PolicyTemplate.DisclaimerFields.CancelBtn]?.Value;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return policy;
        }
    }
}