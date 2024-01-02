using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Adani.EV.Project.Models
{
    public class ContactusSectionsModel
    {
        public ContactusBannerModel banner { get; set; }=new ContactusBannerModel();
        public ContactusReachoutModel reachout { get; set; }= new ContactusReachoutModel();
        public ContactusformModel contactusform { get; set; }=new ContactusformModel();
        public ContactusFaqModel faq { get; set; }=new ContactusFaqModel();
    }
}