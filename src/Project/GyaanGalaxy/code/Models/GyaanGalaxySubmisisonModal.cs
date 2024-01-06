using System;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Linq;
using Sitecore.GyaanGalaxy.Website.Helper;

namespace Sitecore.GyaanGalaxy.Website.Models
{
    public class GyaanGalaxySubmisisonModal
    {

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string Name
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "InvalidAge", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string Age
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string ClassOrGrade
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string HouseAddress
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string CategoryOfProject
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string TitleOfProject
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string SchoolAddress
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidSchoolName", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string SchoolName
        {
            get;
            set;
        }

        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string MobileNo
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string EmailID
        {
            get;
            set;
        }

        public static string InvalidEmailAddress
        {
            get
            {

                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Submission-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Submission-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Submission-Form/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string InvalidSchoolName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Submission-Form/Invalid School Name", "Please enter a valid Name");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Registration-Form/Required", "Please enter value for {0}");
            }
        }

        public string reResponse
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string Purpose
        {
            get;
            set;
        }
        public static string InvalidAge
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Registration-Form/Invalid Age", "Please enter a valid age");
            }
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string Procedure
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string NatureOfDataCollection
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string Conclusions
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxySubmisisonModal))]
        public string PossibleResreachApplication
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
        public HttpPostedFileBase ResearchPaperAttachment
        {
            get;
            set;
        }
        public string UploadedResearchLink
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
        public GyaanGalaxySubmisisonModal()
        {
            GyaanGalaxySubmissionFormHelper helper = new GyaanGalaxySubmissionFormHelper();


        }
    }
}