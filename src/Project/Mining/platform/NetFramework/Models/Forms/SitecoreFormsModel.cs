using System.ComponentModel.DataAnnotations;

namespace Project.Mining.Website.Models.Forms
{
    public class SitecoreFormsModel
    {
        
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please select Solution type")]
        public string SolutionType { get; set; }

        [Required(ErrorMessage = "Please enter Message")]
        [RegularExpression(@"^[\w\s.,@:!?-]+$", ErrorMessage = "Please enter valid message, Special Characters are not allowed!")]
        public string Message { get; set; }

    //    public string Response { get; set; }

    }

    public class SolutionType
    {
        public string SolutionTypeName { get; set; }
        public int SolutionTypeValue { get; set; }
    }
}