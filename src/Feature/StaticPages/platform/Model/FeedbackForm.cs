using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class FeedbackForm
    {
        
        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        [Required]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "airport")]
        [Required]
        public string AirportName { get; set; }

        [JsonProperty(PropertyName = "type")]
        [Required]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "description")]
        [Required]
        [MaxLength(300,ErrorMessage = "Maximum {1} characters allowed")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "flightDetails")]
        public FlightDetail FlightDetail { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{10})$",ErrorMessage ="Please enter valid Mobile number")]
        public string  MobileNumber { get; set; }

        public bool IsAgree { get; set; }
    }

    public class FlightDetail
    {
        [JsonProperty(PropertyName = "flightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty(PropertyName = "flightDate")]
        public string FlightDate { get; set; }
    }
}