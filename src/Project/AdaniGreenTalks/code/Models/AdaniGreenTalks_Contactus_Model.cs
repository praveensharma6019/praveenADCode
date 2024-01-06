// Decompiled with JetBrains decompiler
// Type: Sitecore.AdaniGreenTalks.Website.Models.AdaniGreenTalks_Contactus_Model
// Assembly: Sitecore.AdaniGreenTalks.Website, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E01E7F81-DBA4-49B5-BF38-8DFD57A2798A
// Assembly location: D:\Deployments\Stage\Sitecore.AdaniGreenTalks.Website.dll

using Sitecore.Configuration;
using System;
using System.Collections.Generic;

namespace Sitecore.AdaniGreenTalks.Website.Models
{
      public class AdaniGreenTalks_Contactus_Model
      {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string ContactNumber { get; set; }

            public string CustomerQuery { get; set; }
       
            public string WriteAboutYourself { get; set; }

            public DateTime SubmittedDate { get; set; }

            public string FormType { get; set; }

            public string FormUrl { get; set; }

            public string googleCaptchaToken { get; set; }
      }
    public class JsonResultModel
    {
        public bool IsSuccess { get; set; } = false;

        public bool IsValid { get; set; } = false;

        public ErrorMessage errorModel { get; set; }
        public AdaniGreenTalks_Contactus_Model ContactUs { get; set; }
        public AdaniGreenTalks_Contribute_Model Contribute { get; set; }
        public AdaniGreenTalks_Speak_Model Speak { get; set; }
        public List<ContactValidation> contactvalidationlist { get; set; }
    }
    public class ErrorMessage
    {
        public string errorMessage { get; set; }
        public string ErrorSource { get; set; }
        public bool IsError { get; set; }
    }
    public class ContactValidation
    {
        public string StatusCode { get; set; }
        public string FieldErrorMessage { get; set; }
    }
}
