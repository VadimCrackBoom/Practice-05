namespace DeliveryClub.Entities;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }
    
    public ICollection<ShiftAssignment> ShiftAssignments { get; set; }
    public ICollection<Order> Orders { get; set; }
}