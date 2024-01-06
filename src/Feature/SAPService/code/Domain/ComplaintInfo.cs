using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class ComplaintInfo
    {
        //public string SelectedComplaintSubCategoryType { get; set; }
        //public string SelectedComplaintCategory { get; set; }
        //public string SelectedComplaintSubCategory { get; set; }
        //public string SelectedConsumerCategory { get; set; }

        //public string LoginName { get; set; }
        //public string ComplaintId { get; set; }
        //public string ComplaintStatus { get; set; }
       
        //public string ComplaintStatusDescription { get; set; }
        //public string ComplaintRegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string ConsumerName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        //public string Pincode { get; set; }
        //public string ZoneName { get; set; }
        //public string DivisionName { get; set; }
        //public string ConsumerCategory { get; set; }

        public string ComplaintCategory { get; set; }
        public string ComplaintSubCategory { get; set; }
        public string ComplaintSubType { get; set; }
        public string ComplaintSubSubType { get; set; }

        public string streetArea { get; set; }
        public string streetPoleNo { get; set; }
        public string streetPillarNo { get; set; }
    }
}