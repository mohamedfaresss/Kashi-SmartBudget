using Kashi_SmartBudget.Models;
using Kashi_SmartBudget.Services.CategorySe;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _svc;

        public CategoriesController(ICategoryService svc)
        {
            _svc = svc;
        }
        private string? UserId =>User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var cat= await _svc.CreateAsync(UserId, dto);
            return CreatedAtAction("GetById", new {id=cat.Id}, cat);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _svc.GetAllAsync(UserId);
            return Ok(res);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cat = await _svc.GetByIdAsync(UserId, id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto dto)
        {
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