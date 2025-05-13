namespace DeliveryClub.Entities;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string DeliveryAddress { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
    
    public int ClientId { get; set; }
    public Client Client { get; set; }
    
    public int ManagerId { get; set; }
    public User Manager { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
}