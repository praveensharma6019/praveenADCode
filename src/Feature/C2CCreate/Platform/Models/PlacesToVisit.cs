using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.C2CCreate.Platform.Models
{
	[JsonObject]
	public class PlacesToVisit
	{
		[JsonProperty("name")]
		public string PlaceName { get; set; }

		[JsonProperty("description_text")]
		public string PlaceDescription { get; set; }
	}
}
