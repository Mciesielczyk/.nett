using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace System_Zarz.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                })
                .ToListAsync();

            return Ok(orders);
        }

        public class OrderDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}