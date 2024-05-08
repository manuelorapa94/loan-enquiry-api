using LoanEnquiryApi.Constant;

namespace LoanEnquiryApi.Model.Bank
{
    public class ViewBankModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNo { get; set; }
        public string? BankLogo { get; set; }
        public List<BankPropertyTypeRate> NewPurchaseRates { get; set; }
        public List<BankPropertyTypeRate> RefinanceRates { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BankPropertyTypeRate
    {
        public int Year { get; set; }
        public PropertyType PropertyType { get; set; }
        public string PropertyTypeName { get; set; }
        public RateType RateType { get; set; }
        public string RateTypeName { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MonthlyInstallment { get; set; }

    }
}
