using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Template.Models
{
    public class TemplateData
    {
        public string parentItem { get; set; }
        public string templateId { get; set; }
        public string newItemName { get; set; }
        public string itemPath { get; set; }
        public TemplateFields templateFields { get; set; }
    }
}