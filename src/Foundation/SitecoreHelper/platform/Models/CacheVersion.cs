using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Models
{
    public class CacheVersion
    {

        public long time { get; set; }
        public string statics { get; set; }
        public string cacheVersion { get; set; }
        public string pathPrefix { get; set; }
    }
}