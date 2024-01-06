namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web;
    using Sitecore.Feature.Accounts.SessionHelper;

    [Serializable]
    public class ComplaintFileRegistrationModel
    {
        public ComplaintFileRegistrationModel()
        {
            ComplaintPortalService complaintPortalService = new ComplaintPortalService();
            ComplaintCategorySelectList = new List<ListItem>();

            var listComplaintCategory = complaintPortalService.GetComplaintCategoryList();

            if (UserSession.AEMLComplaintUserSessionContext != null && UserSession.AEMLComplaintUserSessionContext.IsAuthenticated && !UserSession.AEMLComplaintUserSessionContext.IsRegistered)
            {
                listComplaintCategory = listComplaintCategory.Where(c => c.IsVisibleToRegisteredOnly == true).ToList();
            }

            ComplaintCategorySelectList = new List<ListItem>();
            if (listComplaintCategory != null && listComplaintCategory.Any())
            {
                foreach (var item in listComplaintCategory)
                {
                    ComplaintCategorySelectList.Add(new ListItem
                    {
                        Text = item.CategoryName,
                        Value = item.Id.ToString()
                    });
                }
            }

            var listSubType = complaintPortalService.GetComplaintSybTypeList();
            ComplaintSubCategoryTypeSelectList = new List<ListItem>();
            if (listSubType != null && listSubType.Any())
            {
                foreach (var item in listSubType)
                {
                    ComplaintSubCategoryTypeSelectList.Add(new ListItem
                    {
                        Text = item.SubSubTypeDescription,
                        Value = item.Id.ToString()
                    });
                }
            }

            ComplaintSubCategorySelectList = new List<ListItem>();

            #region Rating
            Rating = new List<RatingDetails>();
            Rating.Add(new RatingDetails
            {
                RatingClass = "Excellent",
                RatingName = "Excellent",
                RatingValue = 5
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Good",
                RatingName = "Good",
                RatingValue = 4
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Average",
                RatingName = "Average",
                RatingValue = 3
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Poor",
                RatingName = "Poor",
                RatingValue = 2
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Very-Poor",
                RatingName = "Very Poor",
                RatingValue = 1
            });

            #endregion

            #region Concern Addressed Rating
            ConcernRating = new List<RatingDetails>();
            ConcernRating.Add(new RatingDetails
            {
                RatingClass = "Excellent",
                RatingName = "Excellent",
                RatingValue = 5
            });
            ConcernRating.Add(new RatingDetails
            {
                RatingClass = "Good",
                RatingName = "Good",
                RatingValue = 4
            });
            ConcernRating.Add(new RatingDetails
            {
                RatingClass = "Average",
                RatingName = "Average",
                RatingValue = 3
            });
            ConcernRating.Add(new RatingDetails
            {
                RatingClass = "Poor",
                RatingName = "Poor",
                RatingValue = 2
            });
            ConcernRating.Add(new RatingDetails
            {
                RatingClass = "Very-Poor",
                RatingName = "Very Poor",
                RatingValue = 1
            });

            #endregion
        }

        public string TransactionReceipt { get; set; }
        public string BankAccountStatement { get; set; }

        public List<ListItem> ComplaintSubCategoryTypeSelectList { get; set; }
        public string SelectedComplaintSubCategoryType { get; set; }

        public List<ListItem> ComplaintCategorySelectList { get; set; }
        public string SelectedComplaintCategory { get; set; }

        public List<ListItem> ComplaintSubCategorySelectList { get; set; }
        public string SelectedComplaintSubCategory { get; set; }

        public List<ListItem> ConsumerCategorySelectList { get; set; }
        public string SelectedConsumerCategory { get; set; }

        public string LoginName { get; set; }
        //public string ComplaintNumber { get; set; }
        public string ComplaintId { get; set; }
        public string ComplaintFromPreviousLevel { get; set; }
        public bool IsComplaintFromPreviousLevelValid { get; set; }
        public string RemarksFromPreviousLevel { get; set; }
        public string ComplaintDescriptionFromPreviousLevel { get; set; }
        //public DateTime AppliedDate { get; set; }
        public string ComplaintStatus { get; set; }
        public bool isReadOnly { get; set; }
        public bool IsDocumentUploaded { get; set; }

        public string ComplaintStatusDescription { get; set; }
        public string ComplaintRegistrationNumber { get; set; }

        public string AccountNumber { get; set; }
        public string ConsumerName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string ZoneName { get; set; }
        public string DivisionName { get; set; }
        public string ConsumerCategory { get; set; }

        public string ComplaintCategory { get; set; }
        public string ComplaintSubCategory { get; set; }

        public bool IsRegistered { get; set; }
        public bool IsReadyToShareFeedback { get; set; }
        public string OverallExperience { get; set; }
        public string ConcernAddressed { get; set; }
        public string FeedbackRemarks { get; set; }
        public string Captcha { get; set; }
        public List<RatingDetails> Rating { get; set; }
        public List<RatingDetails> ConcernRating { get; set; }


        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ComplaintFileRegistrationModel))]
        [StringLength(120)]
        public string ComplaintDescription { get; set; }
        //public HttpPostedFileBase ComplaintFile { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter {0}");
        public static string InvalidName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Input", "Invalid input");
    }

    [Serializable]
    public class RatingDetails
    {
        public int RatingValue { get; set; }
        public string RatingName { get; set; }
        public string RatingClass { get; set; }
    }
}