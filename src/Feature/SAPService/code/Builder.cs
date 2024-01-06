//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

using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using Newtonsoft.Json;
using SapPiService.Services;
using SapPiService.Wsdl;

namespace SapPiService
{

    public static class Builder
    {
        private static readonly Lazy<PiConfig> Lazy = new Lazy<PiConfig>(() => JsonConvert.DeserializeObject<PiConfig>(File.ReadAllText("PIConfig.json")));

        private static PiConfig Instance => Lazy.Value;

        public static SI_CHECK_CA_TYPE_inClient BuildClientForAccountType()
        {
            Console.WriteLine($"{Instance.AccountTypeEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.AccountTypeEndPoint);
            var client = new SI_CHECK_CA_TYPE_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_ISUPARTNER_GETDETAIL_inClient BuildClientForAccountDetail()
        {
            Console.WriteLine($"{Lazy.Value.AccountDetailEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.AccountDetailEndPoint);
            var client = new SI_ISUPARTNER_GETDETAIL_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_PAYMT_HISTORY_inClient BuildClientForPaymentHistory()
        {
            Console.WriteLine($"{Lazy.Value.PaymentHistoryServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.PaymentHistoryServiceEndPoint);
            var client = new SI_PAYMT_HISTORY_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_ONLINE_BILL_PDF_inClient BuildClientForViewBill()
        {
            Console.WriteLine($"{Lazy.Value.ViewBillServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.ViewBillServiceEndPoint);
            var client = new SI_ONLINE_BILL_PDF_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_MTRREADDOC_GETLIST_inClient BuildClientForConsumptionHistory()
        {
            Console.WriteLine($"{Lazy.Value.ConsumptionHistoryServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.ConsumptionHistoryServiceEndPoint);
            var client = new SI_MTRREADDOC_GETLIST_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_COMP_STATUS_inClient BuildClientForComplainStatus()
        {
            Console.WriteLine($"{Lazy.Value.ComplainStatusServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.ComplainStatusServiceEndPoint);
            var client = new SI_COMP_STATUS_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_GET_BILL_MONTH_inClient BuildClientForInvoiceHistory()
        {
            Console.WriteLine($"{Lazy.Value.InvoiceHistoryServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.InvoiceHistoryServiceEndPoint);
            var client = new SI_GET_BILL_MONTH_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_Cms_Isu_Ca_Display_inClient BuildClientForConsumerDetails()
        {
            Console.WriteLine($"{Lazy.Value.InvoiceHistoryServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.ConsumerDetailsServiceEndPoint);
            var client = new SI_Cms_Isu_Ca_Display_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_ZCSBLNG_inClient BuildClientForBillingLanguage()
        {
            Console.WriteLine($"{Lazy.Value.BillingLanguageServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.BillingLanguageServiceEndPoint);
            var client = new SI_ZCSBLNG_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_EBILL_REGISTER_inClient BuildClientForNotificationSetting()
        {
            Console.WriteLine($"{Lazy.Value.EBillRegistrationServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.EBillRegistrationServiceEndPoint);
            var client = new SI_EBILL_REGISTER_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_MR_BI_GETLIST_inClient BuildClientForAccountDue()
        {
            Console.WriteLine($"{Lazy.Value.AccountDueServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.AccountDueServiceEndPoint);
            var client = new SI_MR_BI_GETLIST_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_PVC_DISPLAY_inClient BuildClientForPremiumAccountDetail()
        {
            Console.WriteLine($"{Lazy.Value.PremiumAccountDetailServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.PremiumAccountDetailServiceEndPoint);
            var client = new SI_PVC_DISPLAY_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_VDS_WEB_PYMT_inClient BuildClientForNVdsDetail()
        {
            Console.WriteLine($"{Lazy.Value.VdsServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.VdsServiceEndPoint);
            var client = new SI_VDS_WEB_PYMT_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static ZBAPI_VDS_REGISTRATIONClient BuildClientForVdsRegistration()
        {
            Console.WriteLine($"{Lazy.Value.VdsServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.VdsServiceEndPoint);
            var client = new ZBAPI_VDS_REGISTRATIONClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_SD_inClient BuildClientForSecurityDepositDetail()
        {
            Console.WriteLine($"{Lazy.Value.SecurityDepositServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.SecurityDepositServiceEndPoint);
            var client = new SI_SD_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static SI_OUTAGE_INFO_inClient BuildClientForOutageInformation()
        {
            Console.WriteLine($"{Lazy.Value.OutageInfoServiceEndPoint}");
            var remoteAddress = new EndpointAddress(Lazy.Value.OutageInfoServiceEndPoint);
            var client = new SI_OUTAGE_INFO_inClient(ConstructBinder(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            return client;
        }

        public static string GetStatusMessageFromCode(string code)
        {
            return Lazy.Value.ComplainStatuses[code];
        }

        private static void SetClientCredentials(ClientCredentials clientClientCredentials)
        {
            clientClientCredentials.UserName.UserName = Instance.SapPiCredential.Username;
            clientClientCredentials.UserName.Password = Instance.SapPiCredential.Password;
        }

        private static BasicHttpBinding ConstructBinder()
        {
            return new BasicHttpBinding
            {
                MaxBufferSize = int.MaxValue,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true,
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.Transport,
                    Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.Basic }
                }
            };

            /*
                        var binding = new BasicHttpBinding
                        {
                            MaxBufferSize = int.MaxValue,
                            ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                            MaxReceivedMessageSize = int.MaxValue,
                            AllowCookies = true
                        };
            */



            //binding.Security.Mode = BasicHttpSecurityMode.Transport;
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        }
    }

}
