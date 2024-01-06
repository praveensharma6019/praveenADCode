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
using Sitecore.GuwahatiAirport.Website.Model;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.GuwahatiAirport.Website.Services
{
    public class TenderService
    {
        /// <summary>
        /// Registration - Send mail to bidder/user with username and password including link to access login page
        /// </summary>
        /// <param name="email"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="Username"></param>
        /// <param name="pwd"></param>
        /// <param name="NitNo"></param>
        /// <param name="Description"></param>
        public void SendUserRegistrationEmail(string email, string redirectUrl, string Username, string pwd, string NitNo, string Description)
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
                throw;
            }
        }

        /// <summary>
        /// Mail Template - fetch from CMS
        /// </summary>
        /// <returns></returns>
        public MailMessage GetTenderMailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.ID);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

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
        //        item = contextItem.GetAncestorOrSelfOfTemplate(Templates.AccountsSettings.ID);
        //    }
        //    item = item ?? Context.Site.GetContextItem(Templates.AccountsSettings.ID);

        //    return item;
        //}

        

        public MailMessage GetEnvelopUserEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.EnvelopeUser);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetAdminUserEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.AdminUser);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetPQApprovalEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.PQApproval);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetSendForPQApprovalEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.sendForPQApproval);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }


        public MailMessage GetSendForTenderDocApprovalEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.sendForTenderDocApproval);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetPQRejectionEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.PQRejection);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

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
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.CorrigendumCreate);
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
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
                throw;
            }
        }


        public void SendCorrigendumCreateEmail(string email,string NitNo, string Description, string Title, string DateCreated, string baseurl)
        {
            try
            {
                var mail = this.GetCorrigendumCreateEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", baseurl);
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
                throw;
            }
        }

        public void SendEmailOnTenderClose_Scheduler()
        {
            try
            {
                var webDb = Sitecore.Data.Database.GetDatabase("web");
                var settingsItem = webDb.GetItem(Templates.MailTemplate.EnvelopeUserOnTenderClose);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

                Log.Info("Email body created a t" + DateTime.Now.ToString() + "", this);
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    Log.Info("inside dbcontext at " + DateTime.Now.ToString() + "", this);
                    List<TenderUserDetailsForMail> listTenderUserDetailsForMail = new List<TenderUserDetailsForMail>();
                    if (dbcontext.GAU_TenderLists.Where(t => t.Closing_Date.Value.Date == DateTime.Now.Date && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsCloseEmailSent!=true).Any())
                    {
                        var tenders = dbcontext.GAU_TenderLists.Where(t => t.Closing_Date.Value.Date == DateTime.Now.Date && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsCloseEmailSent != true).ToList();
                        Log.Info("inside tenders at " + DateTime.Now.ToString() + " Tenders count:" + tenders.Count, this);
                        foreach (var tender in tenders)
                        {
                            Log.Info("inside tender at " + DateTime.Now.ToString() + " Tender name:" + tender.NITNo + "-" + tender.Description, this);
                            var tenderUsers = dbcontext.GAU_UserTenderMappings.Where(u => u.TenderId == tender.Id).Select(m => m.UserId).ToList();
                            var users = dbcontext.GAU_Registrations.Where(u => tenderUsers.Contains(u.UserId)).ToList();
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
                            GAU_TenderList tenderToUpdate = dbcontext.GAU_TenderLists.Where(t => t.Id == tender.Id).FirstOrDefault();
                            tenderToUpdate.IsCloseEmailSent = true;
                            dbcontext.SubmitChanges();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email - " + ex.Message + "", ex, this);
                throw;
            }
        }

        public void SendReminderForTenderClose_Scheduler()
        {
            try
            {
                var webDb = Sitecore.Data.Database.GetDatabase("web");
                var settingsItem = webDb.GetItem(Templates.MailTemplate.EnvelopeUserReminderForTenderClose);
                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
                Log.Info("Email body created a t" + DateTime.Now.ToString() + "", this);
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    Log.Info("inside dbcontext at " + DateTime.Now.ToString() + "", this);
                    List<TenderUserDetailsForMail> listTenderUserDetailsForMail = new List<TenderUserDetailsForMail>();
                    if (dbcontext.GAU_TenderLists.Where(t => t.Closing_Date.Value.Date == DateTime.Now.AddDays(1).Date && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsReminderMailSent != true).Any())
                    {
                        var tenders = dbcontext.GAU_TenderLists.Where(t => t.Closing_Date.Value.Date == DateTime.Now.AddDays(1).Date && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsReminderMailSent != true).ToList();
                        Log.Info("inside tenders at " + DateTime.Now.ToString() + " Tenders count:" + tenders.Count, this);
                        foreach (var tender in tenders)
                        {
                            Log.Info("inside tender at " + DateTime.Now.ToString() + " Tender name:" + tender.NITNo + "-" + tender.Description, this);
                            var tenderUsers = dbcontext.GAU_UserTenderMappings.Where(u => u.TenderId == tender.Id).Select(m => m.UserId).ToList();
                            var users = dbcontext.GAU_Registrations.Where(u => tenderUsers.Contains(u.UserId)).ToList();
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

                            GAU_TenderList tenderToUpdate = dbcontext.GAU_TenderLists.Where(t => t.Id == tender.Id).FirstOrDefault();
                            tenderToUpdate.IsReminderMailSent = true;
                            dbcontext.SubmitChanges();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email - " + ex.Message + "", ex, this);
                throw;
            }
        }

        public void SendPQApprovalEmail(string email, string redirectUrl,string TenderDescription, string NitNo)
        {
            try
            {
                var mail = this.GetPQApprovalEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                //mail.Body = mail.Body.Replace("#TenderName#", TenderName);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Body = mail.Body.Replace("#TenderDesc#", TenderDescription);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                throw;
            }
        }

        public void SendPQRejectionEmail(string email, string redirectUrl,string TenderDescription, string NitNo)
        {
            try
            {
                var mail = this.GetPQRejectionEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                //mail.Body = mail.Body.Replace("#TenderName#", TenderName);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Body = mail.Body.Replace("#TenderDesc#", TenderDescription);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                throw;
            }
        }

        public void SendAdminUserEmail(string email, string redirectUrl, string Username, string pwd, string NitNo, string Description)
        {
            try
            {
                var mail = this.GetAdminUserEmailTemplate();
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
                throw;
            }
        }

        public void SendForPQApprovalEmail(List<GAU_Registration> userEmailList, bool PQVerifyed, string UserName, string userMobile, string userId, string TenderName, string NitNo)
        {
            try
            {
                if (PQVerifyed)
                {
                    var mail = this.GetSendForTenderDocApprovalEmailTemplate();
                    foreach (GAU_Registration email in userEmailList)
                    {
                        mail.To.Add(email.Email);
                    }
                    mail.Body = mail.Body.Replace("#UserName#", UserName);
                    mail.Body = mail.Body.Replace("#TenderName#", TenderName);
                    mail.Body = mail.Body.Replace("#TenderNit#", NitNo);
                    mail.Subject = mail.Subject.Replace("#Subject#", NitNo);

                    if (userId != null)
                        mail.Body = mail.Body.Replace("#Userid#", userId.ToString());

                    mail.Body = mail.Body.Replace("#Mobile#", userMobile);
                    MainUtil.SendMail(mail);
                }
                else
                {
                    var mail = this.GetSendForPQApprovalEmailTemplate();
                    foreach (GAU_Registration email in userEmailList)
                    {
                        mail.To.Add(email.Email);
                    }
                    mail.Body = mail.Body.Replace("#UserName#", UserName);
                    mail.Body = mail.Body.Replace("#TenderName#", TenderName);
                    mail.Body = mail.Body.Replace("#TenderNit#", NitNo);
                    mail.Subject = mail.Subject.Replace("#Subject#", NitNo);

                    if (userId != null)
                        mail.Body = mail.Body.Replace("#Userid#", userId.ToString());

                    mail.Body = mail.Body.Replace("#Mobile#", userMobile);
                    MainUtil.SendMail(mail);
                }

            
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email For SendForPQApprovalEmail - Error - " + ex.Message + "", ex, this);
                throw;
            }
        }
    }
}