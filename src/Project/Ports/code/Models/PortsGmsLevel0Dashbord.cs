using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGmsLevel0Dashbord
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
        public int? TotalCount { set; get; }
        public int? CloseCount { set; get; }
        public int? OpenCount { set; get; }
        public int? ReOpenCount { set; get; }
        public int? OnBehalfCount { set; get; }
        public int? ResponseCount { set; get; }
        public string Comment { set; get; }
        public string Nature { set; get; }
        public string Location { set; get; }
        public string Subject { set; get; }
        public string Company { set; get; }
        public string WhoImpacted { set; get; }
        public string Brief { set; get; }
        public string LevelInfo { set; get; }
        public string Response { set; get; }
        public string Remarks { set; get; }
        public string Status { set; get; }
        public string UserType { set; get; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingStatus { set; get; }
        public string FinalComment { set; get; }
        public string Level0Comment { set; get; }
        public string Level1Comment { set; get; }
        public string Level2Comment { set; get; }
        public string Level3Comment { set; get; }
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
        public List<PortsGmsLevel0Dashbord> AllBookings
        {
            get;
            set;
        }
        public List<StakeholderModel> StakeholderFinalComment
        {
            get;
            set;
        }

        public PortsGmsLevel0Dashbord()
        {
            this.AllBookings = new List<PortsGmsLevel0Dashbord>();
            //this.UserDetails = new List<PortsGms_Registration>();
            this.StakeholderFinalComment = new List<StakeholderModel>();
        }
    }

    public class StakeholderModel
    {
        public string FinalComment { set; get; }
        public string UserName { set; get; }
        public string BusinessGroup { set; get; }
        public DateTime? Created_Date
        {
            get;
            set;
        }
    }
}