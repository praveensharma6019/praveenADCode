using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System.Net.Mail;
using System.Linq;
using System;
using System.Collections.Generic;
using Sitecore.ElectricityNew.Website.Model;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.ElectricityNew.Website.Services
{
    public class TenderService
    {
        public void SendTenderMail(string email, string redirectUrl)
        {
            try
            {
                var mail = this.GetTenderMailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("{0}", redirectUrl);
                MainUtil.SendMail(mail);
            }
            catch
            {
                //Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                throw;
            }
        }

        //public virtual Item GetAccountsSettingsItem(Item contextItem)
        //{
        //    Item item = null;

        //    if (contextItem != null)
        //    {
        //        item = contextItem.GetAncestorOrSelfOfTemplate(Template.AccountsSettings.ID);
        //    }
        //    item = item ?? Context.Site.GetContextItem(Template.AccountsSettings.ID);

        //    return item;
        //}

        public MailMessage GetTenderMailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Template.MailTemplate.ID);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetEnvelopUserEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Template.MailTemplate.EnvelopeUser);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }
        //tender submission mail template
        public MailMessage GetTenderSubmitEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Template.MailTemplate.TenderSubmission);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }



        public MailMessage GetCorrigendumCreateEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Template.MailTemplate.CorrigendumCreate);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public void SendPasswordResetLink(string email, string redirectUrl, string Username, string pwd, string NitNo, string Description)
        {
            try
            {
                var mail = this.GetTenderMailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                mail.Body = mail.Body.Replace("#UserName#", Username);
                mail.Body = mail.Body.Replace("#Pwd#", pwd);
                mail.Body = mail.Body.Replace("#TenderName#", Description);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                //throw;
            }
        }

        public void SendEnvelopUserEmail(string email, string redirectUrl, string Username, string pwd, string NitNo, string Description)
        {
            try
            {
                var mail = this.GetEnvelopUserEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                mail.Body = mail.Body.Replace("#UserName#", Username);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Body = mail.Body.Replace("#Pwd#", pwd);
                mail.Body = mail.Body.Replace("#TenderName#", Description);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                //throw;
            }
        }
        //send tender submission mail
        public void SendTenderSubmissionMail(string email, string NitNo, string Description)
        {
            try
            {
                var mail = this.GetTenderSubmitEmailTemplate();
                mail.To.Add(email); 
                mail.Body = mail.Body.Replace("#NitNo#", NitNo); 
                mail.Body = mail.Body.Replace("#TenderName#", Description);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the Tender submission email to the " + email + " - Error - " + ex.Message + "", ex, this);
                //throw;
            }
        }

        public void SendCorrigendumCreateEmail(string email,string NitNo, string Description, string Title, string DateCreated)
        {
            try
            {
                var mail = this.GetCorrigendumCreateEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#TenderName#", Description);
                mail.Body = mail.Body.Replace("#TenderNIT#", NitNo);
                mail.Body = mail.Body.Replace("#Title#", Title);
                mail.Body = mail.Body.Replace("#DateCreated#",DateCreated);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                //throw;
            }
        }

        public void SendEmailOnTenderClose_Scheduler()
        {
            try
            {
                var webDb = Sitecore.Data.Database.GetDatabase("web");
                var settingsItem = webDb.GetItem(Template.MailTemplate.EnvelopeUserOnTenderClose);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

                Log.Info("Email body created a t" + DateTime.Now.ToString() + "", this);
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    Log.Info("inside dbcontext at " + DateTime.Now.ToString() + "", this);
                    List<TenderUserDetailsForMail> listTenderUserDetailsForMail = new List<TenderUserDetailsForMail>();
                    if (dbcontext.TenderLists.Where(t => t.Closing_Date.Value.Date == DateTime.Now.Date && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsCloseEmailSent!=true).Any())
                    {
                        var tenders = dbcontext.TenderLists.Where(t => t.Closing_Date.Value.Date == DateTime.Now.Date && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsCloseEmailSent != true).ToList();
                        Log.Info("inside tenders at " + DateTime.Now.ToString() + " Tenders count:" + tenders.Count, this);
                        foreach (var tender in tenders)
                        {
                            Log.Info("inside tender at " + DateTime.Now.ToString() + " Tender name:" + tender.NITNo + "-" + tender.Description, this);
                            var tenderUsers = dbcontext.UserTenderMappings.Where(u => u.TenderId == tender.Id).Select(m => m.UserId).ToList();
                            var users = dbcontext.Registrations.Where(u => tenderUsers.Contains(u.UserId)).ToList();
                            Log.Info("inside tender users at " + DateTime.Now.ToString() + " User count:" + users.Count, this);
                            foreach (var user in users)
                            {
                                var mail = new MailMessage
                                {
                                    From = new MailAddress(fromMail.Value),
                                    Body = body.Value,
                                    Subject = subject.Value,
                                    IsBodyHtml = true
                                };

                                Log.Info("inside tender user at " + DateTime.Now.ToString() + " User email:" + user.Email + "-" + user.UserId, this);
                                mail.To.Add(user.Email); //mail.To.Add("nidhi.paneri@advaiya.com");// mail.To.Add(user.Email);
                                mail.Body = mail.Body.Replace("#UserName#", user.UserId);
                                mail.Body = mail.Body.Replace("#Pwd#", user.Password);
                                mail.Body = mail.Body.Replace("#TenderNIT#", tender.NITNo);
                                mail.Body = mail.Body.Replace("#TenderName#", tender.Description);
                                mail.Subject = mail.Subject.Replace("#Subject#", tender.NITNo);
                                MainUtil.SendMail(mail);
                                Log.Info("Success for email sending at " + DateTime.Now.ToString() + " User email:" + user.Email + "-" + user.UserId, this);
                            }
                            TenderList tenderToUpdate = dbcontext.TenderLists.Where(t => t.Id == tender.Id).FirstOrDefault();
                            tenderToUpdate.IsCloseEmailSent = true;
                            dbcontext.SubmitChanges();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email - " + ex.Message + "", ex, this);
                //throw;
            }
        }

        public void SendReminderForTenderClose_Scheduler()
        {
            try
            {
                var webDb = Sitecore.Data.Database.GetDatabase("web");
                var settingsItem = webDb.GetItem(Template.MailTemplate.EnvelopeUserReminderForTenderClose);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Template.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Template.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Template.MailTemplate.Fields.Subject];

                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
                Log.Info("Email body created a t" + DateTime.Now.ToString() + "", this);
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    Log.Info("inside dbcontext at " + DateTime.Now.ToString() + "", this);
                    List<TenderUserDetailsForMail> listTenderUserDetailsForMail = new List<TenderUserDetailsForMail>();
                    if (dbcontext.TenderLists.Where(t => t.Closing_Date.Value.Date == DateTime.Now.AddDays(1).Date && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsReminderMailSent != true).Any())
                    {
                        var tenders = dbcontext.TenderLists.Where(t => t.Closing_Date.Value.Date == DateTime.Now.AddDays(1).Date && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsReminderMailSent != true).ToList();
                        Log.Info("inside tenders at " + DateTime.Now.ToString() + " Tenders count:" + tenders.Count, this);
                        foreach (var tender in tenders)
                        {
                            Log.Info("inside tender at " + DateTime.Now.ToString() + " Tender name:" + tender.NITNo + "-" + tender.Description, this);
                            var tenderUsers = dbcontext.UserTenderMappings.Where(u => u.TenderId == tender.Id).Select(m => m.UserId).ToList();
                            var users = dbcontext.Registrations.Where(u => tenderUsers.Contains(u.UserId)).ToList();
                            Log.Info("inside tender users at " + DateTime.Now.ToString() + " User count:" + users.Count, this);
                            foreach (var user in users)
                            {
                                Log.Info("inside tender user at " + DateTime.Now.ToString() + " User email:" + user.Email + "-" + user.UserId, this);
                                mail.To.Add(user.Email); //mail.To.Add("nidhi.paneri@advaiya.com");// mail.To.Add(user.Email);
                                mail.Body = mail.Body.Replace("#TenderName#", tender.Description);
                                mail.Body = mail.Body.Replace("#TenderNIT#", tender.NITNo);
                                mail.Subject = mail.Subject.Replace("#Subject#", tender.NITNo);
                                MainUtil.SendMail(mail);
                                Log.Info("Success for email sending at " + DateTime.Now.ToString() + " User email:" + user.Email + "-" + user.UserId, this);
                            }

                            TenderList tenderToUpdate = dbcontext.TenderLists.Where(t => t.Id == tender.Id).FirstOrDefault();
                            tenderToUpdate.IsReminderMailSent = true;
                            dbcontext.SubmitChanges();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email - " + ex.Message + "", ex, this);
                //throw;
            }
        }
    }
}