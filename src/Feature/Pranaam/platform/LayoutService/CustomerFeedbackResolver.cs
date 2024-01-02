using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class CustomerFeedbackResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICustomerFeedback _feedback;
        public CustomerFeedbackResolver(ICustomerFeedback feedback)
        {
            this._feedback = feedback;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _feedback.GetCustomerFeedback(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("CustomerFeedbackResolver throws Exception -> " + ex.Message);
            }
        }
    }
}