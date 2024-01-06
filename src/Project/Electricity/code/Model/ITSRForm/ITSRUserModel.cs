namespace Sitecore.Electricity.Website.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Sitecore.Electricity.Website.Services;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ITSRUserModel
    {
        public ITSRUserModel()
        {
            ITSRBidderFormService objService = new ITSRBidderFormService();
            var userRolesList = objService.GetUserRoles();
            UserRoles = new List<RoleDetails>();
            if (userRolesList != null && userRolesList.Count > 0)
            {
                foreach (var item in userRolesList)
                {
                    UserRoles.Add(new RoleDetails
                    {
                        RoleName = item.RoleName,
                        RoleValue = item.RoleValue
                    });
                }
            }
        }

        public List<RoleDetails> UserRoles { get; set; }

        public static string Invalidinput => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Org Name", "Invalid input");
        public static string InvalidName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Name contains alphabets only");
     
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        public static string InvalidLoginName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Please enter a Valid Login Name");

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ITSRUserModel))]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ITSRUserModel))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ITSRUserModel))]
        public string Mobile { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidLoginName), ErrorMessageResourceType = typeof(ITSRUserModel))]
        public string UserName { get; set; }

        public string Role { get; set; }
        public bool IsActive { get; set; }
    }

    [Serializable]
    public class RoleDetails
    {
        public string RoleValue { get; set; }
        public string RoleName { get; set; }
    }
}