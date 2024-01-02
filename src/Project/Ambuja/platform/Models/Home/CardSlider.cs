namespace Project.AmbujaCement.Website.Models
{
    public class CardSlider : BaseImageContentModel
    {
       
        public string Link { get; set; }
        public string LinkTarget { get; set; }
        public GtmDataModel GtmData { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }

        
    }
}