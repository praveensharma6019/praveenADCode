using Sitecore.Data;
using System.Collections.Generic;

namespace Sitecore.Australia.Website.Models
{
    public class OurTeams
    {
        public OurTeams()
        {

        }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class OurTeamList
    {
        public OurTeamList()
        {
            ListOurTeam = new List<OurTeams>();
        }
        public List<OurTeams> ListOurTeam { get; set; }
    }
}