namespace Exodus.Api.Features.Events;

public sealed class EventsGroup : Group
{
    public EventsGroup()
    {
        Configure("events", ep => ep.Description(b => b.Produces(401)));
    }
}