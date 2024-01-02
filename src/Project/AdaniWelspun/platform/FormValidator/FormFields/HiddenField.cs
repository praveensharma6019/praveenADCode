using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using System;

namespace Adani.BAU.AdaniWelspunSXA.Project.FormValidator.FormFields
{
    [Serializable]
    public class HiddenField : StringInputViewModel
    {
     
        protected override void InitItemProperties(Item item)
        {
            base.InitItemProperties(item);
        }

        protected override void UpdateItemFields(Item item)
        {
            base.UpdateItemFields(item);
        }
    }
}