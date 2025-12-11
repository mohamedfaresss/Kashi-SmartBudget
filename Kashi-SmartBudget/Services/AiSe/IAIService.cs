using Kashi_SmartBudget.Models.DTOs.AI;

namespace Kashi_SmartBudget.Services.AiSe
{
    public interface IAIService
    {
        Task<AIResultDto> ForecastMonthlyExpenseAsync(string userId, ExpenseForecastDto dto);
        Task<AIResultDto> AnalyzeBudgetRiskAsync(string userId, BudgetRiskDto dto);
        Task<AIResultDto> SummarizeMonthlyAsync( string userId, MonthlySummaryRequestDto dto);
        Task<AIResultDto> ExtractTransactionFromTextAsync(string userId, TextToTransactionDto dto);


    }
}
