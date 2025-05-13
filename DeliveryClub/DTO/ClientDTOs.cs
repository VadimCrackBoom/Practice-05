namespace DeliveryClub.DTO;

public class ClientCreateDTO
{
    public string Name { get; set; }
    public string ContactInfo { get; set; }
    public string Address { get; set; }
}

public class ClientReadDTO
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string ContactInfo { get; set; }
    public string Address { get; set; }
}