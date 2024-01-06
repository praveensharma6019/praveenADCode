using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sitecore.Marathon.Website.Controllers
{
    public class MarathonApiController : ApiController
    {
      
        public bool GetRegisterdata(string contactno = null)
        {
            try
            {
                var request = System.Web.HttpContext.Current.Request.Url;

                if (string.IsNullOrEmpty(contactno))
                {
                    return false;
                }
                else 
                {
                    using (AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext())
                    {
                        var RegData = dataContext.AhmedabadMarathonRegistrations.Where(x => x.ContactNumber == contactno).FirstOrDefault();
                        if (RegData != null)
                        {
                            return true;
                        }
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }      
     
    }
}
