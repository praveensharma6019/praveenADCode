using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class ConsumptionHistory
    {
        public List<MeterConsumption> MeterConsumptions { get; set; }
    }

    public class MeterConsumption
    {
        public string MeterNumber { get; set; }
        public List<ConsumptionHistoryRecord> ConsumptionRecords { get; set; }
    }
    
    public class ConsumptionHistoryRecord
    {
        public string ConsumptionDate { get; set; }
        public string Status { get; set; }
        public string Reading { get; set; }
        public string UnitsConsumed { get; set; }      
    }
}