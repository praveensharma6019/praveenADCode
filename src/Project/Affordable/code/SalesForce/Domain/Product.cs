using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sitecore.Affordable.Website.SalesForce.Domain
{
    public class Product
    {
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public string UnitCode { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string FloorCode { get; set; }
        public string FloorDescription { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CarpetArea { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
        }
    }
}