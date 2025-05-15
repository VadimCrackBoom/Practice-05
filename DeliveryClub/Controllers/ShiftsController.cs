using DeliveryClub.DTO;
using DeliveryClub.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShiftsController : ControllerBase
{
    private readonly DeliveryDbContext _context;
    private readonly ILogger<ShiftsController> _logger;

    public ShiftsController(DeliveryDbContext context, ILogger<ShiftsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShiftReadDTO>>> GetShifts()
    {
        try
        {
            var shifts = await _context.Shifts
                .Select(s => new ShiftReadDTO
                {
                    ShiftId = s.ShiftId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .ToListAsync();

            return Ok(shifts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shifts");
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShiftReadDTO>> GetShift(int id)
    {
        try
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null) return NotFound();

            return Ok(new ShiftReadDTO
            {
                ShiftId = shift.ShiftId,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting shift with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ShiftReadDTO>> CreateShift(ShiftCreateDTO shiftDto)
    {
        try
        {
            var shift = new Shift
            {
                StartTime = shiftDto.StartTime,
                EndTime = shiftDto.EndTime
            };

            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();

            var createdShift = new ShiftReadDTO
            {
                ShiftId = shift.ShiftId,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime
            };

            return CreatedAtAction(nameof(GetShift), new { id = shift.ShiftId }, createdShift);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating shift");
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShift(int id, ShiftCreateDTO shiftDto)
    {
        try
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null) return NotFound();

            shift.StartTime = shiftDto.StartTime;
            shift.EndTime = shiftDto.EndTime;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating shift with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        try
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null) return NotFound();

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting shift with ID {id}");
            return StatusCode(500);
        }
    }
}