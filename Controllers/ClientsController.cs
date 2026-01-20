using Microsoft.AspNetCore.Mvc;
using ClientRegistry.Data;
using ClientRegistry.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("clients")]
public class ClientsController : ControllerBase
{
    // Dependency injection
    private readonly ClientDbContext _context;

    // Add logging
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(ClientDbContext context, ILogger<ClientsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]  // Read client data
    public async Task<IEnumerable<Client>> GetAll()
        => await _context.Clients.ToListAsync();

    [HttpGet("{id}")]  // Read id
    public async Task<ActionResult<Client>> Get(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        return client == null ? NotFound("Client with id {id} not found.") : client;
    }

    [HttpPost]  // Create client data
    public async Task<ActionResult<Client>> Create(CreateClientRequest request)
    {
        var client = new Client
        {
            Name = request.Name,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        // Simple validation
        if (client == null)
        {
            _logger.LogWarning("Attempting to create null client");
            return BadRequest();
        }

        if (client.Name.Length > 20) return BadRequest("Name too long");
        if (string.IsNullOrWhiteSpace(client.Email)) return BadRequest("Email is required");

	var emailExists = await _context.Clients.AnyAsync(c => c.Email == client.Email);
        if (emailExists) return Conflict($"Client with email {client.Email} already exists.");

        // Add to clients
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Client created at time {CreatedAt}", client.CreatedAt);
        return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
    }

    [HttpPut("{id}")]  // Update
    public async Task<IActionResult> Update(int id, Client updated)
    {
        if (id != updated.Id) return BadRequest("Id does not match id in edited entry");

        _context.Entry(updated).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]  // Remove
    public async Task<IActionResult> Delete(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound($"Client with id {id} not found.");

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]  // Remove all (restart)
    public async Task<IActionResult> DeleteAll([FromBody] DeleteAllRequest request)
    {
        if (request.Confirmation != "DELETEALL")
        {
            return BadRequest("To delete all entries in the database, the confirmation text must be exactly 'DELETEALL'");
        }

        _context.Clients.RemoveRange(_context.Clients);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    public class CreateClientRequest
    {
        public string Name {get; set;} = "";
        public string Email {get; set;} = "";
    }


    public class DeleteAllRequest
    {
        public string Confirmation {get; set;} = "";
    }

}

