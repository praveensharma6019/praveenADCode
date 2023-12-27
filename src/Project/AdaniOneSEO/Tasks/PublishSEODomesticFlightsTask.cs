using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Publishing;
using Sitecore.SecurityModel;
using Sitecore.Tasks;
using System;

namespace Project.AdaniOneSEO.Website.Tasks
{
    public class PublishSEODomesticFlightsTask
    {
        public void Execute(Item[] items, CommandItem command, ScheduleItem schedule)
        {
            Log.Info("Running PublishSEODomesticFlightsTask at" + DateTime.Now.ToString(), DateTime.Now);
            try
            {
                Log.Info("Inside try block of PublishSEODomesticFlightsTask", DateTime.Now);
                using (new SecurityDisabler())
                {
                    var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                    Item domesticFlightsFolderItem = masterDb.GetItem(new ID("{3FDC2D71-8830-489C-86CE-53800018E124}"));

                    if (domesticFlightsFolderItem != null)
                    {
                        Log.Info("DomesticFlightsFolderItem found with path" + domesticFlightsFolderItem.Paths.Path, domesticFlightsFolderItem);

                        PublishChildrenRecursive(domesticFlightsFolderItem);

                        Log.Info("DomesticFlightsFolderItem Published", domesticFlightsFolderItem);
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Info("Exception raised in PublishSEODomesticFlightsTask, ex:" + ex.StackTrace, ex);
                throw ex;
            }
        }

        public void PublishChildrenRecursive(Item item)
        {
            try
            {
                if (item != null)
                {
                    Log.Info("Inside PublishChildrenRecursive for item:" + item.Paths.FullPath, item);
                    if (item.HasChildren)
                    {
                        Log.Info("Item:" + item.Name + "has children, No:" + item.Children.Count, item);
                        foreach (Item child in item.Children)
                        {
                            Log.Info("Publish started for child item:" + child.Name, child);
                            using (new SecurityDisabler())
                            {
                                Log.Info("Inside SecurityDisabler of PublishChildrenRecursive", DateTime.Now);
                                var targetDatabase = Sitecore.Configuration.Factory.GetDatabase("web");

                                PublishOptions options = new PublishOptions(
                                    child.Database,
                                    targetDatabase,
                                    PublishMode.Smart,
                                    child.Language, DateTime.Now);

                                options.RootItem = child;
                                options.Deep = true;

                                var publisher = new Publisher(options);
                                publisher.Publish();

                                Log.Info("Published item:" + child, child);
                            }

                            PublishChildrenRecursive(child);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exception raised in PublishChildrenRecursive, ex:" + ex.StackTrace, ex);
                throw ex;
            }
        }
    }
}