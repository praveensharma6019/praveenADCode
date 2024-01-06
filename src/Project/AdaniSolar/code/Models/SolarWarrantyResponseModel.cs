using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniSolar.Website.Models
{
    public class SolarWarrantyResponseModel
    {
        public List<Record> Record { get; set; }
    }
    public class SolarWarrantyResponseModelNew
    {
        public Record Record { get; set; }
    }
    public class Record
    {
        public string Serial_No { get; set; }
        public string Pallet_ID { get; set; }
        public string Plant_Code { get; set; }
        public string Waranty_Period { get; set; }
        public int Warranty_Start_Date { get; set; }
        public int Warranty_End_Date { get; set; }
        public string Invoice_No { get; set; }
        public int Invoice_Date { get; set; }
    }



}