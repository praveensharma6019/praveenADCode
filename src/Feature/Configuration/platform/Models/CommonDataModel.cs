using Newtonsoft.Json;
using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Models
{
    public class CommonDataModel
    {
        [JsonProperty(PropertyName = "whyAdani")]
        public ObjectItems WhyAdani { get; set; }
        [JsonProperty(PropertyName = "ourStories")]
        public ObjectItems OurStories { get; set; }
        [JsonProperty(PropertyName = "clubEvents")]
        public ObjectItems ClubEvents { get; set; }
        [JsonProperty(PropertyName = "ourStory")]
        public ObjectItems OurStory { get; set; }
        [JsonProperty(PropertyName = "headerInner")]
        public ObjectItems HeaderInner { get; set; }
        [JsonProperty(PropertyName = "ourValues")]
        public ObjectItems OurValues { get; set; }
        [JsonProperty(PropertyName = "configuration")]
        public List<Configuration> Configuration { get; set; }
        [JsonProperty(PropertyName = "modalText")]
        public ModalText ModalText { get; set; }
        [JsonProperty(PropertyName = "ProejctPropertyData")]
        public ProjectPropertyDataOptions ProjectProertyData { get; set; }
        [JsonProperty(PropertyName = "enquireForm")]
        public EnquireForm EnquireForm { get; set; }
    }
    public class ObjectItems
    {
        [JsonProperty(PropertyName = "items")]
        public List<object> Items { get; set; }
    }
    public class Configuration
    {
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<object> Items { get; set; }
    }
    public class ConfiturationItem
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "keys")]
        public List<Key> Keys { get; set; }
    }
    public class Key
    {
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "keyword")]
        public string Keyword { get; set; }
    }
    public class ImageTitleText
    {
        [JsonProperty(PropertyName = "src")]
        public string Src { get; set; }
        [JsonProperty(PropertyName = "alt")]
        public string Alt { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "desc")]
        public string Decription { get; set; }
        [JsonProperty(PropertyName = "btnUrl")]
        public string ButtonLink { get; set; }
        [JsonProperty(PropertyName = "btnText")]
        public string ButtonText { get; set; }
        [JsonProperty(PropertyName = "imgType")]
        public string imgType { get; set; }
    }

    public class CommonText
    {
        public string seeall { get; set; }
        public string projectResidential { get; set; }
        public string commercialProjects { get; set; }
        public string all { get; set; }
        public string readytomove { get; set; }
        public string underconstruction { get; set; }
        public string commInOtherCity { get; set; }
        public string commOffer { get; set; }
        public string ressInOtherCity { get; set; }
        public string ressOffer { get; set; }
        public string filters { get; set; }
        public string projectfound { get; set; }
        public string faq { get; set; }
        public string planavisit { get; set; }
        public string searchproject { get; set; }
        public string copylink { get; set; }
        public string email { get; set; }
        public string twitter { get; set; }
        public string facebook { get; set; }
        public string whatsapp { get; set; }
        public string search { get; set; }
        public string submit { get; set; }
        public string readless { get; set; }
        public string readmore { get; set; }
        public string livinggoodlife { get; set; }
        public string print { get; set; }
        public string saveaspdf { get; set; }
        public string done { get; set; }
        public string downloadBrochure { get; set; }
        public string share { get; set; }
        public string socialClubs { get; set; }
    }
    public class ModalText
    {
        [JsonProperty(PropertyName = "shareContact")]
        public string ShareContact { get; set; }
        [JsonProperty(PropertyName = "agreeTxt")]
        public string AgreeText { get; set; }
        [JsonProperty(PropertyName = "homeLoanCheck")]
        public string HomeLoanCheck { get; set; }
    }

    public class ProjectPropertyDataOptions
    {
        [JsonProperty(PropertyName = "options")]
        public List<object> Options { get; set; }
    }
    public class ProjectPropertyOptions
    {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }
    public class EnquireForm
    {
        [JsonProperty(PropertyName = "enquireNow")]
        public string EnquireNow { get; set; }
        [JsonProperty(PropertyName = "shareContact")]
        public string ShareContact { get; set; }
        [JsonProperty(PropertyName = "sendusQuery")]
        public string SendusQuery { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "projectType")]
        public List<Dropdown> ProjectTypeOptions { get; set; }
        [JsonProperty(PropertyName = "PropertyType")]
        public List<Dropdown> PropertyTypeOptions { get; set; }
        [JsonProperty(PropertyName = "agreeToConnect")]
        public string AgreeToConnect { get; set; }
        [JsonProperty(PropertyName = "overrideRegistry")]
        public string OverrideRegistry { get; set; }
        [JsonProperty(PropertyName = "submitDetail")]
        public string SubmitDetail { get; set; }
        [JsonProperty(PropertyName = "submit")]
        public string submit { get; set; }
        [JsonProperty(PropertyName = "projectPropertyOptions")]
        public List<Dropdown> ProjectPropertyOptions { get; set; }
        [JsonProperty(PropertyName = "startDate")]
        public string StartDate { get; set; }
        [JsonProperty(PropertyName = "homeLoanInterested")]
        public string HomeLoanInterested { get; set; }
        [JsonProperty(PropertyName = "TimeSlotOptions")]
        public List<Dropdown> TimeSlotsOptions { get; set; }
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }
        [JsonProperty(PropertyName = "To")]
        public string To { get; set; }
    }
    public class Dropdown
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "controllerName")]
        public string controllerName { get; set; }
    }
}