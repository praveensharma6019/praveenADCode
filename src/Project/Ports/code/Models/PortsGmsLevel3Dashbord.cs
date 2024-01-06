using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGmsLevel3Dashbord
    {

        public Guid Id
        {
            get;
            set;
        }
        public Guid RegistrationId
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public string Mobile
        {
            get;
            set;
        }
        public string Company_Name
        {
            get;
            set;
        }
        public string DOB
        {
            get;
            set;
        }
        public string Gender
        {
            get;
            set;
        }
        public string Department
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public Boolean Assigned
        {
            get;
            set;
        }
        public string Level0UserName
        {
            get;
            set;
        }
        public string Level1UserName
        {
            get;
            set;
        }
        public string Level2UserName
        {
            get;
            set;
        }
        public string Level3UserName
        {
            get;
            set;
        }
        public string Nature { set; get; }
        public string Location { set; get; }
        public string Subject { set; get; }
        public string Company { set; get; }
        public string WhoImpacted { set; get; }
        public string Brief { set; get; }
        public string LevelInfo { set; get; }
        public string Response { set; get; }
        public string Status { set; get; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingStatus { set; get; }
        public string Level0Comment { set; get; }
        public string Level1Comment { set; get; }
        public string Level2Comment { set; get; }
        public string Comment { set; get; }
        public string Comments { set; get; }
        public string BusinessGroup { set; get; }
        public string SiteHead { set; get; }
        public string HO { set; get; }
        public string PointMan { set; get; }
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

        public List<PortsGmsLevel3Dashbord> AllBookings
        {
            get;
            set;
        }
       

        public PortsGmsLevel3Dashbord()
        {
            this.AllBookings = new List<PortsGmsLevel3Dashbord>();
            //this.UserDetails = new List<PortsGms_Registration>();



        }

        
    }
}