using NodaTime;

namespace Exodus.Api.Models;

public interface IEventModel
{
    decimal Price { get; }
    Instant Date { get; }
    int? DiscountThreshold { get; }
    decimal? DiscountPercentage { get; }
}