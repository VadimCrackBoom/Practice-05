namespace DeliveryClub.DTO;

public class OrderCreateDTO
{
    public int ClientId { get; set; }
    public int ManagerId { get; set; }
    public string DeliveryAddress { get; set; }
    public string PaymentMethod { get; set; }
    
    public List<OrderItemCreateDTO> OrderItems { get; set; }
}

public class OrderReadDTO
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string DeliveryAddress { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; }
    public int ManagerId { get; set; }
    public string ManagerName { get; set; }
    public List<OrderItemReadDTO> OrderItems { get; set; }
}

public class OrderUpdateDTO
{
    public string DeliveryAddress { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
}