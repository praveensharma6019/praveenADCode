using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Realty.Website.Model
{
    [Serializable]
    public class LoginModel
    {
        public string userId { get; set; }
        public string TenderId { get; set; }
        public string UserType { get; set; }
        public string leadCity { get; set; }
    }
}