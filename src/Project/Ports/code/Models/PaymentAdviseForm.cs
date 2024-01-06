using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sitecore.Ports.Website.Helper;

namespace Sitecore.Ports.Website.Models
{
    public class PaymentAdviseForm
    {

        public Guid Id { get; set; }
        public string Registrationno { set; get; }
        public string CustomerCode { set; get; }
        public string CustomerName { set; get; }
        public string GSTIN { set; get; }
        public string CompanyCode { set; get; }
        public string BankDetail { set; get; }
        public string UTR { set; get; }
        public string InvoiceNumber { get; set; }
        public DateTime RemittanceDate { set; get; }
        public DateTime InvoiceDate { set; get; }
        public string InvoiceAmount { get; set; }
        public string Remarks { get; set; }
        public string TDSAmount { get; set; }
        public string NetPayment { get; set; }
        public string CompanyDetails { get; set; }
        public string PaymentAgainstInvoice { get; set; }
        public string AdvancePayment { get; set; }
        public string InvoiceNumber1 { get; set; }
        //public DateTime RemittanceDate { set; get; }
        public DateTime InvoiceDate1 { set; get; }
        public string InvoiceAmount1 { get; set; }
        public string Remarks1 { get; set; }
        public string TDSAmount1 { get; set; }
        public string NetPayment1 { get; set; }
        public string InvoiceNumber2 { get; set; }
        //public DateTime RemittanceDate { set; get; }
        public DateTime SubmitOnDate { get; set; }
        public DateTime InvoiceDate2 { set; get; }
        public string InvoiceAmount2 { get; set; }
        public string Remarks2 { get; set; }
        public string TDSAmount2 { get; set; }
        public string NetPayment2 { get; set; }
        public string reResponse { get; set; }
        public string NetAmountPaid { get; set; }
        public HttpPostedFileBase customFile
        {
            get;
            set;
        }
    }
}







 