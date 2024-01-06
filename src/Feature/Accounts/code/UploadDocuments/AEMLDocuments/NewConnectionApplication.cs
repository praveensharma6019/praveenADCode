using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web;
using Newtonsoft.Json;

namespace Sitecore.Feature.Accounts.Models
{
    public partial class NewConnectionApplication
    {
        public List<NEW_CON_APPLICATION_DOCUMENT_DETAIL> GetExistingDocuments { get; set; }
        public List<DocumentCheckList> IDDocumentsList { get; set; }
        public List<DocumentCheckList> IDDocumentsListOnly1 { get; set; }
        public string SelectedIDDocumentOnly1 { get; set; }

        public List<DocumentCheckList> ID2DocumentsList { get; set; }
        public List<DocumentCheckList> ID2DocumentsListOnly1 { get; set; }
        public string SelectedID2DocumentOnly1 { get; set; }


        public List<DocumentCheckList> OTDocumentsList { get; set; }
        public List<DocumentCheckList> OTDocumentsListOnly1 { get; set; }
        public string SelectedOTDocumentOnly1 { get; set; }

        public List<DocumentCheckList> ODDocumentsList { get; set; }
        public List<DocumentCheckList> ODDocumentsListOnly1 { get; set; }
        public string SelectedODDocumentOnly1 { get; set; }

        public List<DocumentCheckList> PHDocumentsList { get; set; }
        public List<DocumentCheckList> PHDocumentsListOnly1 { get; set; }
        public string SelectedPHDocumentOnly1 { get; set; }

        public List<DocumentCheckList> SDDocumentsList { get; set; }
        public List<DocumentCheckList> SDDocumentsListOnly1 { get; set; }
        public string SelectedSDDocumentOnly1 { get; set; }

        public string docnumber_ID { get; set; }
        public string docnumber_ID2 { get; set; }

        public string ApplicantType { get; set; }

        public List<NEWCON_DOCUMENTMASTER> DocumentList { get; set; }
        public int SNO { get; set; }

        public string IDDOC_1 { get; set; }
        public string IDDOC_2 { get; set; }
        public string HDNIDDOC_1 { get; set; }
        public string HDNIDDOC_2 { get; set; }

        public string DOCUMENTID_1 { get; set; }
        public string DOCUMENTID_2 { get; set; }

        public string IDJOINTDOC_1 { get; set; }//Joint
        public string IDJOINTDOC_2 { get; set; }//Joint
        public string HDNIDJOINTDOC_1 { get; set; }//Joint
        public string HDNIDJOINTDOC_2 { get; set; }//Joint
        public string JOINTDOCUMENTID_1 { get; set; }//Joint
        public string JOINTDOCUMENTID_2 { get; set; }//Joint

        public string OWNERDOC_1 { get; set; }
        public string OWNERDOC_2 { get; set; }
        public string HDNOWNERDOC_1 { get; set; }
        public string HDNOWNERDOC_2 { get; set; }

        public string OWNERDOC_3 { get; set; }
        public string OWNERDOC_4 { get; set; }
        public string OWNERDOC_5 { get; set; }
        public string HDNOWNERDOC_3 { get; set; }
        public string HDNOWNERDOC_4 { get; set; }
        public string HDNOWNERDOC_5 { get; set; }

        public string HDNPHOTOFILE_1 { get; set; }
        public string HDNPHOTOFILE_2 { get; set; }
        public string HDNPHOTOFILEID_1 { get; set; }
        public string HDNPHOTOFILEID_2 { get; set; }

        public string HDNSEZFILE_1 { get; set; }
        public string HDNSEZFILEID_1 { get; set; }


        public string ApplicationNo { get; set; }
        public string AccountNo { get; set; }
    }
}