using LoanEnquiryApi.Constant;

namespace LoanEnquiryApi.Model.Enquiry
{
    public class UpdateEnquiryModel
    {
        public Guid Id { get; set; }
        public EnquiryStatus Status { get; set; }
    }
}
