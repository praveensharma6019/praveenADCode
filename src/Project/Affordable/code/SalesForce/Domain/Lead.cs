namespace Sitecore.Affordable.Website.SalesForce.Domain
{
    public class Lead : SObject
    {
        public string RecordTypeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string LeadSource { get; set; }
        public string Lead_Classification_New__c { get; set; }
        public string Global_Emp__c { get; set; }
        public string Remarks__c { get; set; }
        public string PG_OrderId__c { get; set; }
        public string PG_Product__c { get; set; }
        public decimal PG_Amount__c { get; set; }
        public string PG_Payment_Mode__c { get; set; }
    }
}
