using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sitecore.Realty.Website.Model
{
    public class AdaniShantigramModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string Location { get; set; }
        public string Captcha { get; set; }
        public string OTP { get; set; }
        public string FormType { get; set; }
        public string PageInfo { get; set; }
        public string UTMSource { get; set; }
        public DateTime FormSubmitOn { get; set; }


    }
}
  