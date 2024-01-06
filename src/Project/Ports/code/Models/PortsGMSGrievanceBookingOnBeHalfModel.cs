using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGMSGrievanceBookingOnBeHalfModel
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
      
        [EmailAddress(ErrorMessage = "Please Enter Valid EmailAddress.")]
        [Required(ErrorMessage = "Email is Required.")]
        public string Email
        {
            get;
            set;
        }


        [RegularExpression("^[0-9]{8,16}$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is Required.")]

        public string Mobile
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Nature is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Nature { set; get; }
        [Required(ErrorMessage = "Email is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Location { set; get; }
        [Required(ErrorMessage = "Email is Required.")]
        [StringLength(255, ErrorMessage = "Maximum 150 characters allowed")]
        public string Subject { set; get; }
        [Required(ErrorMessage = "Email is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Company { set; get; }
        [Required(ErrorMessage = "Email is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string WhoImpacted { set; get; }

        public string Brief { set; get; }

        public string OnBehalf { get; set; }
        
        public DateTime? Modified_Date
        {
            get;
            set;
        }

        public DateTime? Created_Date
        {
            get;
            set;
        }
       public HttpPostedFileBase File { get; set; }

        public List<PortsGms_Registration> StakHolders
        {
            get;
            set;
        }


        public PortsGMSGrievanceBookingOnBeHalfModel()
        {
            this.StakHolders = new List<PortsGms_Registration>();
            //this.UserDetails = new List<PortsGms_Registration>();



        }
    }
}