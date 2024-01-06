using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGmsAdminDashbord
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

        public List<PortsGmsAdminDashbord> AllBookings
        {
            get;
            set;
        }


       

        public PortsGmsAdminDashbord()
        {
            this.AllBookings = new List<PortsGmsAdminDashbord>();
            



        }

        
    }
}