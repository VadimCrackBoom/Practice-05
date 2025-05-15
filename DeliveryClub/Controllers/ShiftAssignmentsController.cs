using DeliveryClub.DTO;
using DeliveryClub.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShiftAssignmentsController : ControllerBase
{
    private readonly DeliveryDbContext _context;
    private readonly ILogger<ShiftAssignmentsController> _logger;

    public ShiftAssignmentsController(DeliveryDbContext context, ILogger<ShiftAssignmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShiftAssignmentReadDTO>>> GetShiftAssignments()
    {
        try
        {
            var assignments = await _context.ShiftAssignments
                .Include(sa => sa.User)
                .Include(sa => sa.Shift)
                .Select(sa => new ShiftAssignmentReadDTO
                {
                    AssignmentId = sa.AssignmentId,
                    UserId = sa.UserId,
                    UserName = sa.User.Name,
                    ShiftId = sa.ShiftId,
                    ShiftStartTime = sa.Shift.StartTime,
                    ShiftEndTime = sa.Shift.EndTime
                })
                .ToListAsync();

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shift assignments");
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShiftAssignmentReadDTO>> GetShiftAssignment(int id)
    {
        try
        {
            var assignment = await _context.ShiftAssignments
                .Include(sa => sa.User)
                .Include(sa => sa.Shift)
                .FirstOrDefaultAsync(sa => sa.AssignmentId == id);

            if (assignment == null) return NotFound();

            return Ok(new ShiftAssignmentReadDTO
            {
                AssignmentId = assignment.AssignmentId,
                UserId = assignment.UserId,
                UserName = assignment.User.Name,
                ShiftId = assignment.ShiftId,
                ShiftStartTime = assignment.Shift.StartTime,
                ShiftEndTime = assignment.Shift.EndTime
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting shift assignment with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ShiftAssignmentReadDTO>> CreateShiftAssignment(ShiftAssignmentCreateDTO assignmentDto)
    {
        try
        {
            var assignment = new ShiftAssignment
            {
                UserId = assignmentDto.UserId,
                ShiftId = assignmentDto.ShiftId
            };

            _context.ShiftAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            var createdAssignment = await _context.ShiftAssignments
                .Include(sa => sa.User)
                .Include(sa => sa.Shift)
                .FirstOrDefaultAsync(sa => sa.AssignmentId == assignment.AssignmentId);

            return CreatedAtAction(nameof(GetShiftAssignment), 
                new { id = assignment.AssignmentId }, 
                new ShiftAssignmentReadDTO
                {
                    AssignmentId = createdAssignment.AssignmentId,
                    UserId = createdAssignment.UserId,
                    UserName = createdAssignment.User.Name,
                    ShiftId = createdAssignment.ShiftId,
                    ShiftStartTime = createdAssignment.Shift.StartTime,
                    ShiftEndTime = createdAssignment.Shift.EndTime
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating shift assignment");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShiftAssignment(int id)
    {
        try
        {
            var assignment = await _context.ShiftAssignments.FindAsync(id);
            if (assignment == null) return NotFound();

            _context.ShiftAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting shift assignment with ID {id}");
            return StatusCode(500);
        }
    }
}