using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class VesselModel
    {
        public VesselsClass vesselsClass { get; set; }
        public DataClass data { get; set; }
        public string totWt { get; set; }
        public int terminalNo { get; set; }
        public string cmdtName { get; set; }
        public string sbuCode { get; set; }
        public string sbuName { get; set; }
        public string terminalName { get; set; }
        public string ataDttm { get; set; }
        public string dschLdBothInd { get; set; }
        public DateTime? plndPobDttm { get; set; }
        public string dschVslAgentCd { get; set; }
        public string vslNm { get; set; }
        public string berthNo { get; set; }
        public string viaNo { get; set; }
        public string etaDttm { get; set; }
        public string etdDttm { get; set; }

        public string allocPobDttm { get; set; }
        public string disembarkDttm { get; set; }
        public string lastLineInDttm { get; set; }
    }

    public class VesselsClass
    {
        public List<VesselModel> vessels;
    }

    public class DataClass
    {
        public List<VesselsClass> data;
    }
}
