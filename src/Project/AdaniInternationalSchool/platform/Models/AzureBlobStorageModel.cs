using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class AzureBlobStorageModel
    {
        [Serializable]
        public class DeleteFileReponseModel
        {
            public bool IsDeleted { get; set; }
            public string StatusMsg { get; set; }
            public bool Status { get; set; }
        }
        [Serializable]
        public class RenameFileReponseModel
        {
            public bool IsExistingDeleted { get; set; }
            public bool IsNewCreated { get; set; }
            public string StatusMsg { get; set; }
            public string URL { get; set; }
            public bool Status { get; set; }
        }
        [Serializable]
        public class UploadFileReponseModel
        {
            public bool IsUploaded { get; set; }
            public string StatusMsg { get; set; }
            public string URL { get; set; }
            public bool Status { get; set; }
        }
    }
}