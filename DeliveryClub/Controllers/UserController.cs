using DeliveryClub.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Controllers;

public class UserController : BaseController<UserReadDTO>
{
    private static readonly List<UserReadDTO> _users = new List<UserReadDTO>();

    protected override int GetId(UserReadDTO item) => item.UserId;
    protected override void SetId(UserReadDTO item, int id) => item.UserId = id;

    [HttpPost("register")]
    public ActionResult<UserReadDTO> Register(UserCreateDTO userDto)
    {
        var user = new UserReadDTO
        {
            UserId = _nextId++,
            Name = userDto.Name,
            Role = userDto.Role,
            Login = userDto.Login,
            Status = "Active"
        };
        _users.Add(user);
        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
    }

    [HttpPost("login")]
    public ActionResult<string> Login(UserLoginDTO loginDto)
    {
        var user = _users.FirstOrDefault(u => u.Login == loginDto.Login);
        if (user == null) return Unauthorized("Invalid credentials");
        
        // В реальном приложении здесь должна быть проверка пароля
        return Ok($"User {user.Name} logged in successfully");
    }
}

public class UserLoginDTO
{
    public string Login { get; set; }
    public string Password { get; set; }
}