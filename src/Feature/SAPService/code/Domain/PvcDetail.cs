namespace SapPiService.Domain
{
    public class PvcDetail : AccountBase
    {
        //Billing Details
        public string AmountPayable { get; set; }
        public string CurrentBillAmount { get; set; }
        public string BillDateDue { get; set; }
        public string DpcApplicableAfterDueDate { get; set; }

        //Incentive You Earned
        public decimal PfIncentive { get; set; }
        public decimal LoadFactorIncentive { get; set; }
        public decimal PaymentModeIncentive { get; set; }
        public decimal TotalIncentive { get; set; }

        //Penalties
        public decimal ExceedingCdPenalty { get; set; }
        public decimal PfPenalty { get; set; }
        public decimal DpcOnPreviousBill { get; set; }
        public decimal TotalPenalty { get; set; }

        //More about your bill
        public decimal PowerFactor { get; set; }
        public decimal LoadFactor { get; set; }
        public decimal UnitsConsumed { get; set; }

        //Services you use
        public string Ecs { get; set; }
        public decimal EcsMandateAmount { get; set; }

        //Demand (KVA)
        public string ContractDemand { get; set; }
        public string MaximumDemand { get; set; }
        public string MaximumDemandExceedingCd { get; set; }

        //Time of day units
        public decimal Units09To12 { get; set; }
        public decimal Amount09To12 { get; set; }
        public decimal Units18To22 { get; set; }
        public decimal Amount18To22 { get; set; }
        public decimal Units22To06 { get; set; }
        public decimal Amount22To06 { get; set; }
        public decimal TotalUnits { get; set; }

        //Discount
        public decimal CurrentPromptPaymentDiscount { get; set; }
        public string LastDateOfPpd { get; set; }
        public decimal LastMonthAvailed { get; set; }
    }
}