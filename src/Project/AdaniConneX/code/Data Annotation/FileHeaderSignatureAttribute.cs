using Sitecore.AdaniConneX.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniConneX.Website
{
    public class FileHeaderSignatureAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "406";
        string _fileSignature = string.Empty;
        public FileHeaderSignatureAttribute(string fileSignature)
        {
            _fileSignature = fileSignature;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
                    
            var file = value as HttpPostedFileBase;
            if (file == null)
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