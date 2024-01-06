using System;
using System.Threading.Tasks;
using SapPiService.Domain;

namespace SapPiService.Services
{
    public interface IAccountService
    {
        Task<SubscriberType> FetchType(string accountNumber);
        Task<SubscriberDetail> FetchDetail(string accountNumber);
        Task<BillingLanguage> FetchBillingLanguage(string accountNumber);
        Task UpdateBillingLanguage(string accountNumber, BillingLanguage language);
        Task UpdateNotificationSetting(string accountNumber, string mobile, string email, bool? paperless);
        Task<BillingInfo> FetchBilling(string accountNumber);
        Task<decimal> FetchSecurityDepositAmount(string accountNumber);
        Task<VdsDetail> FetchVdsAmount(string accountNumber);
        Task<OutageDetail> FetchOutage(string accountNumber);
        Task<PaymentHistory> FetchPaymentHistory(string accountNumber, DateTime? fromDate, DateTime? toDate);
        //Task<string> GenerateViewBillLink(string accountNumber, int year, int month);
        Task<ConsumptionHistory> FetchConsumptionHistory(string accountNumber);
        Task<InvoiceHistory> FetchInvoiceHistory(string accountNumber);
        Task<PvcDetail> FetchPvcDetail(string accountNumber);
        Task FetchInvoice(string accountNumber, int year, int month);
        Task<ComplainStatus> FetchComplainStatus(string accountNumber, string complainNumber);
        ConsumerDetails FetchConsumerDetails(string accountNumber);
    }
}