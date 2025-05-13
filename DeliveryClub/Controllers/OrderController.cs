using DeliveryClub.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : BaseController<OrderReadDTO>
{
    protected override int GetId(OrderReadDTO item) => item.OrderId;
    protected override void SetId(OrderReadDTO item, int id) => item.OrderId = id;

    [HttpGet("client/{clientId}")]
    public ActionResult<IEnumerable<OrderReadDTO>> GetByClient(int clientId)
    {
        var orders = _items.Where(o => o.ClientId == clientId).ToList();
        return Ok(orders);
    }

    [HttpGet("manager/{managerId}")]
    public ActionResult<IEnumerable<OrderReadDTO>> GetByManager(int managerId)
    {
        var orders = _items.Where(o => o.ManagerId == managerId).ToList();
        return Ok(orders);
    }
    
    [HttpPost("create")]
    public ActionResult<OrderReadDTO> CreateOrder(OrderCreateDTO orderDto)
    {
        // Преобразование OrderCreateDTO в OrderReadDTO
        var order = new OrderReadDTO
        {
            OrderId = _nextId++,
            ClientId = orderDto.ClientId,
            ManagerId = orderDto.ManagerId,
            OrderDate = DateTime.Now,
            DeliveryAddress = orderDto.DeliveryAddress,
            PaymentMethod = orderDto.PaymentMethod,
            Status = "New",
            ClientName = "Client Name", // В реальном приложении получаем из БД
            ManagerName = "Manager Name", // В реальном приложении получаем из БД
            OrderItems = new List<OrderItemReadDTO>() // Инициализация пустого списка
        };
        
        _items.Add(order);
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
    }

    [HttpPut("{id}/status")]
    public ActionResult UpdateStatus(int id, [FromBody] string status)
    {
        var order = _items.FirstOrDefault(o => o.OrderId == id);
        if (order == null) return NotFound();

        order.Status = status;
        return NoContent();
    }
}