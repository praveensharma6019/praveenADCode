using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGmsManageUsers
    {

        //public List<PortsGms_Registration> AllGMSUsers
        //{
        //    get;
        //    set;
        //}


        //public PortsGmsManageUsers()
        //{
        //    this.AllGMSUsers = new List<PortsGms_Registration>();
        //}
        public string Name
        {
            get;
            set;
        }
        public string Location { set; get; }
        public Guid Id { set; get; }
        public string User_Type { set; get; }
        public string Department
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
        public List<PortsGmsManageUsers> AllGMSUsers
        {
            get;
            set;
        }


        public PortsGmsManageUsers()
        {
            this.AllGMSUsers = new List<PortsGmsManageUsers>();

        }

    }
}