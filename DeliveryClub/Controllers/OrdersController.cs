using DeliveryClub.DTO;
using DeliveryClub.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly DeliveryDbContext _context;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(DeliveryDbContext context, ILogger<OrdersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderReadDTO>>> GetOrders()
    {
        try
        {
            var orders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Manager)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Select(o => new OrderReadDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    DeliveryAddress = o.DeliveryAddress,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    ClientId = o.ClientId,
                    ClientName = o.Client.Name,
                    ManagerId = o.ManagerId,
                    ManagerName = o.Manager.Name,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemReadDTO
                    {
                        OrderItemId = oi.OrderItemId,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        ProductPrice = oi.Product.Price,
                        Quantity = oi.Quantity
                    }).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders");
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderReadDTO>> GetOrder(int id)
    {
        try
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Manager)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return Ok(new OrderReadDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                DeliveryAddress = order.DeliveryAddress,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                ClientId = order.ClientId,
                ClientName = order.Client.Name,
                ManagerId = order.ManagerId,
                ManagerName = order.Manager.Name,
                OrderItems = order.OrderItems.Select(oi => new OrderItemReadDTO
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    ProductPrice = oi.Product.Price,
                    Quantity = oi.Quantity
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<OrderReadDTO>> CreateOrder(OrderCreateDTO orderDto)
    {
        try
        {
            var order = new Order
            {
                ClientId = orderDto.ClientId,
                ManagerId = orderDto.ManagerId,
                OrderDate = DateTime.UtcNow,
                DeliveryAddress = orderDto.DeliveryAddress,
                PaymentMethod = orderDto.PaymentMethod,
                Status = "New"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Добавляем элементы заказа
            foreach (var itemDto in orderDto.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, await GetOrderDto(order.OrderId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return StatusCode(500);
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
    {
        try
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating status for order with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting order with ID {id}");
            return StatusCode(500);
        }
    }

    private async Task<OrderReadDTO> GetOrderDto(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Manager)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        return new OrderReadDTO
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            DeliveryAddress = order.DeliveryAddress,
            PaymentMethod = order.PaymentMethod,
            Status = order.Status,
            ClientId = order.ClientId,
            ClientName = order.Client.Name,
            ManagerId = order.ManagerId,
            ManagerName = order.Manager.Name,
            OrderItems = order.OrderItems.Select(oi => new OrderItemReadDTO
            {
                OrderItemId = oi.OrderItemId,
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                ProductPrice = oi.Product.Price,
                Quantity = oi.Quantity
            }).ToList()
        };
    }
}