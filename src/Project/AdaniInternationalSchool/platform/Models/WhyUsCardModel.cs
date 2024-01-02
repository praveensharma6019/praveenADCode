namespace Project.AdaniInternationalSchool.Website.Models
{
    public class WhyUsCardModel : BaseCards<WhyUsCardDataModel>
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }

    public class WhyUsCardDataModel : ImageModel
    {
        public string Description { get; set; }
        public string Theme { get; set; }
    }
}