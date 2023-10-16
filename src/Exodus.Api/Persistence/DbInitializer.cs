using Bogus;
using Exodus.Api.Persistence.Entities;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Exodus.Api.Persistence;

[RegisterTransient]
public class DbInitializer : IAsyncInitializer
{
    private readonly AppDbContext _context;

    public DbInitializer(AppDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        // We do that as a safe-net since integration tests are going to use the InMemoryDatabase
        if (_context.Database.IsNpgsql())
        {
            await _context.Database.MigrateAsync(cancellationToken);
            await _context.Database.EnsureCreatedAsync(cancellationToken);
        }

        var hasEvents = await _context.Events.AnyAsync(cancellationToken);

        if (!hasEvents)
        {
            var events = DataFactory.CreateEvents();
            _context.Events.AddRange(events);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public static class DataFactory
{
    public static IEnumerable<Event> CreateEvents()
    {
        var currentDate = SystemClock.Instance.GetCurrentInstant();

        var events = new List<Event>
        {
            new Event
            {
                Name = "Electronic Music Festival", Description = "Join us for an electrifying night with top DJs from around the world.", Location = "Central Park Amphitheater, New York", Price = 150,
                Capacity = 5000, Date = currentDate.Plus(Duration.FromDays(10)), DiscountThreshold = 5, DiscountPercentage = 0.15M,
            },
            new Event
            {
                Name = "Magic Show Extravaganza", Description = "Experience a night of wonder and illusions with the world's best magicians.", Location = "Royal Theatre, London", Price = 75,
                Capacity = 700, Date = currentDate.Plus(Duration.FromDays(20)), DiscountThreshold = 7, DiscountPercentage = 0.10M,
            },
            new Event
            {
                Name = "Jazz and Blues Night", Description = "Soothe your soul with the best of jazz and blues music. Featuring live bands.", Location = "Jazz Corner, Chicago", Price = 50,
                Capacity = 400, Date = currentDate.Minus(Duration.FromDays(5)), DiscountThreshold = 3, DiscountPercentage = 0.05M,
            },
            new Event
            {
                Name = "Classical Music Evening", Description = "An evening of timeless classics performed by the city's leading orchestra.", Location = "Symphony Hall, Vienna", Price = 90,
                Capacity = 1000, Date = currentDate.Minus(Duration.FromDays(15)), DiscountThreshold = 4, DiscountPercentage = 0.12M,
            },
        };

        return events;
    }
}