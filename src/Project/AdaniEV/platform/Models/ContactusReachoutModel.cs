using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class ContactusReachoutModel
    {
         public string title { get; set; }
        public ContactusAddressdetailsModel addressdetails { get; set; }=new ContactusAddressdetailsModel();
        public ContactdetailsModel contactdetails { get; set; }=new ContactdetailsModel();
        public ContactusEmaildetailsModel emaildetails { get; set; }= new ContactusEmaildetailsModel(); 
    }
}