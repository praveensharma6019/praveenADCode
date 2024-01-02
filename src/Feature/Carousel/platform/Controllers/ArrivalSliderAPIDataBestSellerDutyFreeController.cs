using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Controllers
{
    public class ArrivalSliderAPIDataBestSellerDutyFreeController : Controller
    {
        // GET: DutyFree
        Item datasourc = RenderingContext.Current.Rendering.Item;
        string Baseurl = Sitecore.Configuration.Settings.GetSetting("GetDutyFreeProducts").ToString();

        public ActionResult GetDutyFreeProducts()
        {
            //List<Result> bestSellerDutyFrees = new List<Result>();
            if (datasourc == null || String.IsNullOrEmpty(Baseurl))
            {
                return null;
            }
            BestSellerRequest request = new BestSellerRequest
            {
                skuCode = datasourc.Fields["SKUCode"].Value.Split(','),//new string[] { datasourc.Fields["SKUCode"].Value },
                storeType = "arrival"
            };
            using (var client = new HttpClient())
            {
                var model = new Root();
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                var myContent = JsonConvert.SerializeObject(request);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = Task.Run(() => client.PostAsync("api/GetDutyFreeProducts", byteContent)).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    // bestSellerDutyFrees = JsonConvert.DeserializeObject<List<Result>>(ProductResponse);
                    model = JsonConvert.DeserializeObject<Root>(ProductResponse);

                    //var message = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(ProductResponse);
                }
                //returning the employee list to view
                return View("~/Views/Airport/ArrivalSliderAPIDataBestSellerDutyFree.cshtml", model);
            }
        }


    }
}