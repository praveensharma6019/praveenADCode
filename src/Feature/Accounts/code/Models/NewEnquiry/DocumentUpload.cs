using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class DocumentUpload
    {
        public string SAPINQID { get; set; }
        public string SAPTRANSID { get; set; }
        public string AVLFLAG { get; set; }
        public string UPLOADFLAG { get; set; }
        public string PAIDFLAG { get; set; }
        public string FSOID { get; set; }
        public string STATUS { get; set; }
        public string CREATEDATE { get; set; }
        public string SAPINQDT { get; set; }
        public List<sap_inq_attch> sap_inq_attch { get; set; }
    }


    public class sap_inq_attch
    {
        public string ROWID { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
        public string FILENAME { get; set; }
        public string Documnet { get; set; }
        public string FileExt { get; set; }
        public string SAPINQID { get; set; }
        public string CREATEDATE { get; set; }

        //public string DocStatus { get; set; }
        //public int INQID { get; set; }
        //public string UPDATEFLAG { get; set; }
        //public string Remark { get; set; }
        //public string DocNum { get; set; }
        //public string ExpDate { get; set; }

    }


    public class AddDocumentInExistingEnquiry
    {
        public int ROWID { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
        public string FILENAME { get; set; }
        public string Documnet { get; set; }
        public string FileExt { get; set; }
        public int INQID { get; set; }
        public string SAPINQID { get; set; }
        public string CREATEDATE { get; set; }
        public string UPDATEFLAG { get; set; }
        public string DocStatus { get; set; }
        public string Remark { get; set; }
        public string DocNum { get; set; }
        public string ExpDate { get; set; }

    }

    public class DeleteDocument
    {
        public int ROWID { get; set; }
        public int INQID { get; set; }
        public string SAPINQID { get; set; }
        public string UPDATEFLAG { get; set; }
    }
}