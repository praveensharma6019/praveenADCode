using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class QuickAccessServices : ProfileBasicInfo
    {
        [Display(Name = nameof(LastNameCaption), ResourceType = typeof(QuickAccessServices))]
        public string ComplaintNumber { get; set; }

        public static string LastNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Last Name", "Last name");

        public string Captcha { get; set; }

        public string ComplainCode { get; set; }
        public string Message { get; set; }

    }
}