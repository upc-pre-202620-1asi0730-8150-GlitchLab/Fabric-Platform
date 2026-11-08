using System.ComponentModel.DataAnnotations;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

public record ReportMachineDowntimeResource(
    [MaxLength(120, ErrorMessage = "The downtime reason must not exceed 120 characters.")]
    string Reason,
    DateTime? StartTime);
