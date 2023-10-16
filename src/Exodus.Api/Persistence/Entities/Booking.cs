using NodaTime;

namespace Exodus.Api.Persistence.Entities;

/// <summary>
/// This entity represents a reservation made by a user for a specific event.
/// </summary>
public class Booking : BaseEntity
{
    public required int EventId { get; set; }
    public required Guid UserId { get; set; }
    public required int Quantity { get; set; }
    public required decimal TotalPrice { get; set; }
    public required Instant BookingDate { get; set; }

    public Event Event { get; set; } = null!;
}