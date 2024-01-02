using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class AdmissionUpdatesModel : BaseHeadingModel<DescriptionModel>
    {
        public string Theme { get; set; }
    }
}