using System.Collections.Generic;

namespace SapPiService.Domain
{
    public class InvoiceHistory : AccountBase
    {
        public List<InvoiceLine> InvoiceLines { get; set; }
    }
}