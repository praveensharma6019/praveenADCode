using System.ComponentModel.DataAnnotations;

namespace Project.AmbujaCement.Website.Models.Forms
{
    public class VerifyOtpModel
    {
        [Required(ErrorMessage = "Please enter Mobile Number")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Please enter OTP")]
        public string Otp { get; set; }
    }
}