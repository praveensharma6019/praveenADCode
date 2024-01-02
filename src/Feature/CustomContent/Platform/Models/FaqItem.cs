

using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models
{
    public class FaqItem
    {
        public string question { get; set; }
        public string answer { get; set; }
    }
    public class FaqJSONWidget
    {
        public WidgetItem widget { get; set; }
        //  public List<FilterProducts> widgetItems { get; set; }
    }
}