using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniVidyamandir.Website.Models
{
    public class VidyaMandirContactModel
    {
        
            [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(VidyaMandirContactModel))]
            [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(VidyaMandirContactModel))]
            public string Name
            {
                get;
                set;
            }
            [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(VidyaMandirContactModel))]
            [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(VidyaMandirContactModel))]
            public string MobileNo
            {
                get;
                set;
            }
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(VidyaMandirContactModel))]
            [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(VidyaMandirContactModel))]
            public string EmailID
            {
                get;
                set;
            }

            public static string InvalidEmailAddress
            {
                get
                {
                    return DictionaryPhraseRepository.Current.Get("VidyaMandir/Contact-Form/Invalid Email Address", "Please enter a valid email address");
                }
            }

            public static string InvalidMobile
            {
                get
                {
                    return DictionaryPhraseRepository.Current.Get("VidyaMandir/Contact-Form/Invalid Mobile", "Please enter a valid Mobile Number");
                }
            }
            public static string InvalidName
            {
                get
                {
                    return DictionaryPhraseRepository.Current.Get("VidyaMandir/Contact-Form/Invalid Name", "Please enter a valid Name");
                }
            }
            public static string Required
            {
                get
                {
                    return DictionaryPhraseRepository.Current.Get("VidyaMandir/Contact-Form/Required", "Please enter value for {0}");
                }
            }

            public string reResponse
            {
                get;
                set;
            }
            public string Message
            {
                get;
                set;
            }
            public string MessageType
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


        }
    
}
