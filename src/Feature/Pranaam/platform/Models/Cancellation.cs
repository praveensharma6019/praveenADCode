using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class CancellationModel
    {
        public List<CancellationHeader> Header { get; set; }
        public List<CancellationRow> Rows { get; set; }
    }
    public class CancellationRow
    {
        public string SrNo { get; set; }
        public string Services { get; set; }
        public string Charges { get; set; }
    }
    public class CancellationHeader
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }

}