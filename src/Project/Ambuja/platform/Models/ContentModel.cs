namespace Project.AmbujaCement.Website.Models
{
    public class ContentModel : BaseContentModel
    {
        public string Theme { get; set; }
        public bool TextFirst { get; set; }
        public string CardType { get; set; }
        public string MediaType { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
        public string LinkTarget { get; set; }
    }

}