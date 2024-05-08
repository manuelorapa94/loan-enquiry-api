namespace LoanEnquiryApi.Model.Bank
{
    public class ListBankModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNo { get; set; }
        public string? BankLogo { get; set; }
    }
}
