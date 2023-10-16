using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Exodus.Api.Features.Events;

public record GetAvailableEventsQuery : IRequest<List<GetAvailableEventsResponse>>;

public sealed class GetAvailableEventsEndpoint : Endpoint<GetAvailableEventsQuery, List<GetAvailableEventsResponse>>
{
    private readonly ISender _mediator;

    public GetAvailableEventsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/available");
        Group<EventsGroup>();
    }

    public override async Task HandleAsync(GetAvailableEventsQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetAvailableEventsQueryHandler : IRequestHandler<GetAvailableEventsQuery, List<GetAvailableEventsResponse>>
{
    private readonly AppDbContext _context;
    private readonly IClock _clock;

    public GetAvailableEventsQueryHandler(AppDbContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public Task<List<GetAvailableEventsResponse>> Handle(GetAvailableEventsQuery req, CancellationToken ct)
        => _context.Events.Where(x => x.Capacity > x.TicketsBooked && x.Date > _clock.GetCurrentInstant())
            .Select(x => new GetAvailableEventsResponse(x.Id, x.Name, x.Date, x.Capacity, x.TicketsBooked))
            .ToListAsync(ct);
}

public record GetAvailableEventsResponse(int Id, string Name, Instant Date, int Capacity, int TicketsBooked);