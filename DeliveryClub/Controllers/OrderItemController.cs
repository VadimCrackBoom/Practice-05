using DeliveryClub.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Controllers;

public class OrderItemController : BaseController<OrderItemReadDTO>
{
    protected override int GetId(OrderItemReadDTO item) => item.OrderItemId;
    protected override void SetId(OrderItemReadDTO item, int id) => item.OrderItemId = id;

    [HttpGet("order/{orderId}")]
    public ActionResult<IEnumerable<OrderItemReadDTO>> GetByOrder(int orderId)
    {
        var items = _items.Where(i => i.OrderItemId == orderId).ToList();
        return Ok(items);
    }
}