using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class AdaniGasNameTransferUploadDocument
    {

        public string RequestNumber { get; set; }
        public string DocName { get; set; }
        public string Message { get; set; }
        public string MsgFlag { get; set; }
        public string Document { get; set; }
      
    }
}