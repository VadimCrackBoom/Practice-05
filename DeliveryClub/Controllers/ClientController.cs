using DeliveryClub.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Controllers;

public class ClientController : BaseController<ClientReadDTO>
{
    protected override int GetId(ClientReadDTO item) => item.ClientId;
    protected override void SetId(ClientReadDTO item, int id) => item.ClientId = id;
}