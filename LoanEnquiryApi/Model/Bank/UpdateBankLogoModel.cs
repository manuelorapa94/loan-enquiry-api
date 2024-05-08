namespace LoanEnquiryApi.Model.Bank
{
    public class UpdateBankLogoModel
    {
        public Guid Id { get; set; }
        public IFormFile Logo { get; set; }
    }
}
