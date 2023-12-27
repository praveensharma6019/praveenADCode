using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class Terminal
    {
        public List<Terminals> ListOfTerminals { get; set; }
    }
    public class Terminals
    {
        public string TerminalName { get; set; }
        public PackageDetailsList PackageDetails { get; set; }
    }
    public class PackageDetailsList
    {
         public List<PranaamPackageDetail> PackageDetail { get; set; }
    }
    public class PranaamPackageDetail
    {
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        //public string PackageNumber { get; set; }
        public string PackageId { get; set; }
        public string AirportMasterId { get; set; }
        public string PackageImage { get; set; }
        public AirportPackageServices PackageServices { get; set; }
        public PackageAddOns PackageAddOns { get; set; }

    }

    public class AirportPackageServices
    {
        public List<AirportPackageService> PackageService { get; set; }
    }
    public class AirportPackageService
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
    }

    public class PackageAddOns
    {
        public List<PranaamPackageAddOn> PackageAddOn { get; set; }
    }
    public class PranaamPackageAddOn
    {
        //public string Id { get; set; }
        //public string PackageId { get; set; }
        public string AddOnServiceId { get; set; }
        public string AddOnServiceName { get; set; }
        public string AddOnServiceDescription { get; set; }
        public string AddOnImage { get; set; }
        public string AddOnMobileImage { get; set; }
    }
}