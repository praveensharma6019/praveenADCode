using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public class TenderLoginModel
    {
        public string userId { get; set; }
        public string TenderId { get; set; }
        public string UserType { get; set; }
    }
}