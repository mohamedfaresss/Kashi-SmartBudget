using Kashi_SmartBudget.Models.DTOs.AI;
using Kashi_SmartBudget.Services.AiSe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kashi_SmartBudget.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IAIService _ai;

        public AIController(IAIService ai)
        {
            _ai = ai;
        }
        private string UserId =>
    User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";


        [HttpGet("forecast/monthly")]
        public async Task<IActionResult> ForecastMonthly([FromQuery] int year, [FromQuery] int month)
        {
            if (year <= 0 || month < 1 || month > 12)
                return BadRequest("Invalid year/month");
            var result =await _ai.ForecastMonthlyExpenseAsync(UserId,new ExpenseForecastDto { Month = month,Year = year});

            return Ok(result);

        }
        [HttpGet("budget-risk")]
        public async Task<IActionResult> AnalyzeBudgetRisk([FromQuery] Guid budgetId)
        {
            if (budgetId == Guid.Empty)
                return BadRequest("Invalid budgetId");

            var result = await _ai.AnalyzeBudgetRiskAsync(
                UserId,
                new BudgetRiskDto { BudgetId = budgetId });

            return Ok(result);
        }

        [HttpGet("summary/monthly")]
        public async Task<IActionResult> MonthlySummary(
    [FromQuery] int year,
    [FromQuery] int month)
        {
            if (year <= 0 || month < 1 || month > 12)
                return BadRequest("Invalid year/month");

            var result = await _ai.SummarizeMonthlyAsync(
                UserId,
                new MonthlySummaryRequestDto
                {
                    Year = year,
                    Month = month
                });

            return Ok(result);
        }
        [HttpPost("parse-text")]
        public async Task<IActionResult> ParseText([FromBody] TextToTransactionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Text))
                return BadRequest("Text is required.");

            var result = await _ai.ExtractTransactionFromTextAsync(UserId, dto);
            return Ok(result);
        }





    }
}
