using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.C2CCreate.Platform.Models
{
	[JsonObject]
	public class DestinationCity
	{
		[JsonProperty("name")]
		public string DCityName { get; set; }

		[JsonProperty("description_text")]
		public string DCityDescription { get; set; }

		[JsonProperty("places_to_visit")]
		public List<PlacesToVisit> DPlacesToVisitList { get; set; }

		[JsonProperty("airport_name")]
		public string DAirportName { get; set; }

		[JsonProperty("airport_text")]
		public string DAirportDescription { get; set; }
	}
}
