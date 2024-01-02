namespace Project.AmbujaCement.Website.Models.Forms
{
    public class GetInTouchFormModel
    {
        public AmbujaFormsModel getInTouchForm { get; set; }
        public GetInTouchOtpModel getIntouchOtp { get; set; }
        public string successMessage { get; set; }
        public string progressMessage { get; set; }
        public string errorMessage { get; set; }
    }
}