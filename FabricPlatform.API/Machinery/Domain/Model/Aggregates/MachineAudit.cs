using FabricPlatform.API.Shared.Domain.Model.Entities;

namespace FabricPlatform.API.Machinery.Domain.Model.Aggregates;

public partial class Machine : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
