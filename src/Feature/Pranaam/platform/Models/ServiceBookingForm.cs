using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class ServiceBookingForm
    {
            [JsonProperty(PropertyName = "widget")]
            public WidgetItem widget { get; set; }
    }
}