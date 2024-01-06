using System.Collections.Generic;

namespace SapPiService
{
    public class PiConfig
    {
        public string EndPointTemplateUrl { get; set; }

        public SapPiCredential SapPiCredential { get; set; }

        public string AccountTypeServiceName { get; set; }
        public string AccountTypeEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", AccountTypeServiceName);

        public string AccountDetailServiceName { get; set; }
        public string AccountDetailEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", AccountDetailServiceName);

        public string AccountDueServiceName { get; set; }
        public string AccountDueServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", AccountDueServiceName);

        public string BillingLanguageServiceName { get; set; }
        public string BillingLanguageServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", BillingLanguageServiceName);

        public string EBillRegistrationServiceName { get; set; }
        public string EBillRegistrationServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", EBillRegistrationServiceName);

        public string SecurityDepositServiceName { get; set; }
        public string SecurityDepositServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", SecurityDepositServiceName);

        public string VdsServiceName { get; set; }
        public string VdsServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", VdsServiceName);

        public string ConsumptionHistoryServiceName { get; set; }
        public string ConsumptionHistoryServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", ConsumptionHistoryServiceName);

        public string ComplainStatusServiceName { get; set; }
        public string ComplainStatusServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", ComplainStatusServiceName);

        public string PremiumAccountDetailServiceName { get; set; }
        public string PremiumAccountDetailServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", PremiumAccountDetailServiceName);

        public string OutageInfoServiceName { get; set; }
        public string OutageInfoServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", OutageInfoServiceName);

        public string PaymentHistoryServiceName { get; set; }
        public string PaymentHistoryServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", PaymentHistoryServiceName);

        public string InvoiceHistoryServiceName { get; set; }
        public string InvoiceHistoryServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", InvoiceHistoryServiceName);

        public string ConsumerDetailsServiceName { get; set; }
        public string ConsumerDetailsServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", ConsumerDetailsServiceName);

        public string ViewBillServiceName { get; set; }
        public string ViewBillServiceEndPoint => EndPointTemplateUrl.Replace("{SERVICE}", ViewBillServiceName);

        public Dictionary<string, string> ComplainStatuses { get; set; }
    }

    public class SapPiCredential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
