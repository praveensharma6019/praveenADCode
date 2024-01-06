using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Affordable.Website.SalesForce.Domain;
using Salesforce.Force;

namespace Sitecore.Affordable.Website.SalesForce
{
    public static class AccountHelper
    {
        public static async Task RunSample(IForceClient client)
        {
            // retrieve all accounts
            Console.WriteLine("Get Accounts");

            const string qry = "SELECT ID, LastName FROM Account";
            var accts = new List<Account>();
            var results = await client.QueryAsync<Account>(qry);
            var totalSize = results.TotalSize;

            Console.WriteLine("Queried " + totalSize + " records.");

            accts.AddRange(results.Records);
            var nextRecordsUrl = results.NextRecordsUrl;

            if (!string.IsNullOrEmpty(nextRecordsUrl))
            {
                Console.WriteLine("Found nextRecordsUrl.");

                while (true)
                {
                    var continuationResults = await client.QueryContinuationAsync<Account>(nextRecordsUrl);
                    totalSize = continuationResults.TotalSize;
                    Console.WriteLine("Queried an additional " + totalSize + " records.");

                    accts.AddRange(continuationResults.Records);
                    if (string.IsNullOrEmpty(continuationResults.NextRecordsUrl)) break;

                    //pass nextRecordsUrl back to client.QueryAsync to request next set of records
                    nextRecordsUrl = continuationResults.NextRecordsUrl;
                }
            }

            Console.WriteLine("Retrieved accounts = " + accts.Count + ", expected size = " + totalSize);

            // Create a sample record

            Console.WriteLine("Creating test record.");
            var accountRecord = new Account { LastName = "TestAccount" };

            var createdAccRecord = await client.CreateAsync("Account", accountRecord);
            Console.WriteLine("await Id return:" + " " + createdAccRecord.Id);

            // Update the sample record
            // Shows that anonymous types can be used as well
            Console.WriteLine("Updating test record.");
            accountRecord.LastName = "TestUpdate";
            var success = await client.UpdateAsync("Account", createdAccRecord.Id, accountRecord);
            if (!string.IsNullOrEmpty(success.Errors.ToString()))
            {
                Console.WriteLine("Failed to update test record!");
                return;
            }

            Console.WriteLine("Successfully updated the record.");

            // Retrieve the sample record
            // How to retrieve a single record if the id is known
            Console.WriteLine("Retrieving the record by ID.");
            accountRecord = await client.QueryByIdAsync<Account>("Account", createdAccRecord.Id);
            if (accountRecord == null)
            {
                Console.WriteLine("Failed to retrieve the record by ID!");
                return;
            }

            Console.WriteLine("Retrieved the record by ID.");

            // Query for record by LastName
            Console.WriteLine("Querying the record by LastName.");
            var accounts = await client.QueryAsync<Account>("SELECT ID, LastName FROM Account WHERE LastName = '" + accountRecord.LastName + "'");
            accountRecord = accounts.Records.FirstOrDefault();
            if (accountRecord == null)
            {
                Console.WriteLine("Failed to retrieve account by query!");
                return;
            }

            Console.WriteLine("Retrieved the record by LastName.");

            // Delete account
            Console.WriteLine("Deleting the record by ID.");
            var deleted = await client.DeleteAsync("Account", createdAccRecord.Id);
            if (!deleted)
            {
                Console.WriteLine("Failed to delete the record by ID!");
                return;
            }
            Console.WriteLine("Deleted the record by ID.");

            // Selecting multiple accounts into a dynamic
            Console.WriteLine("Querying multiple records.");
            var dynamicAccounts = await client.QueryAsync<dynamic>("SELECT ID, LastName FROM Account LIMIT 10");
            foreach (dynamic acct in dynamicAccounts.Records)
            {
                Console.WriteLine("Account - " + acct.LastName);
            }

            // Creating parent - child records using a Dynamic
            Console.WriteLine("Creating a parent record (Account)");
            dynamic a = new ExpandoObject();
            a.Name = "Account from .Net Toolkit";
            var acc = await client.CreateAsync("Account", a);
            if (acc == null)
            {
                Console.WriteLine("Failed to create parent record.");
                return;
            }

            Console.WriteLine("Creating a child record (Contact)");
            dynamic c = new ExpandoObject();
            c.FirstName = "Joe";
            c.LastName = "Blow";
            c.AccountId = acc.Id;
            var con = await client.CreateAsync("Contact", c);
            if (con.Id == null)
            {
                Console.WriteLine("Failed to create child record.");
                return;
            }

            Console.WriteLine("Deleting parent and child");

            // Delete account (also deletes contact)
            Console.WriteLine("Deleting the Account by Id.");
            deleted = await client.DeleteAsync("Account", acc.Id);
            if (!deleted)
            {
                Console.WriteLine("Failed to delete the record by ID!");
                return;
            }
            Console.WriteLine("Deleted the Account and Contact.");
        }
    }
}
