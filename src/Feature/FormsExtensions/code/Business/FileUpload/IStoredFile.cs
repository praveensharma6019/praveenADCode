namespace Feature.FormsExtensions.Business.FileUpload
{
    public interface IStoredFile
    {
        string Url { get; set; }
        string OriginalFileName { get; set; }
        string ContentType { get; set; }
        int ContentLength { get; set; }
    }
}