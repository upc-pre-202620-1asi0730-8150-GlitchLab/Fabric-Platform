using FabricPlatform.API.Shared.Domain.Model.Entities;

namespace FabricPlatform.API.Machinery.Domain.Model.Entities;

public partial class MaintenanceRecord : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
