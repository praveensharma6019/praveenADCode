using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class CONApplicationStatusUpdateModel
    {
        public string TEMP_REG_NO { get; set; }
        public string Old_CA { get; set; }
        public string New_CA { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string Action_datetime { get; set; }
    }
}