namespace SapPiService.Domain
{
    public class SubscriberDetail : AccountBase
    {
        public string Name { get; set; }
        public string BookNumber { get; set; }
        public string CycleNumber { get; set; }
        public string ZoneNumber { get; set; }
        public string Address { get; set; }
    }
}
