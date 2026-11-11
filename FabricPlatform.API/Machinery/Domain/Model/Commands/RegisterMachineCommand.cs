namespace FabricPlatform.API.Machinery.Domain.Model.Commands;

public record RegisterMachineCommand(string Code, string Type, string Location, DateTime? AcquiredDate);
