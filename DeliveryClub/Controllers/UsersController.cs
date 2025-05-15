using DeliveryClub.DTO;
using DeliveryClub.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly DeliveryDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(DeliveryDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetUsers()
    {
        try
        {
            var users = await _context.Users
                .Select(u => new UserReadDTO
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Role = u.Role,
                    Login = u.Login,
                    Status = u.Status
                })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users");
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserReadDTO>> GetUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(new UserReadDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Role = user.Role,
                Login = user.Login,
                Status = user.Status
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting user with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<UserReadDTO>> CreateUser(UserCreateDTO userDto)
    {
        try
        {
            var user = new User
            {
                Name = userDto.Name,
                Role = userDto.Role,
                Login = userDto.Login,
                Password = userDto.Password,
                Status = "Active"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUser = new UserReadDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Role = user.Role,
                Login = user.Login,
                Status = user.Status
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserUpdateDTO userDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Name = userDto.Name;
            user.Role = userDto.Role;
            user.Login = userDto.Login;
            user.Password = userDto.Password;
            user.Status = userDto.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating user with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user with ID {id}");
            return StatusCode(500);
        }
    }
}