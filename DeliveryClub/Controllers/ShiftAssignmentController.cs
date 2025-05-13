using DeliveryClub.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Controllers;

public class ShiftAssignmentController : BaseController<ShiftAssignmentReadDTO>
{
    protected override int GetId(ShiftAssignmentReadDTO item) => item.AssignmentId;
    protected override void SetId(ShiftAssignmentReadDTO item, int id) => item.AssignmentId = id;

    [HttpGet("user/{userId}")]
    public ActionResult<IEnumerable<ShiftAssignmentReadDTO>> GetByUser(int userId)
    {
        var assignments = _items.Where(a => a.UserId == userId).ToList();
        return Ok(assignments);
    }

    [HttpGet("shift/{shiftId}")]
    public ActionResult<IEnumerable<ShiftAssignmentReadDTO>> GetByShift(int shiftId)
    {
        var assignments = _items.Where(a => a.ShiftId == shiftId).ToList();
        return Ok(assignments);
    }
}