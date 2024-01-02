using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.DealerResult
{
    public class DealerResultModel
    {
        public virtual DealerResult dealersDetails { get; set; }
    }
    public class DealerResult
    {
        public virtual DealerLabels labels { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<DetailsResult> details { get; set; }
    }
    public class DealerLabels
    {
        public virtual string nameLabel { get; set; }
        public virtual string mobileNoLabel { get; set; }
        public virtual string pincodeLabel { get; set; }
        public virtual string dealersNearbyLabel { get; set; }
        public virtual string resultsLabel { get; set; }
        public virtual string buttonLabel { get; set; }
        public virtual string overlayHeading { get; set; }
    }
    public class DetailsResult
    {
        [SitecoreField]
        public virtual string icon { get; set; }
        [SitecoreField]
        public virtual string imageAlt { get; set; }
        [SitecoreField]
        public virtual string name { get; set; }
        [SitecoreField]
        public virtual string contact { get; set; }
        [SitecoreField]
        public virtual string pincode { get; set; }
        [SitecoreField]
        public virtual string organisation { get; set; }
        [SitecoreField]
        public virtual string stateId { get; set; }
        [SitecoreField]
        public virtual string state { get; set; }
        [SitecoreField]
        public virtual string cityId { get; set; }
        [SitecoreField]
        public virtual string city { get; set; }
        [SitecoreField]
        public virtual string areaId { get; set; }
        [SitecoreField]
        public virtual string area { get; set; }
    }
}