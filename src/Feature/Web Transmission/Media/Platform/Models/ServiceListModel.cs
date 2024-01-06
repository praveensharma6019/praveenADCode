using System;
using System.Collections.Generic;
using System.Linq;
using Adani.BAU.Transmission.Foundation.Theming.Platform.Models;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Models
{
    public class ServiceListModel
    {
        public string ServiceTitle { get; set; }
        public List<ServiceItem> serviceItems { get; set; }
    }

    public class ServiceItem
    {
        public string uniqueId { get; set; }

        public string title { get; set; }

        public string imageSrc { get; set; }

        public string ctaUrl { get; set; }

        public TagName TagName { get; set; }
    }
    public class services
    {
        public WidgetItem widget { get; set; }

    }

    public class TagName
    {
        public string name { get; set; }

        public string backgroundColor { get; set; }

        public string textColor { get; set; }
    }
}