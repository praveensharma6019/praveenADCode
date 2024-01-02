using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Feature.Avatar.Models;
using System.Web.Http;
using System;

namespace Adani.SuperApp.Airport.Feature.Avatar.Controlles
{
    public class ProfileAvatarController : ApiController
    {
        private ILogRepository logRepository;
        private Services.IProfileAvatar avatar;

        public ProfileAvatarController(ILogRepository _logRepository, Services.IProfileAvatar _avatar)
        {
            this.logRepository = _logRepository;
            avatar = _avatar;
        }

        [HttpGet]
        [Route("api/GetAvatars")]
        public IHttpActionResult GetAvatars()
        {
            AvatarResultData responseData = new AvatarResultData();
            ResultData resultData = new ResultData();
            try
            {
                resultData.result = avatar.GetAvatarList();
                if (resultData.result != null)
                {
                    resultData.count = resultData.result.Count;
                    responseData.status = true;
                    responseData.data = resultData;
                }
            }
            catch(Exception ex)
            {
                logRepository.Error("GetAvatars in ProfileAvatarController gives error -> " + ex.Message);
            }

            return Json(responseData);
        }
    }
}