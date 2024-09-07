using AnimalServices.Entities;
using Microsoft.EntityFrameworkCore;
using MassTransit;
namespace AnimalServices.DataContext;

public class RabbitDbContext : DbContext
{
    public RabbitDbContext(DbContextOptions context) : base(context)
    {
    }

    public DbSet<Animal> Animals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // add in memory outbox https://masstransit.io/documentation/patterns/in-memory-outbox
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}