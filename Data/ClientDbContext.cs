using Microsoft.EntityFrameworkCore;
using ClientRegistry.Models;

namespace ClientRegistry.Data;

public class ClientDbContext : DbContext
{
    public ClientDbContext(DbContextOptions<ClientDbContext> options)
        : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
}

