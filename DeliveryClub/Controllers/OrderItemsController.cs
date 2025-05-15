using DeliveryClub.DTO;
using DeliveryClub.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderItemsController : ControllerBase
{
    private readonly DeliveryDbContext _context;
    private readonly ILogger<OrderItemsController> _logger;

    public OrderItemsController(DeliveryDbContext context, ILogger<OrderItemsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<IEnumerable<OrderItemReadDTO>>> GetOrderItemsByOrder(int orderId)
    {
        try
        {
            var orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId)
                .Select(oi => new OrderItemReadDTO
                {
                    OrderItemId = oi.OrderItemId,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    ProductPrice = oi.Product.Price,
                    Quantity = oi.Quantity
                })
                .ToListAsync();

            return Ok(orderItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order items for order ID {orderId}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<OrderItemReadDTO>> CreateOrderItem(OrderItemCreateDTO orderItemDto)
    {
        try
        {
            var orderItem = new OrderItem
            {
                ProductId = orderItemDto.ProductId,
                Quantity = orderItemDto.Quantity
            };

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            var createdItem = await _context.OrderItems
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItem.OrderItemId);

            return CreatedAtAction(nameof(GetOrderItemsByOrder), new { orderId = orderItem.OrderId }, 
                new OrderItemReadDTO
                {
                    OrderItemId = createdItem.OrderItemId,
                    OrderId = createdItem.OrderId,
                    ProductId = createdItem.ProductId,
                    ProductName = createdItem.Product.Name,
                    ProductPrice = createdItem.Product.Price,
                    Quantity = createdItem.Quantity
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order item");
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderItem(int id, OrderItemCreateDTO orderItemDto)
    {
        try
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null) return NotFound();

            orderItem.ProductId = orderItemDto.ProductId;
            orderItem.Quantity = orderItemDto.Quantity;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating order item with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        try
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null) return NotFound();

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting order item with ID {id}");
            return StatusCode(500);
        }
    }
}