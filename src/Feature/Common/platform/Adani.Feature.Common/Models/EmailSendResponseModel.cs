using System.Collections.Generic;

namespace Adani.Feature.Common.Models
{
    public class EmailSendResponseModel
    {
        public bool Success { get; set; } = false;
        public IEnumerable<KeyValuePair<string, string>> Errors { get; set; }
    }
}
