using SapPiService.Domain;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Feature.Accounts;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.SessionHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    public class GreenPowerOptInService
    {
        private const int AccountNumberLength = 12;

        public bool IsAlreadyOptedForGreenPower(string AccountNumber)
        {
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                if (dataContext.GreenPowerOptIns.Any(l => l.AccountNumber == AccountNumber))
                    return true;
                else
                    return false;
            }
        }

        public GreenPowerOptIn GetRecordByAccountNumber(string AccountNumber)
        {
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                if (dataContext.GreenPowerOptIns.Any(l => l.AccountNumber == AccountNumber))
                    return dataContext.GreenPowerOptIns.FirstOrDefault(l => l.AccountNumber == AccountNumber);
                else
                    return null;
            }
        }

        public bool UpdateRecordByAccountNumber(GreenPowerOptInModel model)
        {
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                if (dataContext.GreenPowerOptIns.Any(l => l.AccountNumber == model.CANumber))
                {
                    GreenPowerOptIn recordToUpdate = dataContext.GreenPowerOptIns.FirstOrDefault(l => l.AccountNumber == model.CANumber);
                    recordToUpdate.FacebookId = model.FacebookId;
                    recordToUpdate.TwitterId = model.TwitterId;
                    recordToUpdate.IPledge = true;
                    dataContext.SubmitChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool InsertGreenPowerOptRecord(GreenPowerOptInModel model)
        {
            try
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    if (!IsAlreadyOptedForGreenPower(model.CANumber))
                    {
                        GreenPowerOptIn objGreenPowerOptIn = new GreenPowerOptIn
                        {
                            AccountNumber = model.CANumber,
                            CreatedDate = DateTime.Now,
                            MobileNumber = model.MobileNumber,
                            EmailId = model.EmailId,
                            PercentageOptIn = model.SelectedPercentage,
                            OptInBillingFrom = model.ActivateBillingPeriod == "1" ? "Current" : "Next",
                            OptInFlagCurrentOrNextBilling = model.ActivateBillingPeriod == "1" ? true : false,
                        };
                        dbcontext.GreenPowerOptIns.InsertOnSubmit(objGreenPowerOptIn);
                        dbcontext.SubmitChanges();

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("InsertGreenPowerOptRecord error in db save: " + e.Message, this);
            }
            return false;
        }
    }
}