using Kashi_SmartBudget.Models.DTOs.Reports;

namespace Kashi_SmartBudget.Services.Reports
{
    public interface IReportService
    {
        Task<MonthlyReportDto> GetMonthlyReportAsync(string userId, int year, int month);
        Task<List<DailySpendingDto>> GetDailySpendingAsync(string userId, int year, int month);
        Task<YearlyReportDto> GetYearlyReportAsync(string userId, int year);


    }
}
