using System;
using System.Linq;
using System.Web;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Data;
using System.ComponentModel;

namespace Sitecore.AGELPortal.Website.Models
{
    [Serializable]
    public class PortalAddContentModel
    {
        public Guid Id
        {
            get;
            set;
        }
        public string AdobeDCClientId
        {
            get; set;
        }
        [Required(ErrorMessage = "Category is Required")]
        public string category_id
        {
            get;
            set;
        }

        [Required]
        public String title
        {
            get;
            set;
        }
        [Required]
        public string size
        {
            get;
            set;
        }

        [Required]
        public string content_type
        {
            get;
            set;
        }

        [Required]
        public string discription
        {
            get;
            set;
        }
        [Required]
        public string status
        {
            get;
            set;
        }
        [DisplayName("Upload File")]
        public string imagepath
        {
            get;
            set;
        }
        

        public string UserValidation { get; set; }
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]

        public HttpPostedFileBase imagefile { get; set; }
        public string endpoint_url { get; set; }
        public string DocVideoImage_Name { get; set; }
        public string docvideocnt { get; set; }

        public List<AGELPortalContent> contents { get; set; }
        public List<AGELPortalContent> videos { get; set; }
        public List<AGELPortalContent> documents { get; set; }
       
        public List<AGElPortalCategory> contentCategories { get; set; }
        public List<AGElPortalCategory> videoCategories { get; set; }
        public List<AGElPortalCategory> documentCategories { get; set; }


        public DateTime created_date;
        public byte[] document_data { get; set; }
        public string Doc_id { get; set; }

        public int totalRecord { get; set; }

        public AGELPortalContent content { get; set; }
        public List<SelectListItem> categories { get; set; }
        public int GetCount(Guid Id,string contetn_type) {

            AGELPortalDataContext rdb = new AGELPortalDataContext();
            var count = rdb.AGELPortalContents.Where(x => x.category_id == Id && x.contetn_type== contetn_type && x.status==true).Count();
            return count;
        }

        public dynamic GetCategoryName(Guid Id)
        {
            AGELPortalDataContext rdb = new AGELPortalDataContext();
            var catName = rdb.AGElPortalCategories.Where(x => x.Id == Id).FirstOrDefault();
            if(catName!=null)
            return catName.name;
            return null;
        }
        public string ErrorMessage { get; set; }

        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.doc|.mp4|.pdf)$", ErrorMessage = "Only doc files allowed.")]


        public HttpPostedFileBase agel_file { get; set; }


        public string SubscriptionId
        {
            get { return "c2e7914d-a100-468f-b49f-ffbeb4fa32ea"; }
        }

        public string ResourceGroup
        {
            get { return "adanistaging"; }
        }

        public string AccountName
        {
            get { return "mediaservicestage"; }
        }

        public string AadTenantId
        {
            get { return "64d1fc0b-a230-4785-85f8-586ec3b11e3f"; }
        }

        public string AadClientId
        {
            get { return "0e5a248e-f4b2-40f8-8914-006ff976b810"; }
        }

        public string AadSecret
        {
            get { return "LHd7Q~XEnrJ0NSWQ18~BWd3gDixfD5on3UNgA"; }
        }

        public Uri ArmAadAudience
        {
            get { return new Uri("https://management.core.windows.net/"); }
        }

        public Uri AadEndpoint
        {
            get { return new Uri("https://login.microsoftonline.com"); }
        }

        public Uri ArmEndpoint
        {
            get { return new Uri("https://management.azure.com/"); }
        }

        public string EventHubConnectionString
        {
            get { return ""; }
        }

        public string EventHubName
        {
            get { return ""; }
        }

        public string StorageContainerName
        {
            get { return ""; }
        }

        public string StorageAccountName
        {
            get { return ""; }
        }

        public string StorageAccountKey
        {
            get { return ""; }
        }

        public string SymmetricKey
        {
            get { return ""; }
        }

        public string AskHex
        {
            get { return ""; }
        }

        public string FairPlayPfxPath
        {
            get { return ""; }
        }

        public string FairPlayPfxPassword
        {
            get { return ""; }
        }
        public IList<string> AzureURLs { get; set; }

        public List<AGELPortalContent> title1 { get; set; }
        public string Url { get; internal set; }
    }
    public class OTPRequest
    {
        public string mobileNo { get; set; }
        public string countryCode
        {
            get { return "91"; }
        }
        public int type
        {
            get { return 1; }
        }

        public string compaignID
        {
            get { return "~6nqej2"; }
        }

        public List<MessageList> data { get; set; }

    }
    public class MessageList
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    public class OTPDeserialize
    {
        public OTPData data { get; set; }
        public string status { get; set; }
        public string warning { get; set; }
        public string error { get; set; }
    }
    public class OTPData
    {
        public string mobileNo { get; set; }
        public string countryCode { get; set; }
        public string otp { get; set; }
        public bool isSendOtp { get; set; }
    }

    //for sms content
    public class PortalSMS
    {
        public string mobileNo { get; set; }
        public string countryCode
        {
            get { return "91"; }
        }
        public int type
        {
            get { return 1; }
        }

        public string compaignID
        {
            get { return "~6nqej2"; }
        }
        public List<MessageList> data { get; set; }

    }


}