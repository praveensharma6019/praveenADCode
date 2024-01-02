using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Tasks;
using System;
using System.Linq;

namespace Project.AdaniOneSEO.Website.Tasks
{
    public class UpdateFilterOptionDataSourceDateFieldTask
    {
        public void Execute(Item[] items, CommandItem command, ScheduleItem schedule)
        {
            Log.Info("Running UpdateFilterOptionDataSourceDateFieldTask at" + DateTime.Now.ToString(), DateTime.Now);
            try
            {
                Log.Info("Inside try block of UpdateFilterOptionDataSourceDateFieldTask", DateTime.Now);

                string domesticFlightsFolderItemID = "{3FDC2D71-8830-489C-86CE-53800018E124}";
                Item domesticFlightsFolderItem = GetItemById(domesticFlightsFolderItemID);

                Log.Info("DomesticFlightsFolderItem found with path" + domesticFlightsFolderItem.Paths.Path, domesticFlightsFolderItem);

                var appRouteItems = domesticFlightsFolderItem.Children.Where(i => i.TemplateID == ID.Parse("{1C5E12C1-CB4C-4284-9E98-AD75F473346F}"));
                
                Log.Info("Total AppRouteItems found, count:" + appRouteItems.Count(), appRouteItems);

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

                                    DateField dateField = (DateField)cityItem.Fields["Date"];
                                    if (dateField != null)
                                    {
                                        cityItem.Editing.BeginEdit();

                                        dateField.Value = Sitecore.DateUtil.ToIsoDate(DateTime.Now.AddDays(6));

                                        cityItem.Editing.EndEdit();

                                        Log.Info("Updated date field of:" + cityItem.Paths.FullPath, cityItem);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exception raised in UpdateFilterOptionDataSourceDateFieldTask, ex:" + ex.StackTrace, ex);
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