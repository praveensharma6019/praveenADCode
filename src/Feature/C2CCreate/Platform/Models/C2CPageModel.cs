using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.C2CCreate.Platform.Models
{
	[JsonObject]
	public class C2CPageModel
	{
		[JsonProperty("assignment_title")]
		public string PageTitle { get; set; }

		[JsonProperty("article_intro_text")]
		public string PageDescription { get; set; }

		//[JsonProperty("company_id")]
		//public string CompanyId { get; set; }

		[JsonProperty("destination_city")]
		public DestinationCity DestinationCityObject { get; set; }

		[JsonProperty("source_city")]
		public SourceCity SourceCityObject { get; set; }

		[JsonProperty("faqs")]
		public List<Faqs> Faqs { get; set; }
	}
}
