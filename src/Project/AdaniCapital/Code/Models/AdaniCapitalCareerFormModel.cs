using System;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.AdaniCapital.Website.Helper;
using System.Web;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sitecore.AdaniCapital.Website.Models
{
    public class AdaniCapitalCareerFormModel
    {
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string Name
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidLastName", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string LastName
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string MobileNo
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string EmailID
        {
            get;
            set;
        }

        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/Career-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/Career-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/Career-Form/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string InvalidLastName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/Career-Form/Invalid LastName", "Please enter a valid LastName");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/Career-Form/Required", "Please enter value for {0}");
            }
        }

        public string reResponse
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z0-9 ,-]*$", ErrorMessageResourceName = "InvalidData", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string Experience
        {
            get;
            set;
        }
        public static string InvalidData
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/Career-Form/Invalid Data", "No special charchters are allowd except , - .");
            }
        }
        [RegularExpression("^[a-zA-Z0-9 ,-]*$", ErrorMessageResourceName = "InvalidData", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string Position
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z0-9 ,-]*$", ErrorMessageResourceName = "InvalidData", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string ExpectedSalary
        {
            get;
            set;
        }
        
        public DateTime ExpectedJoiningDate
        {
            get;
            set;
        }
        [RegularExpression("^https:\\/\\/[a-z]{2,3}\\.linkedin\\.com\\/.*$", ErrorMessageResourceName = "InvalidLinkdinUrl", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string LinkedinProfile
        {
            get;
            set;
        }
        public static string InvalidLinkdinUrl
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCapital/Career-Form/Invalid Data", "No special charchters are allowd except , - .");
            }
        }
        [RegularExpression("^[a-zA-Z0-9 ,-]*$", ErrorMessageResourceName = "InvalidData", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCapitalCareerFormModel))]
        public string City
        {
            get;
            set;
        }


        public string PageInfo
        {
            get;
            set;
        }
        public DateTime SubmitOnDate
        {
            get;
            set;
        }


        public string SubmittedBy
        {
            get;
            set;
        }
        public string FormName
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }
        public string RegistrationNo
        {
            get;
            set;
        }
        public HttpPostedFileBase ResumeAttachment
        {
            get;
            set;
        }
        public string UploadedResumeLink
        {
            get;
            set;
        }
        public AdaniCapitalCareerFormModel()
        {
            AdaniCapitalCareerForm helper = new AdaniCapitalCareerForm();


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

                    if (mime == "application/pdf" || mime == "application/msword" || mime == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
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