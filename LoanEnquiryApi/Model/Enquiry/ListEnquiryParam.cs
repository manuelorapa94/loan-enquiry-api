using LoanEnquiryApi.Constant;

namespace LoanEnquiryApi.Model.Enquiry
{
    public class ListEnquiryParam
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public LoanType? LoanType { get; set; }
        public PropertyType? PropertyType { get; set; }
    }
}
