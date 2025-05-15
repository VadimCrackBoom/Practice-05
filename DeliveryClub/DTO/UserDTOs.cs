namespace DeliveryClub.DTO;

public class UserCreateDTO
{
    public string Name { get; set; }
    public string Role { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}

public class UserReadDTO
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Login { get; set; }
    public string Status { get; set; }
}

public class UserUpdateDTO
{
    public string Name { get; set; }
    public string Role { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }
}