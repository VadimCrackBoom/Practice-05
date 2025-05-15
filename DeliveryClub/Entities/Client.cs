namespace DeliveryClub.Entities;

public class Client
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string ContactInfo { get; set; }
    public string Address { get; set; }
    
    public ICollection<Order> Orders { get; set; }
}