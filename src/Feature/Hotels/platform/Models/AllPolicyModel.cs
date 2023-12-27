using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Models
{
    public class AllPolicies
    {
        public string title { get; set; }
        public List<Policies> policies { get; set; }


        public AllPolicies()
        {
            policies = new List<Policies>();
        }
    }

    public class Policies
    {
        public string subTitle { get; set; }

        public string code { get; set; }

        public List<string> policiesData { get; set; }

        public Policies()
        {
            policiesData = new List<string>();
        }
    }
}