namespace FabricPlatform.API.Quality.Application.Internal.Commands;

public record UpdateFabricInspectionRollStatusCommand(int InspectionId, string RollStatus);
