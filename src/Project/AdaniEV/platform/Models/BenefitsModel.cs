using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class BenefitsModel
    {
        public string title { get; set; }
        public string detail { get; set; }
        public List<BenefitItemModel> fields { get; set; } = new List<BenefitItemModel>();
    }
}