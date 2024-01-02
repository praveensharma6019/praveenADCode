using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public class GenericContentService : IGenericContentService
    {
        public object Render(Rendering rendering)
        {
            object model = null;
            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return model;

            try
            {
                model = RenderItemAsModel(dsItem);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return model;
        }

        private object RenderItemAsModel(Item item)
        {
            var itemTemplate = item.Template;

            //populate folder items as list
            if (itemTemplate.Name == "Folder")
            {
                return GetFolderItemsAsList(item);
            }

            var model = new Dictionary<string, object>();
            //populate field as single value property
            PopulateItemFields(item, model);
            //populate children as complex property except folder template
            //folder item will populated as list
            PopulateChildrenItems(item, model);
            return model;
        }

        private object GetFolderItemsAsList(Item item)
        {
            var list = new List<object>();
            foreach (Item childItem in item.Children)
            {
                list.Add(RenderItemAsModel(childItem));
            }

            return list;
        }

        private void PopulateChildrenItems(Item item, Dictionary<string, object> model)
        {
            foreach (Item childItem in item.Children)
            {
                var key = GetKey(childItem.Name);
                var children = RenderItemAsModel(childItem);
                if (model.ContainsKey(key))
                {
                    model[key] = children;
                    continue;
                }

                model.Add(key, children);
            }
        }

        private void PopulateItemFields(Item item, Dictionary<string, object> model)
        {
            var itemTemplatefields = item.Template.Fields.Where(x => !x.Name.StartsWith("__"));

            foreach (TemplateFieldItem field in itemTemplatefields)
            {
                string key = GetKey(field.Name);
                switch (field.Type)
                {
                    case "General Link":
                        LinkField lf = item.Fields[field.ID];
                        model.Add(key, Utils.GetLinkURL(lf));
                        if (!string.IsNullOrEmpty(lf.Text))
                        {
                            model.Add(key: $"{key}Text", lf.Text);
                        }
                        if (!string.IsNullOrEmpty(lf.Target))
                        {
                            model.Add(key: $"{key}Target", lf.Target);
                        }
                        break;
                    case "Image":
                        ImageField imgField = item.Fields[field.ID];
                        model.Add(key, imgField.MediaItem == null ? string.Empty : MediaManager.GetMediaUrl(imgField.MediaItem));
                        if (!string.IsNullOrEmpty(imgField.Alt))
                        {
                            model.Add(key: $"{key}Alt", imgField.Alt);
                        }
                        break;
                    case "Checkbox":
                        model.Add(key, Utils.GetBoleanValue(item, field.ID));
                        break;
                    default:
                        model.Add(key, Utils.GetValue(item, field.ID, item.Name));
                        break;
                }
            }
        }

        private string GetKey(string value)
        {
            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}