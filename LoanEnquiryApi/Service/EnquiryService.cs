using LoanEnquiryApi.Constant;
using LoanEnquiryApi.Entity;
using LoanEnquiryApi.Model.Enquiry;
using Microsoft.EntityFrameworkCore;

namespace LoanEnquiryApi.Service
{
    public class EnquiryService(DataContext dataContext, IConfiguration configuration)
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IConfiguration _configuration = configuration;

        internal List<ListRecommendedBankModel> CreateEnquiry(CreateEnquiryModel model)
        {
            var recommendedBanks = GetRecommendedBanks(model.BankId, model.LoanType, model.PropertyType, model.RateType, model.LoanTenure, model.LoanAmount);

            var lastEnquiryCode = _dataContext.Enquiries.OrderByDescending(e => e.EnquiryCode).FirstOrDefault()?.EnquiryCode ?? 0;

            if (recommendedBanks.Any())
            {
                foreach (var recommendedBank in recommendedBanks)
                {
                    var entity = new EnquiryEntity
                    {
                        Id = Guid.NewGuid(),
                        EnquiryCode = lastEnquiryCode + 1,
                        LoanType = model.LoanType,
                        PropertyType = model.PropertyType,
                        LoanAmount = model.LoanAmount,
                        LoanTenure = model.LoanTenure,
                        RateType = model.RateType,
                        FullName = model.FullName,
                        Email = model.Email,
                        ContactNo = model.ContactNo,
                        Status = EnquiryStatus.Pending,
                        CreatedAt = DateTime.Now,
                        ExistingBankId = model.BankId,
                        BankId = recommendedBank.BankId,
                    };

                    recommendedBank.EnquiryId = entity.Id;
                    recommendedBank.EnquiryCode = entity.EnquiryCode;

                    bool isMessageSent = SampleSendMessageAsync(recommendedBanks);

                    entity.WhatsAppMessageStatus = isMessageSent ? WhatsAppMessageStatus.Summitted : WhatsAppMessageStatus.Fail;

                    _dataContext.Enquiries.Add(entity);
                }
            }
            else
            {
                var entity = new EnquiryEntity
                {
                    Id = Guid.NewGuid(),
                    EnquiryCode = lastEnquiryCode + 1,
                    LoanType = model.LoanType,
                    PropertyType = model.PropertyType,
                    LoanAmount = model.LoanAmount,
                    LoanTenure = model.LoanTenure,
                    RateType = model.RateType,
                    FullName = model.FullName,
                    Email = model.Email,
                    ContactNo = model.ContactNo,
                    Status = EnquiryStatus.Pending,
                    CreatedAt = DateTime.Now,
                    ExistingBankId = model.BankId,
                    BankId = null,
                };

                entity.WhatsAppMessageStatus = WhatsAppMessageStatus.NoBanks;
                _dataContext.Enquiries.Add(entity);
            }

            _dataContext.SaveChanges();
            return recommendedBanks;
        }

        private bool SampleSendMessageAsync(List<ListRecommendedBankModel> recommendedBanks)
        {
            var whatsAppService = new WhatsAppService(_configuration);
            var responseBody = whatsAppService.SendWhatsAppMessageAsync(recommendedBanks);

            return responseBody != null;
        }

        internal void UpdateEnquiry(UpdateEnquiryModel param)
        {
            var entity = _dataContext.Enquiries.Find(param.Id);

            if (entity == null) return;

            entity.Status = param.Status;
            entity.UpdatedAt = DateTime.Now;

            _dataContext.SaveChanges();
        }

        internal bool DeleteEnquiry(Guid id)
        {
            var enquiryEntity = new EnquiryEntity { Id = id };
            _dataContext.Entry(enquiryEntity).State = EntityState.Deleted;
            return _dataContext.SaveChanges() > 0;
        }

        internal List<ListEnquiryModel> GetEnquiries(ListEnquiryParam param)
        {
            var models = _dataContext.Enquiries
                .Where(e => !param.DateFrom.HasValue || param.DateFrom <= e.CreatedAt)
                .Where(e => !param.DateTo.HasValue || param.DateTo >= e.CreatedAt)
                .Where(e => !param.LoanType.HasValue || param.LoanType == e.LoanType)
                .Where(e => !param.PropertyType.HasValue || param.PropertyType == e.PropertyType)
                .Select(e => new ListEnquiryModel
                {
                    Id = e.Id,
                    EnquiryNo = "ENQ" + e.EnquiryCode.ToString("D10"),
                    LoanType = e.LoanType,
                    LoanTypeName = e.LoanType.ToString(),
                    PropertyType = e.PropertyType,
                    PropertyTypeName = e.PropertyType.ToString(),
                    LoanAmount = e.LoanAmount,
                    LoanTenure = e.LoanTenure,
                    RateType = e.RateType,
                    RateTypeName = e.RateType.ToString(),
                    FullName = e.FullName,
                    Email = e.Email,
                    ContactNo = e.ContactNo,
                    Status = e.Status,
                    StatusName = e.Status.ToString(),
                    CreatedDate = e.CreatedAt,
                })
                .ToList();

            return models;
        }

        internal ViewEnquiryModel GetEnquiry(Guid id)
        {
            var entity = _dataContext.Enquiries
                .Include(e => e.Bank)
                .SingleOrDefault(e => e.Id == id);

            if (entity == null) return null;

            return new ViewEnquiryModel
            {
                Id = entity.Id,
                EnquiryNo = "ENQ" + entity.EnquiryCode.ToString("D10"),
                LoanType = entity.LoanType,
                LoanTypeName = entity.LoanType.ToString(),
                PropertyType = entity.PropertyType,
                PropertyTypeName = entity.PropertyType.ToString(),
                LoanAmount = entity.LoanAmount,
                LoanTenure = entity.LoanTenure,
                RateType = entity.RateType,
                RateTypeName = entity.RateType.ToString(),
                FullName = entity.FullName,
                Email = entity.Email,
                ContactNo = entity.ContactNo,
                Status = entity.Status,
                StatusName = entity.Status.ToString(),
                WhatsAppMessageStatus = entity.WhatsAppMessageStatus,
                WhatsAppMessageStatusName = entity.WhatsAppMessageStatus.ToString(),
                CreatedDate = entity.CreatedAt,
                Banks = new List<ViewEnquiryBankModel>
                {
                    new ViewEnquiryBankModel
                    {
                        BankName = entity.Bank?.Name ?? string.Empty,
                        BankLogo = entity.Bank?.Logo,
                    }
                }
            };
        }

        private List<ListRecommendedBankModel> GetRecommendedBanks(Guid? existingBankId, LoanType loanType, PropertyType propertyType, RateType rateType, int LoanTenure, decimal LoanAmount)
        {
            var bankRates = _dataContext.BankRates
                .Include(b => b.Bank)
                .Where(b => !existingBankId.HasValue || b.BankId != existingBankId)
                .Where(b => b.LoanType == loanType)
                .Where(b => b.PropertyType == propertyType)
                .Where(b => b.MinLoanAmount <= LoanAmount)
                .Where(b => rateType == RateType.Both || b.RateType == rateType)
                .Where(b => b.Year == 1)
                .OrderBy(b => b.InterestRate)
                .Take(3);

            var models = new List<ListRecommendedBankModel>();

            foreach (var bankRate in bankRates)
            {
                models.Add(new ListRecommendedBankModel
                {
                    BankId = bankRate.BankId,
                    BankName = bankRate.Bank.Name,
                    BankLogo = bankRate.Bank.Logo,
                    RateTypeName = bankRate.RateType.ToString(),
                    LockIn = bankRate.LockIn,
                    InterestRate = bankRate.InterestRate,
                    MonthlyInstallment = Math.Round(bankRate.InterestRate / 100 / 12 * LoanAmount, 0),
                    ContactPersonName = bankRate.Bank.ContactPersonName,
                    ContactEmail = bankRate.Bank.ContactEmail,
                    ContactNo = bankRate.Bank.ContactNo,
                });
            }
            return models;
        }
    }
}
