using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Sitecore.AdaniSolar.Website.Models
{
    public class DownloadHistoryModel
    {

        public int Auto_number { get; set; }
        public string UserName { get; set; }
        public string Invoice_Number { get; set; }
        public DateTime Invoice_Date { get; set; }
        public string Pallet_Id { get; set; }
        public string Module_Serial_Number { get; set; }
        public DateTime? Warranty_valid_till { get; set; }
        public DateTime Currentdate { get; set; }
        public DateTime CurrentTime { get; set; }


    }
}