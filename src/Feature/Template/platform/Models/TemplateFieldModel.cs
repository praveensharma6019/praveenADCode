using Sitecore.Feature.Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Template.Models
{
    public class TemplateFieldModel
    {
        public string parentItem { get; set; }
        public string templateId { get; set; }
        public string newItemName { get; set; }
        public string itemPath { get; set; }
        public string templateFields { get; set; }
        
    }
    public class TemplateFields 
    {
        public List<FieldModel> Fields { get; set; }
    }
}