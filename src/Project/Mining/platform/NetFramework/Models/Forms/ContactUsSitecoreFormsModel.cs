using System.ComponentModel.DataAnnotations;

namespace Project.Mining.Website.Models.Forms
{
    public class ContactUsSitecoreFormsModel
    {

        [Required(ErrorMessage = "Please enter Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please select What can we help you with?")]
        public string HelpTypeDropdown { get; set; }

        [Required(ErrorMessage = "Please enter Message")]
        [RegularExpression(@"^[\w\s.,@:!?-]+$", ErrorMessage = "Please enter valid message, Special Characters are not allowed!")]
        public string Message { get; set; }

   //     public string Response { get; set; }

    }

    public class HelpTypeDropdown
    {
        public string HelpTypeDropdownName { get; set; }
        public int HelpTypeDropdownValue { get; set; }
    }
}