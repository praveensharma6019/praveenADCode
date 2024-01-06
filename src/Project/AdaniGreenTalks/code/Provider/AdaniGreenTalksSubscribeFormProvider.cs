using Sitecore.AdaniGreenTalks.Website.Models;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Provider
{
    public class AdaniGreenTalksSubscribeFormProvider
    {
        public bool SubscribeForm(AdaniGreenTalks_SubscribeUs_Model m)
        {
            Log.Info("Insert SubscribeUsForm", "Start");
            AdaniGreenTalksDataContext rdb = new AdaniGreenTalksDataContext();
            AdaniGreenTalksSubscribeUs subscribeUsTable = new AdaniGreenTalksSubscribeUs();

            var result = new { status = "1" };
            try
            {
               
                Log.Info("Insert SubscribeForm captcha validated successfully", "Start");

                subscribeUsTable.Email = m.Email;
                subscribeUsTable.FormType = m.FormType;
                subscribeUsTable.FormUrl = m.FormUrl;
                subscribeUsTable.SubmittedDate = System.DateTime.Now;

                if (subscribeUsTable.Email != null && subscribeUsTable.FormType != null && subscribeUsTable.FormUrl != null)
                {
                    subscribeUsTable.Id = Guid.NewGuid();
                    #region Insert to DB
                    rdb.AdaniGreenTalksSubscribeUs.InsertOnSubmit(subscribeUsTable);
                    rdb.SubmitChanges();
                    #endregion
                    Log.Info("SubscribeForm data submitted successfully in database", this);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                Log.Error("Insert ContactUsForm inside catch" + ex, this);
                return false;
            }
        }
    }
}