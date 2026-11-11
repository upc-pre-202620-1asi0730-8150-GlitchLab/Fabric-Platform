using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

public class BatchesEventInterceptor : SaveChangesInterceptor
{
    private readonly Func<IEnumerable<object>, CancellationToken, Task> _dispatchEventsAsync;

    // Inyectamos un delegado o un servicio propio en lugar de IMediator
    public BatchesEventInterceptor(Func<IEnumerable<object>, CancellationToken, Task> dispatchEventsAsync)
    {
        _dispatchEventsAsync = dispatchEventsAsync;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        // 1. Buscamos todas las entidades que heredan de AggregateRoot y tienen eventos pendientes
        var aggregateRoots = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        // 2. Extraemos todos los eventos acumulados
        var domainEvents = aggregateRoots
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // 3. Limpiamos los eventos del agregado para evitar despachos duplicados
        aggregateRoots.ForEach(e => e.ClearDomainEvents());

        // 4. Si hay eventos y un mecanismo de despacho configurado, los publicamos
        if (domainEvents.Any() && _dispatchEventsAsync is not null)
        {
            await _dispatchEventsAsync(domainEvents, cancellationToken);
        }
    }
}