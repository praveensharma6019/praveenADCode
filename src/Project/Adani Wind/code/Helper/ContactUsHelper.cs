using Sitecore.AdaniWind.Website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniWind.Website.Helper
{
    public class ContactUsHelper
    {
        public string SaveData(ContactUs contactUs)
        {
            try
            {
                Log.Info("Adani Wind | Save Data Helper Method Start", "");
                string EncryptionKey = DictionaryPhraseRepository.Current.Get("/EncryptionValue/EncryptionKey", ""); ;
                string EncryptionIV = DictionaryPhraseRepository.Current.Get("/EncryptionValue/EncryptionIV", ""); ;   
                Log.Info("Adani Wind | Contact us data start","");
                if (!GoogleRecaptcha.IsReCaptchValidV3(contactUs.reResponse))
                {
                    Log.Info("Adani Wind | Contact us data Captcha validation failed", "");
                    return "2";
                }
                using (AdaniWindDataContext context = new AdaniWindDataContext())
                {
                    AdaniWindContactUs contactDetails = new AdaniWindContactUs();
                    contactDetails.Id = Guid.NewGuid();
                    contactDetails.Fullname = contactUs.Fullname;
                    contactDetails.Email = EncryptionService.EncryptString(EncryptionKey,contactUs.Email, EncryptionIV);
                    contactDetails.ContactNo = EncryptionService.EncryptString(EncryptionKey,contactUs.ContactNo, EncryptionIV);
                    contactDetails.Purpose = contactUs.Purpose;
                    contactDetails.FormSubmittedOn = DateTime.Now;
                    context.AdaniWindContactUs.InsertOnSubmit(contactDetails);
                    context.SubmitChanges();
                    Log.Info("Adani Wind | Contact us data start", "");
                    return "1";
                }

            }
            catch (Exception ex)
            {
                Log.Error("Adani Wind | Contact us data save exception" + ex, ex);
                return "0";
            }
        }
    }
}