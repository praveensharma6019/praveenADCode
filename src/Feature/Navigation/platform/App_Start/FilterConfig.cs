using System.Web;
using System.Web.Mvc;

namespace Adani.SuperApp.Airport.Feature.Navigations.Platform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
