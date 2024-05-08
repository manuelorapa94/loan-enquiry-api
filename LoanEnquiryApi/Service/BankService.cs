using ClosedXML.Excel;
using LoanEnquiryApi.Constant;
using LoanEnquiryApi.Entity;
using LoanEnquiryApi.Model.Bank;
using Microsoft.EntityFrameworkCore;

namespace LoanEnquiryApi.Service
{
    public class BankService(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;

        internal bool CreateBank(CreateBankModel model)
        {
            var entity = new BankEntity
            {
                Name = model.Name,
                ContactPersonName = model.ContactPersonName,
                ContactEmail = model.ContactEmail,
                ContactNo = model.ContactNo,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _dataContext.Banks.Add(entity);

            return _dataContext.SaveChanges() > 0;
        }

        internal bool UpdateBank(UpdateBankModel model)
        {
            var entity = _dataContext.Banks.Find(model.Id);

            if (entity == null) return false;

            entity.Name = model.Name;
            entity.ContactPersonName = model.ContactPersonName;
            entity.ContactEmail = model.ContactEmail;
            entity.ContactNo = model.ContactNo;
            entity.UpdatedAt = DateTime.Now;

            return _dataContext.SaveChanges() > 0;
        }

        internal bool UpdateBankLogo(UpdateBankLogoModel model)
        {
            var entity = _dataContext.Banks.Find(model.Id);

            if (entity == null) return false;

            using var ms = new MemoryStream();
            model.Logo.CopyTo(ms);
            var fileBytes = ms.ToArray();
            string base64Logo = Convert.ToBase64String(fileBytes);

            entity.Logo = base64Logo;
            entity.UpdatedAt = DateTime.Now;

            return _dataContext.SaveChanges() > 0;
        }

        internal bool DeleteBank(Guid id)
        {
            var entity = _dataContext.Banks.Find(id);

            if (entity == null) return false;

            _dataContext.Banks.Remove(entity);

            return _dataContext.SaveChanges() > 0;
        }

        internal List<ListBankModel> GetBanks()
        {
            return _dataContext.Banks
                .Select(b => new ListBankModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    ContactPersonName = b.ContactPersonName,
                    ContactEmail = b.ContactEmail,
                    ContactNo = b.ContactNo,
                    BankLogo = b.Logo,
                }).ToList();
        }

        internal ViewBankModel GetBank(Guid id)
        {
            var entity = _dataContext.Banks
                .Include(b => b.BankRates)
                .FirstOrDefault(b => b.Id == id);

            if (entity == null) return null;

            var newPurchase = entity.BankRates
                .Where(b => b.LoanType == LoanType.NewPurchase)
                .Select(b => new BankPropertyTypeRate
                {
                    Year = b.Year,
                    PropertyType = b.PropertyType,
                    PropertyTypeName = b.PropertyType.ToString(),
                    RateType = b.RateType,
                    RateTypeName = b.RateType.ToString(),
                    InterestRate = b.InterestRate,
                    MonthlyInstallment = b.InterestRate / 100 / 12 * b.MinLoanAmount
                })
                .OrderBy(b => b.PropertyType).ThenBy(b => b.RateType).ThenBy(b => b.Year)
                .ToList();

            var refinance = entity.BankRates
                .Where(b => b.LoanType == LoanType.Refinance)
                .Select(b => new BankPropertyTypeRate
                {
                    Year = b.Year,
                    PropertyType = b.PropertyType,
                    PropertyTypeName = b.PropertyType.ToString(),
                    RateType = b.RateType,
                    RateTypeName = b.RateType.ToString(),
                    InterestRate = b.InterestRate,
                    MonthlyInstallment = b.InterestRate / 100 / 12 * b.MinLoanAmount
                })
                .OrderBy(b => b.PropertyType).ThenBy(b => b.RateType).ThenBy(b => b.Year)
                .ToList();

            return new ViewBankModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ContactPersonName = entity.ContactPersonName,
                ContactEmail = entity.ContactEmail,
                ContactNo = entity.ContactNo,
                BankLogo = entity.Logo,
                NewPurchaseRates = newPurchase,
                RefinanceRates = refinance,
                UpdatedAt = entity.UpdatedAt,
            };
        }

        internal List<BankDropdownModel> GetBankDropdown()
        {
            return _dataContext.Banks
                .Select(b => new BankDropdownModel
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList();
        }
        internal string? ImportBankRate(ImportBankRateModel model)
        {
            var bankEntities = _dataContext.Banks.ToList();

            var bankRateEntities = GetBankRate(bankEntities, model.NewPurchaseRateFile, LoanType.NewPurchase, out string? errorMessage);
            if (!string.IsNullOrEmpty(errorMessage)) return errorMessage;

            bankRateEntities.AddRange(GetBankRate(bankEntities, model.RefinanceRateFile, LoanType.Refinance, out errorMessage));

            if (!string.IsNullOrEmpty(errorMessage)) return errorMessage;

            _dataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE BankRates");

            _dataContext.BankRates.AddRange(bankRateEntities);
            _dataContext.SaveChanges();

            return null;
        }

        private List<BankRateEntity> GetBankRate(List<BankEntity> bankEntities, IFormFile file, LoanType loanType, out string? errorMessage)
        {
            List<BankRateEntity> bankRateEntities = [];
            errorMessage = null;

            using var workbook = new XLWorkbook(file.OpenReadStream());

            var worksheet = workbook.Worksheet(1);

            int row = 2; // row 1 is header
            while (true)
            {
                var bankName = GetValue<string>(worksheet, row, "A");
                var propertyType = GetValue<string>(worksheet, row, "B");
                var minLoanAmount = GetValue<int>(worksheet, row, "C");
                var rateType = GetValue<string>(worksheet, row, "D");
                var lockIn = GetValue<decimal?>(worksheet, row, "E");
                var year1 = GetValue<decimal>(worksheet, row, "F");
                var year2 = GetValue<decimal>(worksheet, row, "G");
                var year3 = GetValue<decimal>(worksheet, row, "H");
                var year4 = GetValue<decimal>(worksheet, row, "I");
                var year5 = GetValue<decimal>(worksheet, row, "J");

                if (string.IsNullOrEmpty(bankName) || string.IsNullOrEmpty(propertyType) || minLoanAmount < 0 || string.IsNullOrEmpty(rateType) || year1 < 0 ||
                    year2 < 0 || year3 < 0 || year4 < 0 || year5 < 0) break;

                var bankEntity = bankEntities.Where(b => b.Name == bankName).FirstOrDefault();
                if (bankEntity == null)
                {
                    errorMessage = $"Invalid Bank - {bankName} at row {row}";
                    return [];
                }
                var isValidPropertyType = Enum.TryParse<PropertyType>(propertyType, out var _propertyType);
                if (!isValidPropertyType)
                {
                    errorMessage = $"Invalid PropertyType - {propertyType} at row {row}";
                    return [];
                }

                var isValidRateType = Enum.TryParse<RateType>(rateType, out var _rateType);
                if (!isValidRateType)
                {
                    errorMessage = $"Invalid RateType - {rateType} at row {row}";
                    return [];
                }

                //var existingBankRateEntities = _dataContext.BankRates
                //    .Where(b => b.BankId == bankEntity.Id)
                //    .Where(b => b.LoanType == loanType)
                //    .Where(b => b.PropertyType == _propertyType)
                //    .Where(b => b.RateType == _rateType)
                //    .Where(b => b.MinLoanAmount == minLoanAmount)
                //    .ToList();

                //var existingBankRateEntities = new List<BankRateEntity>();

                for (int year = 1; year < 37; year++)
                {
                    //var existingBankRateEntity = existingBankRateEntities.FirstOrDefault(b => b.Year == year);
                    //if (existingBankRateEntity == null)
                    //{
                    BankRateEntity bankRateEntity = new BankRateEntity
                    {
                        BankId = bankEntity.Id,
                        LoanType = loanType,
                        PropertyType = _propertyType,
                        RateType = _rateType,
                        MinLoanAmount = minLoanAmount,
                        LockIn = lockIn,
                        Year = year,
                        InterestRate = GetInterestRate(year, year1, year2, year3, year4, year5),
                        CreatedAt = DateTime.Now,
                    };

                    bankRateEntities.Add(bankRateEntity);
                    //}
                    //else
                    //{
                    //    existingBankRateEntity.InterestRate = GetInterestRate(year, year1, year2, year3, year4, year5);
                    //    existingBankRateEntity.UpdatedAt = DateTime.Now;
                    //}
                }

                row++;
            }

            return bankRateEntities;
        }

        private decimal GetInterestRate(int year, decimal year1, decimal year2, decimal year3, decimal year4, decimal year5)
        {
            var latestRateEntry = _dataContext.SoraRates.OrderByDescending(a => a.CreatedAt)
                                   .FirstOrDefault();

            decimal latestRate = latestRateEntry.Rate;

            if (year == 1) return year1 + latestRate;
            if (year == 2) return year2 + latestRate;
            if (year == 3) return year3 + latestRate;
            if (year == 4) return year4 + latestRate;
            return year5 + latestRate;
        }

        private static T? GetValue<T>(IXLWorksheet worksheet, int row, string cell)
        {
            var isValid = worksheet.Row(row).Cell(cell).TryGetValue<T>(out var value);

            if (!isValid) return default;

            return value;
        }
    }
}
