using System;

namespace SapPiService.Domain
{
    public class OutageRecord
    {
        public string Date { get; set; }
        public string StartTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public string EndTime { get; set; }
        public DateTime EndDatetime { get; set; }
        public string ActivityType { get; set; }
        public string ActivityDescription { get; set; }
        public string OutageType { get; set; }
        public string ZFLAG { get; set; }
    }
}