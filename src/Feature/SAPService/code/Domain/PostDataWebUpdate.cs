using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapPiService.Domain
{
    public class PostDataWebUpdate
    {
        public string TempRegistrationNumber { get; set; }
        public bool IsLEC { get; set; }
        public bool IsRooftopsolarInstalled { get; set; }
        public bool IsRooftopPlusGroundCapacity { get; set; }
        public bool IsRooftopOwned { get; set; }
        public string LECNumber { get; set; }
        public bool IsNetMeter { get; set; }
        public string InstallationCost { get; set; }
        public string AccountNumber { get; set; }
        public string ApplicationCategory { get; set; }
        public string VoltageLevelDesc { get; set; }
        public bool IsObligatedEntity { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string VoltageLevel { get; set; }
        public string Workcenter { get; set; }
        public string ApplicationTitle { get; set; }
        public string SectorType { get; set; }
        public string MeterCabin { get; set; }
        public string SelfMeter { get; set; }
        public string GovernmentSelected { get; set; }
        public string BillingFormatEBill { get; set; }
        public string BillingFormatEPBill { get; set; }
        public string ConnectionType { get; set; }
        public string TempStartDate { get; set; }
        public string TempEndDate { get; set; }
        public string LDPNumber { get; set; }
        public string billingdifferentthanAddresswheresupply { get; set; }
        public string RentalAddress { get; set; }
        public string PurposeofSupply { get; set; }
        public string MeterTypeCount1PH { get; set; }
        public string MeterTypeCount3PH { get; set; }
        public string MeterTypeCountHT{ get; set; }
        public string ConnectedLoadKW1PH { get; set; }
        public string ConnectedLoadHP1PH { get; set; }
        public string ConnectedLoadKW3PH { get; set; }
        public string ConnectedLoadHP3PH { get; set; }
        public string ConnectedLoadKWHT { get; set; }
        public string ConnectedLoadHT { get; set; }
        public string AppliedTariff { get; set; }
        public string PremiseType { get; set; }
        public string NearestCAnumber { get; set; }
        public string MeterLoad { get; set; }
        public string NearestMeternumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string MICR { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string ConsumerNumber { get; set; }
        public string Utility { get; set; }
        public string WiringCompleted { get; set; }
        public string NameofRentalOwner { get; set; }
        public string RentalLane { get; set; }
        public string OrganizationName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string HouseNumber { get; set; }
        public string BuildingName { get; set; }
        public string LaneStreet { get; set; }
        public string Landmark { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string FlagBillAddress { get; set; }
        public string BillingHouseNumber { get; set; }
        public string BillingBuildingName { get; set; }
        public string BillingLane { get; set; }
        public string BillingLandmark { get; set; }
        public string BillingSuburb { get; set; }
        public string BillingPincode { get; set; }
        public string BillingCity { get; set; }
        public string FlagRentPremise { get; set; }
        public string RentalHouseNumber { get; set; }
        public string RentalBuildingName { get; set; }
        public string RentalLandmark { get; set; }
        public string RentalSuburb { get; set; }
        public string RentalPincode { get; set; }
        public string RentalCity { get; set; }
        public string RentalPhoneNumber { get; set; }
        public string RentalMobileNumber { get; set; }
        public string RentalEmail { get; set; }
        public string MobileNumber { get; set; }
        public string LandlineNumber { get; set; }
        public string Email { get; set; }
        public string BillLangianguage { get; set; }
        public string TotalLoad { get; set; }
        public string ContractDemand { get; set; }
        public string IsGreenTariffApplied { get; set; }


    }

    public class PostDocumentsWebUpdate
    {
        public string DocumentSerialNumber { get; set; }
        public string DocumentDescription { get; set; }
        public byte[] DocumentData { get; set; }
    }

    public class PostDataResult
    {
        public bool IsSuccess { get; set; }
        public string ExceptionMessage { get; set; }
        public string FLAG_UPD_APPL { get; set; }
        public string FLAG_UPD_ADRC { get; set; }
        public string FLAG_UPD_LOAD { get; set; }
        public string FLAG_UPD_DOC { get; set; }
    }

    public class OrderFetchResult
    {
        public bool IsSuccess { get; set; }
        public string ExceptionMessage { get; set; }
        public string OrderIdSAP { get; set; }
        public string ContractAccountNumber { get; set; }
        public string BusinessPartnerNumber { get; set; }

    }
}