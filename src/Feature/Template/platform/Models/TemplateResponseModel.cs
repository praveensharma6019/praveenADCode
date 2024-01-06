using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Template.Models
{
    public class TemplateResponseModel
    {
        public bool Success { get; set; } = false;
        public IEnumerable<KeyValuePair<string, string>> Errors { get; set; }
    }
}