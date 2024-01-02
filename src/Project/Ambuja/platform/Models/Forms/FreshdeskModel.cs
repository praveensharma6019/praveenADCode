using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.Forms
{
    public class FreshdeskModel
    {
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public CustomFields CustomFields { get; set; }
        public List<string> Tags { get; set; }
    }

    public class CustomFields
    {
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string TypeOfInquiry { get; set; }
        public string State { get; set; }
        public string District { get; set; }
    }
}