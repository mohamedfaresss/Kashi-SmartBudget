using Kashi_SmartBudget.Models.DTOs.Reports;
using Kashi_SmartBudget.Services.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kashi_SmartBudget.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        private readonly IReportService _svc;

        public ReportsController(IReportService svc)
        {
            _svc = svc;
        }
        [HttpGet("monthly")]

        public async Task<ActionResult<MonthlyReportDto>> GetMonthly([FromQuery] int year, [FromQuery] int month)
        {
            if (year <= 0 || month < 1 || month > 12)
                return BadRequest("Invalid year/month");
            var result=await _svc.GetMonthlyReportAsync(UserId,year ,month);
            return Ok(result);



        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily( [FromQuery] int year, [FromQuery] int month)
        {
            if (year <= 0 || month < 1 || month > 12)
                return BadRequest("Invalid year/month");

            var result = await _svc.GetDailySpendingAsync(UserId, year, month);
            return Ok(result);
        }


        [HttpGet("yearly")]
        public async Task<IActionResult> GetYearly([FromQuery] int year)
        {
            if (year <= 0)
                return BadRequest("Invalid year");

            var result = await _svc.GetYearlyReportAsync(UserId, year);
            return Ok(result);
        }


    }
}
