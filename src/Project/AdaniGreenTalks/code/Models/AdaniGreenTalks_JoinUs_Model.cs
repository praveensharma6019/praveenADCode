// Decompiled with JetBrains decompiler
// Type: Sitecore.AdaniGreenTalks.Website.Models.AdaniGreenTalks_JoinUs_Model
// Assembly: Sitecore.AdaniGreenTalks.Website, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E01E7F81-DBA4-49B5-BF38-8DFD57A2798A
// Assembly location: D:\Deployments\Stage\Sitecore.AdaniGreenTalks.Website.dll



using System;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace Sitecore.AdaniGreenTalks.Website.Models
{
    public class AdaniGreenTalks_JoinUs_Model
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter participant name"), MaxLength(30)]
        public string ParticipantName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter business name"), MaxLength(30)]
        public string BusinessName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter valid website")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string BusinessWebsite { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter business type"), MaxLength(30)]
        public string BusinessType { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter contact Number"), MaxLength(10), MinLength(10)]
        public string ContactNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter email ID")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email_Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter address"), MaxLength(250)]
        public string Address { get; set; }



        public string Doc_ProjectName { get; set; }

        public byte[] Doc_ProjectFileData { get; set; }

        public string Doc_OwnershipName { get; set; }

        public byte[] Doc_OwnershipFileData { get; set; }

        public string Doc_ProjectionSaleName { get; set; }

        public byte[] Doc_ProjectionSaleFileData { get; set; }

        public string Doc_ValuationReportName { get; set; }

        public byte[] Doc_ValuationFileData { get; set; }

        public string Doc_ProfitLossName { get; set; }

        public byte[] Doc_ProfitLossFileData { get; set; }

        [Required]
        [FileName(".pdf", ErrorMessage = "Upload file with correct format, size and name")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Max 5 mb size file upload is allowed")]
        [FileHeaderSignature("JVBER", ErrorMessage = "only pdf files are allowed")]
        public HttpPostedFileBase Doc_UploadProject { get; set; }

        [FileName(".pdf", ErrorMessage = "Upload file with correct format, size and name")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Max 5 mb size file upload is allowed")]
        [FileHeaderSignature("JVBER", ErrorMessage = "only pdf files are allowed")]
        public HttpPostedFileBase Doc_Ownership { get; set; }


        [FileName(".pdf", ErrorMessage = "Upload file with correct format, size and name")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Max 5 mb size file upload is allowed")]
        [FileHeaderSignature("JVBER", ErrorMessage = "only pdf files are allowed")]
        public HttpPostedFileBase Doc_ProjectionSales { get; set; }

        [FileName(".pdf", ErrorMessage = "Upload file with correct format, size and name")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Max 5 mb size file upload is allowed")]
        [FileHeaderSignature("JVBER", ErrorMessage = "only pdf files are allowed")]
        public HttpPostedFileBase Doc_ValuationReport { get; set; }

        [FileName(".pdf", ErrorMessage = "Upload file with correct format, size and name")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Max 5 mb size file upload is allowed")]
        [FileHeaderSignature("JVBER", ErrorMessage = "only pdf files are allowed")]
        public HttpPostedFileBase Doc_ProfitLoss { get; set; }


        [Required]
        [IsValidCategory(ErrorMessage = "Please select valid category")]
        public string Category { get; set; }

        public DateTime SubmittedDate { get; set; }

        public string FormType { get; set; }

        public string FormUrl { get; set; }

        public string Participate_AdaniPrizes { get; set; }

        public string chkDocuments_approved { get; set; }


        public string reasonCause { get; set; }

        public string googleCaptchaToken { get; set; }
    }
}
