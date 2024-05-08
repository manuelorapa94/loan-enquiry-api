using LoanEnquiryApi.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanEnquiryApi.Entity
{
    public class BankRateEntity
    {
        [Key]
        public Guid Id { get; set; }
        public LoanType LoanType { get; set; }
        public PropertyType PropertyType { get; set; }
        public RateType RateType { get; set; }
        public int MinLoanAmount { get; set; }
        public decimal? LockIn { get; set; }
        public int Year { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal InterestRate { get; set; }
        public decimal MonthlyInstallment { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Guid BankId { get; set; }
        public BankEntity Bank { get; set; }
    }
}
