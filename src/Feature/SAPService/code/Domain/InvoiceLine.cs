namespace SapPiService.Domain
{
    public class InvoiceLine
    {
        public string AccountNumber { get; set; }
        public string BillMonth { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceUrl { get; set; }
        //public string InvoiceUrl => $"https://iss.adanielectricity.com/VAS/ProcessDownloadPDF.jsp?TXTCANO={AccountNumber}&INVOICENO={InvoiceNumber}";
    }
}