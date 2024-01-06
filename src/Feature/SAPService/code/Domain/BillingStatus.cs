namespace SapPiService.Domain
{
    
    public enum BillingStatus
    {
        //Consumer can make the payment of due amount,Amount to be paid till due date 
        Due,
        //Three days grace period after due date to pay the bill with DPC amount. Amount to be paid with Delay Payment
        Overdue,
        //Consumer is not allowed to make the payment  As next month bill is in process.
        Hold,
        NoInvoice
    }
}