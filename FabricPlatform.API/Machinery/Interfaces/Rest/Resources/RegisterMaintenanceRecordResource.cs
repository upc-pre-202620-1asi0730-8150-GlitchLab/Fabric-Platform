using System.ComponentModel.DataAnnotations;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

public record RegisterMaintenanceRecordResource(
    [MaxLength(80, ErrorMessage = "The technician must not exceed 80 characters.")]
    string Technician,
    string MaintenanceType,
    [MaxLength(240, ErrorMessage = "The description must not exceed 240 characters.")]
    string Description,
    DateTime? PerformedDate);
