using Project.AAHL.Website.Models.ContactUs;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.OurStory;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IContactUsServices
    {
        Address GetAddress(Rendering rendering);
        WriteToUsDetails GetWriteToUsDetails(Rendering rendering);
        WriteToUsForm GetWriteToUsForm(Rendering rendering);
    }
}
