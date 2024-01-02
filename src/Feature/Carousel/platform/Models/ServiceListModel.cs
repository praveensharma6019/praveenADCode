using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class ServiceListModel
    {
        public string tabTitle { get; set; }

        public List<Services> servicesList { get; set; }

        public ServiceListModel()
        {
            servicesList = new List<Services>();
        }
    }

    public class Services
    {
        public string title { get; set; }

        public string serviceId { get; set; }

        public string serviceUrl { get; set; }
    }

}