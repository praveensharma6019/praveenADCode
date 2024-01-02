using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.C2CCreate.Platform.Models
{
	[JsonObject]
	public class SourceCity
	{
		[JsonProperty("name")]
		public string SCityName { get; set; }

		[JsonProperty("description_text")]
		public string SCityDescription { get; set; }

		[JsonProperty("places_to_visit")]
		public List<PlacesToVisit> SPlacesToVisitList { get; set; }

		[JsonProperty("airport_name")]
		public string SAirportName { get; set; }

		[JsonProperty("airport_text")]
		public string SAirportDescription { get; set; }
	}
}
