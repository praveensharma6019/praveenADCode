namespace SapPiService.Domain
{
    public class SubscriberBase : AccountBase
    {
        public ConsumerType ConsumerType { get; set; }
        public string CycleNumber { get; set; }

    }
}