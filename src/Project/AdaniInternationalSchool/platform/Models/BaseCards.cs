using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class BaseCards<T>
    {
        public BaseCards()
        {
            Data = new List<T>();
        }

        public string Variant { get; set; }
        public List<T> Data { get; set; }
    }
}