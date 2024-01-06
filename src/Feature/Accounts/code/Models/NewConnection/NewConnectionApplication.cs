

namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web;
    using Newtonsoft.Json;
    using Sitecore.Feature.Accounts.Validation;

    public partial class NewConnectionApplication
    {
        public NewConnectionApplication()
        {
            NewConnectionService newconnectionservice = new NewConnectionService();
            var ApplicationType = newconnectionservice.ApplicationTypeList();
            ApplicationtypeList = new List<SelectListItem>();
            if (ApplicationType != null && ApplicationType.Any())
            {
                foreach (var item in ApplicationType.OrderBy(a => a.ApplicationId).Distinct().ToList())
                {
                    ApplicationtypeList.Add(new SelectListItem
                    {
                        Text = item.ApplicationType,
                        Value = item.ApplicationId.ToString()
                    });
                }
            }

            

            var ConnectionTypeList = newconnectionservice.ListConnectionTypeMapping();
            ConnectionTypeSelectList= new List<SelectListItem>();
            if (ConnectionTypeList != null && ConnectionTypeList.Any())
            {
                foreach (var item in ConnectionTypeList.OrderBy(a => a.CONNECTION_TYPE).Select(a => a.CONNECTION_TYPE_DESC).Distinct().ToList())
                {
                    ConnectionTypeSelectList.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item.ToString()
                    });
                }
            }
            var GovernmentTypeList = newconnectionservice.ListGovernmentApplicationTypeMapping();
            GovernmentTypeSelectList = new List<SelectListItem>();
           if (GovernmentTypeList != null && GovernmentTypeList.Any())
            {
                foreach (var item in GovernmentTypeList.Where(a=>a.APPLICANT_TYPE_DESC=="Govt").Select(a => a.APPLICANT_TYPE_GovernmentList).Distinct().ToList())
                {
                    GovernmentTypeSelectList.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item.ToString()
                    });
                }
            }
          var ApplicationTitleList = newconnectionservice.ListAPPLICANT_TITLE1Mapping();
            ApplicationTitleSelectList = new List<SelectListItem>();
            if (ApplicationTitleList != null && ApplicationTitleList.Any())
            {
                foreach (var item in ApplicationTitleList.Distinct().ToList())
                {
                    ApplicationTitleSelectList.Add(new SelectListItem
                    {
                        Text = item.APPLICANT_TITLE,
                        Value = item.APPLICANT_TITLE_DESC
                    });
                }
            }
           
            var BillLanguageList = newconnectionservice.ListBillLanguageMapping();
            BillLanguageSelectList = new List<SelectListItem>();
            if (BillLanguageList != null && BillLanguageList.Any())
            {
                foreach (var item in BillLanguageList.OrderBy(a => a.ID).Select(a => a.DESCRIPTION).Distinct().ToList())
                {
                    BillLanguageSelectList.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item.ToString()
                    });
                }
            }
          
            var listAreaMaster = newconnectionservice.ListAreaPinWorkcenterMapping();
            SuburbSelectList = new List<SelectListItem>();
            if (listAreaMaster != null && listAreaMaster.Any())
            {
                foreach (var item in listAreaMaster.Where(a=>a.Pincode == SelectedPincode).Select(a => a.Area).Distinct().ToList())
                {
                    SuburbSelectList.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item.ToString()
                    });
                }
            }
            
            var PincodeList = newconnectionservice.ListAreaPinWorkcenterMapping();
            PincodeSelectList = new List<SelectListItem>();
            if (PincodeList != null && PincodeList.Any())
            {
                foreach (var item in PincodeList.OrderBy(a => a.Pincode).Select(a => a.Pincode).Distinct().ToList())
                {
                    PincodeSelectList.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item.ToString()
                    });
                }
            }
            
            BankAccountTypeList = new List<SelectListItem>();
            BankAccountTypeList = new List<SelectListItem> {
                new SelectListItem{ Value="Savings Bank Account", Text="Savings Bank Account"},
                new SelectListItem{ Value="Current Account", Text="Current Account"},
                  new SelectListItem{ Value="Cash credit account", Text="Cash credit account"},
                   new SelectListItem{ Value="Savings Bank (at par cheque)", Text="Savings Bank (at par cheque)"},

            };
            PurposeOfSupplySelectList = new List<SelectListItem>();
            MeterLoadSelectList = new List<SelectListItem>();
            PremiseTypeSelectList = new List<SelectListItem>();
            VoltageLevelSelectList = new List<SelectListItem>();
            UtilityList = new List<SelectListItem>();
            GreenTariffList = new List<SelectListItem>();
            billingdifferentthanAddresswheresupply = "No";
            AddressInCaseOfRental = "No";
            ExistingConnection = "No";
            WiringCompleted = "No";
            BillFormat = "No";
         

            MeterconnectioninexistingmetercabinList = new List<SelectListItem>();

            MeterconnectioninexistingmetercabinList = new List<SelectListItem> {
                new SelectListItem{ Value="Yes", Text="Yes"},
                new SelectListItem{ Value="No", Text="No"},
                 

            };
            Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");

            Data.Items.Item GreenTariffLists = db.GetItem(Templates.NewConnection.GreenTariffList);
            GreenTariffList = GreenTariffLists.GetChildren().OrderBy(o => int.Parse(o.Fields["Value"].Value)).Select(x => new SelectListItem()
            {
                Text = x.Fields["Value"].Value,
                Value = x.Fields["Value"].Value
            }).ToList();



            UtilityList = new List<SelectListItem> {
                new SelectListItem{ Value="AEML", Text="AEML"},
                new SelectListItem{ Value="TPC", Text="TPC"},
                 new SelectListItem{ Value="BEST", Text="BEST"},
                new SelectListItem{ Value="MSCDCL", Text="MSCDCL"},


            };
            
            //ApplicationtypeList = new List<SelectListItem> {
            //    new SelectListItem{ Value="Individual Application", Text="Individual Application"},
            //    new SelectListItem{ Value="Building Project", Text="Building Project"},
                 


            //};
            //            ApplicationsubtypeList=new List<SelectListItem> {
            //    new SelectListItem{ Value= "Permanent Supply ", Text= "Permanent Supply " },
            //    new SelectListItem{ Value= "Temporary Supply ", Text="Building Application"},
            //        new SelectListItem { Value = "100 to 160 KW - Commercial or Industrial ", Text = "100 to 160 KW - Commercial or Industrial " },



            //};

            //Shekhar - Upload Document - Start
            ID2DocumentsList = new List<DocumentCheckList>();
            IDDocumentsList = new List<DocumentCheckList>();
            ODDocumentsList = new List<DocumentCheckList>();
            OTDocumentsList = new List<DocumentCheckList>();
            PHDocumentsList = new List<DocumentCheckList>();
            SDDocumentsList = new List<DocumentCheckList>();

            ID2DocumentsListOnly1 = new List<DocumentCheckList>();
            IDDocumentsListOnly1 = new List<DocumentCheckList>();
            ODDocumentsListOnly1 = new List<DocumentCheckList>();
            OTDocumentsListOnly1 = new List<DocumentCheckList>();
            PHDocumentsListOnly1 = new List<DocumentCheckList>();
            SDDocumentsListOnly1 = new List<DocumentCheckList>();

            ApplicantType = "1";

            //Shekhar - Upload Document - End
        }
        public string GreenTariff { get; set; }
        public List<SelectListItem> GreenTariffList { get; set; }
        public string IsSez { get; set; }
        public string IsGreenTariffApplied { get; set; }
        public string DedicatedDistributionfacility { get; set; }
        public Double DedicatedDistributionfacilityRs { get; set; }
        public Guid Id { get; set; }
        public string ApplicationType { get; set; }
        public List<SelectListItem> ApplicationtypeList { get; set; }
        public string ApplicationSubType { get; set; }
        public List<SelectListItem> ApplicationsubtypeList { get; set; }
        public string IsExistingCustomer { get; set; }
        public string CANumber { get; set; }
        public string OrganizationName { get; set; }
        public bool IsCAValid { get; set; }
        public string SelectedConnectionType { get; set; }
        public List<SelectListItem> ConnectionTypeSelectList { get; set; }
       
        public string IsApplicantType{ get; set; }
       
        public string selectedGovernmentType { get; set; }
        public List<SelectListItem> GovernmentTypeSelectList { get; set; }
        public string ApplicationTitle { get; set; }
        public List<SelectListItem> ApplicationTitleSelectList { get; set; }
        public string ApplicanttType { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string FirstName { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string MiddleName { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string LastName { get; set; }

        [DOBDateValidation]
        public DateTime? DateofBirth
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string Name1Joint { get; set; }
        [DOBDateValidation]
        public DateTime? Name1JointDateofBirth
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ChangeOfNameApplicationFromModel))]
        public string Name2Joint { get; set; }
        [DOBDateValidation]
        public DateTime? Name2JointDateofBirth
        {
            get;
            set;
        }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string FlatNumber { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string BuildingName { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string Street { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string Landmark { get; set; }
        public string SelectedSuburb { get; set; }
        public List<SelectListItem> SuburbSelectList { get; set; }
        public string SelectedPincode { get; set; }
        public List<SelectListItem> PincodeSelectList { get; set; }
        public string billingdifferentthanAddresswheresupply { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string ApplicantCorrespondenceFlatNumber { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string ApplicantCorrespondenceBuildingName { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string ApplicantCorrespondenceStreet { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string ApplicantCorrespondenceLandmark { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string ApplicantCorrespondenceSuburb { get; set; }
        public string ApplicantCorrespondencePincode { get; set; }
        public string AddressInCaseOfRental { get; set; }
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string RentalNameoftheOwner { get; set; }
        public string RentalContactNumber { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string RentalFlatNumber { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string RentalBuildingName { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string RentalStreet { get; set; }
        [RegularExpression("^([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidInput), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string RentalSuburb { get; set; }
        public string RentalPincode { get; set; }
        public string AreaTobeSent { get; set; }
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        [DataType(DataType.EmailAddress)]
        public string RentalOwnerEmail { get; set; }
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Mobile", "Please enter a valid Mobile Number");
        public static string InvalidInput => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Name", "Input is not valid.");
        public static string InvalidName => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid  Name", "Name contains alphbets only");
        
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string MobileNo { get; set; }
        public static string InvalidLandline => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Landline Number", "Please enter a valid Landline Number");
      
        [RegularExpression(@"^\d{6,9}$", ErrorMessageResourceName = nameof(InvalidLandline), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string LandlineNo { get; set; }
        public bool IsLEC { get; set; }
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/NewConnection/Invalid Email Address", "Please enter a valid email address");
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
 [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string LECMobileNo { get; set; }
        [RegularExpression(@"^\d{6,9}$", ErrorMessageResourceName = nameof(InvalidLandline), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string LECLandlineNo { get; set; }
     [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        [DataType(DataType.EmailAddress)]
        public string LECEmail { get; set; }
      public string BillLanguage { get; set; }
        public List<SelectListItem> BillLanguageSelectList { get; set; }
        public string BillFormat{ get; set; }
        public string MICR { get; set; }
        public string BankAccountType { get; set; }
        public List<SelectListItem> BankAccountTypeList { get; set; }
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(NewConnectionApplication))]
        public string BankHoldersName { get; set; }
        public string Bank { get; set; }
        public string Branch{ get; set; }
        public string BankAccountNumber{ get; set; }
        [System.ComponentModel.DataAnnotations.Compare("BankAccountNumber",ErrorMessage ="Bank Account No and Confirm Bank Account number should be same")]
        public string BankAccountNumberConfirm { get; set; }
        public string ConnectedLoadKW { get; set; }
        public string ConnectedLoadHP { get; set; }
        public Double TotalLoad { get; set; }
        public string MeterType { get; set; }
        public List<SelectListItem> MeterTypeSelectList { get; set; }
        public string PurposeOfSupply { get; set; }
        public List<SelectListItem> PurposeOfSupplySelectList { get; set; }
        public string AppliedTariff { get; set; }
       public string PremiseType { get; set; }
        public List<SelectListItem> PremiseTypeSelectList { get; set; }
        public string MeterLoad { get; set; }
        public List<SelectListItem> MeterLoadSelectList { get; set; }
        public string VoltageLevel { get; set; }
        public List<SelectListItem> VoltageLevelSelectList { get; set; }
      public string ContractDemand{ get; set; }
        public string Meterconnectioninexistingmetercabin { get; set; }
        public List<SelectListItem> MeterconnectioninexistingmetercabinList { get; set; }
        public string NearestConsumerAccountNo { get; set; }
        public string NearestConsumerMeterNo { get; set; }
        public string ExistingConnection { get; set; }
        public string ConsumerNo { get; set; }
        public string Utility { get; set; }
        public List<SelectListItem> UtilityList { get; set; }
        public string WiringCompleted { get; set; }
        public string LECNumber { get; set; }
        public string NameOnLEC { get; set; }
        public string Status { get; set; }
        public string hdnStatus { get; set; }
        public string ApplicationMode { get; set; }

        public Guid hdnID { get; set; }
        public string TRNumber { get; set; }
        public HttpPostedFileBase TRNumberDoc { get; set; }
        public string IsTermConditionAccepted { get; set; }
        public string metersupplier { get; set; }
        public string Meterconnection { get; set; }
        public string IsCopiedSelfAttested { get; set; }

        public string ConnectionTypeCode { get; set; }

        public string LDPOrderNumber { get; set; }

        [ValidateTodayDate]
        public DateTime? TempStartDate { get; set; }
        [DateTimeNotLessThan("TempStartDate")]
        public DateTime? TempEndDate { get; set; }
    }
}
