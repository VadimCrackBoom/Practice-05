using System.Security.Claims;
using DeliveryClub.DTO;
using DeliveryClub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DeliveryDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(DeliveryDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDTO>> Login(LoginRequestDTO loginRequest)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Login == loginRequest.Login && u.Password == loginRequest.Password);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid login or password" });
        }

        if (user.Status != "Active")
        {
            return Unauthorized(new { message = "User account is not active" });
        }

        var tokenResponse = _jwtService.GenerateToken(user);
        return Ok(tokenResponse);
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDTO>> Register(RegisterRequestDTO registerRequest)
    {
        try
        {
            // Проверка на существующего пользователя
            if (await _context.Users.AnyAsync(u => u.Login == registerRequest.Login))
            {
                return Conflict(new { message = "User with this login already exists" });
            }

            // Создание нового пользователя
            var newUser = new User
            {
                Name = registerRequest.Name,
                Login = registerRequest.Login,
                Password = registerRequest.Password,
                Role = registerRequest.Role ?? "User", // По умолчанию роль "User"
                Status = "Active"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Генерация токена для нового пользователя
            var tokenResponse = _jwtService.GenerateToken(newUser);
            return CreatedAtAction(nameof(Login), tokenResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error during registration" });
        }
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserReadDTO>> GetProfile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserReadDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Role = user.Role,
            Login = user.Login,
            Status = user.Status
        });
    }
}