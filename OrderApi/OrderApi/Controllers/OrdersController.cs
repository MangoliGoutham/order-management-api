using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Models;
using System.Security.Claims;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("uid") ?? throw new Exception("User id missing");

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var orders = await _db.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderReadDto
                {
                    Id = o.Id,
                    ProductName = o.ProductName,
                    Quantity = o.Quantity,
                    UnitPrice = o.UnitPrice,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return Ok(orders);
        }

       
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = GetUserId();
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
            if (order == null) return NotFound();

            var dto = new OrderReadDto
            {
                Id = order.Id,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                UnitPrice = order.UnitPrice,
                TotalAmount = order.TotalAmount
            };
            return Ok(dto);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();

            var order = new Order
            {
                ProductName = dto.ProductName,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                TotalAmount = dto.Quantity * dto.UnitPrice,
                UserId = userId
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            var readDto = new OrderReadDto
            {
                Id = order.Id,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                UnitPrice = order.UnitPrice,
                TotalAmount = order.TotalAmount
            };

            return CreatedAtAction(nameof(Get), new { id = order.Id }, readDto);
        }

        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
            if (order == null) return NotFound();

            order.ProductName = dto.ProductName;
            order.Quantity = dto.Quantity;
            order.UnitPrice = dto.UnitPrice;
            order.TotalAmount = dto.Quantity * dto.UnitPrice;

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
            if (order == null) return NotFound();

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
