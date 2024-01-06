using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sitecore.Affordable.Website.SalesForce.Domain
{
    public abstract class SObject
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
        }
    }
}