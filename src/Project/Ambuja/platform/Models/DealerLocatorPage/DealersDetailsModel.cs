using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.DealerLocatorPage
{
    public class DealersDetailsModel
    {
        public DealersLabelsModel labels { get; set; }
        public List<DealerDetails> details { get; set; }
    }
}