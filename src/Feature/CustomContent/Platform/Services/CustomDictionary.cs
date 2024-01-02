using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public class CustomDictionary : ICustomDictionary
    {
        private readonly ILogRepository _logRepository;

        public CustomDictionary(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public object GetDictionary(Rendering rendering)
        {
            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? RenderingContext.Current.Rendering.Item
               : null;
            // Null Check for datasource
            if (datasource == null && datasource.GetChildren().Count == 0)
            {
                _logRepository.Info("Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services.GetDictionary => Rendering Datasource is Empty");
            }
            Dictionary<string, string> customDictionary =
           new Dictionary<string, string>();
            try
            {
                foreach (Sitecore.Data.Items.Item Item in datasource.GetChildren())
                {
                    customDictionary.Add(!string.IsNullOrEmpty(Item.Fields[Constants.Title].Value)? Item.Fields[Constants.Title].Value:string.Empty
                                        , !string.IsNullOrEmpty(Item.Fields[Constants.DictionaryValue].Value) ? Item.Fields[Constants.DictionaryValue].Value : string.Empty);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" GetDictionary gives -> " + ex.Message);
            }
            return customDictionary;
        }
    }
}