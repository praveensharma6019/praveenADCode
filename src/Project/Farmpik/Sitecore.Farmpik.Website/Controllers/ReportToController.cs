using Newtonsoft.Json;
using Sitecore.Farmpik.Website.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Sitecore.Farmpik.Website.Controllers
{
    public class ReportToController : Controller
    {
        [HttpPost]
        [ActionName("csp-endpoint")]
        [Route("api/ReportToDetail/csp-endpoint")]
        public ActionResult ReportToDetail(FormCollection payload)
        {
            try
            {
                var path = HostingEnvironment.MapPath(@"~/App_Data/FarmPikCSPReport/Reporting.txt");
                var numberOfLines = System.IO.File.ReadLines(path).Count();
                var bodyStream = new StreamReader(HttpContext.Request.InputStream);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = bodyStream.ReadToEnd();

                var jsonString = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(bodyText));
                if (bodyText.Contains("csp-report"))
                {
                    var textFileData = jsonString + "," + "Request No:" + numberOfLines + "," + DateTime.Now + Environment.NewLine;
                    if (textFileData != null)
                    {
                        System.IO.File.AppendAllText(path, textFileData);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new
            {
                error = new Object[]
                {
                        new { code = 214, message="Bad request"}
                }
            });
        }
    }
}
