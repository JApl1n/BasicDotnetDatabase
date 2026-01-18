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

    public ClientsController(ClientDbContext context)
    {
        _context = context;
    }

    [HttpGet]  // Read client data
    public async Task<IEnumerable<Client>> GetAll()
        => await _context.Clients.ToListAsync();

    [HttpGet("{id}")]  // Read id
    public async Task<ActionResult<Client>> Get(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        return client == null ? NotFound() : client;
    }

    [HttpPost]  // Create client data
    public async Task<ActionResult<Client>> Create(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
    }

    [HttpPut("{id}")]  // Update
    public async Task<IActionResult> Update(int id, Client updated)
    {
        if (id != updated.Id) return BadRequest();

        _context.Entry(updated).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]  // Remove
    public async Task<IActionResult> Delete(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

