using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Feature.Template.Models;
using Sitecore.Publishing;
using Sitecore.SecurityModel;
using System;

namespace Sitecore.Feature.Template.Services
{
    public class TemplateItemService : ITemplateItemService
    {
        public bool CreateItem(TemplateData templateData)
        {
            Log.Info("TemplateData Recevied from TemplateController" + templateData, templateData);
            // Create a new item based on the provided template ID
            using (new SecurityDisabler())
            {
                Log.Info("TemplateData SecurityDisabler Enabled" , string.Empty);
                try
                {
                    Log.Info("Inside try block of SecurityDisabler", string.Empty);
                    //var masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                    var masterDb = Sitecore.Data.Database.GetDatabase("web");
                    var parentId = Sitecore.Data.ID.Parse(templateData.parentItem);
                    var parentName = masterDb.GetItem(parentId);
                    Sitecore.Data.Serialization.Manager.DumpTree(parentName);
                    parentName = masterDb.GetItem(parentId);
                    var response = "Success";
                    #region working code
                    //Item parentName = masterDb.GetItem(new ID(templateData.parentItem));
                    //Sitecore.Data.Serialization.Manager.DumpItem(parentName);
                    //string itemPath = parentName.Paths.FullPath;
                    // Sitecore.Data.Serialization.Manager.DumpTree(parentName);
                    #endregion

                    // Get the template item
                    var templateItem = masterDb.GetTemplate(templateData.templateId);
                    Log.Info("TemplateItem  Created"+ templateItem, templateItem);
                    if (templateItem != null)
                    {
                        Log.Info("TemplateItem  Is Not Null" + templateItem, templateItem);
                        // Create a new item under the current context item
                        Item newItem = parentName.Add(templateData.newItemName, templateItem);
                        Sitecore.Data.Serialization.Manager.DumpItem(newItem);
                        newItem.Editing.BeginEdit();
                        // Set field values based on the provided model
                        Log.Info("SiteCore Editing Started", string.Empty);
                        foreach (var field in templateData.templateFields.Fields)
                        {
                            newItem[field.FieldName] = field.FieldValue;
                        }

                        // Save the changes
                        newItem.Editing.EndEdit();
                        // Publish the item to the web database 
                        Log.Info("SiteCore Editing Finished", string.Empty);
                        //PublishOptions publishOptions = new PublishOptions(masterDb, Database.GetDatabase("web"), PublishMode.SingleItem, newItem.Language, System.DateTime.Now);
                        //Publisher publisher = new Publisher(publishOptions);
                        ////publisher.Publish();
                        //publisher.Options.RootItem = newItem; publisher.Publish();
                        //Log.Info("SiteCore Item published", string.Empty);
                        // Redirect or return success message

                        return true;
                    }
                    else
                    {
                        Log.Info("TemplateItem Is  Null", string.Empty);
                        // Handle invalid template item
                        response = "Invalid Template";
                        Log.Error("response", this);
                        return false;
                    }
                }

                catch (Exception ex)
                {
                    Log.Info("Exception Recevied in TemplateItemService, Exception:"+ ex, ex);
                    Log.Error("response", this);
                    return false;
                }

            }
        }

        public bool UpdateItem(TemplateData templateData)
        {
            Log.Info("UpdateItem TemplateData Recevied from TemplateController" + templateData, templateData);
            using (new SecurityDisabler())
            {
                Log.Info("UpdateItem TemplateData SecurityDisabler Enabled", string.Empty);
                //You want to alter the item in the master database, so get the item from there
                //Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                var db = Sitecore.Data.Database.GetDatabase("web");
                Item item = db.GetItem(new ID(templateData.itemPath));
                //Begin editing
                item.Editing.BeginEdit();
                Log.Info("UpdateItem Editing started" + item, item);
                try
                {
                    //perform the editing
                    Log.Info("UpdateItem Inside try block of editing", string.Empty);
                    foreach (var field in templateData.templateFields.Fields)
                    {
                        item[field.FieldName] = field.FieldValue;
                    }
                    Log.Info("UpdateItem Inside for loop of editing", string.Empty);
                }
                catch (Exception ex)
                {
                    Log.Info("Could not update item " + item.Paths.FullPath + ": " + ex.Message, this);
                    item.Editing.CancelEdit();
                    var response = "Could not update Item";
                    return false;
                }
                finally
                {
                    //Close the editing state
                    item.Editing.EndEdit();
                    Log.Info("Item Updated in sitecore EndEditing " + item.Paths.FullPath, this);
                    //PublishOptions publishOptions = new PublishOptions(db, Database.GetDatabase("web"), PublishMode.SingleItem, item.Language, System.DateTime.Now);
                    //Publisher publisher = new Publisher(publishOptions);
                    ////publisher.Publish();
                    //publisher.Options.RootItem = item; publisher.Publish();
                }
            }
            return true;
        }


    }
}