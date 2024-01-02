using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Newtonsoft.Json;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models
{
    public class LoyaltyWidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }
}