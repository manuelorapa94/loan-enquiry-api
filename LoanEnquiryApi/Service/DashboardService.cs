using LoanEnquiryApi.Constant;
using LoanEnquiryApi.Model.Dashboard;
using System.Globalization;

namespace LoanEnquiryApi.Service
{
    public class DashboardService(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;

        internal DashboardModel GetNumberOfEnquiry(DashboardParam param, LoanType? loanType = null, PropertyType? propertyType = null)
        {
            var dateRange = DashboardHelper.GetDateRange(param);

            var currentEnquiryCount = _dataContext.Enquiries
                .Where(e => e.CreatedAt >= dateRange.DateFrom && e.CreatedAt <= dateRange.DateTo)
                .Where(e => !loanType.HasValue || e.LoanType == loanType)
                .Where(e => !propertyType.HasValue || e.PropertyType == propertyType)
                .Count();

            var previousEnquiryCount = _dataContext.Enquiries
                .Where(e => e.CreatedAt >= dateRange.PreviousDateFrom && e.CreatedAt <= dateRange.PreviousDateTo)
                .Where(e => !loanType.HasValue || e.LoanType == loanType)
                .Where(e => !propertyType.HasValue || e.PropertyType == propertyType)
                .Count();

            var growthRate = decimal.Zero;

            if (previousEnquiryCount == 0)
            {
                growthRate = currentEnquiryCount * 100;
            }
            else
            {
                growthRate = (currentEnquiryCount - previousEnquiryCount) / (decimal)previousEnquiryCount * 100;
            }

            return new DashboardModel
            {
                Count = currentEnquiryCount,
                GrowthRate = Math.Round(growthRate, 0),
            };
        }

        internal List<DashboardDetailModel> GetNumberOfEnquiryDetail(DashboardDetailParam param, LoanType? loanType = null, PropertyType? propertyType = null)
        {
            var dateRange = DashboardHelper.GetDateRange(param);

            var models = new List<DashboardDetailModel>();

            var enquiries = _dataContext.Enquiries
                .Where(e => e.CreatedAt >= dateRange.DateFrom && e.CreatedAt <= dateRange.DateTo)
                .Where(e => !loanType.HasValue || e.LoanType == loanType)
                .Where(e => !propertyType.HasValue || e.PropertyType == propertyType)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => e.CreatedAt)
                .ToList();

            if (param.DashboardDetailType == DashboardDetailType.Day)
            {
                var enquiryDates = enquiries.Select(e => e.Date).Distinct().OrderByDescending(e => e.Date);

                foreach (var enquiryDate in enquiryDates)
                {
                    models.Add(new DashboardDetailModel
                    {
                        Description = $"{enquiryDate:ddd}",
                        Count = enquiries.Where(e => e.Date == enquiryDate).Count()
                    });
                }

                return models;
            }

            if (param.DashboardDetailType == DashboardDetailType.Week)
            {
                var weekNumbers = enquiries.Select(e => GetWeekNumber(e)).Distinct().OrderByDescending(e => e);

                foreach (var weekNumber in weekNumbers)
                {
                    models.Add(new DashboardDetailModel
                    {
                        Description = $"Week {weekNumber}",
                        Count = enquiries.Where(e => GetWeekNumber(e.Date) == weekNumber).Count()
                    });
                }
            }

            if (param.DashboardDetailType == DashboardDetailType.Month)
            {
                var months = enquiries.Select(e => e.Month).Distinct().OrderByDescending(e => e);

                foreach (var month in months)
                {
                    models.Add(new DashboardDetailModel
                    {
                        Description = $"Week {CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month)}",
                        Count = enquiries.Where(e => e.Month == month).Count()
                    });
                }
            }

            if (param.DashboardDetailType == DashboardDetailType.Year)
            {
                var years = enquiries.Select(e => e.Year).Distinct().OrderByDescending(e => e);

                foreach (var year in years)
                {
                    models.Add(new DashboardDetailModel
                    {
                        Description = $"Year {year}",
                        Count = enquiries.Where(e => e.Year == year).Count()
                    });
                }
            }

            return models;
        }

        private Func<DateTime, int> GetWeekNumber = date => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
    }
}
