using Sitecore.Feature.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Feature.Accounts.Services
{
    public interface IPaymentService
    {
        string GetNameTransferRequestNumber(string customerId, string orderId);
        void StorePaymentRequest(ViewPayBill model);
        void StorePaymentRequestRevamp(ViewPayBill model);
        void StorePaymentRequestSolarApplication(SolarApplicationDetail model);
        void StorePaymentRequestAdaniGas(PayOnline model);
        void StorePaymentRequestAfterSalesAdaniGas(afterSalesServices model);
        void StorePaymentRequestENachRegistration(ENachRegistrationModel model);
        void StorePaymentResponse(ViewPayBill model);
        void StorePaymentResponseAdaniGas(PayOnline model);
        void StoreENachRequestAdaniGas(AdaniGasENachRegistrationModel model);
        void StoreENachResponseAdaniGas(AdaniGasENachRegistrationModel model);

        void UpdateDODOPaymentStatus(PayOnline model);
        string BillDeskENACHRequestAPIRequestPost_AdaniGas(AdaniGasENachRegistrationModel Model);
        string PaytmTransactionRequestAPIRequestPost(ViewPayBill Model);
        string PaytmTransactionRequestAPIRequestAdaniGasPost(PayOnline Model);

        string PaytmTransactionStatusAPIRequestPost(IDictionary<string, string> TransactionRequestAPIResponse);
        string PaytmTransactionStatusAPIRequestPostAdaniGas(IDictionary<string, string> TransactionRequestAPIResponse);
        string PayUMoneyTransactionRequestAPIRequestPost(ViewPayBill Model);
        string PayUMoneyTransactionRequestAPIAdaniGasRequestPost(PayOnline Model);

        string EbixcashTransactionRequestAPIRequestPost(ViewPayBill Model);

        string ICICITransactionRequestAPIRequestPost(ViewPayBill Model);

        string BENOWTransactionRequestAPIRequestGET(ViewPayBill Model);
        string DBSUPITransactionRequestAPIRequestGET(ViewPayBill Model);
        string CityUPITransactionRequestAPIRequestGET(ViewPayBill Model);
        
        string EbixTransactionStatusAPIRequestPost(IDictionary<string, string> TransactionRequestAPIResponse);

        string BillDeskTransactionRequestAPIRequestPost(ViewPayBill Model);
        string BillDeskTransactionRequestAPIRequestPost_SolarApplication(SolarApplicationDetail Model);
        string BillDeskTransactionRequestAPIRequestAdaniGasPost(PayOnline Model);

        string HDFCTransactionRequestAPIRequestPost(ViewPayBill Model);
        string HDFCTransactionRequestAPIRequestAdaniGasPost(PayOnline Model);
        void HDFCTransactionStatusAPIAdaniGasPost(PayOnline Model);
        string BillDeskSDTransactionRequestAPIRequestPost(ViewPayBill Model);
        string BillDeskVDSTransactionRequestAPIRequestPost(ViewPayBill Model); 
        string GenerateHashforPayU(ViewPayBill model, string hashseq, string marchentKey, string salt);

        string Generatehash512(string text);

        string GetHMACSHA256(string text, string key);
        bool StorePaymentRequestBBPS(BBPSModel model);
        bool StorePaymentRequestBeNow(BeNowResponse model);
        bool StorePaymentRequestDBS(DBSResponse model);
        bool StorePaymentRequestCity(PushNotificationToSSGModel model);

        string BillDeskTransactionRequestAPIRequestPostRevamp(ViewPayBill Model);
        string BillDeskSDTransactionRequestAPIRequestPostRevamp(ViewPayBill Model);
        string BillDeskVDSTransactionRequestAPIRequestPostRevamp(ViewPayBill Model);

        string ICICITransactionRequestAPIRequestPostRevamp(ViewPayBill Model);

        string PaytmTransactionRequestAPIRequestPostRevamp(ViewPayBill Model);     

        string PaytmTransactionStatusAPIRequestPostRevamp(IDictionary<string, string> TransactionRequestAPIResponse);

        string DBSUPITransactionRequestAPIRequestGETRevamp(ViewPayBill Model);

        string BENOWTransactionRequestAPIRequestGETRevamp(ViewPayBill Model);
        string CityUPITransactionRequestAPIRequestGETRevamp(ViewPayBill Model);

        string SafeXPayTransactionRequestAPIRequestPost(ViewPayBill Model);

        bool StorePaymentRequestCashFree(Root model, string msg);
    }
}
