using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Models
{
    public class CancellationReason
    {
       
      public List<Reasons> reasons { get; set; }

    }

    public class Reasons {

        public string Code { get; set; }
        public string Label { get; set; }
    }
}