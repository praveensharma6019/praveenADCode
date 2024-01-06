extern alias itextsharp;
using itextsharp::iTextSharp.text;
using itextsharp::iTextSharp.text.pdf;
using CaptchaMvc.HtmlHelpers;
using DotNetIntegrationKit;
using Newtonsoft.Json;
using paytm;
using RestSharp;
using SapPiService.Domain;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.Repositories;
using Sitecore.Feature.Accounts.Services;
using Sitecore.Foundation.Alerts.Models;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Attributes;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;
using System.Xml.Linq;
using Sitecore.Feature.Accounts.SessionHelper;
using System.Data;
using System.Diagnostics;
using static Sitecore.Feature.Accounts.Controllers.AccountsController;
using System.Drawing.Imaging;

namespace Sitecore.Feature.Accounts.UploadDocuments.AEMLDocuments
{
    public class AEMLUploadDocumentLibrary
    {
        public static string validateResult = string.Empty;
        private const int AccountNumberLength = 12;
        public static void GetApplicationDocumentList(NewConnectionApplication model)
        {
            try
            {
                if (model.ApplicanttType != null && model.ApplicationTitle != null)
                {
                    NewConnectionService newconnectionservice = new NewConnectionService();
                    model.GetExistingDocuments = GetExistingDocument(model.ApplicationNo);
                    string LoadType = "1";
                    string titleValue = model.ApplicationTitle;
                    var IsJoint = model.ApplicanttType;

                    titleValue = titleValue.TrimStart('0');
                    if (titleValue != "6")
                    {
                        titleValue = "1";
                    }
                    var listOfDocs = newconnectionservice.GetDocuments(LoadType, titleValue, IsJoint);
                    model.DocumentList = listOfDocs;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Save Customer Documents
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="contenttype"></param>
        /// <param name="doc"></param>
        /// <param name="accountNumber"></param>
        /// <param name="RegistrationSerialNumber"></param>
        /// <param name="isDocSaved"></param>
        /// <param name="docNumber"></param>
        /// <returns></returns>
        public static NEW_CON_APPLICATION_DOCUMENT_DETAIL CreateDocumentObject(byte[] bytes, string fileName, string contenttype, NEWCON_DOCUMENTMASTER doc, string accountNumber, string RegistrationSerialNumber, bool isDocSaved, DateTime? createddate, string docNumber = null)
        {
            accountNumber = string.IsNullOrEmpty(accountNumber) ? accountNumber : accountNumber.PadLeft(AccountNumberLength, '0');
            NEW_CON_APPLICATION_DOCUMENT_DETAIL obj = new NEW_CON_APPLICATION_DOCUMENT_DETAIL();
            Regex reg = new Regex("[*'\",_&#^@$%]");
            fileName = reg.Replace(fileName, string.Empty);
            fileName = fileName.Replace(" ", string.Empty);
            try
            {
                DateTime? sapSentDate = null;
                if (isDocSaved) sapSentDate = DateTime.Now;
                obj = new NEW_CON_APPLICATION_DOCUMENT_DETAIL
                {
                    Id = Guid.NewGuid(),
                    DocumentType = doc.DOCID.ToString(),
                    DocumentTypeCode = doc.DOCTY.ToString(),
                    CreatedBy = accountNumber,
                    CreatedDate = createddate,
                    ModifiedBy = accountNumber,
                    ModifiedDate = DateTime.Now,
                    DocumentChecklistSerialNumber = doc.SRNO.ToString(),
                    DocumentData = bytes,
                    DocumentDescription = doc.DESCRIPTION,
                    DocumentName = fileName,
                    DocumentDataContenttype = contenttype,
                    DocumentNumber = docNumber,
                    IsSentToSAP = isDocSaved,
                    RegistrationSerialNumber = RegistrationSerialNumber,
                    SAPSentDate = sapSentDate
                };
            }
            catch (Exception ex)
            {
            }
            return obj;
        }

        /// <summary>
        /// Get Customer Save Documents
        /// </summary>
        /// <param name="registrationNumber"></param>
        /// <returns></returns>
        public static List<NEW_CON_APPLICATION_DOCUMENT_DETAIL> GetExistingDocument(string registrationNumber)
        {
            try
            {
                List<NEW_CON_APPLICATION_DOCUMENT_DETAIL> docList = new List<NEW_CON_APPLICATION_DOCUMENT_DETAIL>();
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {

                    if (dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.Any(a => a.RegistrationSerialNumber == registrationNumber))
                    {
                        docList = dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.Where(a => a.RegistrationSerialNumber == registrationNumber).ToList();
                        return docList;
                    }
                    else
                        return docList;
                }
            }
            catch (Exception ex)
            {
            }
            return new List<NEW_CON_APPLICATION_DOCUMENT_DETAIL>();
        }

        public static bool ValidateUploadFile(NewConnectionApplication model)
        {
            validateResult = string.Empty;
            bool validateFile = false;
            bool validateFile1 = ValidateIdentityDocuments(model);
            bool validateFile2 = ValidateOwnershipDocuments(model);
            bool validateFile3 = ValidatePhotoDocuments(model);
            bool validateFile4 = ValidateOtherDocuments(model);
            if (validateFile1 && validateFile2 && validateFile3 && validateFile4)
            {
                validateFile = true;
            }
            return validateFile;
        }

        public static bool UploadDocumentFile(NewConnectionApplication model)
        {
            bool status = true;
            string regno = model.ApplicationNo;
            string accountno = model.AccountNo;
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                status = UploadDocumentFile(dataContext, model, "ID", "IDFILE_1", model.IDDOC_1, model.DOCUMENTID_1, regno, accountno, model.HDNIDDOC_1);
                status = UploadDocumentFile(dataContext, model, "ID", "IDFILE_2", model.IDDOC_2, model.DOCUMENTID_2, regno, accountno, model.HDNIDDOC_2);
                if (model.ApplicantType == "2")
                {
                    status = UploadDocumentFile(dataContext, model, "ID", "JOINTIDFILE_1", model.IDJOINTDOC_1, model.JOINTDOCUMENTID_1, regno, accountno, model.HDNIDJOINTDOC_1);
                    status = UploadDocumentFile(dataContext, model, "ID", "JOINTIDFILE_2", model.IDJOINTDOC_1, model.JOINTDOCUMENTID_2, regno, accountno, model.HDNIDJOINTDOC_2);
                }
                status = UploadDocumentFile(dataContext, model, "OD", "OWNERFILE_1", model.OWNERDOC_1, string.Empty, regno, accountno, model.HDNOWNERDOC_1);
                status = UploadDocumentFile(dataContext, model, "OD", "OWNERFILE_2", model.OWNERDOC_2, string.Empty, regno, accountno, model.HDNOWNERDOC_2);
                status = UploadDocumentFile(dataContext, model, "OD", "OWNERFILE_3", model.OWNERDOC_3, string.Empty, regno, accountno, model.HDNOWNERDOC_3);
                status = UploadDocumentFile(dataContext, model, "OD", "OWNERFILE_4", model.OWNERDOC_4, string.Empty, regno, accountno, model.HDNOWNERDOC_4);
                status = UploadDocumentFile(dataContext, model, "OD", "OWNERFILE_5", model.OWNERDOC_5, string.Empty, regno, accountno, model.HDNOWNERDOC_5);
                var UploadFiles = HttpContext.Current.Request.Files;
                var UploadFileBoxNames = UploadFiles.AllKeys.ToList().FindAll(cv => cv.Contains("PHOTOFILE"));
                int i = 1;
                foreach (var fileboxname in UploadFileBoxNames)
                {
                    var file = UploadFiles[fileboxname] as HttpPostedFile;
                    if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                    {
                        var docarray = fileboxname.Split('_');
                        status = UploadDocumentFile(dataContext, model, "PH", fileboxname, docarray[1], string.Empty, regno, accountno, (i == 1 ? model.HDNPHOTOFILEID_1 : model.HDNPHOTOFILEID_2));
                    }
                    i++;
                }

                UploadFileBoxNames = UploadFiles.AllKeys.ToList().FindAll(cv => cv.Contains("SEZFILE"));
                i = 1;
                foreach (var fileboxname in UploadFileBoxNames)
                {
                    var file = UploadFiles[fileboxname] as HttpPostedFile;
                    if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                    {
                        var docarray = fileboxname.Split('_');
                        status = UploadDocumentFile(dataContext, model, "PH", fileboxname, docarray[1], string.Empty, regno, accountno, (i == 1 ? model.HDNPHOTOFILEID_1 : model.HDNPHOTOFILEID_2));
                    }
                    i++;
                }

                UploadFileBoxNames = UploadFiles.AllKeys.ToList().FindAll(cv => cv.Contains("OTHERDOCUMENTFILE"));
                foreach (var fileboxname in UploadFileBoxNames)
                {
                    var file = UploadFiles[fileboxname] as HttpPostedFile;
                    if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                    {
                        var docarray = fileboxname.Split('_');
                        string hdnsnoid = "HDN" + docarray[0] + "ID_" + docarray[1];
                        status = UploadDocumentFile(dataContext, model, "OT", fileboxname, docarray[1], string.Empty, regno, accountno, HttpContext.Current.Request.Form[hdnsnoid]);
                    }
                }

                var objApp = dataContext.NEW_CON_APPLICATION_FORMs.SingleOrDefault(x => x.Id.ToString() == model.Id.ToString());
                if (objApp != null)
                {
                    objApp.ApplicationModel = model.ApplicationMode;
                }
                dataContext.SubmitChanges();

            }
            return status;
        }
        public static bool ValidateIdentityDocuments(NewConnectionApplication model)
        {
            bool validateFile = true;
            double filesize = 2048;
            if (HttpContext.Current.Request.Files["IDFILE_1"] != null && HttpContext.Current.Request.Files["IDFILE_1"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["IDFILE_1"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
                if (string.IsNullOrEmpty(model.DOCUMENTID_1))
                {
                    validateResult = validateResult + "Document Number is required" + "\n";
                    validateFile = false;
                }
            }
            else if (string.IsNullOrEmpty(model.IDDOC_1))
            {
                validateResult = validateResult + "Identity document is required" + "\n";
                validateFile = false;
            }


            if (HttpContext.Current.Request.Files["IDFILE_2"] != null && HttpContext.Current.Request.Files["IDFILE_2"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["IDFILE_2"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
                if (string.IsNullOrEmpty(model.DOCUMENTID_2))
                {
                    validateResult = validateResult + "Document Number is required" + "\n";
                    validateFile = false;
                }
            }
            if (model.ApplicantType == "2")
            {
                if (HttpContext.Current.Request.Files["JOINTIDFILE_1"] != null && HttpContext.Current.Request.Files["JOINTIDFILE_1"].ContentLength > 0)
                {
                    var UploadFiles = HttpContext.Current.Request.Files;
                    HttpPostedFile file = UploadFiles["JOINTIDFILE_1"] as HttpPostedFile;
                    string validate = CheckFileContent(file, filesize, true);
                    if (!string.IsNullOrEmpty(validate))
                    {
                        validateResult = validateResult + validate + "\n";
                        validateFile = false;
                    }
                    if (string.IsNullOrEmpty(model.JOINTDOCUMENTID_1))
                    {
                        validateResult = validateResult + "Document Number is required" + "\n";
                        validateFile = false;
                    }
                }
                else if (string.IsNullOrEmpty(model.IDJOINTDOC_1))
                {
                    validateResult = validateResult + "joint holder Identity document is required" + "\n";
                    validateFile = false;
                }
                if (HttpContext.Current.Request.Files["JOINTIDFILE_2"] != null && HttpContext.Current.Request.Files["JOINTIDFILE_2"].ContentLength > 0)
                {
                    var UploadFiles = HttpContext.Current.Request.Files;
                    HttpPostedFile file = UploadFiles["JOINTIDFILE_2"] as HttpPostedFile;
                    string validate = CheckFileContent(file, filesize, true);
                    if (!string.IsNullOrEmpty(validate))
                    {
                        validateResult = validateResult + validate + "\n";
                        validateFile = false;
                    }
                    if (string.IsNullOrEmpty(model.JOINTDOCUMENTID_2))
                    {
                        validateResult = validateResult + "Document Number is required" + "\n";
                        validateFile = false;
                    }
                }
            }
            return validateFile;
        }

        public static bool ValidateOwnershipDocuments(NewConnectionApplication model)
        {
            bool validateFile = true;
            double filesize = 2048;
            if (HttpContext.Current.Request.Files["OWNERFILE_1"] != null && HttpContext.Current.Request.Files["OWNERFILE_1"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["OWNERFILE_1"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
            }
            else if (string.IsNullOrEmpty(model.OWNERDOC_1))
            {
                validateResult = validateResult + "ownership document is required" + "\n";
                validateFile = false;
            }

            if (HttpContext.Current.Request.Files["OWNERFILE_2"] != null && HttpContext.Current.Request.Files["OWNERFILE_2"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["OWNERFILE_2"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
            }
            if (HttpContext.Current.Request.Files["OWNERFILE_3"] != null && HttpContext.Current.Request.Files["OWNERFILE_3"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["OWNERFILE_3"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
            }
            if (HttpContext.Current.Request.Files["OWNERFILE_4"] != null && HttpContext.Current.Request.Files["OWNERFILE_4"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["OWNERFILE_4"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
            }
            if (HttpContext.Current.Request.Files["OWNERFILE_5"] != null && HttpContext.Current.Request.Files["OWNERFILE_5"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["OWNERFILE_5"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
            }
            return validateFile;
        }

        public static bool ValidatePhotoDocuments(NewConnectionApplication model)
        {
            bool validateFile = true;
            double filesize = 2048;
            if (HttpContext.Current.Request.Files["PHOTOFILE_1"] != null && HttpContext.Current.Request.Files["PHOTOFILE_1"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["PHOTOFILE_1"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
            }
            else if (string.IsNullOrEmpty(model.HDNPHOTOFILE_1))
            {
                validateResult = validateResult + "photo document is required" + "\n";
                validateFile = false;
            }
            if (model.ApplicantType == "2")
            {
                if (HttpContext.Current.Request.Files["PHOTOFILE_2"] != null && HttpContext.Current.Request.Files["PHOTOFILE_2"].ContentLength > 0)
                {
                    var UploadFiles = HttpContext.Current.Request.Files;
                    HttpPostedFile file = UploadFiles["PHOTOFILE_2"] as HttpPostedFile;
                    string validate = CheckFileContent(file, filesize, true);
                    if (!string.IsNullOrEmpty(validate))
                    {
                        validateResult = validateResult + validate + "\n";
                        validateFile = false;
                    }
                }

                else if (string.IsNullOrEmpty(model.HDNPHOTOFILE_2))
                {
                    validateResult = validateResult + "joint holder photo document is required" + "\n";
                    validateFile = false;
                }

            }
            if (model.IsSez == "1" && HttpContext.Current.Request.Files["SEZFILE_1"] != null && HttpContext.Current.Request.Files["SEZFILE_1"].ContentLength > 0)
            {
                var UploadFiles = HttpContext.Current.Request.Files;
                HttpPostedFile file = UploadFiles["SEZFILE_1"] as HttpPostedFile;
                string validate = CheckFileContent(file, filesize, true);
                if (!string.IsNullOrEmpty(validate))
                {
                    validateResult = validateResult + validate + "\n";
                    validateFile = false;
                }
            }
            else if (model.IsSez == "1" && string.IsNullOrEmpty(model.HDNSEZFILE_1))
            {
                validateResult = validateResult + "sez document is required" + "\n";
                validateFile = false;
            }

            return validateFile;
        }

        public static bool ValidateOtherDocuments(NewConnectionApplication model)
        {
            int count = 0;
            bool validateFile = true;
            double filesize = 2048;
            var UploadFiles = HttpContext.Current.Request.Files;
            var UploadFileBoxNames = UploadFiles.AllKeys.ToList().FindAll(cv => cv.Contains("OTHERDOCUMENTFILE"));
            foreach (var fileboxname in UploadFileBoxNames)
            {
                var savevalue = HttpContext.Current.Request.Form["HDN" + fileboxname];
                var file = UploadFiles[fileboxname] as HttpPostedFile;
                if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                {
                    string validate = CheckFileContent(file, filesize, true);
                    if (!string.IsNullOrEmpty(validate))
                    {
                        validateResult = validateResult + validate + "\n";
                        validateFile = false;
                    }
                    count = 1;
                }
                else if (!string.IsNullOrEmpty(savevalue))
                {
                    count = 1;
                }
            }
            if (count == 0 && UploadFileBoxNames.Count > 0)
            {
                validateResult = validateResult + "Atleast one other document is required" + "\n";
                validateFile = false;
            }
            return validateFile;
        }

        public static bool UploadDocumentFile(PaymentHistoryDataContext dataContext, NewConnectionApplication model, string doctype, string fileuploadid, string docid, string documentno, string applicationno, string accountno, string documentSerailNo)
        {
            bool status = true;
            DateTime? createddate = DateTime.Now;
            try
            {
                if (HttpContext.Current.Request.Files[fileuploadid] != null && HttpContext.Current.Request.Files[fileuploadid].ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(documentSerailNo))
                    {
                        var obj = dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.Where(x => x.Id.ToString() == documentSerailNo).FirstOrDefault();
                        if (obj != null)
                        {
                            if (dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.Any(x => x.Id.ToString() == documentSerailNo))
                            {
                                dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.DeleteOnSubmit(dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.FirstOrDefault(x => x.Id.ToString() == documentSerailNo));
                                dataContext.SubmitChanges();
                            }
                            createddate = obj.CreatedDate;
                        }
                    }
                    else
                    {
                        var obj = dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.Where(x => x.DocumentType == docid && x.DocumentTypeCode == doctype && x.RegistrationSerialNumber.ToString() == applicationno).FirstOrDefault();
                        if (obj != null)
                        {
                            dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.DeleteAllOnSubmit(dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.Where(x => x.DocumentType == docid && x.DocumentTypeCode == doctype && x.RegistrationSerialNumber.ToString() == applicationno));
                            dataContext.SubmitChanges();
                        }
                    }
                    var UploadFiles = HttpContext.Current.Request.Files;
                    HttpPostedFile file = UploadFiles[fileuploadid] as HttpPostedFile;
                    var doc = model.DocumentList.Where(d => d.DOCID == System.Convert.ToInt32(docid) && d.DOCTY.Trim() == doctype).FirstOrDefault();
                    if (doc != null)
                    {
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        bool isDocSaved = false;
                        var obj = AEMLUploadDocumentLibrary.CreateDocumentObject(bytes, file.FileName, file.ContentType, doc, accountno, applicationno, isDocSaved, createddate, documentno);
                        dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.InsertOnSubmit(obj);
                        dataContext.SubmitChanges();
                    }
                    else if (doctype == "TR")
                    {
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        bool isDocSaved = false;
                        var objDoc = new NEWCON_DOCUMENTMASTER()
                        {
                            DOCID = System.Convert.ToInt32(docid),
                            DOCTY = doctype,
                            SRNO = System.Convert.ToInt32(docid),
                            DESCRIPTION = "TRDOCUMENT"

                        };
                        var obj = AEMLUploadDocumentLibrary.CreateDocumentObject(bytes, file.FileName, file.ContentType, objDoc, accountno, applicationno, isDocSaved, createddate, documentno);
                        dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.InsertOnSubmit(obj);
                        dataContext.SubmitChanges();
                    }
                    else if (doctype == "SZ")
                    {
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        bool isDocSaved = false;
                        var objDoc = new NEWCON_DOCUMENTMASTER()
                        {
                            DOCID = System.Convert.ToInt32(docid),
                            DOCTY = doctype,
                            SRNO = System.Convert.ToInt32(docid),
                            DESCRIPTION = "SEZDOCUMENT"

                        };
                        var obj = AEMLUploadDocumentLibrary.CreateDocumentObject(bytes, file.FileName, file.ContentType, objDoc, accountno, applicationno, isDocSaved, createddate, documentno);
                        dataContext.NEW_CON_APPLICATION_DOCUMENT_DETAILs.InsertOnSubmit(obj);
                        dataContext.SubmitChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public static string CheckFileContent(HttpPostedFile file, double filesize, bool IsPDFInclude)
        {
            string ErrorMessage = string.Empty;
            try
            {
                var supportedTypes = new[] { "jpg", "jpeg", "png" };
                if (IsPDFInclude)
                    supportedTypes = new[] { "jpg", "jpeg", "pdf", "png" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                    ErrorMessage = file.FileName + " is invalid file - Only Upload jpg/pdf/jpeg/png File";
                }
                else if (file.ContentLength > ((filesize * 1024) * 1024))
                {
                    ErrorMessage = file.FileName + "is too big, file size Should Be UpTo " + filesize + "MB";
                }
                else
                {
                    ErrorMessage = string.Empty;
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Upload Container Should Not Be Empty or Contact Admin";
            }
            return ErrorMessage;
        }
    }
}