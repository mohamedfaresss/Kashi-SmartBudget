using Kashi_SmartBudget.Models;
using Kashi_SmartBudget;
using Kashi_SmartBudget.Services.TransactionSe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kashi_SmartBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _svc;

        public TransactionsController(ITransactionService svc)
        {
            _svc = svc;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _svc.CreateAsync(UserId, dto);
            if (result == null) return BadRequest("Invalid account or user.");

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] Guid? accountId,
            [FromQuery] Guid? categoryId,
            [FromQuery] string? type)
        {
            var list = await _svc.GetAllAsync(UserId, from, to, accountId, categoryId, type);
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var trx = await _svc.GetByIdAsync(UserId, id);
            if (trx == null) return NotFound();
            return Ok(trx);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTransactionDto dto)
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
