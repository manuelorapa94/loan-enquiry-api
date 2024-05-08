using LoanEnquiryApi.Constant;
using System.ComponentModel.DataAnnotations;

namespace LoanEnquiryApi.Entity
{
    public class EnquiryEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int EnquiryCode { get; set; }
        public LoanType LoanType { get; set; }
        public PropertyType PropertyType { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTenure { get; set; }
        public RateType RateType { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public EnquiryStatus Status { get; set; }
        public WhatsAppMessageStatus WhatsAppMessageStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Guid? ExistingBankId { get; set; }
        public BankEntity ExistingBank { get; set; }
        public Guid? BankId { get; set; }
        public BankEntity Bank { get; set; }
    }
}
