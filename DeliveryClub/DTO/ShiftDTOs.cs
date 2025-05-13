namespace DeliveryClub.DTO;

public class ShiftCreateDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class ShiftReadDTO
{
    public int ShiftId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}