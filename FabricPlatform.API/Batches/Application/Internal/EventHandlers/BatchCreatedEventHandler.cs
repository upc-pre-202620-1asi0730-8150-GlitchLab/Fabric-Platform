using FabricPlatform.API.Batches.Domain.Model.Events;

namespace FabricPlatform.API.Batches.Application.Internal.EventHandlers;

public class BatchCreatedEventHandler
{
    private readonly ILogger<BatchCreatedEventHandler> _logger;

    public BatchCreatedEventHandler(ILogger<BatchCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(BatchCreatedEvent notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Domain Event Handled: A new batch with ID {BatchId} and Code '{BatchCode}' was successfully created.",
            notification.Batch.Id.Value,
            notification.Batch.Code.Value
        );

        return Task.CompletedTask;
    }
}