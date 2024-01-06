using Salesforce.Force;
using Sitecore.Affordable.Website.SalesForce.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sitecore.Affordable.Website.SalesForce
{

    public static class BookingHelper
    {
        public static async Task<Project> GetProjectById(IForceClient client, string projectId)
        {
            return await client.QueryByIdAsync<Project>(SObjectNames.Project, projectId);
        }

        /// <summary>
        /// Get list of projects by name
        /// </summary>
        /// <param name="client"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static async Task<Project[]> SearchByProjectName(IForceClient client, string projectName)
        {
            var qry = $"SELECT ID from Project__c where Name LIKE '{projectName}%' LIMIT 1";
            var results = await client.QueryAsync<Project>(qry);
            var totalSize = results.TotalSize;

            Console.WriteLine($"Found count:{totalSize}");

            var projects = new List<Project>();

            foreach (var record in results.Records)
            {
                projects.Add(await GetProjectById(client, record.Id));
            }

            return projects.ToArray();
        }

        public static async Task<Product[]> GetInventory(IForceClient client, string projectId)
        {
            var qry = "SELECT Id, Name, Unit_Status__c, Unit_Status_Code__c, Building__c, Floor_Code__c, " +
                      "Floor_Description__c, Building__r.Name, Project__c, Project_Name__c, " +
                      "Project_Product__c, Token_Amount__c, Total_Amount__c, Total_Carpet_Area__c " +
                      $"FROM Product2 where Project__c = '{projectId}' and Unit_Status__C = 'Available' and Floor_Code__C > 0";

            var results = await client.QueryAsync<dynamic>(qry);
            var totalSize = results.TotalSize;

            Console.WriteLine($"Found count:{totalSize}");

            var products = new List<Product>();

            foreach (var unit in results.Records)
            {
                var product = new Product
                {
                    UnitId = unit.Id,
                    UnitName = unit.Name,
                    UnitCode = unit.Project_Product__c,
                    ProjectId = unit.Project__c,
                    ProjectName = unit.Project_Name__c,
                    BuildingId = unit.Building__c,
                    BuildingName = unit.Building__r.Name,
                    FloorCode = unit.Floor_Code__c,
                    FloorDescription = unit.Floor_Description__c,
                    TokenAmount = unit.Token_Amount__c != null ? unit.Token_Amount__c : 0,
                    TotalAmount = unit.Total_Amount__c != null ? unit.Total_Amount__c : 0,
                    CarpetArea = unit.Carpet_Area__c != null ? unit.Carpet_Area__c : 0,
                    Status = unit.Unit_Status__c,
                    StatusCode = unit.Unit_Status_Code__c != null ? unit.Unit_Status_Code__c : ""
                };

                products.Add(product);
            }

            return products.ToArray();
        }


        //Need to integrate this ID in Sitecore.
        private const string ProjectId = "a0G28000005eQZQEA2";

        public static async Task Run(IForceClient client)
        {
            Console.WriteLine("Searching project by Name: Aangan");
            var projects = await SearchByProjectName(client, "a");

            foreach (var project in projects)
            {
                Console.WriteLine(project.ToString());
            }

            if (projects.Length == 0)
            {
                Console.WriteLine("No records found");
            }

            var units = await GetInventory(client, ProjectId);
            foreach (var product in units)
            {
                Console.WriteLine(product.ToString());
            }
        }
    }
}
