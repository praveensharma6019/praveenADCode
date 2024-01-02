using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services
{
    public class StaticHelper : IStaticHelper
    {

        public string WidgetResponse()
        {
            string response = string.Empty;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Sitecore.Configuration.Settings.GetSetting(Constant.WidgetRequestURL).ToString());
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue(Constant.ContentType));

            var req = new HttpRequestMessage(HttpMethod.Post, Constant.RelativeAddress);
            Sitecore.Data.Items.Item contextItem = Sitecore.Context.Item;
            string airportCode = contextItem["AirportCode"];
            if (airportCode == String.Empty)
            {
                airportCode = "BOM";
            }
            string environment = Sitecore.Configuration.Settings.GetSetting(Constant.EnvironmentWidget).ToString();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1).ToString("ddd, dd MMM");
            // This is the important part:
            req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
               {
               { "data", "{\"searchFlightService\":" +
                "{\"data\":{\"tt\":\"O\",\"ac\":\""+airportCode+"\",\"popup\":\"x\",\"tenant\":\"D\",\"class\":\"ECO\",\"pax\":\"1_0_0\",\"lables\":" +
               "{\"oneWayLabel\":\"One Way\",\"roundTripLabel\":\"Round Trip\",\"fromPlaceholder\":\"From\",\"from\":\"New Delhi (DEL)\"," +
               "\"toPlaceholder\":\"To\",\"to\":\"Mumbai (BOM)\",\"departOnPlaceholder\":\"Depart on\",\"departDate\":\""+tomorrow+"\"," +
               "\"returnOnPlaceholder\":\"Return on\",\"travelAndClassPlaceholder\":\"Travellers &amp; Class\"," +
               "\"searchButtonPlaceholder\":\"Search\"}},\"env\":\""+environment+"\",\"language\":\"en\",\"version\":\"searchFlightService\",\"container\":\"bookYourFlightTicketsWidget\"," +
               "\"rVersion\":{},\"widgetsLoader\":{\"env\":\""+environment+"\"},\"channelId\":\"Web\"}}" }
               });


            HttpResponseMessage resp = Task.Run(() => client.SendAsync(req)).Result;

            if (resp.IsSuccessStatusCode)
            {
                response = resp.Content.ReadAsStringAsync().Result;

            }
            return response;



        }

    }

}