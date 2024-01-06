using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Models
{
    public class ConsumptionHistory
    {
        public string MeterInfo { get; set; }
        public string MeterNumber { get; set; }
        public List<ConsumptionHistoryRecord> ConsumptionHistoryList { get; set; }
    }
    public class ConsumptionHistoryRecord
    {
        public string ConsumptionDate { get; set; }
        public string Status { get; set; }
        public string Reading { get; set; }
        public string UnitsConsumed { get; set; }       
    }
}