using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class InvestorDownload
    {       
        
        public Header HeaderData { get; set; }
        public Cookie CookieData { get; set; }       
        public GenericBredcrumbNavigation GenericBredcrumbNavigation { get; set; }
        public MediaLibrarySection MediaLibrarySection { get; set; }
        public Footer FooterData { get; set; }

    }
     
    public class MediaLibrarySection
    {
        public string Heading { get; set; }
        public List<MediaFolders> MediaFolders { get; set; }
    }
    public class MediaFolders
    {
        public string Heading { get; set; }
        public List<MediaFolderItems> MediaFolderItems { get; set; }
    }
    public class MediaFolderItems
    {
        public string Heading { get; set; }
        public string Link { get; set; }
    }
}