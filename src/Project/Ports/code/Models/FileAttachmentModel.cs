using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class FileAttachmentModel
    {
        public string GrievanceFileID { get; set; }
        public string GrievanceFileGmsRegistrationId { get; set; }
        public string GrievanceFileName { get; set; }
        public string GrievanceFileContentType { get; set; }
        public string GrievanceFileBlobURL { get; set; }
        public string GrievanceFileCreatedDate { get; set; }

    }
}