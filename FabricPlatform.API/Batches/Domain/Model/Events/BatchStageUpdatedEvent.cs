using FabricPlatform.API.Shared.Domain.Model.Events;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Model.Events;

public record BatchStageUpdatedEvent(BatchId BatchId, StageName NewStage) : IEvent;