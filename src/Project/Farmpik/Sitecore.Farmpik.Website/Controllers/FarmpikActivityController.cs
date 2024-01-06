using Newtonsoft.Json;
using Sitecore.Farmpik.Website.Models;
using Sitecore.Farmpik.Website.Repositories;
using System.Web.Mvc;

namespace Sitecore.Farmpik.Website.Controllers
{
    public class FarmpikActivityController : Controller
    {
        private readonly IFarmpikRepository _farmpikRepository;
        public FarmpikActivityController(IFarmpikRepository farmpikRepository)
        {
            _farmpikRepository = farmpikRepository;
        }

        // GET: FarmpikActivity
        public JsonResult GetRecentActivity(string ItemId)
        {
            var activityModel = _farmpikRepository.GetRecentActivities(ItemId);
            var activityModelresult = JsonConvert.SerializeObject(activityModel);
            return Json(activityModelresult, JsonRequestBehavior.AllowGet);
        }


        //GET: KnowledgeHubTabs
        [Route("v1/api/Farmpik/GetKnowledgeHubTabs")]
        public ContentResult GetKnowledgeHubTabs()
        {
            KnowledgeHubTabsModel knowledgeHubTabsModel = _farmpikRepository.GetKnowledgeHubTabsModel();
            var knowledgeHubTabs = JsonConvert.SerializeObject(knowledgeHubTabsModel);
            return Content(knowledgeHubTabs, "application/json");
        }

    }
}