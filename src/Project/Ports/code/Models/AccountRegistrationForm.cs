using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class AccountRegistrationForm
    {
        public HttpPostedFileBase AcknowledgementGSTRegistration
        {
            get;
            set;
        }
        public HttpPostedFileBase IECodeCertificate
        {
            get;
            set;
        }

        public HttpPostedFileBase AEOLicence
        {
            get;
            set;
        }

        public HttpPostedFileBase SCMTRProof
        {
            get;
            set;
        }

        public HttpPostedFileBase AgreementWithPrincipal
        {
            get;
            set;
        }

        public HttpPostedFileBase PowerofAttorney
        {
            get;
            set;
        }

        public string BillingAddress
        {
            get;
            set;
        }

        public string BillingContactNumber
        {
            get;
            set;
        }

        public string BillingCountry
        {
            get;
            set;
        }

        public string BillingEmail
        {
            get;
            set;
        }

        public string BillingState
        {
            get;
            set;
        }

        public HttpPostedFileBase CancelledCheque
        {
            get;
            set;
        }

        public HttpPostedFileBase CertificationOfIncorporation
        {
            get;
            set;
        }

        public string CommunicationEmail1
        {
            get;
            set;
        }

        public string CommunicationEmail2
        {
            get;
            set;
        }

        public string CommunicationEmail3
        {
            get;
            set;
        }

        public DateTime CompanyRegistrationDate
        {
            get;
            set;
        } 
        
        //public DateTime SubmitOnDate
        //{
        //    get;
        //    set;
        //}

        public string CT2RegisterCode
        {
            get;
            set;
        }

        public string CT3RegisterCode
        {
            get;
            set;
        }

        public string CT4RegisterCode
        {
            get;
            set;
        }

        public HttpPostedFileBase CustomDPDPermission
        {
            get;
            set;
        }

        public string Designation
        {
            get;
            set;
        }

        public HttpPostedFileBase DrivingLicense
        {
            get;
            set;
        }

        public string FinanceDesignation
        {
            get;
            set;
        }

        public string FinanceDesignation2
        {
            get;
            set;
        }

        public string FinanceDesignation3
        {
            get;
            set;
        }

        public string FinanceDirectNo
        {
            get;
            set;
        }

        public string FinanceDirectNo2
        {
            get;
            set;
        }

        public string FinanceDirectNo3
        {
            get;
            set;
        }

        public string FinanceEmailID
        {
            get;
            set;
        }

        public string FinanceEmailID2
        {
            get;
            set;
        }

        public string FinanceEmailID3
        {
            get;
            set;
        }

        public string FinanceMobileNumber
        {
            get;
            set;
        }

        public string FinanceMobileNumber2
        {
            get;
            set;
        }

        public string FinanceMobileNumber3
        {
            get;
            set;
        }

        public string FinanceName
        {
            get;
            set;
        }

        public string FinanceName2
        {
            get;
            set;
        }

        public string FinanceName3
        {
            get;
            set;
        }

        //public string FormType
        //{
        //    get;
        //    set;
        //}

        public string GSTNumber
        {
            get;
            set;
        }

        public string LocalOfficeAddress
        {
            get;
            set;
        }

        public string LocalOfficeContactNumber
        {
            get;
            set;
        }

        public string LocalOfficeCountry
        {
            get;
            set;
        }

        public string LocalOfficeEmail
        {
            get;
            set;
        }

        public string LocalOfficeState
        {
            get;
            set;
        }

        public HttpPostedFileBase MemorandumArticleAssociation
        {
            get;
            set;
        }

        public string MICTRegisterCode
        {
            get;
            set;
        }

        public HttpPostedFileBase MunicipalLicence
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string NameofCompany
        {
            get;
            set;
        }

        public string NameofPrincipal
        {
            get;
            set;
        }

        public string OperationDirectNo
        {
            get;
            set;
        }

        public string OperationDirectNo2
        {
            get;
            set;
        }

        public string OperationDirectNo3
        {
            get;
            set;
        }

        public string OperationEmailID
        {
            get;
            set;
        }

        public string OperationEmailID2
        {
            get;
            set;
        }

        public string OperationEmailID3
        {
            get;
            set;
        }

        public string OperationMobileNumber
        {
            get;
            set;
        }

        public string OperationMobileNumber2
        {
            get;
            set;
        }

        public string OperationMobileNumber3
        {
            get;
            set;
        }

        public string OperationsDesignation
        {
            get;
            set;
        }

        public string OperationsDesignation2
        {
            get;
            set;
        }

        public string OperationsDesignation3
        {
            get;
            set;
        }

        public string OperationsName
        {
            get;
            set;
        }

        public string OperationsName2
        {
            get;
            set;
        }

        public string OperationsName3
        {
            get;
            set;
        }

        //public string PageInfo
        //{
        //    get;
        //    set;
        //}

        public HttpPostedFileBase PanCard
        {
            get;
            set;
        }

        public string PANNumber
        {
            get;
            set;
        }

        public string PortType
        {
            get;
            set;
        }

        public string RegisteredOfficeAddress
        {
            get;
            set;
        }

        public string RegisteredOfficeContactNumber
        {
            get;
            set;
        }

        public string RegisteredOfficeCountry
        {
            get;
            set;
        }

        public string RegisteredOfficeEmail
        {
            get;
            set;
        }

        public string RegisteredOfficeState
        {
            get;
            set;
        }

        public string Rupees
        {
            get;
            set;
        }

        public string RupeesInWords
        {
            get;
            set;
        }

        public string ShippingAgentDetails
        {
            get;
            set;
        }

      

        public string T2RegisterCode
        {
            get;
            set;
        }

        public string TANNumber
        {
            get;
            set;
        }

        public string TypeofCustomers
        {
            get;
            set;
        }

        public string SCMTRCode
        {
            get;
            set;
        }
        public string SCMTRID
        {
            get;
            set;
        }

        public string GenerateId()
        {
            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            return number;
        }


        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var contextObject = (AccountRegistrationForm)validationContext.ObjectInstance;

        //    if (contextObject.CompanyRegistrationDate == DateTime.MinValue)
        //    {
        //        yield return new ValidationResult("Please Enter Valid Date", new[] { "CompanyRegistrationDate" });
        //    }
        //}


        public string reResponse { get; set; }
    }
    public class TypeofCustomers
    {
        public string CustomersName { get; set; }
        public int CustomersValue { get; set; }

    }

    public class Port
    {
        public string PortName { get; set; }
        public int PortValue { get; set; }

    }
    public class SCMTRCode
    {
        public string SCMTRCodeName { get; set; }
        public int SCMTRCodeValue { get; set; }

    }
}