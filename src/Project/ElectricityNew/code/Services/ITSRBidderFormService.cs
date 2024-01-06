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
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace Sitecore.ElectricityNew.Website.Services
{
    public class ITSRBidderFormService
    {
        public string ValidateFile(HttpPostedFileBase file)
        {
            string ErrorMessage = string.Empty;
            try
            {
                var supportedTypes = new[] { "jpg", "jpeg", "pdf", "png", "ppt", "pptx", "doc", "docx", "xls", "xlsx", "txt", "zip", "rar" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                    ErrorMessage = file.FileName + " is invalid file type";
                    return ErrorMessage;
                }
                else if (file.ContentLength / 1048576 > 5)
                {
                    ErrorMessage = file.FileName + "is too big, file size Should Be UpTo 5 MB.";
                    return ErrorMessage;
                }
                else
                {
                    ErrorMessage = string.Empty;
                    return ErrorMessage;
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Upload Container Should Not Be Empty or Contact Admin";
                return ErrorMessage;
            }
        }

        public List<ITSR_UserRole> GetUserRoles()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                return dbcontext.ITSR_UserRoles.ToList();
            }
        }

        public string GetProposalNumber()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                long id = dbcontext.ITSR_BidderFormDetails.OrderByDescending(f => f.Id).FirstOrDefault().Id + 1;
                string preoposalNumber = "P" + id.ToString().PadLeft(5, '0');
                return preoposalNumber;
            }
        }

        public Guid? CreateNewForm(ITSRFormCreateModel model)
        {
            try
            {
                DateTime startDate = (DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                DateTime endDate = (DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    string proposalNumber = GetProposalNumber();
                    ITSR_BidderFormDetail formData = new ITSR_BidderFormDetail()
                    {
                        FormId = Guid.NewGuid(),
                        BuyerEmail = model.BuyerOwnerEmailId,
                        BuyerName = model.BuyerOwnerName,
                        ProposalOwnerEmail = model.ProposalOwnerEmailId,
                        CreatedBy = ITSRUserSession.ITSRUserSessionContext.userId,
                        CreatedDate = DateTime.Now,
                        EndDate = endDate,
                        IsActive = true,
                        ProposalOwnerName = model.ProposalOwnerName,
                        TenderNo = proposalNumber,
                        StartDate = startDate,
                        Title = model.Title
                    };
                    dbcontext.ITSR_BidderFormDetails.InsertOnSubmit(formData);
                    dbcontext.SubmitChanges();
                    return formData.FormId;
                }
            }
            catch (Exception e)
            {
                Log.Error("ITSR CreateNewForm " + e.Message, this);
            }
            return null;
        }

        public bool UpdateForm(ITSRFormCreateModel model)
        {
            try
            {
                DateTime startDate = (DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                DateTime endDate = (DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    Guid id = new Guid(model.FormId);
                    var formObj = dbcontext.ITSR_BidderFormDetails.Where(f => f.FormId == id).FirstOrDefault();

                    formObj.BuyerEmail = model.BuyerOwnerEmailId;
                    formObj.BuyerName = model.BuyerOwnerName;
                    formObj.ProposalOwnerEmail = model.ProposalOwnerEmailId;
                    formObj.ModifiedBy = ITSRUserSession.ITSRUserSessionContext.userId;
                    formObj.ModifiedDate = DateTime.Now;
                    formObj.CreatedDate = DateTime.Now;
                    formObj.EndDate = endDate;
                    formObj.IsActive = true;
                    formObj.ProposalOwnerName = model.ProposalOwnerName;
                    formObj.TenderNo = model.TenderNo;
                    formObj.StartDate = startDate;
                    formObj.Title = model.Title;

                    dbcontext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error("ITSR CreateNewForm " + e.Message, this);
            }
            return false;
        }

        public ITSR_BidderFormDetail GetFormDetails(Guid Id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (dbcontext.ITSR_BidderFormDetails.Any(f => f.FormId == Id))
                    return dbcontext.ITSR_BidderFormDetails.FirstOrDefault(f => f.FormId == Id);
            }
            return null;
        }

        public List<ITSR_Login> GetITSRUsers()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                return dbcontext.ITSR_Logins.OrderBy(c => c.Created_Date).ToList();
            }
        }

        public bool DisableITSRUser(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var user = dbcontext.ITSR_Logins.Where(u => u.Id == id).FirstOrDefault();
                user.IsActive = false;
                dbcontext.SubmitChanges();
                return true;
            }
        }

        public bool EnableITSRUser(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var user = dbcontext.ITSR_Logins.Where(u => u.Id == id).FirstOrDefault();
                user.IsActive = true;
                dbcontext.SubmitChanges();
                return true;
            }
        }

        public string GenerateRandomPassword()
        {
            string returnString = string.Empty;
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            returnString = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            return returnString;
        }

        public bool IsUserExists(ITSRUserModel model)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (dbcontext.ITSR_Logins.Any(u => u.UserId == model.UserName))
                    return true;
                else
                    return false;
            }
        }

        public string CreateNewUser(ITSRUserModel model)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                ITSR_Login obj = new ITSR_Login
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    IsActive = true,
                    Mobile = model.Mobile,
                    Name = model.Name,
                    UserRole = model.Role,
                    UserId = model.UserName,
                    Password = GenerateRandomPassword(),
                    Created_Date = DateTime.Now,
                    CreatedBy = ITSRUserSession.ITSRUserSessionContext.userId,
                };
                dbcontext.ITSR_Logins.InsertOnSubmit(obj);
                dbcontext.SubmitChanges();
                return obj.Password;
            }
        }

        public List<ITSR_BidderFormSubmissions_Document> GetBidderSubmittedDocuments(int bidderSubmissionId)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (dbcontext.ITSR_BidderFormSubmissions_Documents.Any(f => f.BidderFormSubmissionId == bidderSubmissionId))
                    return dbcontext.ITSR_BidderFormSubmissions_Documents.Where(f => f.BidderFormSubmissionId == bidderSubmissionId).ToList();
            }
            return null;
        }

        public ITSR_BidderFormSubmission GetBidderSubmittedFormDetails(int Id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (dbcontext.ITSR_BidderFormSubmissions.Any(f => f.Id == Id))
                    return dbcontext.ITSR_BidderFormSubmissions.FirstOrDefault(f => f.Id == Id);
            }
            return null;
        }

        public ITSR_BidderFormSubmissions_Document CreateDocumentObject(HttpPostedFileBase file, string fileDescription, string documentTypeCode, string formId, long formIdNum, long objSubmissionId)
        {
            try
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(file.InputStream))
                {
                    bytes = br.ReadBytes(file.ContentLength);
                }

                ITSR_BidderFormSubmissions_Document obj = new ITSR_BidderFormSubmissions_Document
                {
                    BidderFormDetailsId = formIdNum,
                    BidderFormSubmissionId = objSubmissionId,
                    CreatedDate = DateTime.Now,
                    DocumentContentType = file.ContentType,
                    DocumentData = bytes,
                    DocumentDescription = fileDescription,
                    DocumentName = file.FileName,
                    DocumenttypeCode = documentTypeCode,
                    FormId = formId
                };
                return obj;
            }
            catch (Exception ex)
            {
                Log.Error("Error at CreateDocumentObject:" + ex.Message, this);
            }
            return null;
        }


        public List<ITSRFormListing> GetAllFormsByUser(string userId, string userRole)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (userRole.ToLower() == "superadmin")
                {
                    var listBidderFormDetails = dbcontext.ITSR_BidderFormDetails.OrderByDescending(d => d.CreatedDate).ToList();
                    List<ITSRFormListing> model = new List<ITSRFormListing>();
                    foreach (var d in listBidderFormDetails)
                    {
                        ITSRFormListing obj = new ITSRFormListing
                        {
                            CreatedDate = d.CreatedDate,
                            CreatedBy = d.CreatedBy,
                            EndDate = d.EndDate,
                            StartDate = d.StartDate,
                            TenderNo = d.TenderNo,
                            Title = d.Title,
                            FormId = d.FormId.ToString()
                        };
                        model.Add(obj);
                    }
                    return model;
                }
                else
                {
                    var listBidderFormDetails = dbcontext.ITSR_BidderFormDetails.Where(d => d.CreatedBy == userId).OrderByDescending(d => d.CreatedDate).ToList();
                    List<ITSRFormListing> model = new List<ITSRFormListing>();
                    foreach (var d in listBidderFormDetails)
                    {
                        ITSRFormListing obj = new ITSRFormListing
                        {
                            CreatedDate = d.CreatedDate,
                            CreatedBy = d.CreatedBy,
                            EndDate = d.EndDate,
                            StartDate = d.StartDate,
                            TenderNo = d.TenderNo,
                            Title = d.Title,
                            FormId = d.FormId.ToString()
                        };
                        model.Add(obj);
                    }
                    return model;
                }
            }
        }

        public List<ITSR_BidderFormSubmission> GetAllBidderSubmissionsByForm(string formId)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var listBidderFormDetails = dbcontext.ITSR_BidderFormSubmissions.Where(b => b.FormId == formId).OrderByDescending(d => d.Id).ToList();
                return listBidderFormDetails;
            }
        }


        public bool IsApplicationExistsForMobileNumber(string mobileNumber, string formId)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (dbcontext.ITSR_BidderFormSubmissions.Any(f => f.MobileNumber == mobileNumber && f.FormId == formId))
                    return true;
            }
            return false;
        }

       
        public ITSR_BidderFormSubmission GetBidderFormDetails(string mobileNumber, string formId)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                return dbcontext.ITSR_BidderFormSubmissions.FirstOrDefault(f => f.MobileNumber == mobileNumber && f.FormId == formId);
            }
        }

        public MailMessage GetOnSubmitEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Template.ITSR.BidderFormSubmitEmailToOwners);
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

        public MailMessage GetOnCreateUserEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Template.ITSR.CreateUserEmail);
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

        public MailMessage GetOnCreateEmailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Template.ITSR.BidderFormCreateEmailToOwners);
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

        public void SendOnSubmitEmail(string email, ITSRBidderProcessFormModel model)
        {
            //send email to owners
            try
            {
                var mail = this.GetOnSubmitEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#Title#", model.Title);
                mail.Body = mail.Body.Replace("#TenderNo#", model.TenderNo);
                mail.Body = mail.Body.Replace("#Company#", model.CompanyName);
                mail.Body = mail.Body.Replace("#Location#", model.Location);
                mail.Body = mail.Body.Replace("#Mobile#", model.MobileNumber);
                mail.Body = mail.Body.Replace("#EmailId#", model.EmailId);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                //throw;
            }
        }

        public void SendOnCreateUserEmail(ITSRUserModel model, string password, string redirectUrl)
        {
            //send email to owners
            try
            {
                var mail = this.GetOnCreateUserEmailTemplate();
                mail.To.Add(model.Email);
                mail.Body = mail.Body.Replace("#UserName#", model.UserName);
                mail.Body = mail.Body.Replace("#Password#", password);
                mail.Body = mail.Body.Replace("#FormLink#", redirectUrl);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"SendOnCreateUserEmail Unable to send the email to the " + model.Email + " - Error - " + ex.Message + "", ex, this);
                //throw;
            }
        }

        public void SendOnCreateEmail(string email, string title, string tenderNo, string formLink)
        {
            //send email to owners
            try
            {
                var mail = this.GetOnCreateEmailTemplate();
                mail.To.Add(email);
                mail.Body = mail.Body.Replace("#Title#", title);
                mail.Body = mail.Body.Replace("#TenderNo#", tenderNo);
                mail.Body = mail.Body.Replace("#FormLink#", formLink);
                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + email + " - Error - " + ex.Message + "", ex, this);
                //throw;
            }
        }

        public bool IsFormExists(Guid Id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (dbcontext.ITSR_BidderFormDetails.Any(f => f.FormId == Id && f.EndDate.Date >= DateTime.Now.Date))
                    return true;
            }
            return false;
        }

        public bool CheckForCAMaxOTPLimit(string mobileNumber, string pageName)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var count = dbcontext.OTPValidationAEMLs.Where(o => o.MobileNumber == mobileNumber && o.PageName == pageName && o.CreatedDate.GetValueOrDefault().Date == DateTime.Now.Date).Count();
                if (count >= 20)
                    return false;
                else
                    return true;
            }
        }

        private string GenerateRandomOTP(int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string sOTP = string.Empty;

            string sTempChars = string.Empty;

            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }
            return sOTP;

        }

        public string GenerateOTP(string accountNumber, string pageName, string mobileNumber)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                string sRandomOTP = GenerateRandomOTP(6);
                OTPValidationAEML entity = new OTPValidationAEML()
                {
                    MobileNumber = mobileNumber,
                    OTP = sRandomOTP,
                    AccountNumber = accountNumber,
                    PageName = pageName,
                    CreatedDate = DateTime.Now
                };
                dbcontext.OTPValidationAEMLs.InsertOnSubmit(entity);
                dbcontext.SubmitChanges();
                return sRandomOTP;
            }
        }

        public string GetOTP(string mobilenumber, string accountNumber, string pageName)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var OTPRecord = dbcontext.OTPValidationAEMLs.Where(o => o.MobileNumber == mobilenumber && o.AccountNumber == accountNumber && o.PageName == pageName).OrderByDescending(o => o.CreatedDate).FirstOrDefault();

                if (OTPRecord != null)
                {
                    return OTPRecord.OTP;
                }
                return string.Empty;
            }
        }


    }
}