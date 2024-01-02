using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class BaseContentModel
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
    }

    public class BaseContentModel<T>:BaseContentModel where T : class
    {
        public BaseContentModel()
        {
            Data = new List<T>();
        }

        public List<T> Data { get; set; }
    }
}