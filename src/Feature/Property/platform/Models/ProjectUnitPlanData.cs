using Newtonsoft.Json;
using Sitecore.Mvc.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{

    public class ProjectUnitPlanData
    {
      
        [JsonProperty(PropertyName = "componentID")]
        public string ComponentID { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "projectUnitPlanData")]
        public List<TypicalUnitPlanData> TypicalUnitPlanList { get; set; }
    }

    public class ProjectDetails
    {
        [JsonProperty(PropertyName = "sizeIn")]
        public string SizeIn { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "src")]
        public string Src { get; set; }
        [JsonProperty(PropertyName = "imageAlt")]
        public string ImageAlt { get; set; }
        [JsonProperty(PropertyName = "imageTitle")]
        public string ImageTitle { get; set; }
        [JsonProperty(PropertyName = "tabHeading")]
        public string TabHeading { get; set; }
        [JsonProperty(PropertyName = "specifications")]
        public List<Specification> Specifications { get; set; }
        [JsonProperty(PropertyName = "areaAsPerRera")]
        public string AreaAsPerRera { get; set; }
        [JsonProperty(PropertyName = "reraMeasurementScale")]
        public string ReraMeasurementScale { get; set; }
        [JsonProperty(PropertyName = "reraSpecifications")]
        public List<ReraSpecification> ReraSpecifications { get; set; }
    }

  

    public class ReraSpecification
    {
        [JsonProperty(PropertyName = "areaType")]
        public string AreaType { get; set; }
        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }
    }
    public class Specification
    {

        [JsonProperty(PropertyName = "place")]
        public string Place { get; set; }
        [JsonProperty(PropertyName = "sizeInFeet")]
        public string SizeInFeet { get; set; }
        [JsonProperty(PropertyName = "sizeInMetres")]
        public string SizeInMetres { get; set; }
    }

    public class TypicalUnitPlanData
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "desc")]
        public string Desc { get; set; }
        [JsonProperty(PropertyName = "src")]
        public string Src { get; set; }
        [JsonProperty(PropertyName = "imgAlt")]
        public string imgAlt { get; set; }
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "buttonText")]
        public string ButtonText { get; set; }       
        [JsonProperty(PropertyName = "details")]
        public List<ProjectDetails> ProjectDetails { get; set; }
    }
}