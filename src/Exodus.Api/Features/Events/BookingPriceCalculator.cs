using Exodus.Api.Models;
using NodaTime;

namespace Exodus.Api.Features.Events;

public static class BookingPriceCalculator
{
    public static decimal CalculatePrice(this IEventModel eventModel, int bookingQuantity, IClock clock)
    {
        var totalPrice = eventModel.Price * bookingQuantity;

        if (IsDiscountApplicable(eventModel, clock))
        {
            return totalPrice * (1 - eventModel.DiscountPercentage!.Value);
        }

        return totalPrice;
    }

    private static bool IsDiscountApplicable(IEventModel eventModel, IClock clock)
    {
        if (eventModel.DiscountThreshold is null || eventModel.DiscountPercentage is null)
        {
            return false;
        }
        
        var durationBeforeEventForDiscount = Duration.FromDays(eventModel.DiscountThreshold.Value);
        var discountDeadline = eventModel.Date.Minus(durationBeforeEventForDiscount);
        
        return clock.GetCurrentInstant() < discountDeadline;
    }
}