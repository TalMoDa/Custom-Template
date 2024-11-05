using Microsoft.EntityFrameworkCore;
using My.Custom.Template.Data.Entities.EF;

namespace My.Custom.Template.Data;

public class CustomDbContext : DbContext
{
    
    public virtual DbSet<User> Users { get; set; }
    
    public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired();

        });
        
        base.OnModelCreating(modelBuilder);
    }
    
    
    
}