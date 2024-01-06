namespace SapPiService.Domain
{
    public class VdsDetail
    {
        public int AverageBillingAmount { get; set; }
        public int CurrentOutstanding { get; set; }
        public int ExistingVdsBalance { get; set; }
    }

    public class SDDetails
    {
        public string Message { get; set; }
        public string SDAmount { get; set; }
        public bool IsSuccess { get; set; }
    }
}