using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.CostCalculatorClientAPI
{
    public class ClientAPIDropdownModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<DropdownOptions> dropdownOptions { get; set; }
    }
    public class DropdownOptions
    {
        [SitecoreField]
        public virtual string placeholder { get; set; }
        [SitecoreField]
        public virtual string ErrorMessage { get; set; }
        [SitecoreField]
        public virtual string type { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ItemsOptions> options { get; set; }
    }
    public class ItemsOptions
    {
        [SitecoreField]
        public virtual string label { get; set; }
        [SitecoreField]
        public virtual string Id { get; set; }
    }
}