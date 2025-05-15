using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.DTO;

public class LoginRequestDTO
{
    [Required(ErrorMessage = "Login is required")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}

public class AuthResponseDTO
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserReadDTO User { get; set; }
}

public class RegisterRequestDTO
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Login is required")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; }
}