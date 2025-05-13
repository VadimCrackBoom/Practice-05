using DeliveryClub.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Controllers;

public class ProductController : BaseController<ProductReadDTO>
{
    protected override int GetId(ProductReadDTO item) => item.ProductId;
    protected override void SetId(ProductReadDTO item, int id) => item.ProductId = id;
}