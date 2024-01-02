using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class BankingPartners
    {
        public string Src { get; set; }
        public string ImgAlt { get; set; }
        public string ImgTitle { get; set; }
    }

    public class EmiCalculator
    {
        public EmiCalculatorData EmiCalculatorData { get; set; }
    }

    public class EmiCalculatorData
    {
        public string Heading { get; set; }
        public string Rs { get; set; }
        public string Lakhs { get; set; }
        public string LoanAmountLabel { get; set; }
        public string MinLoanAmountLabel { get; set; }
        public string MaxLoanAmountLabel { get; set; }
        public string MinLoanAmount { get; set; }
        public string MaxLoanAmount { get; set; }
        public string Percent { get; set; }
        public string InterestRateLabel { get; set; }
        public string MinInterestRateLabel { get; set; }
        public string MaxInterestRateLabel { get; set; }
        public string MinInterestRate { get; set; }
        public string MaxInterestRate { get; set; }
        public string LoanTenureLabel { get; set; }
        public string Years { get; set; }
        public string MinLoanTenureLabel { get; set; }
        public string MaxLoanTenureLabel { get; set; }
        public string MinLoanTenure { get; set; }
        public string MaxLoanTenure { get; set; }
        public string InterestAmountLabel { get; set; }
        public string InterestAmountMobileLabel { get; set; }
        public string PrincipalAmountLabel { get; set; }
        public string PrincipalAmountMobileLabel { get; set; }
        public string TotalPayableAmountLabel { get; set; }
        public string TotalPayableAmountMobileLabel { get; set; }
        public string DefaultLoanAmount { get; set; }
        public string DefaultInterestRate { get; set; }
        public string DefaultLoanTenure { get; set; }
        public string OurBankingPartnersLabel { get; set; }
        public List<BankingPartners> bankingPartnersData { get; set; }
    }    
}


