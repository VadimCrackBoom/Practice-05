using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.DTO;

public class OrderItemCreateDTO
{
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class OrderItemUpdateDTO
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class OrderItemReadDTO
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => ProductPrice * Quantity;
}