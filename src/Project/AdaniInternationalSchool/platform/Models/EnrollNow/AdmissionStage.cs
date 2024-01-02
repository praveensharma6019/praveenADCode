using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models.EnrollNow
{
    public class AdmissionStage
    {
        public string SubHeading { get; set; }
        public List<AdmissionDocument> Description { get; set; }
    }

    public class AdmissionDocument
    {
        public string Label { get; set; }
        public List<AdmissionSubDocument> SubDescription { get; set; }
    }

    public class AdmissionSubDocument
    {
        public string Label { get; set; }
    }
}