using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.Entities;

public class User
{
    public int UserId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Login { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Role { get; set; }
    
    [Required]
    public string Status { get; set; }
    
    public ICollection<ShiftAssignment> ShiftAssignments { get; set; }
    public ICollection<Order> Orders { get; set; }
    
}