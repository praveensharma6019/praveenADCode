using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls.Presentation;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public class CustomDictionaryList : ICustomDictionaryList
    {
        private readonly ILogRepository _logRepository;

        public CustomDictionaryList(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetDictionary(Rendering rendering)
        {

            var ItemList = Sitecore.Context.Item;
            Sitecore.Data.Items.Item myitem = (Sitecore.Data.Items.Item)ItemList;
            //Read the Multifield List
            Sitecore.Data.Fields.MultilistField multiselectField = myitem.Fields["SelectFolders"];
            Sitecore.Data.Items.Item[] items = multiselectField.GetItems();

            Dictionary<string, Dictionary<string, Dictionary<string, string>>> results = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            //Iterate through each item
            if (items != null && items.Length > 0)
            {
                try
                {

                    foreach (Sitecore.Data.Items.Item ConfigItem in items)
                    {
                        var customDictionary = new Dictionary<string, Dictionary<string, string>>();
                        foreach (Sitecore.Data.Items.Item Item in ConfigItem.GetChildren())
                        {

                            var mainDictionary = new Dictionary<string, string>();
                            foreach (Sitecore.Data.Items.Item item in Item.GetChildren())
                            {
                                mainDictionary.Add(!string.IsNullOrEmpty(item.Fields[Constants.Title].Value) ? item.Fields[Constants.Title].Value : string.Empty
                                , !string.IsNullOrEmpty(item.Fields[Constants.DictionaryValue].Value) ? item.Fields[Constants.DictionaryValue].Value : string.Empty);
                            }
                            customDictionary.Add(Item.Name, mainDictionary);

                        }
                        results.Add(ConfigItem.Name, customDictionary);

                    }

                }
                catch (Exception ex)
                {
                    _logRepository.Error(" GetDictionary gives -> " + ex.Message);
                }
            }

            //return customDictionary;
            return results;
        }
    }
}