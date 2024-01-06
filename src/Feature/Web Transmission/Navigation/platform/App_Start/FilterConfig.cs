using System.Web;
using System.Web.Mvc;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
