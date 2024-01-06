using Sitecore.Ports.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Ports.Website.Provider
{
    public class ErrorResponse
    {
        public bool IsErrorResonse { get; set; } = false;
        public string FieldName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = "";
    }
    public class ValidationProvider
    {
        public static bool Validate(AccountRegistrationForm m, out ErrorResponse errorResponse)
        {
            errorResponse = new ErrorResponse();// {  IsErrorResonse = false, FieldName = string.Empty, ErrorMessage = "" };

            if (string.IsNullOrEmpty(m.TypeofCustomers))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "TypeofCustomersError", ErrorMessage = "Please select Type of Customers" };
                return false;
            }
            if (string.IsNullOrEmpty(m.PortType))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SelectPortError", ErrorMessage = "Please select Port" };
                return false;
            }

            var TypeofCustomersCount = PortAccountRegistrationDropdowns.TypeofCustomers.Where(c => c.CustomersName == m.TypeofCustomers).Count();
            var PortCount = PortAccountRegistrationDropdowns.Port.Where(c => c.PortName == m.PortType).Count();
            var SCMTRCodeCount = PortAccountRegistrationDropdowns.SCMTRCode.Where(c => c.SCMTRCodeName == m.SCMTRCode).Count();
            if (TypeofCustomersCount == 0)
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "TypeofCustomersError", ErrorMessage = "Please select Type of Customers" };
                return false;
            }
            if (PortCount == 0)
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SelectPortError", ErrorMessage = "Please select Port" };
                return false;
            }
            if (m.TypeofCustomers != "CHA")
            {
                if (string.IsNullOrEmpty(m.SCMTRCode))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SCMTRCodeError", ErrorMessage = "Please select SCMTR Code" };
                    return false;
                }
                if (SCMTRCodeCount == 0)
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SCMTRCodeError", ErrorMessage = "Please select SCMTR Code" };
                    return false;
                }
            }
            if (m.TypeofCustomers != "CHA")
            {
                if (string.IsNullOrEmpty(m.SCMTRID))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SCMTRError", ErrorMessage = "Please enter SCMTR ID" };
                    return false;
                }
                else if (!string.IsNullOrEmpty(m.SCMTRID) && (!Regex.IsMatch(m.SCMTRID, (@"^[ A-Za-z0-9]*$"))))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SCMTRError", ErrorMessage = "Please enter valid SCMTR ID" };
                    return false;
                }
                else if (!string.IsNullOrEmpty(m.SCMTRID) && (m.SCMTRID.Length > 70))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SCMTRError", ErrorMessage = "!Only 70 characters are allowed" };
                    return false;
                }
            }
            if (m.TypeofCustomers == "Shipping-Line" || m.TypeofCustomers == "NVOCC" || m.TypeofCustomers == "Vessel-Agent")
            {
                if (string.IsNullOrEmpty(m.ShippingAgentDetails))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "ShippingAgentDetailError", ErrorMessage = "Please enter Shipping Agent Details" };
                    return false;
                }
                else if (!string.IsNullOrEmpty(m.ShippingAgentDetails) && (!Regex.IsMatch(m.ShippingAgentDetails, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "ShippingAgentDetailError", ErrorMessage = "Please enter valid Shipping Agent Details" };
                    return false;
                }
                else if (!string.IsNullOrEmpty(m.ShippingAgentDetails) && (m.ShippingAgentDetails.Length > 70))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "ShippingAgentDetailError", ErrorMessage = "!Only 70 characters are allowed" };
                    return false;
                }
                if (string.IsNullOrEmpty(m.NameofPrincipal))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameofPrincipalError", ErrorMessage = "Please enter Name of Principal" };
                    return false;
                }
                else if (!string.IsNullOrEmpty(m.NameofPrincipal) && (!Regex.IsMatch(m.NameofPrincipal, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameofPrincipalError", ErrorMessage = "Please enter valid Name of Principal" };
                    return false;
                }
                else if (!string.IsNullOrEmpty(m.NameofPrincipal) && (m.NameofPrincipal.Length > 70))
                {
                    errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameofPrincipalError", ErrorMessage = "!Only 70 characters are allowed" };
                    return false;
                }
            }
            if (string.IsNullOrEmpty(m.NameofCompany))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameofCompanyTError", ErrorMessage = "Please enter Name of Company" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.NameofCompany) && (!Regex.IsMatch(m.NameofCompany, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameofCompanyTError", ErrorMessage = "Please enter valid Name of Company" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.NameofCompany) && (m.NameofCompany.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameofCompanyTError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            var DateTimeVal = DateTime.Parse(DateTime.Now.ToString());
            if (m.CompanyRegistrationDate == DateTime.MinValue)
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "tempCompanyRegistrationDateTError", ErrorMessage = "Please enter valid Company Registration Date" };
                return false;
            }
            if (m.CompanyRegistrationDate >= DateTimeVal)
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "tempCompanyRegistrationDateTError", ErrorMessage = "Please Company Registration Date should be less then today" };
                return false;
            }
            if (string.IsNullOrEmpty(m.PANNumber))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PANNumberError", ErrorMessage = "Please enter PANNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.PANNumber) && (!Regex.IsMatch(m.PANNumber, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PANNumberError", ErrorMessage = "Please enter valid PANNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.PANNumber) && (m.PANNumber.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PANNumberError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }

            if (string.IsNullOrEmpty(m.TANNumber))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "TANNumberError", ErrorMessage = "Please enter TANNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.TANNumber) && (!Regex.IsMatch(m.TANNumber, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "TANNumberError", ErrorMessage = "Please enter valid TANNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.TANNumber) && (m.TANNumber.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "TANNumberError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.GSTNumber))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "GSTNumberError", ErrorMessage = "Please enter GSTNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.GSTNumber) && (!Regex.IsMatch(m.GSTNumber, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "GSTNumberError", ErrorMessage = "Please enter valid GSTNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.GSTNumber) && (m.GSTNumber.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "GSTNumberError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.RegisteredOfficeAddress))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeAddressError", ErrorMessage = "Please enter Registered Office Address" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeAddress) && (!Regex.IsMatch(m.RegisteredOfficeAddress, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeAddressError", ErrorMessage = "Please enter valid Registered Office Address" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeAddress) && (m.RegisteredOfficeAddress.Length > 100))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeAddressError", ErrorMessage = "!Only 100 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.RegisteredOfficeState))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeStateError", ErrorMessage = "Please enter Registered Office State" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeState) && (!Regex.IsMatch(m.RegisteredOfficeState, (@"^[a-zA-Z][a-zA-Z ]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeStateError", ErrorMessage = "Please enter valid Registered Office State" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeState) && (m.RegisteredOfficeState.Length > 50))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeStateError", ErrorMessage = "!Only 50 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.RegisteredOfficeCountry))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeCountryError", ErrorMessage = "Please enter Registered Office Country" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeCountry) && (!Regex.IsMatch(m.RegisteredOfficeCountry, (@"^[a-zA-Z][a-zA-Z ]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeCountryError", ErrorMessage = "Please enter valid Registered Office Country" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeCountry) && (m.RegisteredOfficeCountry.Length > 50))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeCountryError", ErrorMessage = "!Only 50 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.RegisteredOfficeContactNumber))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeContactNumberError", ErrorMessage = "Please enter Registered Office ContactNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeContactNumber) && (!Regex.IsMatch(m.RegisteredOfficeContactNumber, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeContactNumberError", ErrorMessage = "Please enter valid Registered Office ContactNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeContactNumber) && (m.RegisteredOfficeContactNumber.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeContactNumberError", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.RegisteredOfficeEmail))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeEmailError", ErrorMessage = "Please enter Registered Office Email" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RegisteredOfficeEmail) && (!Regex.IsMatch(m.RegisteredOfficeEmail, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RegisteredOfficeEmailError", ErrorMessage = "Please enter valid Registered Office Email" };
                return false;
            }
            if (string.IsNullOrEmpty(m.LocalOfficeAddress))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeAddressError", ErrorMessage = "Please enter Local Office Address" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeAddress) && (!Regex.IsMatch(m.LocalOfficeAddress, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeAddressError", ErrorMessage = "Please enter valid Local Office Address" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeAddress) && (m.LocalOfficeAddress.Length > 100))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeAddressError", ErrorMessage = "!Only 100 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.LocalOfficeState))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeStateError", ErrorMessage = "Please enter Local Office Address" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeState) && (!Regex.IsMatch(m.LocalOfficeState, (@"^[a-zA-Z][a-zA-Z ]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeStateError", ErrorMessage = "Please enter valid Local Office Address" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeState) && (m.LocalOfficeState.Length > 50))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeStateError", ErrorMessage = "!Only 50 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.LocalOfficeCountry))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeCountryError", ErrorMessage = "Please enter Local Office Country" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeCountry) && (!Regex.IsMatch(m.LocalOfficeCountry, (@"^[a-zA-Z][a-zA-Z ]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeCountryError", ErrorMessage = "Please enter valid Local Office Country" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeCountry) && (m.LocalOfficeCountry.Length > 50))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeCountryError", ErrorMessage = "!Only 50 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.LocalOfficeContactNumber))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "candidateNameError", ErrorMessage = "Please enter Local Office Contact Number" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeContactNumber) && (!Regex.IsMatch(m.LocalOfficeContactNumber, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeContactNumberError", ErrorMessage = "Please enter valid Local Office Contact Number" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeContactNumber) && (m.LocalOfficeContactNumber.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeContactNumberError", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.LocalOfficeEmail))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeEmailError", ErrorMessage = "Please enter Local Office Email" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.LocalOfficeEmail) && (!Regex.IsMatch(m.LocalOfficeEmail, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "LocalOfficeEmailError", ErrorMessage = "Please enter valid Local Office Email" };
                return false;
            }
            if (string.IsNullOrEmpty(m.BillingAddress))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingAddressError", ErrorMessage = "Please enter Billing Address" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingAddress) && (!Regex.IsMatch(m.BillingAddress, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingAddressError", ErrorMessage = "Please enter valid Billing Address" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingAddress) && (m.BillingAddress.Length > 100))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingAddressError", ErrorMessage = "!Only 100 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.BillingState))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingStateError", ErrorMessage = "Please enter Billing State" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingState) && (!Regex.IsMatch(m.BillingState, (@"^[a-zA-Z][a-zA-Z ]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingStateError", ErrorMessage = "Please enter valid Billing State" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingState) && (m.BillingState.Length > 50))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingStateError", ErrorMessage = "!Only 50 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.BillingCountry))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingCountryError", ErrorMessage = "Please enter Billing Country" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingCountry) && (!Regex.IsMatch(m.BillingCountry, (@"^[a-zA-Z][a-zA-Z ]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingCountryError", ErrorMessage = "Please enter valid Billing Country" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingCountry) && (m.BillingCountry.Length > 50))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingCountryError", ErrorMessage = "!Only 50 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.BillingContactNumber))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingContactNumberError", ErrorMessage = "Please enter Billing Contact Number" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingContactNumber) && (!Regex.IsMatch(m.BillingContactNumber, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingContactNumberError", ErrorMessage = "Please enter valid Billing Contact Number" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingContactNumber) && (m.BillingContactNumber.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingContactNumberError", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.BillingEmail))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingEmailError", ErrorMessage = "Please enter Billing Email" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.BillingEmail) && (!Regex.IsMatch(m.BillingEmail, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "BillingEmailError", ErrorMessage = "Please enter valid Billing Email" };
                return false;
            }
            if (string.IsNullOrEmpty(m.OperationsName))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsNameError", ErrorMessage = "Please enter Operations Name" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationsName) && (!Regex.IsMatch(m.OperationsName, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsNameError", ErrorMessage = "Please enter valid Operations Name" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationsName) && (m.OperationsName.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsNameError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationsName2) && (!Regex.IsMatch(m.OperationsName2, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsName2Error", ErrorMessage = "Please enter valid Operations Name2" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationsName2) && (m.OperationsName2.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsName2Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationsName3) && (!Regex.IsMatch(m.OperationsName3, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsName3Error", ErrorMessage = "Please enter valid Operations Name3" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationsName3) && (m.OperationsName3.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsName3Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }

            if (string.IsNullOrEmpty(m.OperationsDesignation))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsDesignationError", ErrorMessage = "Please enter Operations Designation" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationsDesignation) && (!Regex.IsMatch(m.OperationsDesignation, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsDesignationError", ErrorMessage = "Please enter valid Operations Designation" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationsDesignation) && (m.OperationsDesignation.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsDesignationError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationsDesignation2) && (!Regex.IsMatch(m.OperationsDesignation2, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsDesignation2Error", ErrorMessage = "Please enter valid Operations Designation2" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationsDesignation2) && (m.OperationsDesignation2.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsDesignation2Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationsDesignation3) && (!Regex.IsMatch(m.OperationsDesignation3, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsDesignation3Error", ErrorMessage = "Please enter valid Operations Designation3" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationsDesignation3) && (m.OperationsDesignation3.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationsDesignation3Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.OperationDirectNo))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationDirectNoError", ErrorMessage = "Please enter Operation DirectNo" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationDirectNo) && (!Regex.IsMatch(m.OperationDirectNo, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationDirectNoError", ErrorMessage = "Please enter valid Operation DirectNo" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationDirectNo) && (m.OperationDirectNo.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationDirectNoError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationDirectNo2) && (!Regex.IsMatch(m.OperationDirectNo2, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationDirectNo2Error", ErrorMessage = "Please enter valid Operation DirectNo2" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationDirectNo2) && (m.OperationDirectNo2.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationDirectNo2Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationDirectNo3) && (!Regex.IsMatch(m.OperationDirectNo3, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationDirectNo3Error", ErrorMessage = "Please enter valid Operation DirectNo3" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationDirectNo3) && (m.OperationDirectNo3.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationDirectNo3Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }

            if (string.IsNullOrEmpty(m.OperationMobileNumber))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationMobileNumberError", ErrorMessage = "Please enter Operation Mobile Number" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationMobileNumber) && (!Regex.IsMatch(m.OperationMobileNumber, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationMobileNumberError", ErrorMessage = "Please enter valid Operation Mobile Number" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationMobileNumber) && (m.OperationMobileNumber.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationMobileNumberError", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationMobileNumber2) && (!Regex.IsMatch(m.OperationMobileNumber2, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationMobileNumber2Error", ErrorMessage = "Please enter valid Operation Mobile Number" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationMobileNumber3) && (!Regex.IsMatch(m.OperationMobileNumber3, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationMobileNumber3Error", ErrorMessage = "Please enter valid Operation Mobile Number" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationMobileNumber3) && (m.OperationMobileNumber3.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationMobileNumber3Error", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationMobileNumber2) && (m.OperationMobileNumber2.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationMobileNumber2Error", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.OperationEmailID))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationEmailIDError", ErrorMessage = "Please enter Operation Email ID" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.OperationEmailID) && (!Regex.IsMatch(m.OperationEmailID, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationEmailIDError", ErrorMessage = "Please enter valid Operation Email ID" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationEmailID2) && (!Regex.IsMatch(m.OperationEmailID2, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationEmailID2Error", ErrorMessage = "Please enter valid Operation Email ID" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.OperationEmailID3) && (!Regex.IsMatch(m.OperationEmailID3, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "OperationEmailID3Error", ErrorMessage = "Please enter valid Operation Email ID" };
                return false;
            }
            if (string.IsNullOrEmpty(m.FinanceName))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceNameError", ErrorMessage = "Please enter Finance Name" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceName) && (!Regex.IsMatch(m.FinanceName, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceNameError", ErrorMessage = "Please enter valid Finance Name" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceName) && (m.FinanceName.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceName2Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceName2) && (!Regex.IsMatch(m.FinanceName2, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceName2Error", ErrorMessage = "Please enter valid Finance Name2" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceName2) && (m.FinanceName2.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceName2Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceName3) && (!Regex.IsMatch(m.FinanceName3, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceName3Error", ErrorMessage = "Please enter valid Finance Name3" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceName3) && (m.FinanceName3.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceName3Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }

            if (string.IsNullOrEmpty(m.FinanceDesignation))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDesignationError", ErrorMessage = "Please enter Finance Designation" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceDesignation) && (!Regex.IsMatch(m.FinanceDesignation, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDesignationError", ErrorMessage = "Please enter valid Finance Designation" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceDesignation) && (m.FinanceDesignation.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDesignationError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceDesignation2) && (!Regex.IsMatch(m.FinanceDesignation2, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDesignation2Error", ErrorMessage = "Please enter valid Finance Designation2" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceDesignation2) && (m.FinanceDesignation2.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDesignation2Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceDesignation3) && (!Regex.IsMatch(m.FinanceDesignation3, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDesignation3Error", ErrorMessage = "Please enter valid Finance Designation3" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceDesignation3) && (m.FinanceDesignation3.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDesignation3Error", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.FinanceDirectNo))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDirectNoError", ErrorMessage = "Please enter Finance DirectNo" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceDirectNo) && (!Regex.IsMatch(m.FinanceDirectNo, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDirectNoError", ErrorMessage = "Please enter valid Finance DirectNo" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceDirectNo) && (m.FinanceDirectNo.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceDirectNoError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.FinanceMobileNumber))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceMobileNumberError", ErrorMessage = "Please enter Operation Finance MobileNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceMobileNumber) && (!Regex.IsMatch(m.FinanceMobileNumber, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceMobileNumberError", ErrorMessage = "Please enter valid Operation Finance MobileNumber" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceMobileNumber) && (m.FinanceMobileNumber.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceMobileNumberError", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceMobileNumber2) && (!Regex.IsMatch(m.FinanceMobileNumber2, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceMobileNumber2Error", ErrorMessage = "Please enter valid Finance Mobile Number" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceMobileNumber3) && (!Regex.IsMatch(m.FinanceMobileNumber3, (@"^([0-9]{10})$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceMobileNumber3Error", ErrorMessage = "Please enter valid Finance Mobile Number" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceMobileNumber3) && (m.FinanceMobileNumber3.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceMobileNumber3Error", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceMobileNumber2) && (m.FinanceMobileNumber2.Length > 11))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceMobileNumber2Error", ErrorMessage = "!Only 11 number are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.FinanceEmailID))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceEmailIDError", ErrorMessage = "Please enter Finance Email ID" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.FinanceEmailID) && (!Regex.IsMatch(m.FinanceEmailID, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceEmailIDError", ErrorMessage = "Please enter valid Finance Email ID1" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceEmailID2) && (!Regex.IsMatch(m.FinanceEmailID2, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceEmailID2Error", ErrorMessage = "Please enter valid Finance Email ID2" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.FinanceEmailID3) && (!Regex.IsMatch(m.FinanceEmailID3, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "FinanceEmailID3Error", ErrorMessage = "Please enter valid Finance Email ID3" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.CT2RegisterCode) && (!Regex.IsMatch(m.CT2RegisterCode, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CT2RegisterCodeError", ErrorMessage = "Please enter valid CT2 Register Code" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.CT2RegisterCode) && (m.CT2RegisterCode.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CT2RegisterCodeError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.CT3RegisterCode) && (!Regex.IsMatch(m.CT3RegisterCode, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CT3RegisterCodeError", ErrorMessage = "Please enter valid CT3 Register Code" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.CT3RegisterCode) && (m.CT3RegisterCode.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CT3RegisterCodeError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.MICTRegisterCode) && (!Regex.IsMatch(m.MICTRegisterCode, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MICTError", ErrorMessage = "Please enter valid MICT Register Code" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.MICTRegisterCode) && (m.MICTRegisterCode.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MICTError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.CT4RegisterCode) && (!Regex.IsMatch(m.CT4RegisterCode, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CT4RegisterCodeError", ErrorMessage = "Please enter valid CT4 Register Code" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.CT4RegisterCode) && (m.CT4RegisterCode.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CT4RegisterCodeError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.T2RegisterCode) && (!Regex.IsMatch(m.T2RegisterCode, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "T2RegisterCodeError", ErrorMessage = "Please enter valid T2 Register Code" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.T2RegisterCode) && (m.T2RegisterCode.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "T2RegisterCodeError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.RupeesInWords))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RupeesInWordsError", ErrorMessage = "Please enter Rupees" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RupeesInWords) && (!Regex.IsMatch(m.RupeesInWords, (@"^[a-zA-Z][a-zA-Z ]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RupeesInWordsError", ErrorMessage = "Please enter valid Rupees" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.RupeesInWords) && (m.RupeesInWords.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RupeesInWordsError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.Rupees))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RupeesError", ErrorMessage = "Please enter Rupees" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.Rupees) && (!Regex.IsMatch(m.Rupees, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RupeesError", ErrorMessage = "Please enter valid Rupees" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.Rupees) && (m.Rupees.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "RupeesError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.Name))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameError", ErrorMessage = "Please enter Name" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.Name) && (!Regex.IsMatch(m.Name, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameError", ErrorMessage = "Please enter valid Name" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.Name) && (m.Name.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "NameError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.Designation))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "DesignationError", ErrorMessage = "Please enter s" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.Designation) && (!Regex.IsMatch(m.Designation, (@"^[ A-Za-z0-9]*$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "DesignationError", ErrorMessage = "Please enter valid Designation" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.Designation) && (m.Designation.Length > 70))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "DesignationError", ErrorMessage = "!Only 70 characters are allowed" };
                return false;
            }
            if (string.IsNullOrEmpty(m.CommunicationEmail1))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CommunicationEmail1Error", ErrorMessage = "Please enter Communication Email1" };
                return false;
            }
            else if (!string.IsNullOrEmpty(m.CommunicationEmail1) && (!Regex.IsMatch(m.CommunicationEmail1, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CommunicationEmail1Error", ErrorMessage = "Please enter valid Communication Email1" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.CommunicationEmail2) && (m.CommunicationEmail2 != "undefined") && (!Regex.IsMatch(m.CommunicationEmail2, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CommunicationEmail1Error", ErrorMessage = "Please enter valid Communication Email1" };
                return false;
            }
            if (!string.IsNullOrEmpty(m.CommunicationEmail3) && (m.CommunicationEmail3 != "undefined") && (!Regex.IsMatch(m.CommunicationEmail3, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
            {
                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CommunicationEmail1Error", ErrorMessage = "Please enter valid Communication Email1" };
                return false;
            }
            return true;
        }
    }
}