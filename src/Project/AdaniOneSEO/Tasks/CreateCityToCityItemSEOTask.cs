using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Tasks;
using System;
using System.Net.Http;

namespace Project.AdaniOneSEO.Website.Tasks
{
    public class CreateCityToCityItemSEOTask
    {
        public void Execute(Item[] items, CommandItem command, ScheduleItem schedule)
        {
            Log.Info("Running CreateCityToCityItemSEOTask at" + DateTime.Now.ToString(), DateTime.Now);

            try
            {
                Log.Info("Inside try block of CreateCityToCityItemSEOTask", DateTime.Now);
                DateTime today = DateTime.Now;
                DateTime itemCreationDate = today.AddDays(13);
                string itemCreationDateString = itemCreationDate.ToString("MM-dd-yyyy");

                Item seoCreateSitecoreItemApiUrlItem = GetItemById("{70BEB56F-37C5-4374-84FB-5A729220F5A7}");
                if (seoCreateSitecoreItemApiUrlItem != null)
                {
                    string url = seoCreateSitecoreItemApiUrlItem.Fields["APIEndpoint"].Value;
                    if (!string.IsNullOrEmpty(url))
                    {
                        string createItemApiUrl = string.Format(url, itemCreationDateString);

                        Log.Info("CreateItemApiUrl:" + createItemApiUrl, DateTime.Now);

                        using (var client = new HttpClient())
                        {
                            client.Timeout = TimeSpan.FromMinutes(10);

                            var response = client.GetAsync(createItemApiUrl).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                string content = response.Content.ReadAsStringAsync().Result;
                                Log.Info("Items created successfully, response from api call:" + content, DateTime.Now);
                            }
                            else
                            {
                                Log.Info("Unable to create items, response from api call:" + response.StatusCode, DateTime.Now);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exception raised in CreateCityToCityItemSEOTask, ex:" + ex.StackTrace, ex);
                throw ex;
            }
        }

        private static Item GetItemById(string itemId)
        {
            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
            return masterDb.GetItem(new ID(itemId));
        }
    }
}