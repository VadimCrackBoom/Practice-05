namespace DeliveryClub.DTO;

public class ShiftAssignmentCreateDTO
{
    public int UserId { get; set; }
    public int ShiftId { get; set; }
}

public class ShiftAssignmentReadDTO
{
    public int AssignmentId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int ShiftId { get; set; }
    public DateTime ShiftStartTime { get; set; }
    public DateTime ShiftEndTime { get; set; }
}