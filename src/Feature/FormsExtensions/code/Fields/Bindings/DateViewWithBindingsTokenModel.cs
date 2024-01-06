﻿using System;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;

namespace Feature.FormsExtensions.Fields.Bindings
{
    [Serializable]
    public class DateViewWithBindingsTokenModel : DateViewModel, IBindingSettings
    {
        public string BindingToken { get; set; }
        public bool PrefillBindingValue { get; set; }
        public bool StoreBindingValue { get; set; }

        protected override void InitItemProperties(Item item)
        {
            base.InitItemProperties(item);
            this.InitBindingSettingsProperties(item);
        }

        protected override void UpdateItemFields(Item item)
        {
            base.UpdateItemFields(item);
            this.UpdateBindingSettingsFields(item);
        }
    }
}