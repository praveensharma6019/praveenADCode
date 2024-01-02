using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.CostCalculatorClientAPI
{
    public class CostCalculatorClientAPIModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<MaterialInfo> materialInfo { get; set; }
    }
    public class MaterialInfo
    {
        [SitecoreField("Filter")]
        public virtual IEnumerable<FilterData> filter { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<ItemsData> data { get; set; }
    }
    public class ItemsData
    {
        [SitecoreField]
        public virtual string Type { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<TypeData> Data { get; set; }
    }

    public class TypeData
    {
        [SitecoreField]
        public virtual string Type { get; set; }
        [SitecoreField]
        public virtual string Label { get; set; }
        [SitecoreField]
        public virtual string Icon { get; set; }
        [SitecoreField]
        public virtual string Qty { get; set; }
        [SitecoreField]
        public virtual int CalculatorNum { get; set; }
    }
    public class FilterData
    {
        [SitecoreField("id")]
        public virtual string id { get; set; }
    }
}