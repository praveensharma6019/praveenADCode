using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Sanitization
{
    public static class StringSanitization
    {
        public static string UseRegex(this string strIn)
        {
            // Replace invalid characters with empty strings.
            return Regex.Replace(strIn, @"[^\w\.@-]", "");
        }
        public static string ReplaceBlank(this string strIn)
        {
            // Replace invalid characters with empty strings.
            return strIn.Replace("%20", " ");
        }
    }
}