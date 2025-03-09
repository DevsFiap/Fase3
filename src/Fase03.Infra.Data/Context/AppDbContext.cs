using Microsoft.EntityFrameworkCore;
using Fase03.Domain.Entities;

namespace Fase03.Infra.Data.Context;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public AppDbContext()
    {}

    public DbSet<Contato> Contato { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}