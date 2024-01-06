using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class GasConsumptionPattern
    {
        public List<ConsumptionDetails> SCMValues { get; set; }
        public List<ConsumptionDetails> AmountValues { get; set; }
        public List<ConsumptionDetails> MMBTUValues { get; set; }
        public GasConsumptionPattern()
        {
            SCMValues = new List<ConsumptionDetails>();
            AmountValues = new List<ConsumptionDetails>();
            MMBTUValues = new List<ConsumptionDetails>();
        }
    }

    public class ConsumptionDetails
    {
        public string Date { get; set; }
        public string Consumption { get; set; }
    }

    public class ConsumptionYear
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}