using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.C2CCreate.Platform.Models
{
	[JsonObject]
	public class Faqs
	{
		[JsonProperty("question")]
		public string Question { get; set; }

		[JsonProperty("answer")]
		public string Answer { get; set; }
	}
}
