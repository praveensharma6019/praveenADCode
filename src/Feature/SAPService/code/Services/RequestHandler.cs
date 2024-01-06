using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SapPiService.Services;
using System.ServiceModel;
using SapPiService.Wsdl;
using SapPiService.Domain;
using System.Threading.Tasks;
using SapPiService.Services.Impl;
using System.ServiceModel.Description;
using static SapPiService.Helper;
using System.Globalization;
using System.Reflection;
using SI_Contactlog_WebService_website;
using SI_DMUPLDMETRREADService;
using Newtonsoft.Json;

namespace SapPiService.Services
{
    public class RequestHandler
    {
        #region Electricity
        public static BasicHttpBinding binding()
        {
            var binding = new System.ServiceModel.BasicHttpBinding();
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.AllowCookies = true;
            binding.SendTimeout = new TimeSpan(0, 2, 0);
            binding.Security = new BasicHttpSecurity
            {
                Mode = BasicHttpSecurityMode.Transport,
                Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.Basic }
            };
            return binding;
        }

        public static Dictionary<string, string> GetCycleNumber(string accountNumber)
        {
            try
            {
                string caTypeUrl = Sitecore.Configuration.Settings.GetSetting("caTypeUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(caTypeUrl);

                var client = new SI_CHECK_CA_TYPE_inClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);
                var req = new DT_CHECK_CA_TYPE_req();

                //Need to append "0" to make 12 digit code         
                req.CONT_ACCT = accountNumber.PadLeft(12, '0');

                //  req.CONT_ACCT = "100517857".PadLeft(12, '0');

                var resp = client.SI_CHECK_CA_TYPE_inAsync(req);
                resp.Wait();
                var ca_type_resp = resp.Result.MT_CHECK_CA_TYPE_resp;
                Dictionary<string, string> objdata = new Dictionary<string, string>();

                objdata.Add("cycleNumber", !string.IsNullOrEmpty(ca_type_resp.CYCLE_NO) ? ca_type_resp.CYCLE_NO.ToString().TrimStart('0') : "");
                objdata.Add("customerType", ca_type_resp.FORM_TYPE == "CYC" ? ConsumerType.Standard.ToString() : ConsumerType.Premium.ToString());

                return objdata;
            }
            catch
            {
                return null;
            }
        }

        public static bool SetEmailAlertFlag(bool emailAlerts)
        {
            return true;
        }

        public static bool SetSMSAlertsFlag(bool smsAlerts)
        {
            return true;
        }

        public static ComplainStatus CheckComplaintStatus(string accountNumber, string complaintNumber)
        {
            string complainStatusServiceUrl = Sitecore.Configuration.Settings.GetSetting("ComplainStatusServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(complainStatusServiceUrl);

            var client = new SI_COMP_STATUS_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new DT_COMP_STATUS_req
            {
                CA_NO = accountNumber == null ? "" : Helper.FormatAccountNumber(accountNumber),
                ORDER_NO = "00" + complaintNumber ?? ""
            };

            //Log.Info("Scheduler started", this);

            var response = client.SI_COMP_STATUS_inAsync(request);
            response.Wait();

            var responseData = response.Result.MT_COMP_STATUS_resp;

            Helper.LogObject(responseData);

            return new ComplainStatus
            {
                AccountNumber = responseData.CANO,
                ComplainCode = responseData.COMPLAINTSTATUS,
                Message = responseData.COMPLAINTSTATUS  //Builder.GetStatusMessageFromCode(responseData.COMPLAINTSTATUS)
            };
        }

        public static NoSupplyComplaintStatus CheckNoSupplyComplaintStatus(string accountNumber, string complaintNumber)
        {
            accountNumber = string.IsNullOrEmpty(accountNumber) ? "" : accountNumber;
            complaintNumber = string.IsNullOrEmpty(complaintNumber) ? "" : complaintNumber;
            GetNoSupplyComplaintStatus.GetComplaintDetailsSoapClient client = new GetNoSupplyComplaintStatus.GetComplaintDetailsSoapClient();
            var result = client.FetchComplaintDetails(accountNumber, complaintNumber, "MDdjLTIeC4qS26wP8eDt");
            NoSupplyComplaintStatus returnResult;

            string[] result1 = result.Split(',');
            if (result1.Length > 1)
            {
                returnResult = new NoSupplyComplaintStatus
                {
                    AccountNumber = result1[0],
                    ComplaintNumber = result1[1],
                    ServiceCode = result1[2],
                    Status = result1[4],
                    ErrorMessage = string.Empty
                };
            }
            else
            {
                returnResult = new NoSupplyComplaintStatus
                {
                    AccountNumber = accountNumber,
                    ComplaintNumber = complaintNumber,
                    ServiceCode = null,
                    Status = null,
                    ErrorMessage = result1[0]
                };
            }
            //100517857,359680918,79,00:00:00,2
            return returnResult;
        }

        public static BillPaymentItem[] UpdatePaymentInformation(BillPaymentItem[] billPaymentItems)
        {
            string PaymentUpdateServiceUrl = Sitecore.Configuration.Settings.GetSetting("PaymentUpdateServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(PaymentUpdateServiceUrl);

            var client = new ZBAPI_UPDATE_ZPAYMENTSREALClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new DT_BAPI_PAYMENTSREAL_Req
            {
                ZINP_UPDATE = billPaymentItems.Select(x => new DT_BAPI_PAYMENTSREAL_ReqItem
                {
                    CONTRACT_ACCOUNT = x.AccountNumber,
                    TRANSACTION_ID = x.TransactionId,
                    PAYMENT_AMOUNT = x.PaymentAmount.ToString(CultureInfo.InvariantCulture),
                    DATE_TIME = x.TransactionTime.ToString("dd/MM/yyyy HH:mm:ss"),
                    PAYMENT_MODE = x.PaymentMode,
                    PAYMENT_TYPE = x.PaymentType,
                    VENDOR = x.PaymentGateway,
                    BANK_ID = "",
                    CENTER_NO = "",
                    TRANSACTION_DATE = "",
                    TRANSACTION_TIME = ""
                }).ToArray()
            };

            var response = client.ZBAPI_UPDATE_ZPAYMENTSREALAsync(request);
            response.Wait();

            for (var i = 0; i < billPaymentItems.Length; i++)
            {
                billPaymentItems[i].IsSuccess = response.Result.MT_BAPI_PAYMENTSREAL_Resp.ZOUT_UPDATE.Count() > 0 && response.Result.MT_BAPI_PAYMENTSREAL_Resp.ZOUT_UPDATE[i].ISU_UPDATE_FLAG == "Y" ? true : false;
                billPaymentItems[i].Message = response.Result.MT_BAPI_PAYMENTSREAL_Resp.RETURN.Count() > 0 ? response.Result.MT_BAPI_PAYMENTSREAL_Resp.RETURN[i].MESSAGE : string.Empty;
            }
            return billPaymentItems;
        }

        public static OutageDetail GetOutageInformation(string accountNumber)
        {
            var outageInformation = FetchOutageInformation(accountNumber);
            return outageInformation;
        }

        public static PaymentHistory GetPaymentHistory(string accountNumber)
        {
            var paymentHistory = FetchPaymentHistory(accountNumber, DateTime.Today.AddYears(-3), DateTime.Today);
            return paymentHistory;
        }

        public static ConsumptionHistory GetConsumptionHistory(string accountNumber)
        {
            var consumptionHistory = FetchConsumptionHistory(accountNumber);
            return consumptionHistory;
        }

        public static PaymentHistory FetchPaymentHistory(string accountNumber, DateTime? fromDate,
          DateTime? toDate)
        {
            //var client = Builder.BuildClientForPaymentHistory();

            string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("paymentHistroyUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

            var client = new SI_PAYMT_HISTORY_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            var paymentDateRange = new DT_PAYMT_HISTORY_reqDATE
            {
                FROM_DATE = fromDate.HasValue ? Helper.ConvertDateToString(fromDate.Value) : "",
                TO_DATE = toDate.HasValue ? Helper.ConvertDateToString(toDate.Value) : ""
            };

            var request = new DT_PAYMT_HISTORY_req
            { DATE = paymentDateRange, CONT_ACCT = Helper.FormatAccountNumber(accountNumber) };
            var response = client.SI_PAYMT_HISTORY_inAsync(request);
            response.Wait();


            var paymentHistory = new PaymentHistory
            {
                PaymentHistoryList = new List<PaymentHistoryRecord>()
            };

            foreach (var item in response.Result.MT_PAYMT_HISTORY_resp.OUTPUT)
            {
                paymentHistory.PaymentHistoryList.Add(new PaymentHistoryRecord
                {
                    PaymentDate = item.ZDATE,
                    Center = item.CENTRE,
                    Amount = item.AMOUNT,
                    PaymentMode = item.CODE,
                    Receipt = item.RECEIPT
                });
            }

            paymentHistory.PaymentHistoryList = paymentHistory.PaymentHistoryList.OrderByDescending(p => p.PaymentDate).ToList();

            return paymentHistory;
        }

        public static OutageDetail FetchOutageInformation(string accountNumber)
        {
            try
            {
                string outageInformationName = Sitecore.Configuration.Settings.GetSetting("OutageInformationName");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(outageInformationName);

                var client = new SI_OUTAGE_INFO_inClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);
                var request = new DT_OUTAGE_INFO_req
                { VKONT = Helper.FormatAccountNumber(accountNumber) };

                var response = client.SI_OUTAGE_INFO_inAsync(request);
                response.Wait();

                var responseData = response.Result.MT_OUTAGE_INFO_resp;

                Helper.LogObject(responseData);

                var currentHTOutage = responseData.ET_HT_OUTAGE.Where(x => x.ZFLAG.ToLower() == "c").ToList();
                var currentLTOutage = responseData.ET_LT_OUTAGE.Where(x => x.ZFLAG.ToLower() == "c").ToList();

                var futureHTOutage = responseData.ET_HT_OUTAGE.Where(x => x.ZFLAG.ToLower() == "f" || x.ZFLAG.ToLower() == "r").ToList();
                var futureLTOutage = responseData.ET_LT_OUTAGE.Where(x => x.ZFLAG.ToLower() == "f" || x.ZFLAG.ToLower() == "r").ToList();

                List<OutageRecord> currentOutage = new List<OutageRecord>();
                foreach (var x in currentHTOutage)
                {
                    currentOutage.Add(new OutageRecord
                    {
                        Date = (Convert.ToDateTime(x.ZUPLD_DATE)).ToString("dd-MM-yyyy"),
                        StartTime = x.FROM_TIME,
                        StartDateTime = DateTime.ParseExact(x.FROM_TIME, "HH:mm:ss", CultureInfo.InvariantCulture),
                        EndTime = x.TO_TIME,
                        EndDatetime = DateTime.ParseExact(x.TO_TIME, "HH:mm:ss", CultureInfo.InvariantCulture),
                        ActivityType = x.ACTIVITY,
                        OutageType = "HT",
                        ZFLAG = x.ZFLAG
                    });
                }
                foreach (var x in currentLTOutage)
                {
                    currentOutage.Add(new OutageRecord
                    {
                        Date = (Convert.ToDateTime(x.ZUPLD_DATE)).ToString("dd-MM-yyyy"),
                        StartTime = x.FROM_TIME,
                        StartDateTime = DateTime.ParseExact(x.FROM_TIME, "HH:mm:ss", CultureInfo.InvariantCulture),
                        EndTime = x.TO_TIME,
                        EndDatetime = DateTime.ParseExact(x.TO_TIME, "HH:mm:ss", CultureInfo.InvariantCulture),
                        ActivityType = x.ACTIVITY,
                        OutageType = "LT",
                        ZFLAG = x.ZFLAG
                    });
                }

                List<OutageRecord> futureOutage = new List<OutageRecord>();
                foreach (var x in futureHTOutage)
                {
                    futureOutage.Add(new OutageRecord
                    {
                        Date = (Convert.ToDateTime(x.ZUPLD_DATE)).ToString("dd-MM-yyyy"),
                        StartTime = x.FROM_TIME,
                        StartDateTime = DateTime.ParseExact(x.FROM_TIME, "HH:mm:ss", CultureInfo.InvariantCulture),
                        EndTime = x.TO_TIME,
                        EndDatetime = DateTime.ParseExact(x.TO_TIME, "HH:mm:ss", CultureInfo.InvariantCulture),
                        ActivityType = x.ACTIVITY,
                        OutageType = "HT",
                        ZFLAG = x.ZFLAG
                    });
                }
                foreach (var x in futureLTOutage)
                {
                    futureOutage.Add(new OutageRecord
                    {
                        Date = (Convert.ToDateTime(x.ZUPLD_DATE)).ToString("dd-MM-yyyy"),
                        StartTime = x.FROM_TIME,
                        StartDateTime = DateTime.ParseExact(x.FROM_TIME, "HH:mm:ss", CultureInfo.InvariantCulture),
                        EndTime = x.TO_TIME,
                        EndDatetime = DateTime.ParseExact(x.TO_TIME, "HH:mm:ss", CultureInfo.InvariantCulture),
                        ActivityType = x.ACTIVITY,
                        OutageType = "LT",
                        ZFLAG = x.ZFLAG
                    });
                }

                var outagedetailsResult = new OutageDetail
                {
                    AccountNumber = accountNumber,
                    Message = responseData.MESSAGE,
                    CurrentOutageDetails = currentOutage,
                    FutureOutageDetails = futureOutage
                };

                return outagedetailsResult;
            }
            catch (Exception ex)
            {
                var outagedetailsResult = new OutageDetail
                {
                    AccountNumber = null,
                    Message = ex.Message
                };
                return outagedetailsResult;
            }

        }


        public static ConsumptionHistory FetchConsumptionHistory(string accountNumber)
        {
            string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("consumptionHistroyUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

            var client = new SI_MTRREADDOC_GETLIST_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            //var client = Builder.BuildClientForConsumptionHistory();
            var request = new DT_MTRREADDOC_GETLIST_req
            { CA_NUMBER = Helper.FormatAccountNumber(accountNumber), REGISTER = "001" };
            var response = client.SI_MTRREADDOC_GETLIST_inAsync(request);
            response.Wait();

            var consumptions = new Dictionary<string, MeterConsumption>();

            foreach (var item in response.Result.MT_MTRREADDOC_GETLIST_resp.USAGE_HISTORY)
            {
                if (consumptions.ContainsKey(item.SERIALNO))
                {
                    consumptions[item.SERIALNO].ConsumptionRecords.Add(new ConsumptionHistoryRecord
                    {
                        ConsumptionDate = item.ZDATE,
                        UnitsConsumed = item.CONSMPT,
                        Reading = item.E_ZWSTNDAB,
                        Status = item.SY_ST_TEXT
                    });
                }
                else
                {
                    consumptions.Add(item.SERIALNO, new MeterConsumption
                    {
                        MeterNumber = item.SERIALNO,
                        ConsumptionRecords = new List<ConsumptionHistoryRecord>()
                    });
                }
            }

            var consumptionHistory = new ConsumptionHistory { MeterConsumptions = new List<MeterConsumption>() };
            if (consumptions.Values != null && consumptions.Values.Count > 0)
                consumptionHistory.MeterConsumptions = consumptions.Values.ToList();

            return consumptionHistory;
        }

        public static SubscriberDetail FetchDetail(string accountNumber)
        {
            string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("accountDetailsFetchServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

            var client = new SI_ISUPARTNER_GETDETAIL_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            var request = new DT_ISUPARTNER_GETDETAIL_req
            {
                CONTRACT = "",
                CA_NUMBER = Helper.FormatAccountNumber(accountNumber)
            };
            var response = client.SI_ISUPARTNER_GETDETAIL_inAsync(request);
            response.Wait();
            var responseData = response.Result.MT_ISUPARTNER_GETDETAIL_resp.OUTPUT;

            var details = new SubscriberDetail
            {
                AccountNumber = accountNumber,
                Name = $"{responseData.FIRSTNAME} {responseData.LASTNAME}",
                BookNumber = responseData.BOOKNO,
                ZoneNumber = responseData.ZONE_DESCRIP,
                CycleNumber = responseData.Z_CYCLE,
                Address = $"{responseData.HOUSE_NO} {responseData.STREET} {responseData.STR_SUPPL1} {responseData.STR_SUPPL2} {responseData.CITY} {responseData.POSTL_COD1}"
            };

            return details;
        }


        public static BillingInfo FetchBilling(string accountNumber)
        {
            string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("billDetailsFetchServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

            var client = new SI_MR_BI_GETLIST_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new DT_MR_BI_GETLIST_req { CA_NUMBER = Helper.FormatAccountNumber(accountNumber) };

            var response = client.SI_MR_BI_GETLIST_inAsync(request);
            response.Wait();

            var responseData = response.Result.MT_MR_BI_GETLIST_resp.EXPORT;

            Helper.LogObject(response.Result.MT_MR_BI_GETLIST_resp);

            var details = new BillingInfo
            {
                AccountNumber = accountNumber,
                BillAmount = responseData.CURRENT_BILL,
                BillMonth = responseData.BILL_MONTH,
                DateDue = responseData.PAY_DUE_DATE,
                UnitsConsumed = responseData.V_ZWSTNDAB,
                CurrentMonthCharge = responseData.CURRENT_BILL,
                BroughtForward = responseData.BRT_FORWARD,
                TariffSlab = responseData.TARIFF,
                TotalCharges = responseData.TOTAL_CHARGES,
                TotalBillAmount = responseData.NET_AMNT,
                AmountPayable = Convert.ToDecimal(responseData.ROUNDSUM_PAY),
                IsOutstanding = responseData.STATUS != "9",
                Flag = responseData.FLAG,
                MeterNumbers = (from n in response.Result.MT_MR_BI_GETLIST_resp.SERIAL_NUM select n.SERNR.TrimStart('0')).ToArray()
            };

            if (responseData.NET_AMNT.IndexOf("CR") != -1 || responseData.STATUS == "9")
            {
                details.AmountPayable = 0;
            }

            if (string.IsNullOrEmpty(responseData.FLAG))
            {
                details.BillingStatus = BillingStatus.NoInvoice;
            }

            //var dueDateAmount = Convert.ToDecimal(responseData.DUE_DATE_BILL_AMT);
            //var dpcBillAmount = Convert.ToDecimal(responseData.DPC_BILL_AMT);

            //switch (responseData.FLAG)
            //{
            //    //FIXME: Mapping of amount payable need to fix
            //    case "0":
            //        details.BillingStatus = BillingStatus.Due;
            //        details.AmountPayable = Convert.ToDecimal(responseData.ROUNDSUM_PAY);
            //        break;
            //    case "1":
            //        details.BillingStatus = BillingStatus.Overdue;
            //        details.AmountPayable = Convert.ToDecimal(responseData.ROUNDSUM_PAY);
            //        break;
            //    case "2":
            //        details.BillingStatus = BillingStatus.Hold;
            //        details.AmountPayable = 0;
            //        break;
            //}

            if (response.Result.MT_MR_BI_GETLIST_resp.MESSAGES.Length > 0)
            {
                details.Message = response.Result.MT_MR_BI_GETLIST_resp.MESSAGES[0].LINE;
            }

            return details;
        }

        public static CAValidateInfo ValidateCAForChangeOfName(string accountNumber)
        {
            try
            {
                string validateCAForChangeOfNameUrl = Sitecore.Configuration.Settings.GetSetting("ValidateCAForChangeOfName");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(validateCAForChangeOfNameUrl);

                var client = new SI_CA_Details_SendService.SI_CA_Details_SendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_CA_Details_SendService.DT_CA_Details_Req
                {
                    VKONT = Helper.FormatAccountNumber(accountNumber)
                };

                var response = client.SI_CA_Details_Send(request);

                var details = new CAValidateInfo
                {
                    AccountNumber = accountNumber,
                    BSTATUS = response.BSTATUS,
                    CHNG_OVER_FLAG = response.CHNG_OVER_FLAG,
                    DISCONNECTEDFLAG = response.DISCONNECTEDFLAG,
                    INVALIDCAFLAG = response.INVALIDCAFLAG,
                    LAST_BIL_AMND_FL = response.LAST_BIL_AMND_FL,
                    MOBILE_NO = response.MOBILE_NO,
                    MOVEOUTFLAG = response.MOVEOUTFLAG,
                    AKLASSE = response.AKLASSE,
                    NAME_CustomerName = response.NAME,
                    NO_LAST_BILL_FLG = response.NO_LAST_BILL_FLG,
                    OVERDUE_AMT = response.OVERDUE_AMT.ToString(),
                    TARIFTYP_Ratecategory = response.TARIFTYP,
                    VIGILANCEFLAG = response.VIGILANCEFLAG,
                    SMTP_ADDR_Email = response.SMTP_ADDR.ToString(),
                    VAPLZ_WORK_CENTER = response.VAPLZ,
                    DivisionName = response.REGIO_AREA_TEXT,
                    ZoneName = response.REGIO_GROUP_TEXT,

                };
                return details;
            }
            catch (Exception ex)
            {
                return new CAValidateInfo { INVALIDCAFLAG = "X" };
            }
        }

        public static decimal FetchSecurityDepositAmount(string accountNumber)
        {
            string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("securityAmountFetchServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

            var client = new SI_SD_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            var request = new DT_SD_req
            {
                VKONT = Helper.FormatAccountNumber(accountNumber)
            };

            var response = client.SI_SD_inAsync(request);
            response.Wait();

            Helper.LogObject(response.Result.MT_SD_resp);

            var sdAmount = Convert.ToDecimal(response.Result.MT_SD_resp.SD);

            return sdAmount;
        }

        public static SDDetails FetchSecurityDepositAmountDetails(string accountNumber)
        {
            SDDetails result = new SDDetails();
            try
            {
                string securityAmountFetchServiceUrl = Sitecore.Configuration.Settings.GetSetting("securityAmountFetchServiceUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(securityAmountFetchServiceUrl);

                var client = new SI_SD_inClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);
                var request = new DT_SD_req
                {
                    VKONT = Helper.FormatAccountNumber(accountNumber)
                };

                var response = client.SI_SD_inAsync(request);
                response.Wait();

                Helper.LogObject(response.Result.MT_SD_resp);

                var sdAmount = Convert.ToDecimal(response.Result.MT_SD_resp.SD);

                result.Message = response.Result.MT_SD_resp.RETURN.MESSAGE;
                result.SDAmount = response.Result.MT_SD_resp.SD;
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = e.Message;
            }
            return result;
        }

        public static VdsDetail FetchVdsAmount(string accountNumber)
        {
            string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("vdsAmountFetchServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

            var client = new SI_VDS_WEB_PYMT_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            var request = new DT_VDS_WEB_PYMT_req
            {
                CONTRACT_ACCOUNT = Helper.FormatAccountNumber(accountNumber)
            };

            var response = client.SI_VDS_WEB_PYMT_inAsync(request);
            response.Wait();
            Helper.LogObject(response.Result.MT_VDS_WEB_PYMT_resp);


            var averageBillAmount = Convert.ToDecimal(response.Result.MT_VDS_WEB_PYMT_resp.AVG_BILL_AMT);
            var outstandingAmount = Convert.ToDecimal(response.Result.MT_VDS_WEB_PYMT_resp.OUTSTAND_AMT);
            var existingVdsBalance = Convert.ToDecimal(response.Result.MT_VDS_WEB_PYMT_resp.EXISTING_VDS_BALANCE);

            return new VdsDetail
            {
                CurrentOutstanding = (int)outstandingAmount,
                AverageBillingAmount = (int)averageBillAmount,
                ExistingVdsBalance = (int)existingVdsBalance
            };
        }

        public static PvcDetail FetchPvcDetail(string accountNumber)
        {

            string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("pvcDetailsFetchServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

            var client = new SI_PVC_DISPLAY_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);
            var request = new DT_PVC_DISPLAY_req
            {
                CONTRACT_ACCT = Helper.FormatAccountNumber(accountNumber),
                BILL_MONTH_TO_MONTH = "",
                BILL_MONTH_FROM_MONTH = ""
            };

            Helper.LogObject(request);
            var response = client.SI_PVC_DISPLAY_inAsync(request);
            response.Wait();

            Helper.LogObject(response.Result.MT_PVC_DISPLAY_resp);

            var responseData = response.Result.MT_PVC_DISPLAY_resp;

            var output = responseData.OUTPUT[0];
            var output1 = responseData.OUTPUT1;
            var output2 = responseData.OUTPUT2;

            var detail = new PvcDetail
            {
                AccountNumber = accountNumber,

                //Billing Details
                AmountPayable = output.RND_PAYABLE,
                CurrentBillAmount = output.CURR_MTH_BILL_AM,
                BillDateDue = output.DUE_DATE,
                DpcApplicableAfterDueDate = output.ZDPC,

                //Incentive You Earned
                PfIncentive = decimal.Parse(output.PF_INCENTIVE),
                LoadFactorIncentive = decimal.Parse(output.LF_INCN),
                PaymentModeIncentive = decimal.Parse(output.ECS_VDS_INT_DISC),
                TotalIncentive = 0,


                //Penalties
                ExceedingCdPenalty = decimal.Parse(output.PENALTY_EX_CD),
                PfPenalty = decimal.Parse(output.PF_PENALTY),
                DpcOnPreviousBill = decimal.Parse(output.DPC_LEVIED_AMNT),
                TotalPenalty = 0,

                PowerFactor = decimal.Parse(output.AVG_POWER_FACTOR),
                LoadFactor = decimal.Parse(output.LOAD_FACTOR),
                UnitsConsumed = decimal.Parse(output.TOT_UNITS_BILLED),

                //More about your bill
                Ecs = output.ECS,
                EcsMandateAmount = decimal.Parse(output.MANDATE_AMOUNT),
                ContractDemand = output.CD,
                MaximumDemand = output.MD,
                MaximumDemandExceedingCd = output.MD_EXCEED_CD,

                //Services you use
                Units09To12 = decimal.Parse(output.TODUNITS_0912),
                Amount09To12 = decimal.Parse(output.TODCHARGE_0912),
                Units18To22 = decimal.Parse(output.TODUNITS_1822),
                Amount18To22 = decimal.Parse(output.TODCHARGE_1822),
                Units22To06 = decimal.Parse(output.TODUNITS_2206),
                Amount22To06 = decimal.Parse(output.TODCHARGE_2206),
                TotalUnits = decimal.Parse(output.TOTAL_TODUNITS_PER),
                CurrentPromptPaymentDiscount = decimal.Parse(output.CURR_PPD_AMOUNT),
                LastDateOfPpd = output.LAST_DATE_PPD,
                LastMonthAvailed = decimal.Parse(output.DISC_INCN_ON_PAY)
            };
            detail.TotalIncentive = detail.PaymentModeIncentive + detail.PfIncentive + detail.Amount22To06 +
                                   detail.LastMonthAvailed + detail.LoadFactorIncentive;

            detail.TotalPenalty = detail.ExceedingCdPenalty + detail.PfPenalty + detail.DpcOnPreviousBill;

            return detail;
        }

        public static InvoiceHistory FetchInvoiceHistory(string accountNumber)
        {
            string downloadViewBillServiceUrl = Sitecore.Configuration.Settings.GetSetting("downloadViewBillServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(downloadViewBillServiceUrl);

            var client = new SI_GET_BILL_MONTH_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new DT_GET_BILL_MONTH_req { CONTRACT_ACCOUNT = Helper.FormatAccountNumber(accountNumber) };
            var response = client.SI_GET_BILL_MONTH_inAsync(request);
            response.Wait();

            Helper.LogObject(response.Result.MT_GET_BILL_MONTH_resp);

            var details = new InvoiceHistory
            {
                InvoiceLines = new List<InvoiceLine>()
            };

            foreach (var item in response.Result.MT_GET_BILL_MONTH_resp.BILL_DATA)
            {
                details.InvoiceLines.Add(new InvoiceLine
                {
                    AccountNumber = accountNumber,
                    BillMonth = item.BILL_MONTH,
                    InvoiceNumber = item.INVOICE_NO
                });
            }

            return details;
        }
        public static DT_ONLINE_BILL_PDF_resp FetchOnlineBillPDF(string accountNumber, string invoiceNumber)
        {
            string downloadViewBillServiceUrl = Sitecore.Configuration.Settings.GetSetting("OnlineBillPDFFetchURL");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(downloadViewBillServiceUrl);

            var client = new SI_ONLINE_BILL_PDF_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new DT_ONLINE_BILL_PDF_req { CONT_ACCT = Helper.FormatAccountNumber(accountNumber), INVOICE_NO = invoiceNumber };
            var response = client.SI_ONLINE_BILL_PDF_inAsync(request);
            response.Wait();


            Helper.LogObject(response.Result.MT_ONLINE_BILL_PDF_resp);

            return response.Result.MT_ONLINE_BILL_PDF_resp;
        }

        private static void SetClientCredentials(ClientCredentials clientClientCredentials)
        {
            clientClientCredentials.UserName.UserName = Sitecore.Configuration.Settings.GetSetting("SapPiCredentialUserName");
            clientClientCredentials.UserName.Password = Sitecore.Configuration.Settings.GetSetting("SapPiCredentialPassword");
        }

        public static BillingLanguage FetchBillingLanguage(string accountNumber)
        {
            string fetchBillingLanguageUrl = Sitecore.Configuration.Settings.GetSetting("fetchBillingLanguageServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(fetchBillingLanguageUrl);

            var client = new SI_ZCSBLNG_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new DT_ZCSBLNG_req
            {
                ZZ_VKONT = Helper.FormatAccountNumber(accountNumber)
            };

            var response = client.SI_ZCSBLNG_inAsync(request);
            response.Wait();

            Helper.LogObject(request);
            Helper.LogObject(response);
            return EnumHelper.ParseLanguage(response.Result.MT_ZCSBLNG_resp.ZZ_EXP_LANGU);
        }

        public static void UpdateBillingLanguage(string accountNumber, BillingLanguage language)
        {
            string updateBillingLanguageUrl = Sitecore.Configuration.Settings.GetSetting("updateBillingLanguageServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(updateBillingLanguageUrl);

            var client = new SI_ZCSBLNG_inClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new DT_ZCSBLNG_req
            {
                ZZ_VKONT = Helper.FormatAccountNumber(accountNumber),
                ZZ_LANGU = language.GetDescription()
            };

            var response = client.SI_ZCSBLNG_inAsync(request);
            response.Wait();

            Helper.LogObject(request);
            Helper.LogObject(response.Result.MT_ZCSBLNG_resp);
        }

        public static string GetMobileNumber(string accountNumber)
        {
            try
            {
                string contactDetailServiceUrl = Sitecore.Configuration.Settings.GetSetting("ContactDetailServiceUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(contactDetailServiceUrl);

                var client = new ZBAPI_DISPTELNO_01Client(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var response = client.ZBAPI_DISPTELNO_01Async(new DT_DispTelNo_Req { CA = Helper.FormatAccountNumber(accountNumber) });
                response.Wait();

                var responseData = response.Result.MT_DispTelNo_Resp;

                return responseData.MOBILENO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static VDSRegistration VdsRegistration(string accountNumber, string transactionId, string mobileNumber, string emailID, string PANNumber, string amount, string date)
        {
            string EndPointTemplateUrl = "https://iss.adanielectricity.com/XISOAPAdapter/MessageServlet?senderParty=&senderService=Rel_Mob_Web&receiverParty=&receiverService=&interface={SERVICE}";
            string VdsRegistrationServiceName = "ZBAPI_VDS_REGISTRATION&interfaceNamespace=http://aeml.com/Web";
            string VdsRegistrationServiceUrl = EndPointTemplateUrl.Replace("{SERVICE}", VdsRegistrationServiceName);

            //string VdsRegistrationServiceUrl = Sitecore.Configuration.Settings.GetSetting("VdsRegistrationServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(VdsRegistrationServiceUrl);

            var client = new ZBAPI_VDS_REGISTRATIONClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new DT_VDS_REGISTR_Req
            {
                ZDATE = date,   //date
                ZEMAILID = emailID,    //email id
                ZLGNUM = PANNumber,  //PAN No.
                ZMOBNO = mobileNumber,    //mobile number
                ZTRNSACTNID = transactionId, //transaction  id
                ZVKONT = Helper.FormatAccountNumber(accountNumber),   //CA number
                ZBETRW = amount  //amount
            };

            var response = client.ZBAPI_VDS_REGISTRATION(request);
            //response.Wait();

            VDSRegistration result = new VDSRegistration
            {
                AccountNumber = response.ZZVKONT,
                Amount = response.ZZBETRW,
                EmailId = response.ZZEMAILID,
                MobileNumber = response.ZZMOBNO,
                PANNumber = response.ZZLGNUM,
                ResultFlag = response.ZUPD_FLAG,
                TransactionId = response.ZZTRNSACTNID,
                DateOfTransaction = response.ZZDATE
            };

            return result;
        }

        public static string UpdateEmailSmsPaperlessSettings(string accountNumber, string mobile, string telephone, string email, bool? paperless)
        {
            try
            {
                string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("updateEmailSmsPaperlessSettingsServiceUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

                var client = new SI_EBILL_REGISTER_inClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new DT_EBILL_REGISTER_req
                {
                    VKONT = Helper.FormatAccountNumber(accountNumber),
                    MOBILE = mobile,
                    SMTP_ADDR = email,
                    TELEPHONE = string.IsNullOrEmpty(telephone) ? "" : telephone
                };

                if (paperless.HasValue)
                {
                    request.PAPERLESS_EBILL = paperless.Value ? "X" : "D";
                }

                Helper.LogObject(request);
                var response = client.SI_EBILL_REGISTER_inAsync(request);
                response.Wait();

                Helper.LogObject(response.Result.MT_EBILL_REGISTER_resp);
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static bool CheckPaperlessBillingSettings(string accountNumber)
        {
            try
            {
                string checkPaperlessBillingSettingsUrl = Sitecore.Configuration.Settings.GetSetting("FetchConsumerPANGSTDetailsUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(checkPaperlessBillingSettingsUrl);

                var client = new SI_Find_Extract_BPInfoService.SI_Find_Extract_BPInfoClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_Find_Extract_BPInfoService.DT_Find_Extract_BPInfo_Req
                {
                    VKONT = Helper.FormatAccountNumber(accountNumber),
                    PARTNER = ""
                };
                var response = client.SI_Find_Extract_BPInfo(request);

                Helper.LogObject(request);
                Helper.LogObject(response);
                if (response.SENDCONTROL_GP == "Z024" || response.SENDCONTROL_GP == "Z724")
                    return true;
                else
                    return false;

            }
            catch (Exception e)
            {
                Helper.LogObject(e);
                return false;
            }
        }
        public static UpdatePANGSTDetailsResult UpdatePANGST(string accountNumber, string BPNumber, string PANNumber, string GSTNumber)
        {
            try
            {
                string updatePANGSTUrl = Sitecore.Configuration.Settings.GetSetting("UpdatePANGSTUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(updatePANGSTUrl);

                var client = new ZBAPI_LEGITIMATEService_website.ZBAPI_LEGITIMATEClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new ZBAPI_LEGITIMATEService_website.DT_ZBAPI_LEGITIMATE_Req
                {
                    PARTNER = BPNumber,
                    LGNUM = string.IsNullOrEmpty(PANNumber) ? GSTNumber : PANNumber, //entered PAN no. or GST no. as LGNUM 
                    LGTYP = string.IsNullOrEmpty(PANNumber) ? "0100" : "0020" //LGTYP=0100 for GST no. and 0020 for PAN no. 
                };
                var response = client.ZBAPI_LEGITIMATE(request);

                Helper.LogObject(request);
                Helper.LogObject(response);

                var details = new UpdatePANGSTDetailsResult
                {
                    FLAG = response.FLAG1,
                    LGNUM = response.LGNUM1,
                    MESSAGE = response.MESSAGE,
                };

                return details;
            }
            catch (Exception ex)
            {
                return new UpdatePANGSTDetailsResult();
            }
        }

        public static ConsumerDetails FetchConsumerDetails(string accountNumber)
        {
            try
            {
                string fetchConsumerDetailsUrl = Sitecore.Configuration.Settings.GetSetting("FetchConsumerDetailsUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(fetchConsumerDetailsUrl);

                var client = new SI_Cms_Isu_Ca_Display_inClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new DT_Cms_Isu_Ca_Display_req
                {
                    IMPORT_CANUMBER = new DT_Cms_Isu_Ca_Display_reqIMPORT_CANUMBER
                    {
                        CA_NUMBER = Helper.FormatAccountNumber(accountNumber)
                    }
                };
                var response = client.SI_Cms_Isu_Ca_Display_in(request);

                Helper.LogObject(request);
                Helper.LogObject(response);

                var details = new ConsumerDetails
                {
                    Name = response.EXPORT_CADETAILS[0].BP_NAME,
                    Email = response.EXPORT_CADETAILS[0].E_MAIL,
                    Mobile = response.EXPORT_CADETAILS[0].TEL1_NUMBR,
                    CANumber = response.EXPORT_CADETAILS[0].CA_NUMBER,
                    ConnectionType = response.EXPORT_CADETAILS[0].RATE_CAT,
                    MeterNumber = response.EXPORT_CADETAILS[0].DEVICE_SR_NUMBER,
                    HouseNumber = response.EXPORT_CADETAILS[0].HOUSE_NUMBER,
                    Street = response.EXPORT_CADETAILS[0].STREET,
                    Street2 = response.EXPORT_CADETAILS[0].STREET2,
                    Street3 = response.EXPORT_CADETAILS[0].STREET3,
                    City = response.EXPORT_CADETAILS[0].CITY,
                    PinCode = response.EXPORT_CADETAILS[0].POST_CODE,
                    Vertrag_Contract = response.EXPORT_CADETAILS[0].VERTRAG,
                    BP_Number = response.EXPORT_CADETAILS[0].BP_NUMBER,
                    MOVEOUTFLAG = response.EXPORT_CADETAILS[0].MOVE_OUT_DATE,





                };

                return details;
            }
            catch (Exception ex)
            {
                return new ConsumerDetails();
            }
        }

        public static ConsumerPANGSTDetails FetchConsumerPANGSTDetails(string accountNumber)
        {
            try
            {
                string FetchConsumerPANGSTDetailsUrl = Sitecore.Configuration.Settings.GetSetting("FetchConsumerPANGSTDetailsUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(FetchConsumerPANGSTDetailsUrl);

                var client = new SI_Find_Extract_BPInfoService.SI_Find_Extract_BPInfoClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_Find_Extract_BPInfoService.DT_Find_Extract_BPInfo_Req
                {
                    VKONT = Helper.FormatAccountNumber(accountNumber),
                    PARTNER = ""
                };
                var response = client.SI_Find_Extract_BPInfo(request);

                Helper.LogObject(request);
                Helper.LogObject(response);

                var details = new ConsumerPANGSTDetails
                {
                    CANumber = accountNumber,
                    PANNumber = response.IDNUMBER,
                    GSTNumber = response.GST_IDNUMBER
                };

                return details;
            }
            catch (Exception ex)
            {
                return new ConsumerPANGSTDetails();
            }
        }

        public static ContactLogWebServiceResponse CreateContactLogWeb(SI_Contactlog_WebService_website.DT_Contactlog_req request)
        {
            var details = new ContactLogWebServiceResponse();
            try
            {
                string CreateContactLogWebUrl = Sitecore.Configuration.Settings.GetSetting("CreateContactLogWebUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(CreateContactLogWebUrl);

                var client = new SI_Contactlog_WebService_website.SI_Contactlog_WebClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                Helper.LogObject(request);

                var response = client.SI_Contactlog_Web(request);

                Helper.LogObject(response);

                details = new ContactLogWebServiceResponse
                {
                    CONTACT = response.CONTACT,
                    CONTACTOBJECTS_WITH_ROLE = response.CONTACTOBJECTS_WITH_ROLE,
                    MESSAGE = response.MESSAGE,
                    NOTICETEXT = response.NOTICETEXT,
                    RETURN = response.RETURN
                };
            }
            catch (Exception e)
            {

            }
            return details;
        }


        public static decimal GetSecurityDeposityAmountCON(string accountNumber)
        {
            try
            {
                string fetchSecurityDeposityAmountChangeOfName = Sitecore.Configuration.Settings.GetSetting("fetchSecurityDeposityAmountChangeOfName");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(fetchSecurityDeposityAmountChangeOfName);

                var client = new SI_PSD_CA_WebService.SI_PSD_CA_WebClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_PSD_CA_WebService.DT_PSD_CA_Req
                {
                    CONTRACT_ACCOUNT_NUMBER = Helper.FormatAccountNumber(accountNumber)
                };
                var response = client.SI_PSD_CA_Web(request);

                Helper.LogObject(request);
                Helper.LogObject(response);


                return response.PAID_SD_AMOUNT;
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        /// <summary>
        /// EMI option
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public static EMIBillingInfo ValidateCAForEMIOption(string accountNumber, string processtype = "E", string source = "02")
        {
            try
            {
                string validateCAForEMIOptionUrl = Sitecore.Configuration.Settings.GetSetting("ValidateCAForEMIOption");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(validateCAForEMIOptionUrl);

                var client = new SI_EMIOptinService.SI_EMIOptinClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_EMIOptinService.DT_EMIOptin_Req
                {
                    VKONT = Helper.FormatAccountNumber(accountNumber),
                    SOURCE = source,
                    PROCESS_TYPE = processtype
                };

                var response = client.SI_EMIOptin(request);

                var details = new EMIBillingInfo
                {
                    AccountNumber = response.EMI_OUTPUT.VKONT,
                    BillMonth = response.EMI_OUTPUT.BILL_MONTH,
                    ConsumerName = response.EMI_OUTPUT.NAME,
                    EMIInstallmentAmount = response.EMI_OUTPUT.INST_AMNT,
                    InvoiveNumber = response.EMI_OUTPUT.INVOICE_NO,
                    Remarks = response.EMI_OUTPUT.REMARKS,
                    Status = response.EMI_OUTPUT.STATUS,
                    TotalOutstanding = response.EMI_OUTPUT.TOT_OUTSTANDING
                };
                return details;
            }
            catch
            {
                return null;
            }
        }

        #endregion Electricity

        #region Self Meter Reading

        public static ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_GETMETERNUMBERRequest ReadMeterNumberForSelfMeterReading(string accountNumber, string meterNumber, string source)
        {
            string ReadMeterNumbersServiceUrl = Sitecore.Configuration.Settings.GetSetting("ReadMeterNumbersServiceUrl");
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(ReadMeterNumbersServiceUrl);

            var client = new ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_GETMETERNUMBERPortTypeClient(binding(), remoteAddress);
            SetClientCredentials(client.ClientCredentials);

            var request = new ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_GETMETERNUMBERRequest
            {
                CA_NUMBER = Helper.FormatAccountNumber(accountNumber),
                METER_NUMBER = meterNumber,
                UNIQUE_SOURCE_KEY = source,
                IT_OUTPUT = null,
                IT_RETURN = null
            };

            client.ZBAPI_DM_GETMETERNUMBER(request.CA_NUMBER, request.METER_NUMBER, request.UNIQUE_SOURCE_KEY, ref request.IT_OUTPUT, ref request.IT_RETURN);

            return request;
        }

        public static ZBAPI_DM_T_GETMETERNUMBER_OUT[] SelfMeterReadingDataUpload(string accountNumber, string meterNumber, string source, string meterReadingDate, string selfMeterReading, string selfMeterReadingDate, string mobileNumber)
        {
            ZBAPI_DM_T_GETMETERNUMBER_OUT[] IT_Return = null;
            try
            {
                string SelfMeterReadingDataUploadServiceUrl = Sitecore.Configuration.Settings.GetSetting("SelfMeterReadingDataUploadServiceUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(SelfMeterReadingDataUploadServiceUrl);

                var client = new SI_DMUPLDMETRREADService.SI_DMUPLDMETRREADClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var reqitem = new SI_DMUPLDMETRREADService.DT_DMUPLDMETRREAD_ReqItem
                {
                    CA_NUMBER = Helper.FormatAccountNumber(accountNumber),
                    METER_NUMBER = meterNumber,
                    MTR_READ_DATE = meterReadingDate,
                    SELF_MTR_READ = selfMeterReading,
                    SMRD = selfMeterReadingDate,
                    UNIQUE_SOURCE_KEY = source,
                    MOBILE_NUMBER = mobileNumber,
                };
                SI_DMUPLDMETRREADService.DT_DMUPLDMETRREAD_ReqItem[] objReq = new SI_DMUPLDMETRREADService.DT_DMUPLDMETRREAD_ReqItem[1];
                objReq[0] = reqitem;
                var request = new SI_DMUPLDMETRREADService.DT_DMUPLDMETRREAD_Req
                {
                    CA_NUMBER = Helper.FormatAccountNumber(accountNumber),
                    IT_INPUT = objReq,
                    METER_NUMBER = ""
                };

                var response = client.SI_DMUPLDMETRREAD(request);

                /*    new SI_DMUPLDMETRREADService.DT_DMUPLDMETRREAD_Req ()
                {
                    CA_NUMBER = accountNumber,
                    IT_INPUT = objReq,
                    METER_NUMBER = meterNumber
                });*/

                IT_Return = response.IT_OUTPUT;

            }
            catch (Exception ex)
            {
                //IT_Return[0].MESSAGE = ex.Message;
                Helper.LogObject(ex);
            }

            return IT_Return;
        }

        public static ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] SelfMeterReadingImageUpload(string imageBinary, string FileName, string meterNumber, string AUFNR, string DocType, string accountNumber)
        {
            ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[] objReq = new ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2[1];
            try
            {
                string SelfMeterReadingImageUpload = Sitecore.Configuration.Settings.GetSetting("SelfMeterReadingImageUpload");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(SelfMeterReadingImageUpload);

                var client = new ZBAPI_IMAGE_TRANSFER_TO_SAP.ZBAPI_IMAGE_TRANSFER_TO_SAPPortTypeClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var objReqItem = new ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2
                {

                };

                objReq[0] = objReqItem;
                //pass correct values
                client.ZBAPI_IMAGE_TRANSFER_TO_SAP(AUFNR, meterNumber, DocType, FileName, imageBinary, accountNumber, ref objReq);
            }
            catch (Exception ex)
            {
                objReq[0] = new ZBAPI_IMAGE_TRANSFER_TO_SAP.BAPIRET2
                {
                    MESSAGE = ex.Message
                };
                Helper.LogObject(ex);
            }
            return objReq;


        }
        #endregion

        #region Solar Application

        public static ConsumerDetailsForSolar FetchConsumerDetailsForSolar(string accountNumber)
        {
            try
            {
                string fetchConsumerDetailsUrl = Sitecore.Configuration.Settings.GetSetting("FetchConsumerDetailsUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(fetchConsumerDetailsUrl);

                var client = new SI_Cms_Isu_Ca_Display_inClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new DT_Cms_Isu_Ca_Display_req
                {
                    IMPORT_CANUMBER = new DT_Cms_Isu_Ca_Display_reqIMPORT_CANUMBER
                    {
                        CA_NUMBER = Helper.FormatAccountNumber(accountNumber)
                    }
                };
                var response = client.SI_Cms_Isu_Ca_Display_in(request);

                var details = new ConsumerDetailsForSolar
                {
                    Name = response.EXPORT_CADETAILS[0].BP_NAME,
                    Email = response.EXPORT_CADETAILS[0].E_MAIL,
                    Mobile = response.EXPORT_CADETAILS[0].TEL1_NUMBR,
                    CANumber = response.EXPORT_CADETAILS[0].CA_NUMBER,
                    ConnectionType = response.EXPORT_CADETAILS[0].RATE_CAT,
                    MeterNumber = response.EXPORT_CADETAILS[0].DEVICE_SR_NUMBER,
                    HouseNumber = response.EXPORT_CADETAILS[0].HOUSE_NUMBER,
                    Street = response.EXPORT_CADETAILS[0].STREET,
                    Street2 = response.EXPORT_CADETAILS[0].STREET2,
                    Street3 = response.EXPORT_CADETAILS[0].STREET3,
                    City = response.EXPORT_CADETAILS[0].CITY,
                    PinCode = response.EXPORT_CADETAILS[0].POST_CODE,
                    Vertrag_Contract = response.EXPORT_CADETAILS[0].VERTRAG,
                    TATA_CONSUMER = response.EXPORT_CADETAILS[0].TATA_CONSUMER,
                    MOVE_OUT_DATE = response.EXPORT_CADETAILS[0].MOVE_OUT_DATE,
                    BILL_CLASS = response.EXPORT_CADETAILS[0].BILL_CLASS,
                    Rate_Category = response.EXPORT_CADETAILS[0].RATE_CAT,
                    BP_Type = response.EXPORT_CADETAILS[0].BP_TYPE,
                    CON_OBJ_NO = response.EXPORT_CADETAILS[0].CON_OBJ_NO,
                    MRU = response.EXPORT_CADETAILS[0].MRU,
                    FUNC_DESCR = response.EXPORT_CADETAILS[0].FUNC_DESCR,
                    DESC_CON_OBJECT = response.EXPORT_CADETAILS[0].DESC_CON_OBJECT,
                    REGSTRGROUP = response.EXPORT_CADETAILS[0].REG_STR_GROUP
                };

                return details;
            }
            catch
            {
                return new ConsumerDetailsForSolar();
            }
        }

        public static ConsumerOutstandingDetails FetchAmountDetailsForSolar(string accountNumber)
        {
            try
            {
                string fetchAmountDetailsForSolarUrl = Sitecore.Configuration.Settings.GetSetting("FetchAmountDetailsForSolarUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(fetchAmountDetailsForSolarUrl);

                var client = new SI_Overdue_Amount_SendService.SI_Overdue_Amount_SendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_Overdue_Amount_SendService.DT_Overdue_Amount_Req
                {
                    CONTRACT_ACCOUNT = Helper.FormatAccountNumber(accountNumber)
                };
                var response = client.SI_Overdue_Amount_SendAsync(request);

                var details = new ConsumerOutstandingDetails
                {
                    OUTSTANDING_AMOUNT = response.Result.MT_Overdue_Amount_Res.OUTSTANDING_AMOUNT,
                    SANCTIONED_LOAD = response.Result.MT_Overdue_Amount_Res.SANCTIONED_LOAD
                };

                return details;
            }
            catch (Exception ex)
            {
                return new ConsumerOutstandingDetails();
            }
        }

        public static OrderFetchResult GetOrderIdForSolar(string tempRegistrationNumber)
        {
            OrderFetchResult result = new OrderFetchResult();
            try
            {
                string orderCreationSolarUrl = Sitecore.Configuration.Settings.GetSetting("OrderCreationSolarUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(orderCreationSolarUrl);

                var client = new SI_CSWebmaster_sendService.SI_CSWebmaster_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_CSWebmaster_sendService.DT_CS_Webmaster_req
                {
                    ZTEMPNO = tempRegistrationNumber,
                    FLAG_CRT = "1"
                };

                var response = client.SI_CSWebmaster_send(request);
                result.IsSuccess = true;
                result.ExceptionMessage = response.MESSAGE;
                result.OrderIdSAP = response.AUFNR;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ExceptionMessage = ex.Message;
                return result;
            }
        }

        public static PostDataResult PostDataForSolar(PostDataWebUpdate solarApplicationDetails, List<PostDocumentsWebUpdate> applicationDocuments)
        {
            PostDataResult result = new PostDataResult();
            try
            {
                string postDataForSolarUrl = Sitecore.Configuration.Settings.GetSetting("PostDataSolarUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(postDataForSolarUrl);

                var client = new SI_CSWebupdate_send_Website.SI_CSWebupdate_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_CSWebupdate_send_Website.DT_CS_Webupdatereq();

                request.IT_APPL = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqIT_ITEM[] {
                    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqIT_ITEM{
                        ZTEMPNO=solarApplicationDetails.TempRegistrationNumber,
                        ZALTYP="SLR",
                        ZALSTYP=solarApplicationDetails.IsRooftopsolarInstalled?"10":"11",
                        ZAPPLBY="11",
                        ZDATE_REG=DateTime.Now.ToString(),
                        ZTYPE=(solarApplicationDetails.VoltageLevel=="01" || solarApplicationDetails.VoltageLevel=="02")? "LT":"HT",
                        ZMTRTYP=solarApplicationDetails.IsRooftopPlusGroundCapacity?"1":"0",
                        ZANLZU=solarApplicationDetails.IsRooftopOwned?"1":"0",
                        ZZ_LICNO=solarApplicationDetails.LECNumber,
                        ZMTROWN=solarApplicationDetails.IsNetMeter?"01":"02",
                        ZBANKN=solarApplicationDetails.InstallationCost,
                        ZVKONT_EXT=Helper.FormatAccountNumber(solarApplicationDetails.AccountNumber),
                        ZUTILITY=solarApplicationDetails.ApplicationCategory,
                        ZVOLTLVL=solarApplicationDetails.VoltageLevel,
                        ZFLAG_WIR=solarApplicationDetails.IsObligatedEntity?"Y":"N",
                     ERDAT=   DateTime.Now.ToString(),
                    }
                };

                request.IT_ADRS = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM[] {
                    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM{
                        ZTEMPNO=solarApplicationDetails.TempRegistrationNumber,
                        ARBPL=solarApplicationDetails.Workcenter,
                        ZNAME_R=solarApplicationDetails.VendorName,
                        ZSTREET1_R=solarApplicationDetails.VendorCode,
                        ERDAT=DateTime.Now.ToString()
                    }
                };

                request.IT_LOAD = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM1[] {
                    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM1{
                        ZTEMPNO=solarApplicationDetails.TempRegistrationNumber,
                        ZCOUNT_1PH=solarApplicationDetails.VoltageLevel=="01"?"1":"0",
                        ZALKW_1=solarApplicationDetails.VoltageLevel=="01"?"Proposed AC capacity":null,
                        ZCOUNT_3PH=solarApplicationDetails.VoltageLevel=="02"?"1":"0",
                        ZALKW_3=solarApplicationDetails.VoltageLevel=="02"?"Proposed AC capacity":null,
                        ZCOUNT_HT=(solarApplicationDetails.VoltageLevel!="01" && solarApplicationDetails.VoltageLevel!="02")?"1":"0",
                        ZALKW_H=(solarApplicationDetails.VoltageLevel!="01" && solarApplicationDetails.VoltageLevel!="02")?"Proposed AC capacity":null,
                    }
                };

                //Assuming only 2 documents will be there every time
                request.IT_DOC = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM2[] {
                    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM2{
                        ZALTYP="SLT",
                        ZTEMPNO=solarApplicationDetails.TempRegistrationNumber,
                        ZDESC=applicationDocuments[0].DocumentDescription,
                        Z_DOCTYPE=applicationDocuments[0].DocumentSerialNumber
                    },
                    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM2{
                        ZALTYP="SLT",
                        ZTEMPNO=solarApplicationDetails.TempRegistrationNumber,
                        ZDESC=applicationDocuments[1].DocumentDescription,
                        Z_DOCTYPE=applicationDocuments[1].DocumentSerialNumber
                    }
                };

                var responseDetails = client.SI_CSWebupdate_send(request);
                result = new PostDataResult
                {
                    FLAG_UPD_APPL = responseDetails.FLAG_UPD_APPL,
                    FLAG_UPD_ADRC = responseDetails.FLAG_UPD_ADRC,
                    FLAG_UPD_LOAD = responseDetails.FLAG_UPD_LOAD,
                    FLAG_UPD_DOC = responseDetails.FLAG_UPD_DOC
                };
                if (result.FLAG_UPD_APPL == "1" && result.FLAG_UPD_ADRC == "1" && result.FLAG_UPD_LOAD == "1" && result.FLAG_UPD_DOC == "1")
                    result.IsSuccess = true;
                else
                    result.IsSuccess = false;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ExceptionMessage = ex.Message;
                return result;
            }
        }

        #endregion

        public static bool ValidateLEC(string registrationNumber)
        {
            try
            {
                string validateLECUrl = Sitecore.Configuration.Settings.GetSetting("ValidateLECUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(validateLECUrl);

                var client = new SI_LECval_sendService_website.SI_LECval_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_LECval_sendService_website.DT_LECvalreq
                {
                    //NEW_LEC_NO = registrationNumber.Length > 10 ? registrationNumber : "",
                    I_LEC = registrationNumber.Length > 10 ? "" : registrationNumber
                };
                var response = client.SI_LECval_send(request);

                if (response.FLAG == "2" && response.IT_ZLEC_MASTER.Count() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static LECValidateInfo ValidateAndFetchLECDetailsByLicenseNumber(string registrationNumber)
        {
            LECValidateInfo resultObj = new LECValidateInfo();
            try
            {
                string validateLECUrl = Sitecore.Configuration.Settings.GetSetting("ValidateLECUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(validateLECUrl);

                var client = new SI_LECval_sendService_website.SI_LECval_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_LECval_sendService_website.DT_LECvalreq
                {
                    //NEW_LEC_NO = registrationNumber.Length > 10 ? registrationNumber : "",
                    I_LEC = registrationNumber.Length > 10 ? "" : registrationNumber
                };
                var response = client.SI_LECval_send(request);

                if (response.FLAG == "2" && response.IT_ZLEC_MASTER.Count() > 0)
                {
                    resultObj.IsValid = true;
                    resultObj.LEC_Name = response.IT_ZLEC_MASTER[0].ZZ_LEC;
                    resultObj.LEC_MOBILE_NO = response.IT_ZLEC_MASTER[0].ZMOBILE_NO;
                    resultObj.LEC_Email = response.IT_ZLEC_MASTER[0].ZEMAIL;
                    resultObj.LEC_License_NO = response.IT_ZLEC_MASTER[0].ZZ_LICNO;
                    resultObj.LEC_Validity_Info = response.IT_ZLEC_MASTER[0].ZLICENCE_VAL_DT;
                }
                else
                {
                    resultObj.IsValid = false;
                }
                return resultObj;
            }
            catch (Exception ex)
            {
                return resultObj;
            }
        }

        public static LECValidateInfo ValidateLECAndFetchDetails(string registrationNumber, string mobileNumber, string emailId, bool isUpdate)
        {
            LECValidateInfo resultObj = new LECValidateInfo();
            try
            {
                string validateLECUrl = Sitecore.Configuration.Settings.GetSetting("ValidateLECUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(validateLECUrl);

                var client = new SI_LECval_sendService_website.SI_LECval_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_LECval_sendService_website.DT_LECvalreq
                {
                    //NEW_LEC_NO = registrationNumber.Length > 10 ? registrationNumber : "",
                    I_LEC = registrationNumber.Length > 10 ? "" : registrationNumber,
                    MOBILE_NO = mobileNumber,
                    EMAIL_ADDRESS = emailId,
                    COMM_DETAILS_FLAG = isUpdate ? "X" : null
                };
                var response = client.SI_LECval_send(request);

                if (response.IT_ZLEC_MASTER.Count() > 0)
                {
                    resultObj.IsValid = true;
                    resultObj.LEC_Name = response.IT_ZLEC_MASTER[0].ZZ_LEC;
                    resultObj.LEC_MOBILE_NO = response.IT_ZLEC_MASTER[0].ZMOBILE_NO;
                    resultObj.LEC_Email = response.IT_ZLEC_MASTER[0].ZEMAIL;
                    resultObj.LEC_License_NO = response.IT_ZLEC_MASTER[0].ZZ_LICNO;
                    resultObj.LEC_Validity_Info = response.IT_ZLEC_MASTER[0].ZLICENCE_VAL_DT;
                }
                else
                {
                    resultObj.IsValid = false;
                    resultObj.Message = response.FLAG;
                }
                resultObj.IsUpdated = isUpdate ? true : false;
            }
            catch (Exception ex)
            {
                resultObj.IsValid = false;
                resultObj.Message = ex.Message;
            }
            return resultObj;
        }


        public static string ValidateMobileAndGetCA(string MobileNumber)
        {
            try
            {
                string getCAByMobileUrl = Sitecore.Configuration.Settings.GetSetting("GetCAByMobileUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(getCAByMobileUrl);

                var client = new SI_MobAppRegService_website.SI_MobAppRegClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_MobAppRegService_website.DT_MobAppReg_Req
                {
                    MOBILE_NUMBER = MobileNumber
                };
                var response = client.SI_MobAppReg(request);

                if (response.IT_MOBILE_DATA.Count() > 0)
                    return response.IT_MOBILE_DATA.First().VKONTO;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static bool IsMobileAndCACombinationValid(string AccoutnNumber, string MobileNumber)
        {
            try
            {
                string getCAByMobileUrl = Sitecore.Configuration.Settings.GetSetting("GetCAByMobileUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(getCAByMobileUrl);

                var client = new SI_MobAppRegService_website.SI_MobAppRegClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_MobAppRegService_website.DT_MobAppReg_Req
                {
                    MOBILE_NUMBER = MobileNumber
                };
                var response = client.SI_MobAppReg(request);

                if (response.IT_MOBILE_DATA.Count() > 0)
                {
                    foreach (var m in response.IT_MOBILE_DATA)
                    {
                        if (m.VKONTO == Helper.FormatAccountNumber(AccoutnNumber))
                            return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }


        public static ComplaintRegistrationResponse RegisterComplaint(string accountNumber, string mobileNumber, string categoryId, string subCategoryId, string source = "4")
        {
            ComplaintRegistrationResponse result = new ComplaintRegistrationResponse(); ;

            try
            {
                accountNumber = accountNumber.TrimStart(new Char[] { '0' });
                string registerComplaintAllWebUrl = Sitecore.Configuration.Settings.GetSetting("RegisterComplaintAllWebUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(registerComplaintAllWebUrl);

                var client = new SI_RegcomplaintAllClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new DT_RegcomplaintAll_Req
                {
                    CA_No = accountNumber,
                    Contact_No = mobileNumber,
                    Complaint_Type = categoryId,
                    Complaint_Subtype = subCategoryId,
                    Source = source
                };
                //REL_SUCCESS,151434673,0,20250321,91,0

                DT_Regcomplaint_Resp response = client.SI_RegcomplaintAll(request);
                string[] returnParam = response.Response.Split(',');
                result = new ComplaintRegistrationResponse
                {
                    Message = returnParam[0],
                    AccountNumber = returnParam[1],
                    TATInfo = returnParam[2],
                    ComplaintNumber = returnParam[3],
                    ComplaintStatus = returnParam[4],
                    LTHTInfo = returnParam[5],
                    IsRegistered = returnParam[4] == "99" ? true : false
                };
                return result;
            }
            catch (Exception ex)
            {
                return result = new ComplaintRegistrationResponse
                {
                    AccountNumber = accountNumber,
                    ComplaintNumber = null,
                    ComplaintStatus = null,
                    LTHTInfo = null,
                    Message = ex.Message,
                    TATInfo = null
                };
            }
        }

        public static ComplaintRegistrationResponse ReportTheftCSOrderCreate(string Area, string Name, string MobileNumber, string Email, string ReportTheftRemark)
        {
            ComplaintRegistrationResponse result = new ComplaintRegistrationResponse();
            try
            {
                string registerComplaintCSOrderCreateUrl = Sitecore.Configuration.Settings.GetSetting("RegisterComplaintCSOrderCreateUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(registerComplaintCSOrderCreateUrl);

                var client = new ZBAPI_CS_ORDER_CREATE_website.ZBAPI_CS_ORDER_CREATEClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new ZBAPI_CS_ORDER_CREATE_website.DT_CS_ORDER_CREATE_Req
                {
                    CA = "",
                    METER = "",
                    ORDERTYPE = "ZMSC",
                    PMACT = "M45",
                    PROCGROUP = "61",
                    PLANNERGROUP = "VIG",
                    FLAG_ADDRESS = "X",
                    FLAG_REF = "X",
                    FLAG_POST = "",
                    IT_ADDRESS = new ZBAPI_CS_ORDER_CREATE_website.DT_CS_ORDER_CREATE_ReqItem[]
                    {
                        new ZBAPI_CS_ORDER_CREATE_website.DT_CS_ORDER_CREATE_ReqItem
                        {
                         //NAME1 = Name,
                           NAME2 = Name,
                           CITY = "MUMBAI",
                           EMAIL = Email,
                           MOBILE = MobileNumber,
                           VAPLZ = Area
                        }
                    },
                    IT_TEXT_LINES = new ZBAPI_CS_ORDER_CREATE_website.DT_CS_ORDER_CREATE_ReqItem4[]
                    {
                        new ZBAPI_CS_ORDER_CREATE_website.DT_CS_ORDER_CREATE_ReqItem4
                        {
                            TDFORMAT = "",
                            TDLINE = ReportTheftRemark
                        }
                    }
                };

                var response = client.ZBAPI_CS_ORDER_CREATE(request);
                result = new ComplaintRegistrationResponse
                {
                    ComplaintNumber = string.IsNullOrEmpty(response.AUFNR) ? string.Empty : response.AUFNR.TrimStart('0'),
                    IsRegistered = string.IsNullOrEmpty(response.AUFNR) ? false : true
                };
                return result;
            }
            catch (Exception ex)
            {
                return result = new ComplaintRegistrationResponse
                {
                    Error = ex.Message
                };
            }
        }

        public static ComplaintRegistrationResponse RegisterStreetLightComplaint(ComplaintInfo model, string source = "4")
        {
            ComplaintRegistrationResponse result = new ComplaintRegistrationResponse(); ;
            try
            {
                string accountNumber = model.AccountNumber.TrimStart(new Char[] { '0' });
                string registerComplaintStreetLightUrl = Sitecore.Configuration.Settings.GetSetting("RegisterComplaintStreetLightUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(registerComplaintStreetLightUrl);

                var client = new SI_CreateCaseStreetService.SI_CreateCaseStreetClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_CreateCaseStreetService.DT_CreateCase_Req
                {
                    ActualAddress1 = model.Address,
                    ActualAddress2 = model.Address,
                    //ActualAddress3 = model.Address, //Optional
                    ActualEmailAddress = model.EmailId,
                    ActualMobileNumber = model.MobileNumber,
                    ActualName = model.ConsumerName,
                    ActualTelephoneNumber = model.MobileNumber,
                    AreaDetails = model.streetArea, //from GIS service
                    CANumber = model.AccountNumber,
                    ComplaintSubType = model.ComplaintSubCategory,
                    ComplaintSusSubType = model.ComplaintSubType,
                    ComplaintType = "RTST",
                    ConsumerType = "1",
                    LandMark = model.City,
                    Pillarno = model.streetPillarNo,
                    //Remark = "",  //Optional
                    Source = source,
                    StreetlightPoleno = model.streetPoleNo //GIS Service
                };
                //REL_SUCCESS,151434673,0,20250321,91,0

                var response = client.SI_CreateCaseStreet(request);
                //string[] returnParam = response.Response.Split(',');
                result = new ComplaintRegistrationResponse
                {
                    //Message = returnParam[0],
                    //AccountNumber = returnParam[1],
                    //TATInfo = returnParam[2],
                    //ComplaintNumber = returnParam[3],
                    //ComplaintStatus = returnParam[4],
                    //LTHTInfo = returnParam[5],
                };
                return result;
            }
            catch (Exception ex)
            {
                return result = new ComplaintRegistrationResponse
                {
                    AccountNumber = model.AccountNumber,
                    ComplaintNumber = null,
                    ComplaintStatus = null,
                    LTHTInfo = null,
                    Message = ex.Message,
                    TATInfo = null,
                    Error = ex.Message
                };
            }
        }

        public static ComplaintRegistrationResponse RegisterComplaintCSOrderCreate(string accountNumber, string PMACT, string remarks = "")
        {
            ComplaintRegistrationResponse result = new ComplaintRegistrationResponse(); ;

            try
            {
                accountNumber = accountNumber.TrimStart(new Char[] { '0' });
                string registerComplaintCSOrderCreateUrl = Sitecore.Configuration.Settings.GetSetting("RegisterComplaintCSOrderCreateUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(registerComplaintCSOrderCreateUrl);

                var client = new ZBAPI_CS_ORDER_CREATE_website.ZBAPI_CS_ORDER_CREATEClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new ZBAPI_CS_ORDER_CREATE_website.DT_CS_ORDER_CREATE_Req
                {
                    CA = accountNumber,
                    ORDERTYPE = PMACT == "I15" ? "ZDIV" : "ZMSC",
                    PMACT = PMACT,
                    PROCGROUP = "61",
                    IT_TEXT_LINES = new ZBAPI_CS_ORDER_CREATE_website.DT_CS_ORDER_CREATE_ReqItem4[]
                    {
                        new ZBAPI_CS_ORDER_CREATE_website.DT_CS_ORDER_CREATE_ReqItem4
                        {
                            TDLINE=remarks
                        }
                    }
                };

                var response = client.ZBAPI_CS_ORDER_CREATE(request);
                result = new ComplaintRegistrationResponse
                {
                    AccountNumber = accountNumber,
                    AUFNR = response.AUFNR,
                    Message = response.MESSAGE,
                    ComplaintNumber = response.AUFNR,
                    IsRegistered = string.IsNullOrEmpty(response.AUFNR) ? false : true
                };
                return result;
            }
            catch (Exception ex)
            {
                return result = new ComplaintRegistrationResponse
                {
                    AccountNumber = accountNumber,
                    Error = ex.Message
                };
            }
        }

        public static List<ComplaintDetailsIGR> FetchComplaints(string accountNumber)
        {
            List<ComplaintDetailsIGR> result = new List<ComplaintDetailsIGR>();
            try
            {
                accountNumber = accountNumber.TrimStart(new Char[] { '0' });
                string fetchComplaintsUrl = Sitecore.Configuration.Settings.GetSetting("FetchComplaintsUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(fetchComplaintsUrl);

                var client = new SI_compstatnewService_Website.SI_compstatnewClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_compstatnewService_Website.DT_compstatnew_Req
                {
                    IT_INPUT = new SI_compstatnewService_Website.DT_compstatnew_ReqItem[]
                    {
                        new SI_compstatnewService_Website.DT_compstatnew_ReqItem
                        {
                            VKONT=accountNumber
                        }
                    },
                    MONTH = "2"
                };

                var response = client.SI_compstatnew(request);

                if (response.IT_OUTPUT != null && response.IT_OUTPUT.Count() > 0)
                {
                    foreach (var r in response.IT_OUTPUT)
                    {
                        result.Add(new ComplaintDetailsIGR
                        {
                            AUFNR = r.AUFNR,
                            AUART = r.AUART,
                            Complaint_Status = r.COMPLAINT_STATUS,
                            Complaint_Type = r.ZCOMPTYP,
                            ERDate = r.ERDAT,
                            GLTRP = r.GLTRP,
                            ILART = r.ILART,
                            IPHAS = r.IPHAS
                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public static SDOptInResult SDInstallmentOptInPost(string accountNumber, string noOfInstallment, string source)
        {
            SDOptInResult result = new SDOptInResult();
            try
            {
                accountNumber = FormatAccountNumber(accountNumber);

                decimal SDAmount = Convert.ToDecimal(FetchSecurityDepositAmountDetails(accountNumber).SDAmount);
                string SDInstallmentOptInUrl = Sitecore.Configuration.Settings.GetSetting("SDInstallmentOptInUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(SDInstallmentOptInUrl);

                var client = new SI_UPD_ZSDINSTLMNT_Website.SI_UPD_ZSDINSTLMNTClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                string dateString = DateTime.Now.ToString("yyyyMMdd");
                string timeString = DateTime.Now.ToString("HHmmss");

                if (source == null)
                    source = "1";
                var response = client.SI_UPD_ZSDINSTLMNT(accountNumber, noOfInstallment, SDAmount, source, dateString, timeString);

                result.IsSuccess = true;
                result.Message = response.MESSAGE;
                result.Number = response.NUMBER;
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        //public static ITDeclarationResult ITDeclarationPost(string accountNumber, ITDeclarationInfo model)
        //{
        //    ITDeclarationResult result = new ITDeclarationResult();
        //    try
        //    {
        //        accountNumber = FormatAccountNumber(accountNumber);

        //        string ITDeclarationPostUrl = Sitecore.Configuration.Settings.GetSetting("ITDeclarationPostUrl");
        //        System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(ITDeclarationPostUrl);

        //        var client = new SI_FI_WEB_ITAX_Website.SI_FI_WEB_ITAXClient(binding(), remoteAddress);
        //        SetClientCredentials(client.ClientCredentials);

        //        //if (model.DeclarationType == "1")
        //        //{
        //        //    var response = client.SI_FI_WEB_ITAX(
        //        //        new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
        //        //        {
        //        //            ZFLAG_DECLARATION = "X",
        //        //            ZFLAG = "U",
        //        //            ZSOURCE = "1"
        //        //        },
        //        //       new SI_FI_WEB_ITAX_Website.BAPIRET2
        //        //       {
        //        //           TYPE = "",
        //        //           ID = "",
        //        //           NUMBER = "",
        //        //           MESSAGE = "",
        //        //           LOG_NO = "",
        //        //           LOG_MSG_NO = "",
        //        //           MESSAGE_V1 = "",
        //        //           MESSAGE_V2 = "",
        //        //           MESSAGE_V3 = "",
        //        //           MESSAGE_V4 = "",
        //        //           PARAMETER = "",
        //        //           ROW = "",
        //        //           FIELD = "",
        //        //           SYSTEM = "",
        //        //       }
        //        //        , new SI_FI_WEB_ITAX_Website.ZFIDECLARATION
        //        //        {
        //        //            VKONT = accountNumber,
        //        //            ZDECLARATION_TYPE = "139AA",
        //        //            DATE_TO = DateTime.Now.ToString(), //End Fin Year
        //        //            DATE_FROM = DateTime.Now.ToString(),
        //        //            ZOPTION = model.AgreeOption == "1" ? "Y" : "NA",
        //        //            PAN_NO = model.PANNumber,
        //        //            AADHAR_NO = model.AadharNumber,
        //        //            CREATED_ON = DateTime.Now.ToString(),
        //        //            SOURCE = model.Source,
        //        //            USER_ID = "",
        //        //            FIN_YR1 = "",
        //        //            IT_ACK_NO1 = "",
        //        //            FILING_DATE1 = "",
        //        //            FIN_YR2 = "",
        //        //            IT_ACK_NO2 = "",
        //        //            FILING_DATE2 = ""
        //        //        },
        //        //       new SI_FI_WEB_ITAX_Website.ZFINTIMATION
        //        //       {
        //        //           VKONT = accountNumber,
        //        //           PAYMENT_DATE = "",
        //        //           PAN_NO = "",
        //        //           BILL_PAYABLE_AMT = 0,
        //        //           PAYMENT_AMT = 0,
        //        //           TDS_RATE = "",
        //        //           TDS_AMOUNT = "",
        //        //           DOCUMENT_NO = "",
        //        //           POSTING_DATE = "",
        //        //           SOURCE = "",
        //        //           USER_ID = "",
        //        //           CREATED_ON = ""
        //        //       }
        //        //        );
        //        //}
        //        //else if (model.DeclarationType == "2")
        //        //{
        //        //    var response = client.SI_FI_WEB_ITAX(
        //        //        new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
        //        //        {
        //        //            ZFLAG_DECLARATION = "X",
        //        //            ZFLAG = "U",
        //        //            ZSOURCE = "1"
        //        //        },
        //        //       new SI_FI_WEB_ITAX_Website.BAPIRET2
        //        //       {

        //        //       }
        //        //        , new SI_FI_WEB_ITAX_Website.ZFIDECLARATION
        //        //        {
        //        //            VKONT = accountNumber,
        //        //            ZDECLARATION_TYPE = "139AA",
        //        //            DATE_TO = DateTime.Now.ToString(), //End Fin Year
        //        //            DATE_FROM = DateTime.Now.ToString(),
        //        //            ZOPTION = "",
        //        //            PAN_NO = "",
        //        //            AADHAR_NO = "",
        //        //            CREATED_ON = DateTime.Now.ToString(),
        //        //            SOURCE = "1",
        //        //            USER_ID = "",
        //        //            FIN_YR1 = "",
        //        //            IT_ACK_NO1 = "",
        //        //            FILING_DATE1 = "",
        //        //            FIN_YR2 = "",
        //        //            IT_ACK_NO2 = "",
        //        //            FILING_DATE2 = ""
        //        //        },
        //        //       new SI_FI_WEB_ITAX_Website.ZFINTIMATION
        //        //       {
        //        //           VKONT = accountNumber,
        //        //           PAYMENT_DATE = "",
        //        //           PAN_NO = "",
        //        //           BILL_PAYABLE_AMT = "",
        //        //           PAYMENT_AMT = "",
        //        //           TDS_RATE = "",
        //        //           TDS_AMOUNT = "",
        //        //           DOCUMENT_NO = "",
        //        //           POSTING_DATE = "",
        //        //           SOURCE = "",
        //        //           USER_ID = "",
        //        //           CREATED_ON = ""
        //        //       }
        //        //        );
        //        //}
        //        //else if (model.DeclarationType == "3")
        //        //{
        //        //    var response = client.SI_FI_WEB_ITAX(
        //        //        new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
        //        //        {
        //        //            ZFLAG_DECLARATION = "X",
        //        //            ZFLAG = "U",
        //        //            ZSOURCE = "1"
        //        //        },
        //        //       new SI_FI_WEB_ITAX_Website.BAPIRET2
        //        //       {

        //        //       }
        //        //        , new SI_FI_WEB_ITAX_Website.ZFIDECLARATION
        //        //        {
        //        //            VKONT = accountNumber,
        //        //            ZDECLARATION_TYPE = "139AA",
        //        //            DATE_TO = DateTime.Now.ToString(), //End Fin Year
        //        //            DATE_FROM = DateTime.Now.ToString(),
        //        //            ZOPTION = "",
        //        //            PAN_NO = "",
        //        //            AADHAR_NO = "",
        //        //            CREATED_ON = DateTime.Now.ToString(),
        //        //            SOURCE = "1",
        //        //            USER_ID = "",
        //        //            FIN_YR1 = "",
        //        //            IT_ACK_NO1 = "",
        //        //            FILING_DATE1 = "",
        //        //            FIN_YR2 = "",
        //        //            IT_ACK_NO2 = "",
        //        //            FILING_DATE2 = ""
        //        //        },
        //        //       new SI_FI_WEB_ITAX_Website.ZFINTIMATION
        //        //       {
        //        //           VKONT = accountNumber,
        //        //           PAYMENT_DATE = "",
        //        //           PAN_NO = "",
        //        //           BILL_PAYABLE_AMT = "",
        //        //           PAYMENT_AMT = "",
        //        //           TDS_RATE = "",
        //        //           TDS_AMOUNT = "",
        //        //           DOCUMENT_NO = "",
        //        //           POSTING_DATE = "",
        //        //           SOURCE = "",
        //        //           USER_ID = "",
        //        //           CREATED_ON = ""
        //        //       }
        //        //        );
        //        //}
        //        //else if (model.DeclarationType == "4")
        //        //{
        //        //    var response = client.SI_FI_WEB_ITAX(
        //        //        new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
        //        //        {
        //        //            ZFLAG_DECLARATION = "X",
        //        //            ZFLAG = "U",
        //        //            ZSOURCE = "1"
        //        //        },
        //        //       new SI_FI_WEB_ITAX_Website.BAPIRET2
        //        //       {

        //        //       }
        //        //        , new SI_FI_WEB_ITAX_Website.ZFIDECLARATION
        //        //        {
        //        //            VKONT = accountNumber,
        //        //            ZDECLARATION_TYPE = "139AA",
        //        //            DATE_TO = DateTime.Now.ToString(), //End Fin Year
        //        //            DATE_FROM = DateTime.Now.ToString(),
        //        //            ZOPTION = "",
        //        //            PAN_NO = "",
        //        //            AADHAR_NO = "",
        //        //            CREATED_ON = DateTime.Now.ToString(),
        //        //            SOURCE = "1",
        //        //            USER_ID = "",
        //        //            FIN_YR1 = "",
        //        //            IT_ACK_NO1 = "",
        //        //            FILING_DATE1 = "",
        //        //            FIN_YR2 = "",
        //        //            IT_ACK_NO2 = "",
        //        //            FILING_DATE2 = ""
        //        //        },
        //        //       new SI_FI_WEB_ITAX_Website.ZFINTIMATION
        //        //       {
        //        //           VKONT = accountNumber,
        //        //           PAYMENT_DATE = "",
        //        //           PAN_NO = "",
        //        //           BILL_PAYABLE_AMT = "",
        //        //           PAYMENT_AMT = "",
        //        //           TDS_RATE = "",
        //        //           TDS_AMOUNT = "",
        //        //           DOCUMENT_NO = "",
        //        //           POSTING_DATE = "",
        //        //           SOURCE = "",
        //        //           USER_ID = "",
        //        //           CREATED_ON = ""
        //        //       }
        //        //        );
        //        //}

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return result;
        //    }
        //}

        public static ITDeclarationResult ITDeclarationPost(string accountNumber, ITDeclarationInfo model)
        {
            ITDeclarationResult result = new ITDeclarationResult();
            try
            {
                model.Source = string.IsNullOrEmpty(model.Source) ? "1" : model.Source;
                accountNumber = FormatAccountNumber(accountNumber);
                string declarationTypeString = "139AA";
                if (model.DeclarationType == "2")
                    declarationTypeString = "194Q";
                if (model.DeclarationType == "3")
                    declarationTypeString = "206CCA";


                string ITDeclarationPostUrl = Sitecore.Configuration.Settings.GetSetting("ITDeclarationPostUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(ITDeclarationPostUrl);

                var client = new SI_FI_WEB_ITAX_Website.SI_FI_WEB_ITAXClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_FI_WEB_ITAX_Website.SI_FI_WEB_ITAXRequest();

                int FYEndYear = DateTime.Now.Year;
                if (DateTime.Now.Month >= 4)
                {
                    FYEndYear = DateTime.Now.Year + 1;
                };

                //Date at Which a Time Slice Expires
                string fyEndDate = (new DateTime(FYEndYear, 3, 31)).ToString("yyyyMMdd");


                int currentFYYear = DateTime.Now.Year;
                if (DateTime.Now.Month <= 3)
                {
                    currentFYYear = DateTime.Now.Year - 1;
                };

                string FY_3 = (currentFYYear - 3).ToString() + "-" + ((currentFYYear - 2) - 2000).ToString();
                string FY_2 = (currentFYYear - 2).ToString() + "-" + ((currentFYYear - 1) - 2000).ToString();

                if (model.DeclarationType == "1")
                {
                    request.IT_ZFIDECLARATION = new SI_FI_WEB_ITAX_Website.ZFIDECLARATION[] {
                    new SI_FI_WEB_ITAX_Website.ZFIDECLARATION
                        {
                           VKONT = accountNumber,
                           ZDECLARATION_TYPE =declarationTypeString, //139AA , 194Q or 206CCA
                           DATE_TO = fyEndDate,
                           DATE_FROM=DateTime.Now.ToString("yyyyMMdd"),
                           ZOPTION=model.AgreeOption=="1"?"Y":"NA",
                           PAN_NO = model.PANNumber,
                           AADHAR_NO=model.AadharNumber,
                           CREATED_ON=DateTime.Now.ToString("yyyyMMdd"),
                           SOURCE=model.Source
                        }
                    };

                    request.IM_INPUT = new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
                    {
                        ZFLAG_DECLARATION = "X",
                        ZFLAG = "U",
                        ZSOURCE = model.Source
                    };
                }
                else if (model.DeclarationType == "2")
                {
                    request.IT_ZFIDECLARATION = new SI_FI_WEB_ITAX_Website.ZFIDECLARATION[] {
                    new SI_FI_WEB_ITAX_Website.ZFIDECLARATION
                        {
                           VKONT = accountNumber,
                           ZDECLARATION_TYPE =declarationTypeString, //139AA , 194Q or 206CCA
                           DATE_TO = fyEndDate,
                           DATE_FROM=DateTime.Now.ToString("yyyyMMdd"),
                           ZOPTION=model.AgreeOption=="1"?"Y":"NA",
                           PAN_NO = model.PANNumber,
                           //AADHAR_NO=model.AadharNumber,
                           CREATED_ON=DateTime.Now.ToString("yyyyMMdd"),
                           SOURCE=model.Source
                        }
                    };

                    request.IM_INPUT = new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
                    {
                        ZFLAG_DECLARATION = "X",
                        ZFLAG = "U",
                        ZSOURCE = model.Source
                    };

                }
                else if (model.DeclarationType == "3")
                {
                    request.IT_ZFIDECLARATION = new SI_FI_WEB_ITAX_Website.ZFIDECLARATION[] {
                    new SI_FI_WEB_ITAX_Website.ZFIDECLARATION
                        {
                           VKONT = accountNumber,
                           ZDECLARATION_TYPE =declarationTypeString, //139AA , 194Q or 206CCA
                           DATE_TO = fyEndDate,
                           DATE_FROM=DateTime.Now.ToString("yyyyMMdd"),
                           ZOPTION=model.AgreeOption=="1"?"Y":"NA",
                           PAN_NO = model.PANNumber,
                           //AADHAR_NO=model.AadharNumber,
                           CREATED_ON=DateTime.Now.ToString("yyyyMMdd"),
                           SOURCE=model.Source,
                           FIN_YR1=FY_3,
                           IT_ACK_NO1=model.FY_3AcknowledgementNumber,
                           FILING_DATE1=string.IsNullOrEmpty(model.FY_3DateOfFilingReturn)?null: (DateTime.ParseExact(model.FY_3DateOfFilingReturn, "dd/MM/yyyy", CultureInfo.CurrentCulture)).ToString("yyyyMMdd"),
                           FIN_YR2=FY_2,
                           IT_ACK_NO2=model.FY_2AcknowledgementNumber,
                           FILING_DATE2=string.IsNullOrEmpty(model.FY_2DateOfFilingReturn)?null:(DateTime.ParseExact(model.FY_2DateOfFilingReturn, "dd/MM/yyyy", CultureInfo.CurrentCulture)).ToString("yyyyMMdd"),
                        }
                    };

                    request.IM_INPUT = new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
                    {
                        ZFLAG_DECLARATION = "X",
                        ZFLAG = "U",
                        ZSOURCE = model.Source
                    };

                }
                if (model.DeclarationType == "4")
                {
                    decimal rate = 0.1M;
                    decimal TDS_Amount = string.IsNullOrEmpty(model.TDS_AMOUNT) ? 0 : Math.Ceiling(Convert.ToDecimal(model.TDS_AMOUNT) * rate / 100);
                    request.IT_ZFINTIMATION = new SI_FI_WEB_ITAX_Website.ZFINTIMATION[] {
                    new SI_FI_WEB_ITAX_Website.ZFINTIMATION
                        {
                           VKONT = accountNumber,
                          PAYMENT_DATE=string.IsNullOrEmpty(model.POSTING_DATE)?null:(DateTime.ParseExact(model.POSTING_DATE, "dd/MM/yyyy", CultureInfo.CurrentCulture)).ToString("yyyyMMdd"), //input date posting date
                           PAN_NO = model.PANNumber,
                           BILL_PAYABLE_AMT=string.IsNullOrEmpty(model.BILL_PAYABLE_AMT)?0: Convert.ToDecimal( model.BILL_PAYABLE_AMT), //fetched amount
                           PAYMENT_AMT=string.IsNullOrEmpty(model.TDS_AMOUNT)?0:Convert.ToDecimal(model.TDS_AMOUNT), //fetched amount
                           TDS_RATE=model.TDS_RATE,  //0.1
                            TDS_AMOUNT=TDS_Amount, //input by consumer
                            //DOCUMENT_NO=model.DOCUMENT_NO,
                            //POSTING_DATE=model.POSTING_DATE,
                           CREATED_ON=DateTime.Now.ToString("yyyyMMdd"),
                           SOURCE=model.Source,
                           PAYMENT_AMTSpecified=true,
                           BILL_PAYABLE_AMTSpecified=true,
                           TDS_AMOUNTSpecified=true
                        }
                    };

                    request.IM_INPUT = new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
                    {
                        ZFLAG_INTIMATION = "X",
                        ZFLAG = "U",
                        ZSOURCE = model.Source
                    };

                }

                var response = client.SI_FI_WEB_ITAX(request.IM_INPUT, ref request.IT_RETURN, ref request.IT_ZFIDECLARATION, ref request.IT_ZFINTIMATION);

                if (request.IT_RETURN != null && request.IT_RETURN.Count() > 0)
                {
                    result.Message = request.IT_RETURN[0].MESSAGE;
                    if (request.IT_RETURN[0].NUMBER == "001")
                        result.IsSuccess = true;
                    else
                        result.IsSuccess = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }


        public static ITDeclarationResult ITDeclarationCheck(string accountNumber, string declarationType, string PANNumber)
        {
            ITDeclarationResult result = new ITDeclarationResult();
            try
            {
                accountNumber = FormatAccountNumber(accountNumber);
                string declarationTypeString = "139AA";
                if (declarationType == "2")
                    declarationTypeString = "194Q";
                if (declarationType == "3")
                    declarationTypeString = "206CCA";

                string ITDeclarationPostUrl = Sitecore.Configuration.Settings.GetSetting("ITDeclarationPostUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(ITDeclarationPostUrl);

                var client = new SI_FI_WEB_ITAX_Website.SI_FI_WEB_ITAXClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_FI_WEB_ITAX_Website.SI_FI_WEB_ITAXRequest();

                int FYEndYear = DateTime.Now.Year;
                if (DateTime.Now.Month >= 4)
                {
                    FYEndYear = DateTime.Now.Year + 1;
                };

                //Date at Which a Time Slice Expires
                string fyEndDate = (new DateTime(FYEndYear, 3, 31)).ToString("yyyyMMdd");

                request.IT_ZFIDECLARATION = new SI_FI_WEB_ITAX_Website.ZFIDECLARATION[] {
                    new SI_FI_WEB_ITAX_Website.ZFIDECLARATION
                    {
                       VKONT = accountNumber,
                       ZDECLARATION_TYPE =declarationTypeString, //139AA , 194Q or 206CCA
                       DATE_TO = fyEndDate,
                       PAN_NO = PANNumber,
                       SOURCE="1"
                    }
                };
                request.IM_INPUT = new SI_FI_WEB_ITAX_Website.ZST_FI_WEB_ITAX
                {
                    ZFLAG_DECLARATION = "X",
                    ZFLAG = "D",
                    ZSOURCE = "1"
                };
                var response = client.SI_FI_WEB_ITAX(request.IM_INPUT, ref request.IT_RETURN, ref request.IT_ZFIDECLARATION, ref request.IT_ZFINTIMATION);

                if (request.IT_RETURN != null && request.IT_RETURN.Count() > 0)
                {
                    result.Message = request.IT_RETURN[0].MESSAGE;
                    if (request.IT_RETURN[0].NUMBER == "001")
                        result.IsSuccess = true;
                    else
                        result.IsSuccess = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string FetchAmountForTDS(string accountNumber)
        {
            try
            {
                string paymentHistroyUrl = Sitecore.Configuration.Settings.GetSetting("billDetailsFetchServiceUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(paymentHistroyUrl);

                var client = new SI_MR_BI_GETLIST_inClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new DT_MR_BI_GETLIST_req { CA_NUMBER = Helper.FormatAccountNumber(accountNumber) };

                var response = client.SI_MR_BI_GETLIST_inAsync(request);
                response.Wait();

                var responseData = response.Result.MT_MR_BI_GETLIST_resp.EXPORT;

                string PAY_PPI_DATE = responseData.PAY_PPI_DATE;
                string PAY_DUE_DATE = responseData.PAY_DUE_DATE;

                if (!string.IsNullOrEmpty(PAY_PPI_DATE) && !string.IsNullOrEmpty(PAY_DUE_DATE))
                {
                    DateTime PAY_PPI_DATE_Obj = Convert.ToDateTime(PAY_PPI_DATE);
                    DateTime PAY_DUE_DATE_Obj = Convert.ToDateTime(PAY_DUE_DATE);
                    if (DateTime.Now <= PAY_PPI_DATE_Obj)
                        return responseData.PPI_BILL_AMT;
                    else if (DateTime.Now > PAY_DUE_DATE_Obj)
                        return responseData.DPC_BILL_AMT;
                    else if (PAY_PPI_DATE_Obj <= DateTime.Now && DateTime.Now <= PAY_DUE_DATE_Obj)
                        return responseData.DUE_DATE_BILL_AMT;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public static ComplaintDetailsStatus ExtrOrdGISService(string orderId)
        {
            try
            {
                string ExtrOrdGISServiceUrl = Sitecore.Configuration.Settings.GetSetting("ExtrOrdGISServiceUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(ExtrOrdGISServiceUrl);

                var client = new SI_ExtrOrd_GISService.SI_ExtrOrd_GISClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_ExtrOrd_GISService.ZBAPI_CS_ORDER_EXT1
                {
                    ORDER_NO = new SI_ExtrOrd_GISService.ZBAPI_CS_ORDER_EXT[]
                    {
                        new SI_ExtrOrd_GISService.ZBAPI_CS_ORDER_EXT
                        {
                            ORD_NO=orderId
                        }
                    }
                };

                var response = client.SI_ExtrOrd_GIS(request);

                if (response.IT_ORD != null)
                {
                    ComplaintDetailsStatus ComplaintDetailsStatusobj = new ComplaintDetailsStatus
                    {
                        CreatedDate = response.IT_ORD.FirstOrDefault().ERDAT,
                        Complaint_Status = response.IT_ORD.FirstOrDefault().IPHAS,
                        CompletionDate = response.IT_ORD.FirstOrDefault().IDAT2,
                        ZoneName = response.IT_ORD.FirstOrDefault().ARBPL,
                        PMActivityType = response.IT_ORD.FirstOrDefault().ILART
                    };
                    return ComplaintDetailsStatusobj;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        #region For New connection

        //ValidateLEC

        public static BankAccountDetails CheckBankDetails(string BankKey)
        {
            try
            {
                string checkBankAccountDetailsUrl = Sitecore.Configuration.Settings.GetSetting("CheckBankAccountDetailsUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(checkBankAccountDetailsUrl);

                var client = new SI_CS_CHECKBANKL_Website.SI_CS_CHECKBANKLClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_CS_CHECKBANKL_Website.DT_CS_CHECKBANKL_Req
                {
                    BANKCOUNTRY = "IN",
                    BANKKEY = BankKey
                };

                var response = client.SI_CS_CHECKBANKL(request);

                BankAccountDetails result = new BankAccountDetails
                {
                    Flag = response.FLAG,
                    BANK_BRANCH = response.BANK_ADDRESS.BANK_BRANCH,
                    BANK_COUNTRY = response.BANK_ADDRESS.BANK_COUNTRY,
                    BANK_KEY = response.BANK_ADDRESS.BANK_KEY,
                    BANK_NAME = response.BANK_ADDRESS.BANK_NAME,
                    CITY = response.BANK_ADDRESS.CITY,
                    CREATED_BY = response.BANK_ADDRESS.CREATED_BY,
                    CREATED_ON = response.BANK_ADDRESS.CREATED_ON,
                    REGION = response.BANK_ADDRESS.REGION,
                    STREET = response.BANK_ADDRESS.STREET,
                    SWIFT_CODE = response.BANK_ADDRESS.SWIFT_CODE
                };

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool ValidateTRNumber(string TRNumber, string LECNumber)
        {
            try
            {
                string updateTRNumberUrl = Sitecore.Configuration.Settings.GetSetting("UpdateTRNumberUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(updateTRNumberUrl);

                var client = new SI_Update_TR_Website.SI_Update_TRClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_Update_TR_Website.DT_Update_TR_Req
                {

                    TR_NUMBER = TRNumber,
                    ZZ_LICNO1 = LECNumber
                };

                var response = client.SI_Update_TR(request);

                if (response.FLAG == "2" && response.VALIDF == "1")
                    return true;
                else
                    return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool ValidateLDP(string LDPNumber)
        {
            try
            {
                string ValidateLDPUrl = Sitecore.Configuration.Settings.GetSetting("ValidateLDPUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(ValidateLDPUrl);

                var client = new SI_LDPval_send_Website.SI_LDPval_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_LDPval_send_Website.DT_LDPvalreq
                {
                    AUFNR = LDPNumber
                };

                var response = client.SI_LDPval_send(request);

                if (response.EXPORTTABLE != null && response.EXPORTTABLE.Count() > 0 && !string.IsNullOrEmpty(response.EXPORTTABLE[0].ORDERID))
                    return true;
                else
                    return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static OrderFetchResult GetOrderIdForNewCon(string tempRegistrationNumber)
        {
            OrderFetchResult result = new OrderFetchResult();
            try
            {
                string orderCreationSolarUrl = Sitecore.Configuration.Settings.GetSetting("OrderCreationSolarUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(orderCreationSolarUrl);

                var client = new SI_CSWebmaster_sendService.SI_CSWebmaster_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_CSWebmaster_sendService.DT_CS_Webmaster_req
                {
                    ZTEMPNO = tempRegistrationNumber,
                    FLAG_CRT = "X"
                };

                var response = client.SI_CSWebmaster_send(request);
                result.IsSuccess = true;
                result.ExceptionMessage = response.MESSAGE;
                result.OrderIdSAP = response.AUFNR;
                result.BusinessPartnerNumber = response.PARTNER;
                result.ContractAccountNumber = response.VKONT;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ExceptionMessage = ex.Message;
                return result;
            }
        }

        public static PostDataResult PostDataForNewCon(PostDataWebUpdate applicationDetails, List<PostDocumentsWebUpdate> applicationDocuments)
        {
            PostDataResult result = new PostDataResult();
            try
            {
                string postDataForSolarUrl = Sitecore.Configuration.Settings.GetSetting("PostDataSolarUrl");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(postDataForSolarUrl);

                var client = new SI_CSWebupdate_send_Website.SI_CSWebupdate_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_CSWebupdate_send_Website.DT_CS_Webupdatereq();
                request.FLAG_APPL = "X";
                request.FLAG_ADRC = "X";
                request.FLAG_LOAD = "X";
                request.FLAG_DOC = "X";
                request.FLAG_LDP1 = "";
                request.FLAG_LDP2 = "";
                request.FLAG_OPAC = "";
                request.FLAG_SLR = "";

                request.IT_APPL = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqIT_ITEM[] {
                    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqIT_ITEM{
                        ZTEMPNO=applicationDetails.TempRegistrationNumber,
                        BKONT="",
                        MANDT="",
                        Z_VERSION="000",
                        ZALTYP="NCO",
                        ZALSTYP=applicationDetails.TotalLoad,
                        ZAPPLBY= "11",    //applicationDetails.IsLEC?"12":"11",
                        ZDATE_REG=DateTime.Now.ToString("yyyyMMdd"),
                        ZANLZU=applicationDetails.MeterCabin,
                        ZTYPE =applicationDetails.ConnectionType,
                        ZATYPE=applicationDetails.SectorType,
                        Z_VBEGIN=applicationDetails.TempStartDate,
                        Z_VENDE=applicationDetails.TempEndDate,
                        ZLDP_ORDER=applicationDetails.LDPNumber,
                        ZPSUPP=applicationDetails.PurposeofSupply,
                        ZALTARIFF=applicationDetails.AppliedTariff,
                        ZPRETYP=applicationDetails.PremiseType,
                        ZNRCA=applicationDetails.NearestCAnumber,
                        ZNRMT=applicationDetails.NearestMeternumber,
                        ZMTRTYP=applicationDetails.MeterLoad,
                        ZZ_LICNO=applicationDetails.LECNumber,
                        ZMTROWN="",
                        ZFLAG_SPACEAVAIL="",
                        ZBANKN=applicationDetails.BankAccountNumber,
                        ZBANKL=applicationDetails.MICR,
                        ZBANKA=applicationDetails.Bank,
                        ZBRNCH=applicationDetails.Branch,
                        ZVKONT_EXT=applicationDetails.ConsumerNumber,
                        ZUTILITY=applicationDetails.Utility,
                        ZVOLTLVL=applicationDetails.VoltageLevel,
                        ZFLAG_WIR=applicationDetails.WiringCompleted,

                       ZFLAG_GREENENERGY=applicationDetails.IsGreenTariffApplied,
                       ZFLAG_SELFMETER=applicationDetails.SelfMeter,
                        ERDAT= DateTime.Now.ToString("yyyyMMdd"),



                    }
                };


                request.IT_ADRS = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM[] {
                    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM{
                        ZTEMPNO=applicationDetails.TempRegistrationNumber,
                        MANDT="",
                        Z_VERSION="000",
                        ARBPL=applicationDetails.Workcenter,
                        ZNAME_R=applicationDetails.NameofRentalOwner,
                        ZSTREET1_R=applicationDetails.RentalLane,
                        TITLE=applicationDetails.ApplicationTitle,
                        NAME_ORG1=applicationDetails.OrganizationName,
                         ZFLAG_EBILL=applicationDetails.BillingFormatEBill,
                         ZFLAG_EP_BILL=applicationDetails.BillingFormatEPBill,
                        NAME_FIRST=applicationDetails.FirstName,
                        NAME_MIDDLE=applicationDetails.MiddleName,
                        NAME_LAST=applicationDetails.LastName,
                        HOUSE_NUM1=applicationDetails.HouseNumber,
                        STREET=applicationDetails.BuildingName,
                        STR_SUPPL1=applicationDetails.LaneStreet,
                        STR_SUPPL2=applicationDetails.Landmark,
                        ZSUBURB=applicationDetails.Suburb,
                        CITY1=applicationDetails.City,
                        POST_CODE1=applicationDetails.Pincode,
                       ZFLAG_BILLADR=applicationDetails.billingdifferentthanAddresswheresupply,
                       ZHOUSE_NO_B=applicationDetails.BillingHouseNumber,
                       ZSTREET_B=applicationDetails.BillingBuildingName,
                       ZSTREET1_B=applicationDetails.BillingLane,
                       ZSTREET2_B=applicationDetails.BillingLandmark,
                       ZSURB_B=applicationDetails.BillingSuburb,
                       ZPIN_B=applicationDetails.BillingPincode,
                       ZCITY1_B="",
                       ZFLAG_RENT=applicationDetails.RentalAddress,
                       ZHOUSE_NO_R=applicationDetails.RentalHouseNumber,
                       ZSTREET_R=applicationDetails.RentalBuildingName,
                       ZSTREET2_R="",
                       ZSURB_R=applicationDetails.RentalSuburb,
                       ZPIN_R=applicationDetails.RentalPincode,
                       ZCITY1_R="",
                       ZTEL_R="",
                      ZMOB_R=applicationDetails.RentalMobileNumber,
                      ZEML_R=applicationDetails.RentalEmail,
                     ZMOB=applicationDetails.MobileNumber,
                     ZTELE=applicationDetails.LandlineNumber,
                     ZEMAIL=applicationDetails.Email,
                     LANGU=applicationDetails.BillLangianguage,

                    ERDAT=DateTime.Now.ToString("yyyyMMdd"),
                    }
                };
                if (applicationDetails.MeterTypeCount1PH == "1")
                {
                    applicationDetails.MeterTypeCount3PH = "0";
                    applicationDetails.MeterTypeCountHT = "0";
                    applicationDetails.ConnectedLoadKW3PH = "0";
                    applicationDetails.ConnectedLoadHP3PH = "0";
                    applicationDetails.ConnectedLoadKWHT = "0";
                    applicationDetails.ConnectedLoadHT = "0";

                }
                else if (applicationDetails.MeterTypeCount3PH == "1")
                {
                    applicationDetails.MeterTypeCount1PH = "0";
                    applicationDetails.MeterTypeCountHT = "0";
                    applicationDetails.ConnectedLoadKW1PH = "0";
                    applicationDetails.ConnectedLoadHP1PH = "0";
                    applicationDetails.ConnectedLoadKWHT = "0";
                    applicationDetails.ConnectedLoadHT = "0";
                }
                else if (applicationDetails.MeterTypeCountHT == "1")
                {
                    applicationDetails.MeterTypeCount1PH = "0";
                    applicationDetails.MeterTypeCount3PH = "0";
                    applicationDetails.ConnectedLoadKW1PH = "0";
                    applicationDetails.ConnectedLoadHP1PH = "0";
                    applicationDetails.ConnectedLoadKW3PH = "0";
                    applicationDetails.ConnectedLoadHP3PH = "0";
                }
                request.IT_LOAD = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM1[] {
                    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM1{
                        ZTEMPNO=applicationDetails.TempRegistrationNumber,
                        MANDT="",
                        Z_VERSION="000",
                        ZLTYP=applicationDetails.MeterLoad,
                        ZSRNUMBER=applicationDetails.TempRegistrationNumber,
                        ZCOUNT_1PH=applicationDetails.MeterTypeCount1PH,
                        ZALKW_1=applicationDetails.ConnectedLoadKW1PH,
                        ZALHP_1=applicationDetails.ConnectedLoadHP1PH,
                        ZCOUNT_3PH=applicationDetails.MeterTypeCount3PH,
                        ZALKW_3=applicationDetails.ConnectedLoadKW3PH,
                        ZALHP_3=applicationDetails.ConnectedLoadHP3PH,
                        ZCOUNT_HT=applicationDetails.MeterTypeCountHT,
                        ZALKW_H=applicationDetails.ConnectedLoadKWHT,
                        ZALHP_H=applicationDetails.ConnectedLoadHT,
                        ZALCD=applicationDetails.ContractDemand,
                        ERDAT=DateTime.Now.ToString("yyyyMMdd"),

            }

            };


                //Assuming only 2 documents will be there every time
                request.IT_DOC = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM2[applicationDocuments.Count];
                int idocument = 0;
                foreach (var document in applicationDocuments)
                {
                    request.IT_DOC[idocument] = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM2
                    {
                        ZALTYP = "SLT",
                        MANDT = "",
                        Z_VERSION = "000",
                        ZTEMPNO = applicationDetails.TempRegistrationNumber,
                        ZDESC = applicationDocuments[idocument].DocumentDescription,
                        Z_DOCTYPE = applicationDocuments[idocument].DocumentSerialNumber
                    };
                    idocument++;
                }

                request.IT_LDP1 = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM3[] {
                 new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM3
                 {
                    MANDT="",
                    ZTEMPNO="",
                    Z_VERSION="",
                    ZZ_IODCCID="",
                    ZZ_IODCCNO="",
                    ZZ_IODCCNO1="",
                    ZZ_IODDT="",
                    ZZ_IOD1DT="",
                    ZZ_CTSNO="",
                    ZZ_VALID_UPTO="",
                    ZZ_NAMEDEVELOPMT="",
                    ZZ_TELDEV="",
                    ZZ_MOBILE="",
                    ZZ_EMAIL="",
                    ZZ_DEV_TYPE="",
                    ZZ_PLOT_AREA=0,
                    ZTOT_PLOT_AREA_PR=0,
                    ZZ_SUB_PLOT_AREA=0,
                    ZZ_SUB_PLOT_PR=0,
                    ZZ_OTH_RESER_AR=0,
                    ZZ_OTH_RESER_PR=0,
                    ZZ_SET_BACK_AREA=0,
                    ZZ_SET_BACK_PR=0,
                    ZZ_AVLB_PLT_AREA=0,
                    ZZ_AVLB_PLOT_PR=0,
                    ZZ_FSI_N=0,
                    ZZ_FSI_N_PR=0,
                    ZZ_BLDG_N=0,
                    ZZ_BUILDING_PR=0,
                    ZZ_LOAD_PH1=0,
                    ZZ_LOAD_PH2=0,
                    ZZ_LOAD_PH3=0,
                    ZZ_LOAD_PH4=0,
                    ZZ_LOAD_PH5=0,
                    ZZ_DUEBY_PH1="",
                    ZZ_DUEBY_PH2="",
                    ZZ_DUEBY_PH3="",
                    ZZ_DUEBY_PH4="",
                    ZZ_DUEBY_PH5="",
                    ZZ_ARCH_ID="",
                    ZZ_ARCH_NAME="",
                   ZZ_ARCH_ADD1="",
                   ZZ_ARCH_ADD2="",
                   ZZ_ARCH_ADD3="",
                   ZZ_ARCH_TELNO="",
                   ZZ_ARCH_MOBNO="",
                   ZZ_ARCH_EMAIL="",
                   ERDAT="",
                   ZZ_MAHARERA="",
                   ZZ_NFFCL="",
                   ZZ_NFFMD="",
                   ZZ_AFFCL="",
                   ZZ_AFFMD="",
                   ZZ_CSCL="",
                   ZZ_CSMD="",
                   ZZ_NFFMTR="",
                   ZZ_AFFMTR="",
                   ZZ_CSMTR="",
                   ZZ_CSST="",
                   ZZ_NFFST="",
                   ZZ_AFFST="",








                 }
};
                request.IT_LDP2 = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqIT_ITEM1[] {
                 new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqIT_ITEM1
                 {
                     MANDT="",
                     ZTEMPNO="",
                     Z_VERSION="",
                     ZLTYP="",
                     ZFLAG="",
                     ZZ_CONNTYPE_EDU="",
                     ZZ_FLR="",
                     ZZ_BUILTAREA_FUN="",
                     ZZ_BUILTAREA="",
                     ZZ_LOAD=0,
                     ZZ_MD_LT=0,
                     ZZ_MD_HT=0,
                     ZZ_TOT_FLR="",
                     ZZ_TOT_BUILTAREA="",
                     ZZ_TOT_LOAD=0,
                     ZZ_TOT_MD_LT=0,
                     ZZ_TOT_MD_HT=0,
                     ERDAT="",
                     ZBLDG="",

                 }
                };
                request.IT_OPAC = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM4[]{
                 new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM4
                 {
MANDT="",
ZTEMPNO="",
ZALTYP="",
ZALSTYP="",
ZAPPLBY="",
ZDATE_REG="",
ZTIME="",
ZSUPTYP="",
ZTYP="",
Z_VBEGIN="",
Z_VENDE="",
ZSUPP="",
ZLOAD="",
ZVKONT="",
ZVERTRAG="",
ZANLAGE="",
ERDAT="",
ZTRADER_LEC="",
ZTRADER_NAME="",
ZFLAG1="",
ZFLAG2="",
DD_NUMBER="",
DD_BANK="",
RTGS_URN="",
AMOUNT="",
DUTY_DATE="",
COMMENCEMENT_DATE="",
CALCULATED_DATE="",
ZSRLOAD="",
                 }
                };
                //request.IT_DOC = new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM2[] {
                //    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM2{
                //        ZALTYP="SLT",
                //        ZTEMPNO=applicationDetails.TempRegistrationNumber,
                //        ZDESC=applicationDocuments[0].DocumentDescription,
                //        Z_DOCTYPE=applicationDocuments[0].DocumentSerialNumber
                //    },
                //    new SI_CSWebupdate_send_Website.DT_CS_WebupdatereqITEM2{
                //        ZALTYP="SLT",
                //        ZTEMPNO=applicationDetails.TempRegistrationNumber,
                //        ZDESC=applicationDocuments[1].DocumentDescription,
                //        Z_DOCTYPE=applicationDocuments[1].DocumentSerialNumber
                //    }
                //};

                string abc = JsonConvert.SerializeObject(request);
                var responseDetails = client.SI_CSWebupdate_send(request);
                result = new PostDataResult
                {
                    FLAG_UPD_APPL = responseDetails.FLAG_UPD_APPL,
                    FLAG_UPD_ADRC = responseDetails.FLAG_UPD_ADRC,
                    FLAG_UPD_LOAD = responseDetails.FLAG_UPD_LOAD,
                    FLAG_UPD_DOC = responseDetails.FLAG_UPD_DOC,
                    ExceptionMessage = responseDetails.MSG_SLR,
                };
                if (result.FLAG_UPD_APPL == "1" && result.FLAG_UPD_ADRC == "1" && result.FLAG_UPD_LOAD == "1" && result.FLAG_UPD_DOC == "1")
                    result.IsSuccess = true;
                else
                    result.IsSuccess = false;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ExceptionMessage = ex.Message;
                return result;
            }
        }

        public static TrackApplication TrackApplications(string refNumber)
        {
            TrackApplication result = new TrackApplication();
            try
            {
                string trackApplicationUrl = Sitecore.Configuration.Settings.GetSetting("CSNotiforderURL");
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(trackApplicationUrl);

                var client = new SI_CSNotiforder_send_Web.SI_CSNotiforder_sendClient(binding(), remoteAddress);
                SetClientCredentials(client.ClientCredentials);

                var request = new SI_CSNotiforder_send_Web.DT_CS_Notiforder_req
                {
                    ZAUFNR = refNumber,

                };
                var response = client.SI_CSNotiforder_send(request);
                if (response.IT_MILESTONE != null)
                {
                    if (response.IT_MILESTONE[0].ZNCOSITEVISIT != null)
                    {
                        result.SitVisistTobeDone = response.IT_MILESTONE[0].ZNCOSITEVISIT;
                    }
                    if (response.IT_MILESTONE[0].Z_PRINTDATE != null)
                    {
                        result.ComplianceTobeMateByApplication = response.IT_MILESTONE[0].Z_PRINTDATE;
                    }
                    if (response.IT_MILESTONE[0].ZDINSITEVISIT != null)
                    {
                        result.SentToMeterInstallation = response.IT_MILESTONE[0].ZDINSITEVISIT;
                    }
                }
                result.MNKAT = response.IT_NONCOMPOTH[0].MNKAT;
                result.MNGRP = response.IT_NONCOMPOTH[0].MNGRP;
                result.MNCOD = response.IT_NONCOMPOTH[0].MNCOD;
                result.MNGRPNonComplaineByapplicant = response.IT_NONCOMPCUST[0].MNGRP;
                if (result.MNKAT == "S" && result.MNGRP == "AC001" && result.MNCOD == "1075")
                {
                    result.SiteVisistDone = response.IT_NONCOMPOTH[0].ERDAT;
                }
                if (result.MNKAT == "S" && result.MNGRP == "AC001" && result.MNCOD == "1155")
                {
                    result.EstimateCreated = response.IT_NONCOMPOTH[0].ERDAT;

                }
                if (result.MNKAT == "S" && result.MNGRP == "AC001" && result.MNCOD == "1230")
                {
                    result.Senttoservice = response.IT_NONCOMPOTH[0].ERDAT;

                }
                if (result.MNKAT == "S" && result.MNGRP == "AC001" && result.MNCOD == "1060")
                {
                    result.MeterinstalledSupplyRelese = response.IT_NONCOMPOTH[0].ERDAT;

                }
                if (result.MNGRPNonComplaineByapplicant == "AC005")
                {
                    result.NonComplaineByapplicant = response.IT_NONCOMPOTH[0].ERDAT;

                }

            }

            catch (Exception e)
            {
                return null;
            }
            return result;
        }
        #endregion

    }
}