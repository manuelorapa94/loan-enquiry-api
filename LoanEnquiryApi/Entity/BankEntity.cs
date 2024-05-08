using System.ComponentModel.DataAnnotations;

namespace LoanEnquiryApi.Entity
{
    public class BankEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNo { get; set; }
        public string? Logo { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<BankRateEntity> BankRates { get; set; }
    }
}
