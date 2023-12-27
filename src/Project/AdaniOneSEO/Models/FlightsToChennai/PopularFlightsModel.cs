using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models.FlightsToDestination
{
    public class PopularFlightsModel
    {
        public List<PopularFlightsList> PopularFlights { get; set; }
    }
    public class PopularFlightsList
    {
        public  string Heading { get; set; }

        public List<PopularFlightsItemsList> PopularFlightsItems { get; set; }
    }
    public class PopularFlightsItemsList
    {
       public PopularFlightsItemsListComponent Link { get; set; }
    }
    public class PopularFlightsItemsListComponent
    {
        public string Url { get; set; }
        public string Text { get; set; }
        public string Target { get; set; }
    }

}