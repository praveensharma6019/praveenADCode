using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Models
{
    public class InvestorMeetingModel
    {
        public HtmlString HeaderTitle { get; set; }
        public List<InvestorModel> InvestorList { get; set; }
    }
    public class InvestorModel
    {
        public string filePath;

        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
      
    }
}