using Salesforce.Common.Models.Json;
using Salesforce.Force;
using Sitecore.Affordable.Website.SalesForce.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sitecore.Affordable.Website.SalesForce
{
    public static class LeadHelper
    {
        /// <summary>
        /// Usage: await UpdateAgent(client, lead.Id, "Agent 3");
        /// </summary>
        /// <param name="client"></param>
        /// <param name="recordId"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        //private static async Task<SuccessResponse> UpdateAgent(IForceClient client, string recordId, string agent)
        //{
        //    var lead = new Domain.Lead
        //    {
        //        Presales_Agent__c = agent,
        //        Projects_Interested__c = "Elysium;Aangan"
        //    };

        //    return await client.UpdateAsync(SObjectNames.Lead, recordId, lead);
        //}

        ///// <summary>
        ///// Usage: await UpdateLeadStatus(client, lead.Id, "Qualified");
        ///// </summary>
        ///// <param name="client"></param>
        ///// <param name="recordId"></param>
        ///// <param name="status">Open, Contacted, Qualified</param>
        ///// <returns></returns>
        //private static async Task<SuccessResponse> UpdateLeadStatus(IForceClient client, string recordId, string status)
        //{
        //    var lead = new Domain.Lead
        //    {
        //        Status = status
        //    };

        //    return await client.UpdateAsync(SObjectNames.Lead, recordId, lead);
        //}

        //private static async Task<SuccessResponse> ChangeLeadOwner(IForceClient client, string recordId, string ownerName)
        //{
        //    var qry = $"SELECT ID from User where Name = '{ownerName}' LIMIT 1";
        //    var results = await client.QueryAsync<Domain.User>(qry);

        //    if(results.TotalSize != 1) return new SuccessResponse
        //    {
        //        Success = false
        //    };

        //    //"Rajeev Chawla"

        //    var lead = new Domain.Lead
        //    {
        //        OwnerId = results.Records[0].Id
        //    };

        //    return await client.UpdateAsync(SObjectNames.Lead, recordId, lead);
        //}

        //public static async Task<Domain.Lead[]> SearchByPhone(IForceClient client, string phoneNumber)
        //{
        //    var qry = $"SELECT ID from Lead where MobilePhone = '{phoneNumber}' LIMIT 1";
        //    var results = await client.QueryAsync<Domain.Lead>(qry);
        //    var totalSize = results.TotalSize;

        //    Console.WriteLine($"Found lead count:{totalSize}");

        //    var leads = new List<Domain.Lead>();

        //    foreach (var record in results.Records)
        //    {
        //        leads.Add(await client.QueryByIdAsync<Domain.Lead>(SObjectNames.Lead, record.Id));
        //    }

        //    return leads.ToArray();
        //}

        //private static async Task<bool> Sample(IForceClient client)
        //{
        //    // retrieve all accounts
        //    Console.WriteLine("Get Leads");

        //    const string qry = "SELECT ID from Lead where MobilePhone = '2222222222' LIMIT 1";
        //    var results = await client.QueryAsync<Domain.Lead>(qry);
        //    var totalSize = results.TotalSize;

        //    if (totalSize == 1)
        //    {
        //        var record = await client.QueryByIdAsync<Domain.Lead>(SObjectNames.Lead, results.Records[0].Id);
        //        record.Id = null;
        //        record.MobilePhone = "2222222221";
        //        await client.UpdateAsync(SObjectNames.Lead, results.Records[0].Id, record);
        //    }

        //    Console.WriteLine("Queried " + totalSize + " records.");
        //    return true;
        //}

        //private static async Task<bool> CreateNewLead(IForceClient client, string firstName, string lastName, string mobile, string email)
        //{
        //    var lead = new Lead
        //    {
        //        FirstName = firstName,
        //        LastName = lastName,
        //        Email = email,
        //        MobilePhone = mobile
        //    };

        //    var response = await client.CreateAsync(SObjectNames.Lead, lead);
        //    return response.Success;
        //}

        //private static async Task Run(ForceClient client)
        //{
        //    string mobile;
        //    Console.WriteLine("Please enter mobile number");
        //    while ((mobile = Console.ReadLine()) != "")
        //    {
        //        Console.WriteLine("Please enter mobile number");
        //        Console.WriteLine($"Searching lead by Mobile:{mobile}");
        //        var leads = await LeadHelper.SearchByPhone(client, mobile);

        //        foreach (var lead in leads)
        //        {
        //            Console.WriteLine(lead.ToString());
        //        }

        //        if (leads.Length == 0)
        //        {
        //            Console.WriteLine("No records found");
        //        }
        //    }
        //}

        /*var response = await UpdateAgent(client, lead.Id, "Agent 4");

            //var response = await ChangeLeadOwner(client, lead.Id, "Rohit Gopalani");
            //var response = await UpdateLeadStatus(client, lead.Id, "Contacted");
            Console.WriteLine(response.Success);
            if (!response.Success)
            {
                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented, new StringEnumConverter()));
            }*/

        //CreateNewLead(client, "Sunny", "Rajwadi", "9374021723", "sunnyrajwadi@gmail.com").Wait();


    }
}
