using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace Sitecore.Marathon.Website.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SignatureHeaderAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool isValid = true;
            if (value!=null)
            {
                HttpPostedFileBase file = value as HttpPostedFileBase;
                 isValid = false;

                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] bindata = b.ReadBytes(file.ContentLength);
                string filecontent = System.Convert.ToBase64String(bindata);

                /*pdf/png/jpj */
                if (filecontent.StartsWith("JVBER") == true || filecontent.StartsWith("iVBOR") == true || filecontent.StartsWith("/9j/") == true)
                {
                    return true;
                }
                return isValid;
            }
            return isValid;
        }
    }
}