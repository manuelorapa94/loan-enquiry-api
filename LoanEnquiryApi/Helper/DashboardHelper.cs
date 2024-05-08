
using LoanEnquiryApi.Constant;
using LoanEnquiryApi.Model.Dashboard;

namespace LoanEnquiryApi.Service
{
    public static class DashboardHelper
    {
        public static DashboardDateRange GetDateRange(DashboardParam param)
        {
            var dateRange = new DashboardDateRange();
            var todayDate = DateTime.Today;

            switch (param.DashboardPeriod)
            {
                case DashboardPeriod.Today:
                    dateRange.DateTo = DateTime.Now;
                    dateRange.DateFrom = todayDate;
                    dateRange.PreviousDateTo = todayDate.AddTicks(-1);
                    dateRange.PreviousDateFrom = dateRange.DateFrom.AddDays(-1);
                    break;
                case DashboardPeriod.ThisWeek:
                    DateTime currentDate = todayDate;
                    dateRange.DateFrom = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                    dateRange.DateTo = dateRange.DateFrom.AddDays(7).AddTicks(-1);
                    dateRange.PreviousDateFrom = dateRange.DateFrom.AddDays(-6);
                    dateRange.PreviousDateTo = dateRange.DateTo.AddDays(-6);
                    break;
                case DashboardPeriod.ThisMonth:
                    dateRange.DateFrom = new DateTime(todayDate.Year, todayDate.Month, 1);
                    dateRange.DateTo = dateRange.DateFrom.AddMonths(1).AddTicks(-1);
                    dateRange.PreviousDateFrom = dateRange.DateFrom.AddMonths(-1);
                    dateRange.PreviousDateTo = dateRange.DateTo.AddMonths(-1);
                    break;
                case DashboardPeriod.ThisYear:
                    dateRange.DateFrom = new DateTime(todayDate.Year, 1, 1);
                    dateRange.DateTo = dateRange.DateFrom.AddYears(1).AddTicks(-1);
                    dateRange.PreviousDateFrom = dateRange.DateFrom.AddYears(-1);
                    dateRange.PreviousDateTo = dateRange.DateTo.AddYears(-1);
                    break;
            }

            return dateRange;
        }

        public static DashboardDateRange GetDateRange(DashboardDetailParam param)
        {
            var dateRange = new DashboardDateRange();
            var todayDate = DateTime.Today;

            switch (param.DashboardDetailType)
            {
                case DashboardDetailType.Day: // 7 days
                    dateRange.DateTo = DateTime.Now;
                    dateRange.DateFrom = todayDate.AddDays(-6);
                    break;
                case DashboardDetailType.Week: // 4 weeks
                    var thisWeekFrom = todayDate.AddDays(-(int)todayDate.DayOfWeek);
                    var thisWeekTo = thisWeekFrom.AddDays(7).AddTicks(-1);

                    dateRange.DateTo = thisWeekTo;
                    dateRange.DateFrom = thisWeekFrom.AddDays(-20);
                    break;
                case DashboardDetailType.Month: // 4 months
                    var thisMonthFrom = new DateTime(todayDate.Year, todayDate.Month, 1);
                    var thisMonthTo = thisMonthFrom.AddMonths(1).AddTicks(-1);

                    dateRange.DateTo = thisMonthTo;
                    dateRange.DateFrom = thisMonthFrom.AddMonths(-3);
                    break;
                case DashboardDetailType.Year: // 4 years
                    var thisYearFrom = new DateTime(todayDate.Year, 1, 1);
                    var thisYearTo = thisYearFrom.AddYears(1).AddTicks(-1);

                    dateRange.DateTo = thisYearTo;
                    dateRange.DateFrom = thisYearFrom.AddYears(-3);
                    break;
            }

            return dateRange;
        }
    }
}
