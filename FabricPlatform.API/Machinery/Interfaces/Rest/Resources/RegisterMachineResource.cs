using System.ComponentModel.DataAnnotations;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

public record RegisterMachineResource(
    [MaxLength(30, ErrorMessage = "The machine code must not exceed 30 characters.")]
    string Code,
    [MaxLength(50, ErrorMessage = "The machine type must not exceed 50 characters.")]
    string Type,
    [MaxLength(100, ErrorMessage = "The machine location must not exceed 100 characters.")]
    string Location,
    DateTime? AcquiredDate);
