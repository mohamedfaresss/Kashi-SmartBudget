using Kashi_SmartBudget.Models;
using Kashi_SmartBudget.Services;
using Kashi_SmartBudget.Services.BudgetSe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kashi_SmartBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _svc;
        public BudgetsController(IBudgetService svc) => _svc = svc;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBudgetDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _svc.CreateAsync(UserId, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _svc.GetAllAsync(UserId);
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var b = await _svc.GetByIdAsync(UserId, id);
            if (b == null) return NotFound();
            return Ok(b);
        }

        // GET api/budgets/month/2025/7  -> month=7 year=2025
        [HttpGet("month/{year:int}/{month:int}")]
        public async Task<IActionResult> GetByMonthYear(int year, int month)
        {
            var list = await _svc.GetByMonthYearAsync(UserId, month, year);
            return Ok(list);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBudgetDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ok = await _svc.UpdateAsync(UserId, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _svc.DeleteAsync(UserId, id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
