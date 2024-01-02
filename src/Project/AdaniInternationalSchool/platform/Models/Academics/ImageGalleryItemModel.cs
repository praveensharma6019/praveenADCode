namespace Project.AdaniInternationalSchool.Website.Models.Academics
{
    public class ImageGalleryItemModel : ImageModel
    {
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public string ThumbImageSource { get; set; }
        public string ThumbImageSourceMobile { get; set; }
        public string ThumbImageSourceTablet { get; set; }
        public string ThumbImageAlt { get; set; }

    }
}
