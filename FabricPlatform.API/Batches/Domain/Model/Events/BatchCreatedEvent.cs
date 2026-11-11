using FabricPlatform.API.Shared.Domain.Model.Events;
using FabricPlatform.API.Batches.Domain.Model.Aggregates;

namespace FabricPlatform.API.Batches.Domain.Model.Events;

public record BatchCreatedEvent(Batch Batch) : IEvent;