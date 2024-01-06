using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Defence.Website
{
    public class Templates
    {
        public struct MailConfiguration
        {
            public static readonly ID MailConfigurationItemID = new ID("{84E8873F-3A51-4286-B2B7-BDD4E5B31A65}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{5E65827B-E20A-413B-9CDC-9082FEE24F28}");
                public static readonly ID Customer_MailFrom = new ID("{DBD8F622-899E-4C29-A658-F48DAE04E783}");
                public static readonly ID Customer_SuccessMessage = new ID("{ADEB4E78-16B6-46A1-A1F8-670AC7A3AA0C}");
                public static readonly ID Customer_FailureMessage = new ID("{E29566C9-1B27-4398-8BFD-82C1731F0637}");
                public static readonly ID Officials_SubjectName = new ID("{201F76F9-39A5-47DE-873E-8457B9677DCD}");
                public static readonly ID Officials_RecipientMail = new ID("{4AADF6A9-E717-4455-A9F1-4C29532D1819}");
                public static readonly ID Officials_Message = new ID("{673F0318-A311-465C-8890-743F7886A183}");
                public static readonly ID Officials_MailFrom = new ID("{F57980C1-D4BB-439E-9399-B280D50FFB1C}");
            }
        }
    }
}