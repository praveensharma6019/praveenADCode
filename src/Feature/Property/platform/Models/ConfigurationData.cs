using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class Key
    {
        public string link { get; set; }
        public string keyword { get; set; }
    }

    public class ConfigurationData
    {
        public string title { get; set; }
        public List<Key> keys { get; set; }
    }

    public class ConfigurationDataModel
    {
        public string city { get; set; }

        public List<ConfigurationData> items { get; set; }
    }
}