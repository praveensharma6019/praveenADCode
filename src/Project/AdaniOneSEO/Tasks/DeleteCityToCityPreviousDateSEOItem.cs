using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Tasks;
using System;
using System.Linq;

namespace Project.AdaniOneSEO.Website.Tasks
{
    public class DeleteCityToCityPreviousDateSEOItem
    {
        public void Execute(Item[] items, CommandItem command, ScheduleItem schedule)
        {
            Log.Info("Running DeleteCityToCityPreviousDateSEOItem at" + DateTime.Now.ToString(), DateTime.Now);
            try
            {
                Log.Info("Inside try block of DeleteCityToCityPreviousDateSEOItem", DateTime.Now);

                string domesticFlightsFolderItemID = "{3FDC2D71-8830-489C-86CE-53800018E124}";
                Item domesticFlightsFolderItem = GetItemById(domesticFlightsFolderItemID);

                Log.Info("DomesticFlightsFolderItem found with path" + domesticFlightsFolderItem.Paths.Path, domesticFlightsFolderItem);

                var appRouteItems = domesticFlightsFolderItem.Children.Where(i => i.TemplateID == ID.Parse("{1C5E12C1-CB4C-4284-9E98-AD75F473346F}"));

                Log.Info("Total AppRouteItems found, count: " + appRouteItems.Count(), appRouteItems);

                if (appRouteItems.Any())
                {
                    foreach (Item mainItem in appRouteItems)
                    {
                        Log.Info("ApprouteItem found at:" + mainItem.Paths.Path, mainItem);

                        var datasourceFolderItem = mainItem.Children.Select(i => i).Where(i => i.Name == "Datasource").FirstOrDefault();

                        if (datasourceFolderItem != null)
                        {
                            Log.Info("datasourceFolderItem found at:" + datasourceFolderItem.Paths.Path, datasourceFolderItem);

                            var citiesFolderItem = datasourceFolderItem.Children.Select(i => i).Where(j => j.Name == "Cities").FirstOrDefault();

                            if (citiesFolderItem != null)
                            {
                                Log.Info("citiesFolderItem found at:" + citiesFolderItem.Paths.Path, citiesFolderItem);

                                var cityItem = citiesFolderItem.Children.Select(i => i).Where(j => j.TemplateID == ID.Parse("{5B84E0FE-50A0-4373-840C-89D3E5DD3707}")).FirstOrDefault();

                                if (cityItem != null)
                                {
                                    Log.Info("cityItem found at:" + cityItem.Paths.Path, cityItem);

                                    DateTime today = DateTime.Now;
                                    DateTime itemCreationDate = today.AddDays(1);
                                    string itemToDeleteDateString = itemCreationDate.ToString("dd-MM-yyyy");

                                    var itemToDelete = cityItem.Children.Select(i => i).Where(j => j.Name == itemToDeleteDateString).FirstOrDefault();
                                    if (itemToDelete != null)
                                    {
                                        Log.Info("itemToDelete found at:" + itemToDelete.Paths.Path, itemToDelete);
                                        DeleteItemAndSubitems(itemToDelete);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exception raised in DeleteCityToCityPreviousDateSEOItem, ex:" + ex.StackTrace, ex);
                throw ex;
            }
        }

        private static Item GetItemById(string itemId)
        {
            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
            return masterDb.GetItem(new ID(itemId));
        }
        
        private void DeleteItemAndSubitems(Item item)
        {
            Log.Info("Inside DeleteItemAndSubitems method", DateTime.Now);
            if(item != null) 
            {
                foreach (Item subitem in item.GetChildren())
                {
                    DeleteItemAndSubitems(subitem);
                }

                item.Delete();
            }
        }
    }
}