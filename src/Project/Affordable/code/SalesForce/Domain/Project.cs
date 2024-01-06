namespace Sitecore.Affordable.Website.SalesForce.Domain
{
    public class Project : SObject
    {
        public bool Active__c { get; set; }
        public string Address__c { get; set; }
        public string City__c { get; set; }
        public string Email__c { get; set; }
        public string Notes__c { get; set; }
    }
}