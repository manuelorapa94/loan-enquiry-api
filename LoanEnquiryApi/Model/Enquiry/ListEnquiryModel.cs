using LoanEnquiryApi.Constant;

namespace LoanEnquiryApi.Model.Enquiry
{
    public class ListEnquiryModel
    {
        public Guid Id { get; set; }
        public string EnquiryNo { get; set; }
        public LoanType LoanType { get; set; }
        public string LoanTypeName { get; set; }
        public PropertyType PropertyType { get; set; }
        public string PropertyTypeName { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTenure { get; set; }
        public RateType RateType { get; set; }
        public string RateTypeName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public EnquiryStatus Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
