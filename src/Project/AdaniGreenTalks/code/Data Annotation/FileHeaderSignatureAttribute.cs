using Sitecore.AdaniGreenTalks.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website
{
    public class FileHeaderSignatureAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "only pdf files are allowed";
        string _fileSignature = string.Empty;
        public FileHeaderSignatureAttribute(string fileSignature)
        {
            _fileSignature = fileSignature;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            AdaniGreenTalks_JoinUs_Model loginModel = (AdaniGreenTalks_JoinUs_Model)validationContext.ObjectInstance;
            var file1 = loginModel.Doc_UploadProject as HttpPostedFileBase;

            var file = value as HttpPostedFileBase;
            if (file1 == null)
            {
                return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
            }
            if (file == null && loginModel.Participate_AdaniPrizes == null)
            {
                return ValidationResult.Success;
            }
            if (file == null && loginModel.Participate_AdaniPrizes == "check")
            {
                return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
            }
            BinaryReader b = new BinaryReader(file.InputStream);
            byte[] bindata = b.ReadBytes(file.ContentLength);
            string filecontent = System.Convert.ToBase64String(bindata);
            if (!(filecontent.StartsWith(_fileSignature)))
            {
                return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);

            }
            b.Close();
            return ValidationResult.Success;

        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_fileSignature.ToString());
        }
    }
}