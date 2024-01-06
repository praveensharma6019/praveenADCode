using Sitecore.AdaniGreenTalks.Website.Models;
using Sitecore.Data.Query;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Provider
{
    public class AdaniGreenTalksContactFormProvider
    {
        public string ContactusForm(AdaniGreenTalks_Contactus_Model m)
        {
            Log.Info("Insert ContactUsForm", "Start");
            AdaniGreenTalksDataContext rdb = new AdaniGreenTalksDataContext();
            AdaniGreenTalks_ContactForm contactTable = new AdaniGreenTalks_ContactForm();

            try
            {

                Log.Info("Insert ContactUsForm captcha validated successfully", "Start");
                contactTable.FirstName = m.FirstName;
                contactTable.LastName = m.LastName;
                contactTable.CustomerQuery = m.CustomerQuery;
                contactTable.Email = m.Email;
                contactTable.ContactNumber = m.ContactNumber;
                contactTable.FormType = m.FormType;
                contactTable.FormUrl = m.FormUrl;
                contactTable.FormSubmittedOn = DateTime.Now;

                if (contactTable.FirstName != null && contactTable.LastName != null && contactTable.CustomerQuery != null && contactTable.Email != null && contactTable.ContactNumber != null && contactTable.FormType != null && contactTable.FormUrl != null && contactTable.FormSubmittedOn != null)
                {
                    contactTable.Id = Guid.NewGuid();
                    rdb.AdaniGreenTalks_ContactForms.InsertOnSubmit(contactTable);
                    rdb.SubmitChanges();
                    Log.Info("Insert ContactUsForm submitted successfully", this);
                    return "successfully";
                }
                else
                {
                    Log.Info("Insert ContactUsForm captcha validatation failed", this);
                    return "failed";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Insert ContactUsForm inside catch" + ex, this);
                return "failed";
            }
        }


        public string AttendForm(AdaniGreenTalks_Contactus_Model m)
        {
            Log.Info("Insert AttendForm", "Start");
            AdaniGreenTalksDataContext rdb = new AdaniGreenTalksDataContext();
            AdaniGreenTalks_ContactForm contactTable = new AdaniGreenTalks_ContactForm();

            try
            {

                Log.Info("Insert AttendForm captcha validated successfully", "Start");
                contactTable.FirstName = m.FirstName;
                contactTable.LastName = m.LastName;
                contactTable.CustomerQuery = m.CustomerQuery;
                contactTable.Email = m.Email;
                contactTable.ContactNumber = m.ContactNumber;
                contactTable.FormType = m.FormType;
                contactTable.FormUrl = m.FormUrl;
                contactTable.FormSubmittedOn = DateTime.Now;

                if (contactTable.FirstName != null && contactTable.LastName != null && contactTable.CustomerQuery != null && contactTable.Email != null && contactTable.ContactNumber != null && contactTable.FormType != null && contactTable.FormUrl != null && contactTable.FormSubmittedOn != null)
                {
                    contactTable.Id = Guid.NewGuid();
                    rdb.AdaniGreenTalks_ContactForms.InsertOnSubmit(contactTable);
                    rdb.SubmitChanges();
                    Log.Info("Insert AttendForm submitted successfully", this);
                    return "successfully";
                }
                else
                {
                    Log.Info("Insert AttendForm captcha validatation failed", this);
                    return "failed";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Insert AttendForm inside catch" + ex, this);
                return "failed";
            }
        }

        public string ContributeForm(AdaniGreenTalks_Contribute_Model m)
        {
            Log.Info("Insert ContributeForm", "Start");
            AdaniGreenTalksDataContext rdb = new AdaniGreenTalksDataContext();
            AdaniGreenTalks_ContributeForm contactTable = new AdaniGreenTalks_ContributeForm();

            try
            {
                Log.Info("Insert ContributeForm captcha validated successfully", "Start");
                contactTable.FirstName = m.FirstName;
                contactTable.LastName = m.LastName;
                contactTable.City = m.City;
                contactTable.FellowName = m.FellowName;
                contactTable.Goal = m.Goal;
                contactTable.Email = m.Email;
                contactTable.ContactNumber = m.ContactNumber;
                contactTable.FormType = m.FormType;
                contactTable.FormUrl = m.FormUrl;
                contactTable.FormSubmittedOn = DateTime.Now;

                if (contactTable.FirstName != null && contactTable.LastName != null && contactTable.City != null && contactTable.Email != null && contactTable.Goal != null && contactTable.ContactNumber != null && contactTable.FormType != null && contactTable.FormUrl != null && contactTable.FormSubmittedOn != null)
                {
                    contactTable.Id = Guid.NewGuid();
                    rdb.AdaniGreenTalks_ContributeForms.InsertOnSubmit(contactTable);
                    rdb.SubmitChanges();
                    Log.Info("Insert ContributeForm submitted successfully", this);
                    return "successfully";
                }
                else
                {
                    Log.Info("Insert ContributeForm captcha validatation failed", this);
                    return "failed";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Insert ContributeForm inside catch" + ex, this);
                return "failed";
            }
        }

        public string SpeakForm(AdaniGreenTalks_Speak_Model m)
        {
            Log.Info("Insert SpeakForm", "Start");
            AdaniGreenTalksDataContext rdb = new AdaniGreenTalksDataContext();
            AdaniGreenTalks_Speak contactTable = new AdaniGreenTalks_Speak();

            try
            {
                Log.Info("Insert SpeakForm captcha validated successfully", "Start");
                contactTable.FirstName = m.FirstName;
                contactTable.LastName = m.LastName;
                contactTable.Email = m.Email;
                contactTable.MobileNumber = m.MobileNumber;
                contactTable.NomineeFirstName = m.NomineeFirstName;
                contactTable.NomineeLastName = m.NomineeLastName;
                contactTable.NomineeEmail = m.NomineeEmail;
                contactTable.ContactNumber = m.ContactNumber;
                contactTable.City = m.City;
                contactTable.Country = m.Country;
                contactTable.Takeaway = m.Takeaway;
                contactTable.linkforarticle = m.linkforarticle;
                contactTable.linkaudioorvideo = m.linkaudioorvideo;
                contactTable.Goal = m.Goal;
                contactTable.fileUploadPhoto = m.fileUploadPhoto;
                contactTable.fileUploadbiograph = m.fileUploadbiograph;
                contactTable.fileOriginalConcept = m.fileOriginalConcept;
                contactTable.NomineeMyself = m.checknominatingmyself;
                contactTable.FormType = m.FormType;
                contactTable.FormUrl = m.FormUrl;
                contactTable.FormSubmittedOn = DateTime.Now;

                if (contactTable.FirstName != null && contactTable.LastName != null && contactTable.City != null && contactTable.Email != null && contactTable.Goal != null && contactTable.MobileNumber != null && contactTable.ContactNumber != null && contactTable.FormType != null && contactTable.FormUrl != null && contactTable.FormSubmittedOn != null && contactTable.NomineeEmail != null)
                {
                    contactTable.Id = Guid.NewGuid();
                    rdb.AdaniGreenTalks_Speaks.InsertOnSubmit(contactTable);
                    rdb.SubmitChanges();
                    Log.Info("Insert SpeakForm submitted successfully", this);
                    return "successfully";
                }
                else
                {
                    Log.Info("Insert SpeakForm captcha validatation failed", this);
                    return "failed";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Insert SpeakForm inside catch" + ex, this);
                return "failed";
            }
        }

        public string CustomHtmlEncode(string str)
        {
            try
            {
               return str.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace(@"&quot;",System.Convert.ToString('"') ).Replace("&#39;", "'");

            }
            catch (Exception)
            {

                return str;
            }

        }



    }
}