namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

public class BatchIdValueConverter() : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<Domain.Model.ValueObjects.BatchId, int>(
    batchId  => batchId.Value,
    value => new Domain.Model.ValueObjects.BatchId(value));