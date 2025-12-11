using Kashi_SmartBudget.Models.DTOs.Account;
using Kashi_SmartBudget.Services.Accountse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kashi_SmartBudget.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _svc;

        // NEED MORE ATTENTION 
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";

        public AccountsController(IAccountService svc)
        {
            _svc = svc;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
        {
            if (ModelState.IsValid)
            {
                var acc = await _svc.CreateAsync(UserId, dto);
                return CreatedAtAction("GetById", new { Id = acc.Id }, acc);

            }
            return BadRequest(ModelState);

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
            var acc = await _svc.GetByIdAsync(UserId, id);
            if (acc == null) return NotFound();
            return Ok(acc);
        }

        [HttpGet("{id:guid} /balance")]
        public async Task<IActionResult> GetBalance(Guid id)
        {
            var bal = await _svc.GetByIdAsync(UserId, id);
            if (bal == null) return NotFound();
            return Ok(bal.Balance);
            // return Ok(new { Balance = bal.Value });

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountDto dto)
        {
            if (ModelState.IsValid)
            {
                var acc = await _svc.UpdateAsync(UserId, id, dto);
                if (acc == false) return NotFound();
                return NoContent();

            }
            return BadRequest(ModelState);

        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var acc = await _svc.DeleteAsync(UserId, id);
                if (acc == false) return NotFound("Not found or balance not zero.");
                return NoContent();

            }
            return BadRequest(ModelState);

        }


    }
}
