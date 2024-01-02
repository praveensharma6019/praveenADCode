namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class ModalShare
    {
        public string titleHeading { get; set; }
        public string src { get; set; }
        public string imgAlt { get; set; }
        public string label { get; set; }
        public string location { get; set; }
        public string copylink { get; set; }
        public string email { get; set; }
        public string twitter { get; set; }
        public string facebook { get; set; }
        public string whatsapp { get; set; }
        public string PageTitle { get; set; }
    }

    public class ProjectData
    {
        public string downloadBrochure { get; set; }
        public string downloadModalTitle { get; set; }
        public string enquireNow { get; set; }
        public string share { get; set; }
        public ModalShare modalShare { get; set; }
        public string downloadPDFLink { get; set; }
    }

    public class ProjectActions
    {
        public ProjectData projectActions { get; set; }
    }
}