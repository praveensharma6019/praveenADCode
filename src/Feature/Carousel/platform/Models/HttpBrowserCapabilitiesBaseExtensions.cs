using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class HttpBrowserCapabilitiesBaseExtensions
    {
        public bool IsMobileNotTablet(HttpBrowserCapabilitiesBase browser)
        {
            if (browser != null && browser.Capabilities != null && browser.Capabilities[""] != null)
            {
                var userAgent = browser.Capabilities[""].ToString();
                var r = new Regex("ipad|(android(?!.*mobile))|xoom|sch-i800|playbook|tablet|kindle|nexus|silk", RegexOptions.IgnoreCase);
                var isTablet = r.IsMatch(userAgent) && browser.IsMobileDevice;
                return !isTablet && browser.IsMobileDevice;
            }
            else
                return false;
        }
    }
}