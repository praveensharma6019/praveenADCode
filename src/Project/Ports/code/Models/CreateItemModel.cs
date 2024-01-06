using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class CreateItemModel
    {
        public TemplateFields templateFields { get; set; }
        
    }

    public class TemplateFields
    {
        public List<FieldModel> Fields { get; set; }

    }

    public class FieldModel
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}