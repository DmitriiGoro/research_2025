using Microsoft.EntityFrameworkCore;

namespace ThomasonAlgorithm.Models;

public class AppDbContext : DbContext
{
    public DbSet<Experiment> Experiments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Experiment>()
            .HasKey(e => e.Id);
    } 
}