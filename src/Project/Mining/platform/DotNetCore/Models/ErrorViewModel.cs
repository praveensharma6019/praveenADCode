namespace Project.MiningRenderingHost.Website.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public bool IsInvalidRequest { get; set; }
    }
}
