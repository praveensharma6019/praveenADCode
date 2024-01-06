using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsLogisticContainerTracking
    {
        public string cont_no { set; get; }
        public string cont_size { set; get; }
        public string train_no_vehicle_no { set; get; }
        public string origin { set; get; }
        public string destination { set; get; }
        public string current_status { set; get; }
        public string movement_type { set; get; }
        public string last_updated_time { set; get; }
        public string current_location { set; get; }
        public string Longitude { set; get; }
        public string Latitude { set; get; }
        public string landmark { set; get; }
        public string address { set; get; }
        public string city { set; get; }
    }
    [Serializable]
    public class PortsLogisticsResult
    {
        public PortsLogisticsResult()
        {
            LogsticsResultList = new List<PortsLogisticContainerTracking>();
        }
        public List<PortsLogisticContainerTracking> LogsticsResultList { set; get; }
    }
}