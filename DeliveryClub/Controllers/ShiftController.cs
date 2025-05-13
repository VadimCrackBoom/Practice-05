using DeliveryClub.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Controllers;

public class ShiftController : BaseController<ShiftReadDTO>
{
    protected override int GetId(ShiftReadDTO item) => item.ShiftId;
    protected override void SetId(ShiftReadDTO item, int id) => item.ShiftId = id;
}