using Sitecore.Affordable.Website.SalesForce.Domain;
using System;

namespace Sitecore.Affordable.Website.SalesForce
{
    public class SalesForceAuthentication
    {
        public SalesForceAuthentication() { }

        public Project[] AuthenticationApi(string projectname)
        {
            Project[] tasks = null;
            try
            {
                //var client = GetAuthenticatedClientForSalesForce();
                //tasks = BookingHelper.Run(client, projectname);
            }
            catch (Exception e)
            {
                var innerException = e.InnerException;
                while (innerException != null)
                {
                    innerException = innerException.InnerException;
                }
            }
            return tasks;
        }

        //private async Task<Project[]> GetAuthenticatedClientForSalesForce()
        //{
        //    var auth = new AuthenticationClient(new HttpClient());
        //    Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
        //    var itemInfo = dbWeb.GetItem(new Data.ID(Templates.SalesForce.Id.ToString()));

        //    var ConsumerKey = itemInfo.Fields[Templates.SalesForce.Fields.ConsumerKey].Value;
        //    var ConsumerSecret = itemInfo.Fields[Templates.SalesForce.Fields.ConsumerSecret].Value;
        //    var Password = itemInfo.Fields[Templates.SalesForce.Fields.Password].Value;
        //    var Username = itemInfo.Fields[Templates.SalesForce.Fields.Username].Value;
        //    var SecurityToken = itemInfo.Fields[Templates.SalesForce.Fields.SecurityToken].Value;
        //    var IsSandboxUser = itemInfo.Fields[Templates.SalesForce.Fields.IsSandboxUser].Value.ToLower();


        //    var url = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)
        //        ? "https://test.salesforce.com/services/oauth2/token"
        //        : "https://login.salesforce.com/services/oauth2/token";
        //    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    //await auth.UsernamePasswordAsync(ConsumerKey, ConsumerSecret, Username, Password, url).ConfigureAwait(false);
        //    var aResult = auth.UsernamePasswordAsync(ConsumerKey, ConsumerSecret, Username, Password, url);
        //    while (!aResult.IsCompleted)
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //    var forceClient = new ForceClient(auth.InstanceUrl, auth.AccessToken, auth.ApiVersion);

        //    var dataProject = await BookingHelper.SearchByProjectName(forceClient, "a");
        //    string ProjectId = "a0G28000005eQZQEA2";
        //    var dataInventory = await BookingHelper.GetInventory(forceClient, ProjectId);
        //    return dataProject;

        //}
    }
}