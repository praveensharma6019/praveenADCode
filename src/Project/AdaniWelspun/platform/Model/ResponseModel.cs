using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.AdaniWelspunSXA.Project.Model
{
    public class ResponseModel
    {
        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool isSucess { get; set; }

        public bool isError { get; set; }

        public string ErrorMessage { get; set; }
    }
}