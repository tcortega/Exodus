using Exodus.Api.Models;
using NodaTime;

namespace Exodus.Api.Persistence.Entities;

/// <summary>
/// This entity represents an organized occasion or activity that people can attend, often requiring a booked ticket.
/// </summary>
public class Event : BaseEntity, IEventModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Location { get; set; }
    public required decimal Price { get; set; }
    
    /// <summary>
    /// The maximum number of tickets that can be booked for this event.
    /// </summary>
    public required int Capacity { get; set; }
    
    public required Instant Date { get; set; }

    /// <summary>
    /// The number of tickets that have already been booked for this event.
    /// </summary>
    public int TicketsBooked { get; set; }
    
    /// <summary>
    /// The early bird discount threshold for this event. If the booking is made
    /// before the event date minus this days threshold, the discount will be applied.
    /// </summary>
    public int? DiscountThreshold { get; set; }
    
    /// <summary>
    /// The early bird discount percentage for this event. Represented as a decimal
    /// between 0.00 and 1.00.
    /// </summary>
    public decimal? DiscountPercentage { get; set; }
    
    public List<Booking> Bookings { get; set; } = null!;
}