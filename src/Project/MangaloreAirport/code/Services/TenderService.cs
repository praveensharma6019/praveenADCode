using Sitecore;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.MangaloreAirport.Website;
using Sitecore.MangaloreAirport.Website.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace Sitecore.MangaloreAirport.Website.Services
{
    public class TenderService
    {
        public TenderService()
        {
        }

        public MailMessage GetAdminUserEmailTemplate()
        {
            Item mailTemplateItem = Context.Database.GetItem(Templates.MailTemplate.AdminUser);
            Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }
            Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
            return new MailMessage()
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetCorrigendumCreateEmailTemplate()
        {
            Item mailTemplateItem = Context.Database.GetItem(Templates.MailTemplate.CorrigendumCreate);
            Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }
            Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
            return new MailMessage()
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetEnvelopUserEmailTemplate()
        {
            Item mailTemplateItem = Context.Database.GetItem(Templates.MailTemplate.EnvelopeUser);
            Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }
            Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
            return new MailMessage()
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetPQApprovalEmailTemplate()
        {
            Item mailTemplateItem = Context.Database.GetItem(Templates.MailTemplate.PQApproval);
            Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }
            Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
            return new MailMessage()
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetPQRejectionEmailTemplate()
        {
            Item mailTemplateItem = Context.Database.GetItem(Templates.MailTemplate.PQRejection);
            Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }
            Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
            return new MailMessage()
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetSendForPQApprovalEmailTemplate()
        {
            Item mailTemplateItem = Context.Database.GetItem(Templates.MailTemplate.sendForPQApproval);
            Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }
            Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
            return new MailMessage()
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetSendForTenderDocApprovalEmailTemplate()
        {
            Item mailTemplateItem = Context.Database.GetItem(Templates.MailTemplate.sendForTenderDocApproval);
            Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }
            Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
            return new MailMessage()
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetTenderMailTemplate()
        {
            Item mailTemplateItem = Context.Database.GetItem(Templates.MailTemplate.ID);
            Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }
            Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
            return new MailMessage()
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public void SendAdminUserEmail(string email, string redirectUrl, string Username, string pwd, string NitNo, string Description)
        {
            try
            {
                MailMessage mail = this.GetAdminUserEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                mail.Body = mail.Body.Replace("#UserName#", Username);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Body = mail.Body.Replace("#Pwd#", pwd);
                mail.Body = mail.Body.Replace("#TenderName#", Description);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email to the ", email, " - Error - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }

        public void SendCorrigendumCreateEmail(string email, string NitNo, string Description, string Title, string DateCreated, string baseurl)
        {
            try
            {
                MailMessage mail = this.GetCorrigendumCreateEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", baseurl);
                mail.Body = mail.Body.Replace("#TenderName#", Description);
                mail.Body = mail.Body.Replace("#TenderNIT#", NitNo);
                mail.Body = mail.Body.Replace("#Title#", Title);
                mail.Body = mail.Body.Replace("#DateCreated#", DateCreated);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email to the ", email, " - Error - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }

        public void SendEmailOnTenderClose_Scheduler()
        {
            try
            {
                Item mailTemplateItem = Database.GetDatabase("web").GetItem(Templates.MailTemplate.EnvelopeUserOnTenderClose);
                Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }
                Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
                DateTime now = DateTime.Now;
                Log.Info(string.Concat("Email body created a t", now.ToString()) ?? "", this);
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    now = DateTime.Now;
                    Log.Info(string.Concat("inside dbcontext at ", now.ToString()) ?? "", this);
                    List<TenderUserDetailsForMail> listTenderUserDetailsForMail = new List<TenderUserDetailsForMail>();
                    if ((
                        from t in dbcontext.TenderLists
                        where (t.Closing_Date.Value.Date == DateTime.Now.Date) && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsCloseEmailSent != (bool?)true
                        select t).Any<TenderList>())
                    {
                        List<TenderList> tenders = (
                            from t in dbcontext.TenderLists
                            where (t.Closing_Date.Value.Date == DateTime.Now.Date) && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsCloseEmailSent != (bool?)true
                            select t).ToList<TenderList>();
                        object[] str = new object[] { "inside tenders at ", null, null, null };
                        now = DateTime.Now;
                        str[1] = now.ToString();
                        str[2] = " Tenders count:";
                        str[3] = tenders.Count;
                        Log.Info(string.Concat(str), this);
                        foreach (TenderList tenderList in tenders)
                        {
                            string[] nITNo = new string[] { "inside tender at ", null, null, null, null, null };
                            now = DateTime.Now;
                            nITNo[1] = now.ToString();
                            nITNo[2] = " Tender name:";
                            nITNo[3] = tenderList.NITNo;
                            nITNo[4] = "-";
                            nITNo[5] = tenderList.Description;
                            Log.Info(string.Concat(nITNo), this);
                            List<string> list = (
                                from u in dbcontext.UserTenderMappings
                                where u.TenderId == tenderList.Id
                                select u into m
                                select m.UserId).ToList<string>();
                            List<Registration> users = (
                                from u in dbcontext.Registrations
                                where list.Contains(u.UserId)
                                select u).ToList<Registration>();
                            object[] count = new object[] { "inside tender users at ", null, null, null };
                            now = DateTime.Now;
                            count[1] = now.ToString();
                            count[2] = " User count:";
                            count[3] = users.Count;
                            Log.Info(string.Concat(count), this);
                            foreach (Registration user in users)
                            {
                                MailMessage mail = new MailMessage()
                                {
                                    From = new MailAddress(fromMail.Value),
                                    Body = body.Value,
                                    Subject = subject.Value,
                                    IsBodyHtml = true
                                };
                                string[] email = new string[] { "inside tender user at ", null, null, null, null, null };
                                now = DateTime.Now;
                                email[1] = now.ToString();
                                email[2] = " User email:";
                                email[3] = user.Email;
                                email[4] = "-";
                                email[5] = user.UserId;
                                Log.Info(string.Concat(email), this);
                                mail.To.Add(user.Email);
                                mail.Body = mail.Body.Replace("#UserName#", user.UserId);
                                mail.Body = mail.Body.Replace("#Pwd#", user.Password);
                                mail.Body = mail.Body.Replace("#TenderNIT#", tenderList.NITNo);
                                mail.Body = mail.Body.Replace("#TenderName#", tenderList.Description);
                                mail.Subject = mail.Subject.Replace("#Subject#", tenderList.NITNo);
                                MainUtil.SendMail(mail);
                                string[] userId = new string[] { "Success for email sending at ", null, null, null, null, null };
                                now = DateTime.Now;
                                userId[1] = now.ToString();
                                userId[2] = " User email:";
                                userId[3] = user.Email;
                                userId[4] = "-";
                                userId[5] = user.UserId;
                                Log.Info(string.Concat(userId), this);
                            }
                            TenderList tenderToUpdate = (
                                from t in dbcontext.TenderLists
                                where t.Id == tenderList.Id
                                select t).FirstOrDefault<TenderList>();
                            tenderToUpdate.IsCloseEmailSent = new bool?(true);
                            dbcontext.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }

        public void SendEnvelopUserEmail(string email, string redirectUrl, string Username, string pwd, string NitNo, string Description)
        {
            try
            {
                MailMessage mail = this.GetEnvelopUserEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                mail.Body = mail.Body.Replace("#UserName#", Username);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Body = mail.Body.Replace("#Pwd#", pwd);
                mail.Body = mail.Body.Replace("#TenderName#", Description);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email to the ", email, " - Error - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }

        public void SendForPQApprovalEmail(List<Registration> userEmailList, bool PQVerifyed, string UserName, string userMobile, string userId, string TenderName, string NitNo)
        {
            try
            {
                if (!PQVerifyed)
                {
                    MailMessage mail = this.GetSendForPQApprovalEmailTemplate();
                    foreach (Registration email in userEmailList)
                    {
                        mail.To.Add(email.Email);
                    }
                    mail.Body = mail.Body.Replace("#UserName#", UserName);
                    mail.Body = mail.Body.Replace("#TenderName#", TenderName);
                    mail.Body = mail.Body.Replace("#TenderNit#", NitNo);
                    mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                    if (userId != null)
                    {
                        mail.Body = mail.Body.Replace("#Userid#", userId.ToString());
                    }
                    mail.Body = mail.Body.Replace("#Mobile#", userMobile);
                    MainUtil.SendMail(mail);
                }
                else
                {
                    MailMessage mail = this.GetSendForTenderDocApprovalEmailTemplate();
                    foreach (Registration email in userEmailList)
                    {
                        mail.To.Add(email.Email);
                    }
                    mail.Body = mail.Body.Replace("#UserName#", UserName);
                    mail.Body = mail.Body.Replace("#TenderName#", TenderName);
                    mail.Body = mail.Body.Replace("#TenderNit#", NitNo);
                    mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                    if (userId != null)
                    {
                        mail.Body = mail.Body.Replace("#Userid#", userId.ToString());
                    }
                    mail.Body = mail.Body.Replace("#Mobile#", userMobile);
                    MainUtil.SendMail(mail);
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email For SendForPQApprovalEmail - Error - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }

        public void SendPQApprovalEmail(string email, string redirectUrl, string TenderDescription, string NitNo)
        {
            try
            {
                MailMessage mail = this.GetPQApprovalEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Body = mail.Body.Replace("#TenderDesc#", TenderDescription);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email to the ", email, " - Error - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }

        public void SendPQRejectionEmail(string email, string redirectUrl, string TenderDescription, string NitNo)
        {
            try
            {
                MailMessage mail = this.GetPQRejectionEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Body = mail.Body.Replace("#TenderDesc#", TenderDescription);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email to the ", email, " - Error - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }

        public void SendReminderForTenderClose_Scheduler()
        {
            try
            {
                Item mailTemplateItem = Database.GetDatabase("web").GetItem(Templates.MailTemplate.EnvelopeUserReminderForTenderClose);
                Field fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];
                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }
                Field body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
                Field subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
                DateTime now = DateTime.Now;
                Log.Info(string.Concat("Email body created a t", now.ToString()) ?? "", this);
                using (TenderDBDataContext dbcontext = new TenderDBDataContext())
                {
                    now = DateTime.Now;
                    Log.Info(string.Concat("inside dbcontext at ", now.ToString()) ?? "", this);
                    List<TenderUserDetailsForMail> listTenderUserDetailsForMail = new List<TenderUserDetailsForMail>();
                    if ((
                        from t in dbcontext.TenderLists
                        where (t.Closing_Date.Value.Date == DateTime.Now.AddDays(1).Date) && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsReminderMailSent != (bool?)true
                        select t).Any<TenderList>())
                    {
                        List<TenderList> tenders = (
                            from t in dbcontext.TenderLists
                            where (t.Closing_Date.Value.Date == DateTime.Now.AddDays(1).Date) && t.Closing_Date.Value.Hour <= DateTime.Now.Hour && t.IsReminderMailSent != (bool?)true
                            select t).ToList<TenderList>();
                        object[] str = new object[] { "inside tenders at ", null, null, null };
                        now = DateTime.Now;
                        str[1] = now.ToString();
                        str[2] = " Tenders count:";
                        str[3] = tenders.Count;
                        Log.Info(string.Concat(str), this);
                        foreach (TenderList tenderList in tenders)
                        {
                            string[] nITNo = new string[] { "inside tender at ", null, null, null, null, null };
                            now = DateTime.Now;
                            nITNo[1] = now.ToString();
                            nITNo[2] = " Tender name:";
                            nITNo[3] = tenderList.NITNo;
                            nITNo[4] = "-";
                            nITNo[5] = tenderList.Description;
                            Log.Info(string.Concat(nITNo), this);
                            List<string> list = (
                                from u in dbcontext.UserTenderMappings
                                where u.TenderId == tenderList.Id
                                select u into m
                                select m.UserId).ToList<string>();
                            List<Registration> users = (
                                from u in dbcontext.Registrations
                                where list.Contains(u.UserId)
                                select u).ToList<Registration>();
                            object[] count = new object[] { "inside tender users at ", null, null, null };
                            now = DateTime.Now;
                            count[1] = now.ToString();
                            count[2] = " User count:";
                            count[3] = users.Count;
                            Log.Info(string.Concat(count), this);
                            foreach (Registration user in users)
                            {
                                string[] email = new string[] { "inside tender user at ", null, null, null, null, null };
                                now = DateTime.Now;
                                email[1] = now.ToString();
                                email[2] = " User email:";
                                email[3] = user.Email;
                                email[4] = "-";
                                email[5] = user.UserId;
                                Log.Info(string.Concat(email), this);
                                mail.To.Add(user.Email);
                                mail.Body = mail.Body.Replace("#TenderName#", tenderList.Description);
                                mail.Body = mail.Body.Replace("#TenderNIT#", tenderList.NITNo);
                                mail.Subject = mail.Subject.Replace("#Subject#", tenderList.NITNo);
                                MainUtil.SendMail(mail);
                                string[] userId = new string[] { "Success for email sending at ", null, null, null, null, null };
                                now = DateTime.Now;
                                userId[1] = now.ToString();
                                userId[2] = " User email:";
                                userId[3] = user.Email;
                                userId[4] = "-";
                                userId[5] = user.UserId;
                                Log.Info(string.Concat(userId), this);
                            }
                            TenderList tenderToUpdate = (
                                from t in dbcontext.TenderLists
                                where t.Id == tenderList.Id
                                select t).FirstOrDefault<TenderList>();
                            tenderToUpdate.IsReminderMailSent = new bool?(true);
                            dbcontext.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }

        public void SendTenderMail(string email, string redirectUrl)
        {
            try
            {
                MailMessage mail = this.GetTenderMailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("{0}", redirectUrl);
                MainUtil.SendMail(mail);
            }
            catch
            {
                throw;
            }
        }

        public void SendUserRegistrationEmail(string email, string redirectUrl, string Username, string pwd, string NitNo, string Description)
        {
            try
            {
                MailMessage mail = this.GetTenderMailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#url#", redirectUrl);
                mail.Body = mail.Body.Replace("#UserName#", Username);
                mail.Body = mail.Body.Replace("#Pwd#", pwd);
                mail.Body = mail.Body.Replace("#TenderName#", Description);
                mail.Body = mail.Body.Replace("#NitNo#", NitNo);
                mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                MainUtil.SendMail(mail);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Unable to send the email to the ", email, " - Error - ", ex.Message) ?? "", ex, this);
                throw;
            }
        }
    }
}