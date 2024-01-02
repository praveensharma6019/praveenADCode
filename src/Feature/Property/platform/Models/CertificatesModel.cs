using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{

    public class CertificatesModel
    {
        public string certificateID { get; set; }
        public List<PropertyType> data { get; set; }
    }
    public class PropertyType
    {
        public string heading { get; set; }
        public List<Details> data { get; set; }
    }
    public class Details
    {
        public string src { get; set; }
        public string alt { get; set; }
        public string propertyName { get; set; }
        public string link { get; set; }
        public string target { get; set; }
        public string reraHeading { get; set; }
        public string downloadRera { get; set; }
        public List<Rera> rera { get; set; }
        public string envHeading { get; set; }
        public string downloadEnv { get; set; }
        public List<envModal> envModal { get; set; }
    }
    public class Rera
    {
        public string reraTitle { get; set; }
        public string downloadurl { get; set; }
        public string title { get; set; }
        public string titleLink { get; set; }
        public string titleLinkTarget { get; set; }
        public string reraNumber { get; set; }
        public string download { get; set; }
        public string qrCodeImage { get; set; }
    }
    public class envModal
    {
        public string url { get; set; }
        public string envMonth { get; set; }
        public string download { get; set; }
        public string downloadurl { get; set; }
        public string envMonthTarget { get; set; }
    }
}