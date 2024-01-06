using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class GeyserConnection
    {
        public double Amount { get; set; }
        public string Msgflag { get; set; }
        public string Message { get; set; }
        public string Tax { get; set; }
        public string ExtraAmount { get; set; }
        public string MaxLengthgas { get; set; }
        public string Quantity_Min { get; set; }
        public string Quantity_Max { get; set; }
        public string TempQuantity_Min { get; set; }
        public string TempQuantity_Max { get; set; }
    }
}