using System;
using System.Linq;
using System.Web;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Data;

namespace Sitecore.AGELPortal.Website.Models
{
    public class ConfigWrapper
    {
        

            public string SubscriptionId
            {
                get { return "c2e7914d-a100-468f-b49f-ffbeb4fa32ea"; }
            }

            public string ResourceGroup
            {
                get { return "adanistaging"; }
            }

            public string AccountName
            {
                get { return "mediaservicestage"; }
            }

            public string AadTenantId
            {
                get { return "64d1fc0b-a230-4785-85f8-586ec3b11e3f"; }
            }

            public string AadClientId
            {
                get { return "0e5a248e-f4b2-40f8-8914-006ff976b810"; }
            }

            public string AadSecret
            {
                get { return "LHd7Q~XEnrJ0NSWQ18~BWd3gDixfD5on3UNgA"; }
            }

            public Uri ArmAadAudience
            {
                get { return new Uri("https://management.core.windows.net/"); }
            }

            public Uri AadEndpoint
            {
                get { return new Uri("https://login.microsoftonline.com"); }
            }

            public Uri ArmEndpoint
            {
                get { return new Uri("https://management.azure.com/"); }
            }

            public string EventHubConnectionString
            {
                get { return ""; }
            }

            public string EventHubName
            {
                get { return ""; }
            }

            public string StorageContainerName
            {
                get { return ""; }
            }

            public string StorageAccountName
            {
                get { return ""; }
            }

            public string StorageAccountKey
            {
                get { return ""; }
            }

            public string SymmetricKey
            {
                get { return ""; }
            }

            public string AskHex
            {
                get { return ""; }
            }

            public string FairPlayPfxPath
            {
                get { return ""; }
            }

            public string FairPlayPfxPassword
            {
                get { return ""; }
            }
            public IList<string> AzureURLs { get; set; }
    }
}