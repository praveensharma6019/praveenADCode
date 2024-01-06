using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SapPiService.Domain;
using SapPiService.Wsdl;


namespace SapPiService.Services.Impl
{
    public class AccountService : IAccountService
    {
        public async Task<SubscriberType> FetchType(string accountNumber)
        {
            var caTypeClient = Builder.BuildClientForAccountType();

            var caTypeRequest = new DT_CHECK_CA_TYPE_req { CONT_ACCT = Helper.FormatAccountNumber(accountNumber) };

            var caTypeResponse = await caTypeClient.SI_CHECK_CA_TYPE_inAsync(caTypeRequest);
            var caTypeResponseObj = caTypeResponse.MT_CHECK_CA_TYPE_resp;

            Helper.LogObject(caTypeResponse.MT_CHECK_CA_TYPE_resp);

            var details = new SubscriberType
            {
                AccountNumber = accountNumber,
                ConsumerType = caTypeResponseObj.FORM_TYPE == "CYC" ? ConsumerType.Standard : ConsumerType.Premium,
                CycleNumber = caTypeResponseObj.CYCLE_NO
            };

            return details;
        }

        public async Task<SubscriberDetail> FetchDetail(string accountNumber)
        {
            var client = Builder.BuildClientForAccountDetail();
            var request = new DT_ISUPARTNER_GETDETAIL_req
            {
                CONTRACT = "",
                CA_NUMBER = Helper.FormatAccountNumber(accountNumber)
            };
            var response = await client.SI_ISUPARTNER_GETDETAIL_inAsync(request);
            var responseData = response.MT_ISUPARTNER_GETDETAIL_resp.OUTPUT;
            Helper.LogObject(responseData);

            var details = new SubscriberDetail
            {
                AccountNumber = accountNumber,
                Name = $"{responseData.FIRSTNAME} {responseData.LASTNAME}",
                BookNumber = responseData.BOOKNO,
                ZoneNumber = responseData.Z_ZONE,
                CycleNumber = responseData.Z_CYCLE,
                Address = $"{responseData.HOUSE_NO} {responseData.STREET} {responseData.STR_SUPPL1} {responseData.STR_SUPPL2} {responseData.CITY} {responseData.POSTL_COD1}"
            };

            return details;
        }

        public async Task<BillingLanguage> FetchBillingLanguage(string accountNumber)
        {
            var client = Builder.BuildClientForBillingLanguage();
            var request = new DT_ZCSBLNG_req
            {
                ZZ_VKONT = Helper.FormatAccountNumber(accountNumber)
            };

            var response = await client.SI_ZCSBLNG_inAsync(request);
            Helper.LogObject(request);
            Helper.LogObject(response);
            return EnumHelper.ParseLanguage(response.MT_ZCSBLNG_resp.ZZ_EXP_LANGU);
        }

        public async Task UpdateBillingLanguage(string accountNumber, BillingLanguage language)
        {
            var client = Builder.BuildClientForBillingLanguage();
            var request = new DT_ZCSBLNG_req
            {
                ZZ_VKONT = Helper.FormatAccountNumber(accountNumber),
                ZZ_LANGU = language.GetDescription()
            };

            var response = await client.SI_ZCSBLNG_inAsync(request);
            Helper.LogObject(request);
            Helper.LogObject(response.MT_ZCSBLNG_resp);
        }

        public async Task UpdateNotificationSetting(string accountNumber, string mobile, string email, bool? paperless)
        {
            var client = Builder.BuildClientForNotificationSetting();
            var request = new DT_EBILL_REGISTER_req
            {
                VKONT = Helper.FormatAccountNumber(accountNumber),
                MOBILE = mobile,
                SMTP_ADDR = email,
                TELEPHONE = "",
                // PAPERLESS_EBILL = paperless.HasValue ? "YEA" : "NO",
                // SOCIAL_MEDIA_REG = ""
            };
            Helper.LogObject(request);
            var response = await client.SI_EBILL_REGISTER_inAsync(request);
            Helper.LogObject(response.MT_EBILL_REGISTER_resp);
        }

        public async Task<BillingInfo> FetchBilling(string accountNumber)
        {
            var client = Builder.BuildClientForAccountDue();

            var request = new DT_MR_BI_GETLIST_req { CA_NUMBER = Helper.FormatAccountNumber(accountNumber) };

            var response = await client.SI_MR_BI_GETLIST_inAsync(request);
            var responseData = response.MT_MR_BI_GETLIST_resp.EXPORT;

            Helper.LogObject(response.MT_MR_BI_GETLIST_resp);

            var details = new BillingInfo
            {
                AccountNumber = accountNumber,
                BillAmount = responseData.CURRENT_BILL,
                BillMonth = responseData.BILL_MONTH,
                DateDue = responseData.BILL_DATE,
                UnitsConsumed = responseData.V_ZWSTNDAB,
                CurrentMonthCharge = responseData.CURRENT_BILL,
                BroughtForward = responseData.BRT_FORWARD,
                TariffSlab = responseData.TARIFF,
                TotalCharges = responseData.NET_AMNT,
                TotalBillAmount = responseData.DUE_DATE_BILL_AMT,
                AmountPayable = 0,
                IsOutstanding = responseData.STATUS != "9",
                MeterNumbers = (from n in response.MT_MR_BI_GETLIST_resp.SERIAL_NUM select n.SERNR.TrimStart('0')).ToArray()
            };

            var dueDateAmount = Convert.ToDecimal(responseData.DUE_DATE_BILL_AMT);
            var dpcBillAmount = Convert.ToDecimal(responseData.DPC_BILL_AMT);

            switch (responseData.FLAG)
            {
                //FIXME: Mapping of amount payable need to fix
                case "0":
                    details.BillingStatus = BillingStatus.Due;
                    details.AmountPayable = dueDateAmount;
                    break;
                case "1":
                    details.BillingStatus = BillingStatus.Overdue;
                    details.AmountPayable = dpcBillAmount;
                    break;
                case "2":
                    details.BillingStatus = BillingStatus.Hold;
                    details.AmountPayable = 0;
                    break;
            }

            if (response.MT_MR_BI_GETLIST_resp.MESSAGES.Length > 0)
            {
                details.Message = response.MT_MR_BI_GETLIST_resp.MESSAGES[0].LINE;
            }

            return details;
        }

        public async Task<decimal> FetchSecurityDepositAmount(string accountNumber)
        {
            var client = Builder.BuildClientForSecurityDepositDetail();
            var request = new DT_SD_req
            {
                VKONT = Helper.FormatAccountNumber(accountNumber)
            };

            var response = await client.SI_SD_inAsync(request);
            Helper.LogObject(response.MT_SD_resp);

            var sdAmount = Convert.ToDecimal(response.MT_SD_resp.SD);

            return sdAmount;
        }


        /*
         *vdsAmount = vdsApi.getVDSAmount(request).replaceAll("\\.0*$", StringPool.BLANK);
            if(Integer.valueOf(vdsAmount) >= 3000 && Integer.valueOf(vdsAmount) <= 100000){
                int iRemainder = Integer.valueOf(vdsAmount)%500;
                vdsAmount = String.valueOf(Integer.valueOf(vdsAmount) - iRemainder);
            }else if(Integer.valueOf(vdsAmount) <= 3000){
                vdsAmount = "3000";
            }else if(Integer.valueOf(vdsAmount) >= 100000){
                vdsAmount = "100000";
            }* 
            
            VDS amount is average billing amount - existing balance rounded to 500
            If Amount is below 3000, consider as 3000
            If amount is above 10000, consider 10000
            If amount is between 3000 and 10000, consider to rounded 500.
         */
        public async Task<VdsDetail> FetchVdsAmount(string accountNumber)
        {
            var client = Builder.BuildClientForNVdsDetail();

            var request = new DT_VDS_WEB_PYMT_req
            {
                CONTRACT_ACCOUNT = Helper.FormatAccountNumber(accountNumber)
            };

            var response = await client.SI_VDS_WEB_PYMT_inAsync(request);
            Helper.LogObject(response.MT_VDS_WEB_PYMT_resp);


            var averageBillAmount = Convert.ToDecimal(response.MT_VDS_WEB_PYMT_resp.AVG_BILL_AMT);
            var outstandingAmount = Convert.ToDecimal(response.MT_VDS_WEB_PYMT_resp.OUTSTAND_AMT);
            var existingVdsBalance = Convert.ToDecimal(response.MT_VDS_WEB_PYMT_resp.EXISTING_VDS_BALANCE);

            return new VdsDetail
            {
                CurrentOutstanding = (int)outstandingAmount,
                AverageBillingAmount = (int)averageBillAmount,
                ExistingVdsBalance = (int)existingVdsBalance
            };
        }

        public async Task<OutageDetail> FetchOutage(string accountNumber)
        {
            var client = Builder.BuildClientForOutageInformation();

            var request = new DT_OUTAGE_INFO_req { VKONT = Helper.FormatAccountNumber(accountNumber) };

            var response = await client.SI_OUTAGE_INFO_inAsync(request);
            var responseData = response.MT_OUTAGE_INFO_resp;

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
                    Date = x.ZUPLD_DATE,
                    StartTime = x.FROM_TIME,
                    EndTime = x.TO_TIME,
                    ActivityType = x.ACTIVITY,
                    OutageType = "HT"
                });
            }
            foreach (var x in currentLTOutage)
            {
                currentOutage.Add(new OutageRecord
                {
                    Date = x.ZUPLD_DATE,
                    StartTime = x.FROM_TIME,
                    EndTime = x.TO_TIME,
                    ActivityType = x.ACTIVITY,
                    OutageType = "LT"
                });
            }

            List<OutageRecord> futureOutage = new List<OutageRecord>();
            foreach (var x in futureHTOutage)
            {
                futureOutage.Add(new OutageRecord
                {
                    Date = x.ZUPLD_DATE,
                    StartTime = x.FROM_TIME,
                    EndTime = x.TO_TIME,
                    ActivityType = x.ACTIVITY,
                    OutageType = "HT"
                });
            }
            foreach (var x in futureLTOutage)
            {
                futureOutage.Add(new OutageRecord
                {
                    Date = x.ZUPLD_DATE,
                    StartTime = x.FROM_TIME,
                    EndTime = x.TO_TIME,
                    ActivityType = x.ACTIVITY,
                    OutageType = "LT"
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

        public async Task<PaymentHistory> FetchPaymentHistory(string accountNumber, DateTime? fromDate,
            DateTime? toDate)
        {
            var client = Builder.BuildClientForPaymentHistory();

            var paymentDateRange = new DT_PAYMT_HISTORY_reqDATE
            {
                FROM_DATE = fromDate.HasValue ? Helper.ConvertDateToString(fromDate.Value) : "",
                TO_DATE = toDate.HasValue ? Helper.ConvertDateToString(toDate.Value) : ""
            };

            var request = new DT_PAYMT_HISTORY_req
            { DATE = paymentDateRange, CONT_ACCT = Helper.FormatAccountNumber(accountNumber) };
            var response = await client.SI_PAYMT_HISTORY_inAsync(request);
            Helper.LogObject(response.MT_PAYMT_HISTORY_resp);

            var paymentHistory = new PaymentHistory
            {
                PaymentHistoryList = new List<PaymentHistoryRecord>()
            };

            foreach (var item in response.MT_PAYMT_HISTORY_resp.OUTPUT)
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

            return paymentHistory;
        }

        //public async Task<string> GenerateViewBillLink(string accountNumber, int year, int month)
        //{
        //    return "";
        //}

        public async Task<ConsumptionHistory> FetchConsumptionHistory(string accountNumber)
        {
            var client = Builder.BuildClientForConsumptionHistory();
            var request = new DT_MTRREADDOC_GETLIST_req
            { CA_NUMBER = Helper.FormatAccountNumber(accountNumber), REGISTER = "001" };
            var response = await client.SI_MTRREADDOC_GETLIST_inAsync(request);
            Helper.LogObject(response.MT_MTRREADDOC_GETLIST_resp);

            var consumptions = new Dictionary<string, MeterConsumption>();

            foreach (var item in response.MT_MTRREADDOC_GETLIST_resp.USAGE_HISTORY)
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

            var consumptionHistory = new ConsumptionHistory { MeterConsumptions = consumptions.Values.ToList() };
            return consumptionHistory;
        }

        public async Task<InvoiceHistory> FetchInvoiceHistory(string accountNumber)
        {
            var client = Builder.BuildClientForInvoiceHistory();
            var request = new DT_GET_BILL_MONTH_req { CONTRACT_ACCOUNT = Helper.FormatAccountNumber(accountNumber) };
            var response = await client.SI_GET_BILL_MONTH_inAsync(request);
            Helper.LogObject(response.MT_GET_BILL_MONTH_resp);

            var details = new InvoiceHistory
            {
                InvoiceLines = new List<InvoiceLine>()
            };

            foreach (var item in response.MT_GET_BILL_MONTH_resp.BILL_DATA)
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

        public ConsumerDetails FetchConsumerDetails(string accountNumber)
        {
            var client = Builder.BuildClientForConsumerDetails();
            var request = new DT_Cms_Isu_Ca_Display_req
            {
                IMPORT_CANUMBER = new DT_Cms_Isu_Ca_Display_reqIMPORT_CANUMBER
                {
                    CA_NUMBER = Helper.FormatAccountNumber(accountNumber)
                }
            };
            var response = client.SI_Cms_Isu_Ca_Display_in(request);
            Helper.LogObject(response.EXPORT_CADETAILS);

            var details = new ConsumerDetails
            {
                Name = response.EXPORT_CADETAILS[0].BP_NAME,
                Email = response.EXPORT_CADETAILS[0].E_MAIL,
                Mobile = response.EXPORT_CADETAILS[0].TEL1_NUMBR
            };

            return details;
        }


        public async Task<PvcDetail> FetchPvcDetail(string accountNumber)
        {
            var client = Builder.BuildClientForPremiumAccountDetail();

            var request = new DT_PVC_DISPLAY_req
            {
                CONTRACT_ACCT = Helper.FormatAccountNumber(accountNumber),
                BILL_MONTH_TO_MONTH = "",
                BILL_MONTH_FROM_MONTH = ""
            };

            Helper.LogObject(request);
            var response = await client.SI_PVC_DISPLAY_inAsync(request);
            Helper.LogObject(response.MT_PVC_DISPLAY_resp);

            var responseData = response.MT_PVC_DISPLAY_resp;

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

        //Do not use this
        public async Task FetchInvoice(string accountNumber, int year, int month)
        {
            var client = Builder.BuildClientForViewBill();
            var request = new DT_ONLINE_BILL_PDF_req { BILL_IND = "JUN-17", CONT_ACCT = "000100695559", INVOICE_NO = "100427748821" };
            var response = await client.SI_ONLINE_BILL_PDF_inAsync(request);
            Helper.LogObject(response.MT_ONLINE_BILL_PDF_resp);
        }

        public async Task<ComplainStatus> FetchComplainStatus(string accountNumber, string complainNumber)
        {
            var client = Builder.BuildClientForComplainStatus();
            var request = new DT_COMP_STATUS_req
            {
                CA_NO = accountNumber == null ? "" : Helper.FormatAccountNumber(accountNumber),
                ORDER_NO = complainNumber ?? ""
            };
            Helper.LogObject(request);
            var response = await client.SI_COMP_STATUS_inAsync(request);
            Helper.LogObject(response.MT_COMP_STATUS_resp);

            var responseData = response.MT_COMP_STATUS_resp;

            return new ComplainStatus
            {
                AccountNumber = responseData.CANO,
                ComplainCode = responseData.COMPLAINTSTATUS,
                Message = Builder.GetStatusMessageFromCode(responseData.COMPLAINTSTATUS)
            };
        }

        //public async Task<VDSRegistration> VdsRegistration(string accountNumber, string transactionId, string mobileNumber, string emailID, string PANNumber, string amount, string date)
        //{
        //    var client = Builder.BuildClientForVdsRegistration();

        //    var request = new DT_VDS_REGISTR_Req
        //    {
        //        ZDATE = date,   //date
        //        ZEMAILID = emailID,    //email id
        //        ZLGNUM = PANNumber,  //PAN No.
        //        ZMOBNO = mobileNumber,    //mobile number
        //        ZTRNSACTNID = transactionId, //transaction  id
        //        ZVKONT = Helper.FormatAccountNumber(accountNumber),   //CA number
        //        ZBETRW = amount  //amount
        //    };

        //    var response = await client.ZBAPI_VDS_REGISTRATIONAsync(request);
        //    Helper.LogObject(response.MT_VDS_REGISTR_Resp);

        //    VDSRegistration result = new VDSRegistration
        //    {
        //        AccountNumber = response.MT_VDS_REGISTR_Resp.ZZVKONT,
        //        Amount = response.MT_VDS_REGISTR_Resp.ZZBETRW,
        //        EmailId = response.MT_VDS_REGISTR_Resp.ZZEMAILID,
        //        MobileNumber = response.MT_VDS_REGISTR_Resp.ZZMOBNO,
        //        PANNumber = response.MT_VDS_REGISTR_Resp.ZZLGNUM,
        //        ResultFlag = response.MT_VDS_REGISTR_Resp.ZUPD_FLAG,
        //        TransactionId = response.MT_VDS_REGISTR_Resp.ZZTRNSACTNID,
        //        DateOfTransaction= response.MT_VDS_REGISTR_Resp.ZZDATE
        //    };

        //    return result;
        //}

    }
}