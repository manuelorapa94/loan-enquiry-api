using LoanEnquiryApi.Constant;

namespace LoanEnquiryApi.Model.Enquiry
{
    public class ViewEnquiryModel
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
        public WhatsAppMessageStatus WhatsAppMessageStatus { get; set; }
        public string WhatsAppMessageStatusName { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ViewEnquiryBankModel> Banks { get; set; }
    }

    public class ViewEnquiryBankModel
    {
        public string BankName { get; set; }
        public string? BankLogo { get; set; }
    }
}
