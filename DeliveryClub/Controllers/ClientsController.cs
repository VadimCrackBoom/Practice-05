using DeliveryClub.DTO;
using DeliveryClub.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly DeliveryDbContext _context;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(DeliveryDbContext context, ILogger<ClientsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientReadDTO>>> GetClients()
    {
        try
        {
            var clients = await _context.Clients
                .Select(c => new ClientReadDTO
                {
                    ClientId = c.ClientId,
                    Name = c.Name,
                    ContactInfo = c.ContactInfo,
                    Address = c.Address
                })
                .ToListAsync();

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting clients");
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientReadDTO>> GetClient(int id)
    {
        try
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            return Ok(new ClientReadDTO
            {
                ClientId = client.ClientId,
                Name = client.Name,
                ContactInfo = client.ContactInfo,
                Address = client.Address
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting client with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ClientReadDTO>> CreateClient(ClientCreateDTO clientDto)
    {
        try
        {
            var client = new Client
            {
                Name = clientDto.Name,
                ContactInfo = clientDto.ContactInfo,
                Address = clientDto.Address
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var createdClient = new ClientReadDTO
            {
                ClientId = client.ClientId,
                Name = client.Name,
                ContactInfo = client.ContactInfo,
                Address = client.Address
            };

            return CreatedAtAction(nameof(GetClient), new { id = client.ClientId }, createdClient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating client");
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, ClientCreateDTO clientDto)
    {
        try
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            client.Name = clientDto.Name;
            client.ContactInfo = clientDto.ContactInfo;
            client.Address = clientDto.Address;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating client with ID {id}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting client with ID {id}");
            return StatusCode(500);
        }
    }
}