using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class BaseHeadingModel
    {
        public string Heading { get; set; }
    }

    public class BaseHeadingModel<T>: BaseHeadingModel where T : class
    {
        public BaseHeadingModel()
        {
            Data = new List<T>();
        }

        public List<T> Data { get; set; }
    }
}