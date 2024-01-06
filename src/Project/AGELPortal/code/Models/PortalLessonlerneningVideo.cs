using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Sitecore.AGELPortal.Website.Models
{
    public class PortalLessonlerneningVideo
    {
        public Guid Id
        {
            get;
            set;
        }
        public string video_name
        {
            get;
            set;
        }

        public string video_url
        {
            get;
            set;
        }
        public string user_id
        {
            get;
            set;
        }
        public string user_name
        {
            get;
            set;

        }
        public string part_user_name
        {
            get;
            set;

        }
        public string video_id
        {

            get;
            set;
        }

        public List<PortalLessonlerneningVideo> videos { get; set; }
        public List<AGELPortalLessonLearningVideo> lesson_learningVideo { get; set; }
        public string created_date { get; set; }
        public string DocVideoImage_Name { get; set; }

        public string User_Site { get; set; }

        public string Content_Type { get; set; }
        
        public string Category_Name { get; set; }
        public int ContentCount { get; set; }

        public Guid? userId { get; set; }
        public int VideoCount { get; set; }
        public int DocCount { get; set; }

        public int docvideocnt { get; set; }

    }
}
