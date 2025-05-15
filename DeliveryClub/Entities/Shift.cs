namespace DeliveryClub.Entities;

public class Shift
{
    public int ShiftId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public ICollection<ShiftAssignment> ShiftAssignments { get; set; }
}