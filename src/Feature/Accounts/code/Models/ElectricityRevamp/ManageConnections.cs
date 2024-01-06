using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models.ElectricityRevamp
{
    public class ManageConnections
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string LoginName { get; set; }
        public string MasterAccountNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Division { get; set; }
        public string Address { get; set; }
        public string BillMonth { get; set; }
        public string MeterNumber { get; set; }
        public string SecurityDeposit { get; set; }
        public string TariffCategory { get; set; }
    }
}