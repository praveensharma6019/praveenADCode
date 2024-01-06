using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCapital.Website.Models
{
    public class TransactionsModel
    {
        public string FinancedAmount
        {
            get;
            set;
        }

        public List<LastNBounces> LastNBouncesList
        {
            get;
            set;
        }

        public List<LastPaymentsbyCustomer> LastPaymentsbyCustomerList
        {
            get;
            set;
        }

        public string LoanAccountNumber
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public List<NextNInstallments> NextNInstallmentsList
        {
            get;
            set;
        }

        public TransactionsModel()
        {
            this.LastPaymentsbyCustomerList = new List<LastPaymentsbyCustomer>();
            this.NextNInstallmentsList = new List<NextNInstallments>();
            this.LastNBouncesList = new List<LastNBounces>();
        }
    }
}