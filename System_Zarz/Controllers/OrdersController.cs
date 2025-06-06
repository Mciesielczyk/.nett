using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System_Zarz.Mappers;
using System_Zarz.DTOs;

namespace System_Zarz.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderMapper _mapper;
        public OrdersController(ApplicationDbContext context, OrderMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .ToListAsync();
            var orderDtos = orders.Select(o => _mapper.ToDto(o));
            return Ok(orderDtos);
        }
    }
}