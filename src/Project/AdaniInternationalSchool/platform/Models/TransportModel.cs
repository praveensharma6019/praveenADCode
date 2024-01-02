using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class TransportModel
    {
        public TransportModel()
        {
            SideNav = new List<SideNavItemModel>();
        }

        public List<SideNavItemModel> SideNav { get; set;}
        public TransportRoutes RouteDetails  { get; set;}
    }

    public class TransportRoutes
    {
        public string Heading { get; set; }
        public string Details { get; set; }
        public List<TransportRouteDetails> Routes { get; set; }
    }
}