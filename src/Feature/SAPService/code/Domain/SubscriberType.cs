namespace SapPiService.Domain
{
    public class SubscriberType : AccountBase
    {
        public ConsumerType ConsumerType { get; set; }
        public string CycleNumber { get; set; }

    }
}