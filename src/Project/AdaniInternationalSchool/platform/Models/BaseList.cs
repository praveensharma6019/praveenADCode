using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class BaseList<T>
    {
        public BaseList()
        {
            List = new List<T>();
        }

        public List<T> List { get; set; }
    }
}