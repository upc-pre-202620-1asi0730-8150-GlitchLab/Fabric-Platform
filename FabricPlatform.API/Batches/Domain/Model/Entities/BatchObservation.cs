using FabricPlatform.API.Batches.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Model.Entities;

public class BatchObservation
{
    public int Id { get; private set; }
    public BatchId BatchId { get; private set; }
    public string AuthorName { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
    protected BatchObservation() { }

    public BatchObservation(BatchId batchId, string authorName, string category, string content, DateTimeOffset createdAt)
    {
        BatchId = batchId;
        AuthorName = authorName;
        Category = category;
        Content = content;
        CreatedAt = createdAt;
    }
}