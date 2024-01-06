using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class NewInquiryCNG
    {
        public string Bpkind { get; set; }
        public string Plant { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        public string Name_first { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        public string Name_last { get; set; }
        [Required(ErrorMessage = "Mobile No. Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Invalid Mobile No.")]
        public string Mob_number { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Smtp_addr { get; set; }

        [Required(ErrorMessage = "City Required")]
        public string City1 { get; set; }
       
        public string Post_code1 { get; set; }
        [Required(ErrorMessage = "City Required")]
        public string Regioarea { get; set; }

        
        
        
        public string Street { get; set; }
        
        public string House_num1 { get; set; }
        
        public string Str_suppl1 { get; set; }
        public string Str_suppl2 { get; set; }
        public string Str_suppl3 { get; set; }
        public string Region { get; set; }
        public string GroupDesc { get; set; }
        [Required(ErrorMessage = "Region Required")]
        public string RegioGroup { get; set; }

        [Required(ErrorMessage="Vehicle Type Required")]
        public string VehilcleType { get; set; }
        [Required(ErrorMessage = "Vehicle Category Required")]
        public string VehicleCatagory { get; set; }
        [Required(ErrorMessage = "Vehicle Company Required")]
        public string VehicleMake { get; set; }
        //[Required(ErrorMessage = "Vehicle Model Required")]
        public string VehicleModel { get; set; }
        public string VehicleNumber { get; set; }

        public string Compno { get; set; }
        public string Qmnum { get; set; }
        public string MsgFlag { get; set; }
        public string Message { get; set; }
    }
}