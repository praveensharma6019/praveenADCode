using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Models
{
    public class TermsConditions
    {
        public string title { get; set; }

        public List<string> lines { get; set; }

        public TermsConditions() 
        {
            lines = new List<string>();
        }
    }
}