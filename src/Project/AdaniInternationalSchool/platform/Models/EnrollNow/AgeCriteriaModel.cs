using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models.EnrollNow
{
    public class AgeCriteriaModel
    {
        public string Heading { get; set; }
        public AgeCriteria Data { get; set; }
    }
    public class AgeCriteria
    {
        public List<TH> Th { get; set; }
        public List<List<TH>> Td { get; set; }
    }
    public class TH
    {
        public string Label { get; set; }
    }
}