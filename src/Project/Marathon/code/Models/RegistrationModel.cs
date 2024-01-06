using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Marathon.Website.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using static Sitecore.Marathon.Website.CustomHelpers;

namespace Sitecore.Marathon.Website.Models
{
    [Serializable]
    public class RegistrationModel
    {
        public string BIBNumber { get; set; }
        public bool? IsRaceDistanceChanged { get; set; }

        public string RaceLocation { get; set; }

        [RegularExpression("^[0-9a-z A-Z.-]{3,15}$", ErrorMessage = "Please enter a valid Reference Code.")]
        public string ReferenceCode { get; set; }

        [RegularExpression("^[0-9a-z A-Z.-]{3,15}$", ErrorMessage = "Please enter a valid Affiliate Code.")]
        public string AffiliateCode { get; set; }

        [Required(ErrorMessage = "Please enter first name.")]
        [RegularExpression("^[a-z A-Z]{2,50}$", ErrorMessage = "Please enter a valid first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        [RegularExpression("^[a-z A-Z]{2,50}$", ErrorMessage = "Please enter a valid last name.")]
        public string LastName { get; set; }

        public string ErrorMsg { get; set; }

        [Required(ErrorMessage = "Please enter Date of Birth.")]
        public string DateofBirth { get; set; }

        [Required(ErrorMessage = "Please enter mail id.")]
        [RegularExpression("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,17}$", ErrorMessage = "Please enter a valid email id")]
        public string Email { get; set; }

        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[6789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(RegistrationModel))]
        public string ContactNumber { get; set; }

        [RegularExpression("^[a-z A-Z]{3,50}$", ErrorMessage = "Please enter a valid Nationality.")]
        public string Nationality { get; set; }

        [RegularExpression("^[a-z A-Z]{3,50}$", ErrorMessage = "Please enter a valid Registration Status.")]
        public string RegistrationStatus { get; set; }

        [NonSerialized]
        private List<SelectListItem> _NationalityList;
        public List<SelectListItem> NationalityList
        {
            get { return _NationalityList; }
            set { _NationalityList = value; }
        }
        
        public string OrderId { get; set; }

        public string TShirtSize { get; set; }
        [NonSerialized]
        private List<SelectListItem> _TShirtSizeList;
        public List<SelectListItem> TShirtSizeList
        {
            get { return _TShirtSizeList; }
            set { _TShirtSizeList = value; }
        }
        public DateTime FormSubmitOn { get; set; }
        public string NamePreferredonBIB { get; set; }
        public string IdentityProofType { get; set; }
        [NonSerialized]
        private List<SelectListItem> _IdentityProofTypeList;
        public List<SelectListItem> IdentityProofTypeList
        {
            get { return _IdentityProofTypeList; }
            set { _IdentityProofTypeList = value; }
        }
         public string IdentityProofNumber { get; set; }

        [NonSerialized]
        private HttpPostedFileBase _IDCardAttachment;
        [Required(ErrorMessage = "Please attach ID Card.")]
        [DoubleExtension(ErrorMessage = "Please enter a valid file with single extension")]
        [SignatureHeader(ErrorMessage = "Please upload only Supported Files .png | .jpg |.pdf")]
        [AllowFileSize(FileSize = 10 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 10 MB")]
        [AllowExtensions(Extensions = "png,jpg,pdf,jpeg", ErrorMessage = "Please upload only Supported Files .png | .jpg |.pdf")]
        public HttpPostedFileBase IDCardAttachment
        {
            get { return _IDCardAttachment; }
            set { _IDCardAttachment = value; }
        }



        public string Country { get; set; }
        [NonSerialized]
        private List<SelectListItem> _CountryList;
        public List<SelectListItem> CountryList
        {
            get { return _CountryList; }
            set { _CountryList = value; }
        }
         [NonSerialized]
        public List<SelectListItem> _StateList;
        public List<SelectListItem> StateList
        {
            get { return _StateList; }
            set { _StateList = value; }
        }
         public string RunMode { get; set; }
        [NonSerialized]
        private List<SelectListItem> _RunmodeList;
        public List<SelectListItem> RunModeList
        {
            get { return _RunmodeList; }
            set { _RunmodeList = value; }
        }
        public string RunDate { get; set; }
        [NonSerialized]
        private List<SelectListItem> _RunDateList;
        public List<SelectListItem> RunDateList
        {
            get { return _RunDateList; }
            set { _RunDateList = value; }
        }
         public string TimeSlot { get; set; }
        [NonSerialized]
        public List<CustomSelectItem> _TimeSlotList;
        public List<CustomSelectItem> TimeSlotList
        {
            get { return _TimeSlotList; }
            set { _TimeSlotList = value; }
        }
         public string Vaccinationted { get; set; }

        [NonSerialized]
        private HttpPostedFileBase _VaccinationCertificate;
        [AllowFileSize(FileSize = 10 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 10 MB")]
        [AllowExtensions(Extensions = "png,jpg,pdf", ErrorMessage = "Please upload only Supported Files .png | .jpg |.pdf")]
        [DoubleExtension(ErrorMessage = "Please enter a valid file with single extension")]
        [SignatureHeader(ErrorMessage = "Please upload only Supported Files .png | .jpg |.pdf")]
        public HttpPostedFileBase VaccinationCertificate
        {
            get { return _VaccinationCertificate; }
            set { _VaccinationCertificate = value; }
        }
        public string VaccinationCertificateName { get; set; }

        public string State { get; set; }
       
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter Address.")]
        [RegularExpression("^[a-zA-Z0-9() .,-/'\"]{3,500}$", ErrorMessage = "Please enter a valid address.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter Pin code.")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Please enter a valid pin code")]
        public string Pincode { get; set; }

        [Required(ErrorMessage = "Please enter Emergency Contact Name.")]
        [RegularExpression("^[a-z A-Z]{2,50}$", ErrorMessage = "Please enter a valid Emergency Contact Name.")]
        public string EmergencyContactName { get; set; }

        [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[6789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(RegistrationModel))]
        public string EmergencyContactRelationship { get; set; }


        public string EmergencyContactNumber { get; set; }
        public string BloodGroup { get; set; }
        [NonSerialized]
        private List<SelectListItem> _BloodGroupList;
        public List<SelectListItem> BloodGroupList
        {
            get { return _BloodGroupList; }
            set { _BloodGroupList = value; }
        }
         public string ChronicIllness { get; set; }
        [NonSerialized]
        private List<SelectListItem> _ChronicIllnessList;
        public List<SelectListItem> ChronicIllnessList
        {
            get { return _ChronicIllnessList; }
            set { _ChronicIllnessList = value; }
        }
        public string HeartAilment { get; set; }
        [NonSerialized]
        private List<SelectListItem> _Heartailment;
        public List<SelectListItem> HeartAilmentList
        {
            get { return _Heartailment; }
            set { _Heartailment = value; }
        }
        public string RespiratoryAilment { get; set; }
        public string AnyFaintingEpisodesinPast { get; set; }
        [NonSerialized]
        private List<SelectListItem> _AnyFaintingEpisodesinPastList;
        public List<SelectListItem> AnyFaintingEpisodesinPastList
        {
            get { return _AnyFaintingEpisodesinPastList; }
            set { _AnyFaintingEpisodesinPastList = value; }
        }
        public string AnyOtherAilment { get; set; }
        [NonSerialized]
        private List<SelectListItem> _AnyOtherAilmentList;
        public List<SelectListItem> AnyOtherAilmentList
        {
            get { return _AnyOtherAilmentList; }
            set { _AnyOtherAilmentList = value; }
        }

        [RegularExpression("^[a-zA-Z0-9() .,-]{2,500}$", ErrorMessage = "Please enter a valid address.")]
        public string AnyKnownAllergies { get; set; }
        public string PayrollCompany { get; set; }
        [NonSerialized]
        private List<SelectListItem> _PayrollCompanyList;
        public List<SelectListItem> PayrollCompanyList
        {
            get { return _PayrollCompanyList; }
            set { _PayrollCompanyList = value; }
        }
        [RegularExpression("^[0-9a-z A-Z]{3,50}$", ErrorMessage = "Please enter a valid employee  id")]
        public string EmployeeID { get; set; }
        [RegularExpression("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,17}$", ErrorMessage = "Please enter a valid employee email id")]
        public string EmployeeEmailId { get; set; }
        public string UnitStation { get; set; }

        [NonSerialized]
        private HttpPostedFileBase _ShantigramIdProof;
        [AllowFileSize(FileSize = 10 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 10 MB")]
        [AllowExtensions(Extensions = "png,jpg,pdf", ErrorMessage = "Please upload only Supported Files .png | .jpg |.pdf")]
        [DoubleExtension(ErrorMessage = "Please enter a valid file with single extension")]
        [SignatureHeader(ErrorMessage = "Please upload only Supported Files .png | .jpg |.pdf")]
        public HttpPostedFileBase ShantigramIdProof
        {
            get { return _ShantigramIdProof; }
            set { _ShantigramIdProof = value; }
        }

        public string Gender { get; set; }

        [NonSerialized]
        private List<SelectListItem> _GenderList;
        public List<SelectListItem> GenderList
        {
            get { return _GenderList; }
            set { _GenderList = value; }
        }

        [RegularExpression("^[0-9.a-z A-Z]{3,50}$", ErrorMessage = "Please enter a valid Race Distance.")]
        public string RaceDistance { get; set; }

        [DecimalValidator(ErrorMessage = "Please enter a valid donation amount")]
        [DonationAmountValidator(ErrorMessage = "Please enter a valid donation amount")]
        public decimal DonationAmount { get; set; }
        [RegularExpression("^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Please enter a valid PAN Number.")]
        public string PANNumber { get; set; }
        public bool TaxExemptionCertificate { get; set; }
        [RegularExpression("^[0-9.a-z A-Z]{3,500}$", ErrorMessage = "Please enter a valid Tax Exemption Cause.")]
        public string TaxExemptionCause { get; set; }


        public decimal RaceAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string DiscountRate { get; set; }
        public string PaymentStatus { get; set; }
        public decimal AmountReceived { get; set; }
        public string OTP { get; set; }
        public string Rstat { get; set; }
        public Guid Userid { get; set; }
        public string IdCardFilename { set; get; }
        public string ShantigramIdProofFilename { set; get; }
        public string Age { get; set; }
        public long Id { get; set; }

        [NonSerialized]
        private HttpPostedFileBase _RaceCertificate;
        [AllowFileSize(FileSize = 10 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 10 MB")]
        [AllowExtensions(Extensions = "png,jpg,pdf", ErrorMessage = "Please upload only Supported Files .png | .jpg |.pdf")]
        [DoubleExtension(ErrorMessage = "Please enter a valid file with single extension")]
        [SignatureHeader(ErrorMessage = "Please upload only Supported Files .png | .jpg |.pdf")]
        public HttpPostedFileBase RaceCertificate
        {
            get { return _RaceCertificate; }
            set { _RaceCertificate = value; }
        }
        public string RaceCertificateName { get; set; }
        public string DetailsOfFullHalfMarathon { get; set; }
        public string DefencePersonnel { get; set; }
        
        public string Useridstring { get; set; }
        public bool Updated { get; set; }

        public List<RaceDetails> ReceDetailsLists { get; set; }

        [RegularExpression("^[a-z A-Z]{3,50}$", ErrorMessage = "Please select a valid run type.")]
        public string RunType { get; set; }
        public string Count { get; set; }

        public RegistrationModel()
        {
            RunModeList = new List<SelectListItem>
            {
                new SelectListItem{ Value = "Physical Run", Text = "Physical Run" },
                new SelectListItem{ Value="Remote Run", Text="Remote Run" }
              };
            ReceDetailsLists = new List<RaceDetails>();
            RunDateList = new List<SelectListItem>();
            TimeSlotList = new List<CustomSelectItem>();
            NationalityList = new List<SelectListItem> {
                new SelectListItem{
                                        Value ="Indian", Text="Indian"},
                                        new SelectListItem{ Value="Others", Text="Others"},                                      
            };
            PayrollCompanyList = new List<SelectListItem>
            {
                new SelectListItem{ Value="ADANI AGRI LOGISTICS (BATHINDA) LIMITED", Text="ADANI AGRI LOGISTICS (BATHINDA) LIMITED"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS (DEWAS) LTD", Text="ADANI AGRI LOGISTICS (DEWAS) LTD"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS (HARDA) LTD", Text="ADANI AGRI LOGISTICS (HARDA) LTD"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS (KANNAUJ) LIMITED", Text="ADANI AGRI LOGISTICS (KANNAUJ) LIMITED"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS (KATIHAR) LIMITED", Text="ADANI AGRI LOGISTICS (KATIHAR) LIMITED"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS (KOTKAPURA) LIMITED", Text="ADANI AGRI LOGISTICS (KOTKAPURA) LIMITED"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS (MP) LTD", Text="ADANI AGRI LOGISTICS (MP) LTD"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS (PANIPAT) LIMITED", Text="ADANI AGRI LOGISTICS (PANIPAT) LIMITED"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS (SATNA) LTD", Text="ADANI AGRI LOGISTICS (SATNA) LTD"},
                                        new SelectListItem{ Value="ADANI AGRI LOGISTICS(UJJAIN) LTD", Text="ADANI AGRI LOGISTICS(UJJAIN) LTD"},
                                        new SelectListItem{ Value="ADANI AGRIFRESH LIMITED", Text="ADANI AGRIFRESH LIMITED"},
                                        new SelectListItem{ Value="ADANI AGRILOGISTICS (HOSHANGABAD) LTD", Text="ADANI AGRILOGISTICS (HOSHANGABAD) LTD"},
                                        new SelectListItem{ Value="ADANI AGRILOGISTICS LTD", Text="ADANI AGRILOGISTICS LTD"},
                                        new SelectListItem{ Value="ADANI BRAHMA SYNERGY PRIVATE LIMITED", Text="ADANI BRAHMA SYNERGY PRIVATE LIMITED"},
                                        new SelectListItem{ Value="ADANI BUNKERING PRIVATE LIMITED", Text="ADANI BUNKERING PRIVATE LIMITED"},
                                        new SelectListItem{ Value="ADANI CAPITAL PVT LTD", Text="ADANI CAPITAL PVT LTD"},
                                        new SelectListItem{ Value="ADANI CEMENTATION LTD", Text="ADANI CEMENTATION LTD"},
                                        new SelectListItem{ Value="ADANI CMA MUNDRA TERMINAL PRIVATE LIMITED", Text="ADANI CMA MUNDRA TERMINAL PRIVATE LIMITED"},
                                        new SelectListItem{ Value="ADANI ENNORE CONTAINER TERMINAL PRIVATE LIMITED", Text="ADANI ENNORE CONTAINER TERMINAL PRIVATE LIMITED"},
                                        new SelectListItem{ Value="ADANI ENTERPRISES LIMITED", Text="ADANI ENTERPRISES LIMITED"},
                                        new SelectListItem{ Value="ADANI ENTERPRISES LIMITED (MINING DIVISION)", Text="ADANI ENTERPRISES LIMITED (MINING DIVISION)"},
                                        new SelectListItem{ Value="ADANI ESTATES PRIVATE LIMITED", Text="ADANI ESTATES PRIVATE LIMITED"},
                                        new SelectListItem{ Value="ADANI FINSERVE PVT LTD", Text="ADANI FINSERVE PVT LTD"},
                                        new SelectListItem{ Value="ADANI FOUNDATION", Text="ADANI FOUNDATION"},
                                        new SelectListItem{ Value="ADANI GAS LIMITED", Text="ADANI GAS LIMITED"},
                                        new SelectListItem{ Value="ADANI GREEN ENERGY (TAMIL NADU) LTD", Text="ADANI GREEN ENERGY (TAMIL NADU) LTD"},
                                        new SelectListItem{ Value="ADANI GREEN ENERGY (UP) LIMITED", Text="ADANI GREEN ENERGY (UP) LIMITED"},
                                        new SelectListItem{ Value="ADANI GREEN ENERGY LIMITED", Text="ADANI GREEN ENERGY LIMITED"},
                                        new SelectListItem{ Value="ADANI HAZIRA PORT PVT LTD", Text="ADANI HAZIRA PORT PVT LTD"},
                                        new SelectListItem{ Value="ADANI HOSPITALS MUNDRA PRIVATE LIMITED", Text="ADANI HOSPITALS MUNDRA PRIVATE LIMITED"},
                                        new SelectListItem{ Value="ADANI INFRA &amp;amp; DEV PVT LTD", Text="ADANI INFRA &amp;amp; DEV PVT LTD"},
                                        new SelectListItem{ Value="ADANI INFRA (INDIA) LTD", Text="ADANI INFRA (INDIA) LTD"},
                                        new SelectListItem{ Value="ADANI INFRASTRUCTURE MANAGEMENT SERVICES LIMITED", Text="ADANI INFRASTRUCTURE MANAGEMENT SERVICES LIMITED"},
                                        new SelectListItem{ Value="ADANI INSTITUTE FOR EDUCATION AND RESEARCH", Text="ADANI INSTITUTE FOR EDUCATION AND RESEARCH"},
                                        new SelectListItem{ Value="ADANI INTERNATIONAL CONTAINER TERMINAL PVT.LTD", Text="ADANI INTERNATIONAL CONTAINER TERMINAL PVT.LTD"},
                                        new SelectListItem{ Value="ADANI KANDLA BULK TERMINAL PVT LTD", Text="ADANI KANDLA BULK TERMINAL PVT LTD"},
                                        new SelectListItem{ Value="ADANI LOGISTICS LIMITED", Text="ADANI LOGISTICS LIMITED"},
                                        new SelectListItem{ Value="ADANI M2K PROJECTS LLP", Text="ADANI M2K PROJECTS LLP"},
                                        new SelectListItem{ Value="ADANI MURMUGAO PTP LTD", Text="ADANI MURMUGAO PTP LTD"},
                                        new SelectListItem{ Value="ADANI PENCH POWER LTD", Text="ADANI PENCH POWER LTD"},
                                        new SelectListItem{ Value="ADANI PETRONET (DAHEJ)", Text="ADANI PETRONET (DAHEJ)"},
                                        new SelectListItem{ Value="ADANI PORTS AND SPECIAL ECONOMIC ZONE LIMITED", Text="ADANI PORTS AND SPECIAL ECONOMIC ZONE LIMITED"},
                                        new SelectListItem{ Value="ADANI POWER (JHARKHAND) LTD", Text="ADANI POWER (JHARKHAND) LTD"},
                                        new SelectListItem{ Value="ADANI POWER (MUNDRA) LIMITED", Text="ADANI POWER (MUNDRA) LIMITED"},
                                        new SelectListItem{ Value="ADANI POWER DAHEJ LTD", Text="ADANI POWER DAHEJ LTD"},
                                        new SelectListItem{ Value="ADANI POWER LIMITED", Text="ADANI POWER LIMITED"},
                                        new SelectListItem{ Value="ADANI POWER MAH LIMITED", Text="ADANI POWER MAH LIMITED"},
                                        new SelectListItem{ Value="ADANI POWER RAJASTHAN LTD", Text="ADANI POWER RAJASTHAN LTD"},
                                        new SelectListItem{ Value="ADANI PROPERTIES PVT LTD", Text="ADANI PROPERTIES PVT LTD"},
                                        new SelectListItem{ Value="ADANI RENEWABLE ENY PARK (RAJASTHAN) LTD", Text="ADANI RENEWABLE ENY PARK (RAJASTHAN) LTD"},
                                        new SelectListItem{ Value="ADANI SHIPPING (I) PVT LTD", Text="ADANI SHIPPING (I) PVT LTD"},
                                        new SelectListItem{ Value="ADANI SKILL DEVELOPMENT CENTRE", Text="ADANI SKILL DEVELOPMENT CENTRE"},
                                        new SelectListItem{ Value="ADANI SYNENERGY LIMITED", Text="ADANI SYNENERGY LIMITED"},
                                        new SelectListItem{ Value="ADANI TOWNSHIP &amp;amp; REAL ESTATE COMPANY PVT. LTD", Text="ADANI TOWNSHIP &amp;amp; REAL ESTATE COMPANY PVT. LTD"},
                                        new SelectListItem{ Value="ADANI TRANSMISSION (INDIA) LTD", Text="ADANI TRANSMISSION (INDIA) LTD"},
                                        new SelectListItem{ Value="ADANI TRANSMISSION (RAJASTHAN) LTD", Text="ADANI TRANSMISSION (RAJASTHAN) LTD"},
                                        new SelectListItem{ Value="ADANI TRANSMISSION LIMITED", Text="ADANI TRANSMISSION LIMITED"},
                                        new SelectListItem{ Value="ADANI VIZAG COAL TERMINAL PRIVATE LIMITED", Text="ADANI VIZAG COAL TERMINAL PRIVATE LIMITED"},
                                        new SelectListItem{ Value="ADANI VIZHINJAM PORT PRIVATE LTD", Text="ADANI VIZHINJAM PORT PRIVATE LTD"},
                                        new SelectListItem{ Value="ADANI WELSPUN EXPLORATION", Text="ADANI WELSPUN EXPLORATION"},
                                        new SelectListItem{ Value="ADANI WILMAR LIMITED", Text="ADANI WILMAR LIMITED"},
                                        new SelectListItem{ Value="ADANI WIND ENERGY (GUJARAT) PRIVATE LIMITED", Text="ADANI WIND ENERGY (GUJARAT) PRIVATE LIMITED"},
                                        new SelectListItem{ Value="BELVEDERE GOLF AND COUNTRY CLUB PRIVATE LIMITED", Text="BELVEDERE GOLF AND COUNTRY CLUB PRIVATE LIMITED"},
                                        new SelectListItem{ Value="BUDHPUR BUILDCON PVT LTD", Text="BUDHPUR BUILDCON PVT LTD"},
                                        new SelectListItem{ Value="CHHATTISGARH - WR TRANSMISSION LIMITED", Text="CHHATTISGARH - WR TRANSMISSION LIMITED"},
                                        new SelectListItem{ Value="GARE PELMA III COLLIERIES LTD", Text="GARE PELMA III COLLIERIES LTD"},
                                        new SelectListItem{ Value="GOLDEN VALLEY AGROTECH", Text="GOLDEN VALLEY AGROTECH"},
                                        new SelectListItem{ Value="GUJARAT ADANI INSTITUTE OF MEDICAL SCIENCES", Text="GUJARAT ADANI INSTITUTE OF MEDICAL SCIENCES"},
                                        new SelectListItem{ Value="HOWE ENGINEERING PROJECTS INDIA PVT LTD", Text="HOWE ENGINEERING PROJECTS INDIA PVT LTD"},
                                        new SelectListItem{ Value="INDIAN OIL ADANI GAS PVT LTD", Text="INDIAN OIL ADANI GAS PVT LTD"},
                                        new SelectListItem{ Value="KARNAVATI AVIATION P LTD", Text="KARNAVATI AVIATION P LTD"},
                                        new SelectListItem{ Value="MAHARASHTRA EASTERN GRID POWER TRANSMISSION CO.LTD", Text="MAHARASHTRA EASTERN GRID POWER TRANSMISSION CO.LTD"},
                                        new SelectListItem{ Value="MISTRY CONSTRUCTION COMPANY PVT LTD", Text="MISTRY CONSTRUCTION COMPANY PVT LTD"},
                                        new SelectListItem{ Value="MPSEZ UTILITIES PVT LTD", Text="MPSEZ UTILITIES PVT LTD"},
                                        new SelectListItem{ Value="MUNDRA SOLAR PV LTD", Text="MUNDRA SOLAR PV LTD"},
                                        new SelectListItem{ Value="MUNDRA SOLAR TECHNO PARK PVT LTD", Text="MUNDRA SOLAR TECHNO PARK PVT LTD"},
                                        new SelectListItem{ Value="NORTH KARANPURA TRANSCO LTD", Text="NORTH KARANPURA TRANSCO LTD"},
                                        new SelectListItem{ Value="PARAMPUJYA SOLAR ENERGY PRIVATE LIMITED", Text="PARAMPUJYA SOLAR ENERGY PRIVATE LIMITED"},
                                        new SelectListItem{ Value="PARSA KENTE COLLIERIES LIMITED", Text="PARSA KENTE COLLIERIES LIMITED"},
                                        new SelectListItem{ Value="PRAYATNA DEVELOPERS PRIVATE LTD", Text="PRAYATNA DEVELOPERS PRIVATE LTD"},
                                        new SelectListItem{ Value="RAIPUR - RAJNANDGAON - WARORA TRANSMISSION LIMITED", Text="RAIPUR - RAJNANDGAON - WARORA TRANSMISSION LIMITED"},
                                        new SelectListItem{ Value="RAJASTHAN COLLIERIES LIMITED", Text="RAJASTHAN COLLIERIES LIMITED"},
                                        new SelectListItem{ Value="SARGUJA RAIL CORRIDOR PRIVATE LIMITED", Text="SARGUJA RAIL CORRIDOR PRIVATE LIMITED"},
                                        new SelectListItem{ Value="SHANTI SAGAR INTERNATIONAL DREDGING PRIVATE LIMITED", Text="SHANTI SAGAR INTERNATIONAL DREDGING PRIVATE LIMITED"},
                                        new SelectListItem{ Value="SHANTIGRAM UTILITY SERVICES PRIVATE LIMITED", Text="SHANTIGRAM UTILITY SERVICES PRIVATE LIMITED"},
                                        new SelectListItem{ Value="SIPAT TRANSMISSION LIMITED", Text="SIPAT TRANSMISSION LIMITED"},
                                        new SelectListItem{ Value="TALABIRA (ODISHA) MINING PRIVATE LIMITED", Text="TALABIRA (ODISHA) MINING PRIVATE LIMITED"},
                                        new SelectListItem{ Value="THE ADANI HARBOUR SERVICES PRIVATE LIMITED", Text="THE ADANI HARBOUR SERVICES PRIVATE LIMITED"},
                                        new SelectListItem{ Value="THE DHAMRA PORT COMPANY LIMITED", Text="THE DHAMRA PORT COMPANY LIMITED"},
                                        new SelectListItem{ Value="UDUPI POWER CORPORATION LTD", Text="UDUPI POWER CORPORATION LTD"},
                                        new SelectListItem{ Value="VALUABLE PROPERTIES PVT LTD", Text="VALUABLE PROPERTIES PVT LTD"},
                                        new SelectListItem{ Value="WARDHA SOLAR (MAHARASHTRA) PRIVATE LIMITED", Text="WARDHA SOLAR (MAHARASHTRA) PRIVATE LIMITED"},
                                        new SelectListItem{ Value="WELSPUN ENERGY ANUPPUR PRIVATE LIMITED", Text="WELSPUN ENERGY ANUPPUR PRIVATE LIMITED"}
            };
            IdentityProofTypeList = new List<SelectListItem>
            {
                new SelectListItem{ Value="Passport", Text="Passport"},
                new SelectListItem{ Value="Aadhar Card", Text="Aadhar Card"},
                new SelectListItem{ Value="Driving License", Text="Driving License"},
                new SelectListItem{ Value="Pan Card", Text="Pan Card"},

            };
            TShirtSizeList = new List<SelectListItem> {
                new SelectListItem{ Value="32", Text="Kids (32 Inch)"},
                new SelectListItem{ Value="34", Text="Kids (34 Inch)"},
                new SelectListItem{ Value="36", Text="X-Small (36 Inch)"},
                new SelectListItem{ Value="38", Text="Small (38 Inch)"},
                new SelectListItem{ Value="40", Text="Medium (40 Inch)"},
                new SelectListItem{ Value="42", Text="Large (42 Inch)"},
                new SelectListItem{ Value="44", Text="X-Large (44 Inch)"},
                new SelectListItem{ Value="46", Text="XX-Large (46 Inch)"}
            };
          
            CountryList = new List<SelectListItem>
            {
                new SelectListItem{
                                        Value="India", Text="India" },
                                        new SelectListItem{ Value="Others", Text="Others" }                                      
            };
            BloodGroupList = new List<SelectListItem> {
                new SelectListItem{ Value="O+", Text="O+"},
                                        new SelectListItem{ Value="O-", Text="O-"},
                                        new SelectListItem{ Value="A+", Text="A+"},
                                        new SelectListItem{ Value="A-", Text="A-"},
                                        new SelectListItem{ Value="B+", Text="B+"},
                                        new SelectListItem{ Value="B-", Text="B-"},
                                        new SelectListItem{ Value="AB+", Text="AB+"},
                                        new SelectListItem{ Value="AB-", Text="AB-"}
            };
            GenderList = new List<SelectListItem>
            {
                new SelectListItem{ Value="Male", Text="Male"},
                new SelectListItem{ Value="Female", Text="Female"},
                new SelectListItem{ Value="Transgender", Text="Transgender"},
            };
            ChronicIllnessList = new List<SelectListItem>
            {
                new SelectListItem{ Value="Yes",Text="Yes"},
            };
            HeartAilmentList = new List<SelectListItem>
            {
                new SelectListItem{ Value="Yes",Text="Yes"},
            };
            AnyFaintingEpisodesinPastList = new List<SelectListItem>
            {
                new SelectListItem{ Value="Yes",Text="Yes"},
            };
            AnyOtherAilmentList = new List<SelectListItem>
            {
                new SelectListItem{ Value="Yes",Text="Yes"},
            };
        }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter {0}");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Invalid Contact", "Please enter a valid Mobile Number");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

    }
    public class temp
    {
        public ID racelocationID { get; set; }

        public temp()
        {
            racelocationID = new ID("{9D55601D-21DA-46AC-94B7-DCB0DE626927}");

        }
    }
    [Serializable]
    public class RegistrationCheck
    {
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(RegistrationModel))]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,17}$", ErrorMessage = "Please enter a valid email id")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter contat number")]
        [RegularExpression("^[6-9][0-9]{9}$", ErrorMessage = "Please enter a valid contact number")]
        public string PhoneNumber { get; set; }
        [RegularExpression("^[0-9]{5,9}$", ErrorMessage = "Please enter a valid OTP")]
        public string OTP { get; set; }
        [Required(ErrorMessage = "Please enter distance.")]
        public string distance { get; set; }
        [Required(ErrorMessage = "Please enter Run Mode.")]
        public string RunMode { get; set; }
        public string DateofBirth { get; set; }

        [Required(ErrorMessage = "Please select Run Type.")]
        public string RunType { get; set; }

        public string Count { get; set; }
        public string FinalAmount { get; set; }
        public string DonationAmount { get; set; }
        public string reResponse { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter {0}");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
    }
    [Serializable]
    public class RaceDetails
    {
        public string Distance { get; set; }
        public string Amount { get; set; }
        public string DistanceTxt { get; set; }
    }
    [Serializable]
    public class TimeSlotList : SelectListItem
    {
       
        public bool Enabled { get; set; }
        public int MaxAllowedCount { get; set; }
    }
    public class MarathonHelper
    {
        public static List<RaceDetails> GetRaceDistance()
        {
            List<RaceDetails> model = new List<RaceDetails>();
            Item RaceDetails = Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
            if (RaceDetails != null)
            {
                if (RaceDetails.HasChildren)
                {
                    foreach (Item p in RaceDetails.GetChildren())
                    {
                        RaceDetails rd = new RaceDetails();
                        rd.Distance = p["Distance"];
                        rd.DistanceTxt = p["Text"];
                        rd.Amount = p["Amount"];
                        model.Add(rd);
                    }
                }
            }
            return model;
        }
        public static List<RaceDetails> GetRemoteRaceDistance()
        {
            List<RaceDetails> model = new List<RaceDetails>();
            Item RaceDetails = Context.Database.GetItem("{CABFBF85-E4D5-4167-BE77-2F3F1CAE2223}");
            if (RaceDetails != null)
            {
                if (RaceDetails.HasChildren)
                {
                    foreach (Item p in RaceDetails.GetChildren())
                    {
                        RaceDetails rd = new RaceDetails();
                        rd.Distance = p["Distance"];
                        rd.DistanceTxt = p["Text"];
                        rd.Amount = p["Amount"];
                        model.Add(rd);
                    }
                }
            }
            return model;
        }
        public static string GetRaceAmount(string distance)
        {
            Item RaceDetails = Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
            if (RaceDetails != null)
            {
                if (RaceDetails.HasChildren)
                {
                    foreach (Item p in RaceDetails.GetChildren())
                    {
                        if (p.Fields["Distance"].Value.Trim() == distance)
                        {
                            return p.Fields["Amount"].Value.Trim();
                        }
                    }
                }
            }
            return null;
        }
        public static string GetCharityBibRaceAmount(string distance)
        {
            Item RaceDetails = Context.Database.GetItem("{7B220E91-F649-4E9C-8DDA-A72451876EDE}");
            if (RaceDetails != null)
            {
                if (RaceDetails.HasChildren)
                {
                    foreach (Item p in RaceDetails.GetChildren())
                    {
                        if (p.Fields["Distance"].Value.Trim() == distance)
                        {
                            return p.Fields["Amount"].Value.Trim();
                        }
                    }
                }
            }
            return null;
        }
        public static string GetRemoteRaceAmount(string distance)
        {
            Item RaceDetails = Context.Database.GetItem("{CABFBF85-E4D5-4167-BE77-2F3F1CAE2223}");
            if (RaceDetails != null)
            {
                if (RaceDetails.HasChildren)
                {
                    foreach (Item p in RaceDetails.GetChildren())
                    {
                        if (p.Fields["Distance"].Value.Trim() == distance)
                        {
                            return p.Fields["Amount"].Value.Trim();
                        }
                    }
                }
            }
            return null;
        }

        public static bool EmailIsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
   
}