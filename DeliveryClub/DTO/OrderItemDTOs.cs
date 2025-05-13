namespace DeliveryClub.DTO;

public class OrderItemCreateDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderItemReadDTO
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => ProductPrice * Quantity;
}