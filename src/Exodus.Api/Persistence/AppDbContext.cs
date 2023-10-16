using System.Reflection;
using Exodus.Api.Persistence.Entities;
using Exodus.Api.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Exodus.Api.Persistence;

public class AppDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) 
        : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    
    public required DbSet<User> Users { get; set; }
    public required DbSet<Event> Events { get; set; }
    public required DbSet<Booking> Bookings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}