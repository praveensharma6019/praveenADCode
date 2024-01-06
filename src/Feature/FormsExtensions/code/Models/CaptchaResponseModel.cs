using System.Collections.Generic;

namespace Sitecore.Feature.FormsExtensions.Models
{
    public class CaptchaResponseModel
    {
        public bool Success { get; set; } = false;
        public IEnumerable<KeyValuePair<string, string>> Errors { get; set; }
    }
}