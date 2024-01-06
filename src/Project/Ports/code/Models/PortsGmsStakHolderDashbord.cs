using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGmsStakHolderDashbord
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
        public string Department
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
        public string Level0Comment
        {
            get;
            set;
        }
        public string Level1Comment
        {
            get;
            set;
        }
        public string Level2Comment
        {
            get;
            set;
        }
        public string Level3Comment
        {
            get;
            set;
        }
        public string Assignedlevel
        {
            get;
            set;
        }
        public string UserType
        {
            get;
            set;
        }
        public string Comment { set; get; }
        public string StakeholderRemarks { set; get; }
        public string FinalComment { set; get; }
        public string FromDate { set; get; }
        public string ToDate { set; get; }
        public string Status { set; get; }
        public bool ChkResponse { set; get; }
        public string BookingStatus { set; get; }
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

        public List<PortsGmsStakHolderDashbord> AllBookings
        {
            get;
            set;
        }
        public List<StakeholderModel> StakeholderFinalComment
        {
            get;
            set;
        }

        public PortsGmsStakHolderDashbord()
        {
            this.AllBookings = new List<PortsGmsStakHolderDashbord>();
            //this.UserDetails = new List<PortsGms_Registration>();
            this.StakeholderFinalComment = new List<StakeholderModel>();
            
        }
        public string encId { get; set; }

        
    }
}