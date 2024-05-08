using LoanEnquiryApi.Constant;

namespace LoanEnquiryApi.Model.Enquiry
{
    public class CreateEnquiryModel
    {
        public LoanType LoanType { get; set; }
        public PropertyType PropertyType { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTenure { get; set; }
        public RateType RateType { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public Guid? BankId { get; set; }
    }
}
