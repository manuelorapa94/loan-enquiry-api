namespace LoanEnquiryApi.Model.Enquiry
{
    public class ListRecommendedBankModel
    {
        public Guid BankId { get; set; }
        public string BankName { get; set; }
        public string? BankLogo { get; set; }
        public string RateTypeName { get; set; }
        public decimal? LockIn { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNo { get; set; }
        public Guid EnquiryId { get; set; }
        public int EnquiryCode { get; set; }
    }
}
