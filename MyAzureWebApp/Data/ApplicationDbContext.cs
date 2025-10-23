using Microsoft.EntityFrameworkCore;
using MyAzureWebApp.Models;

namespace MyAzureWebApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");
            entity.HasKey(e => e.ClientID);
            entity.Property(e => e.ClientName).HasMaxLength(255);
        });
    }
}
