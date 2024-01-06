using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models.AdaniGasCNGRegistration
{
    public class AdaniGasCNG_DealerDetails
    {


        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasCNG_DealerDetails))]
        public string VendorNumber { get; set; }
       
        
        [StringLength(15, ErrorMessage = "CNGKitNumber must not be more than 15 char")]

        public string CNGKitNumber { get; set; }
        public string VehicleType { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string VehicleModel { get; set; }
        public string RegistrationNumber { get; set; }
        public string VehicleCompany{ get; set; }
        public string VehicleNumber { get; set; }
        public bool IsVendorNumberValid { get; set; }
        public string PageInfo { get; set; }
        public Guid Id
        {
            get;
            set;
        }
        public string FormName { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/CNG Registration/Required", "Please enter a value for {0}");
        public HttpPostedFileBase UploadVehicleInsurance
        {
            get;
            set;
        }
        public string UploadVehicleInsuranceLink
        {
            get;
            set;
        }
        public HttpPostedFileBase UploadRCBook
        {
            get;
            set;
        }
        public string UploadRCBookLink
        {
            get;
            set;
        }
        public HttpPostedFileBase UploadAadharCard
        {
            get;
            set;
        }
        public string UploadAadharCardLink
        {
            get;
            set;
        }
        public HttpPostedFileBase UploadPanCard
        {
            get;
            set;
        }
        public string UploadPanCardLink
        {
            get;
            set;
        }
        public class FileUpload1
        {
            public string ErrorMessage { get; set; }
            public decimal filesize { get; set; }
            public string UploadUserFile(HttpPostedFileBase ResumeAttachment)
            {
                try
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "pdf", "png" };
                    var fileExt = System.IO.Path.GetExtension(ResumeAttachment.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt.ToLower()))
                    {
                        ErrorMessage = ResumeAttachment.FileName + " is invalid file - Only Upload jpg/pdf/jpeg/png File";
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

            public bool FileMIMEisValid(HttpPostedFileBase attchment)
            {
                {
                    var mime = attchment.ContentType;

                    if (mime == "application/pdf" || mime == "image/jpeg")
                    {
                        // upload the File because file is valid  
                        return true;
                    }
                    else
                    {
                        //  file is Invalid  
                        return false;

                    }
                }
            }

        }
    }
       
}