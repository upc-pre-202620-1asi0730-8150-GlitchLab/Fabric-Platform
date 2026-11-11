using FabricPlatform.API.Batches.Domain.Model;
using FabricPlatform.API.Batches.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Transform;

public static class TraceabilitySummaryResourceFromEntityAssembler
{
    public static TraceabilitySummaryResource ToResourceFromEntity(BatchTraceabilitySummary summary)
    {
        return new TraceabilitySummaryResource(
            summary.BatchId,
            summary.ProjectedQuantity,
            summary.CurrentQuantity,
            summary.Difference,
            summary.CurrentStage.Value);
    }
}