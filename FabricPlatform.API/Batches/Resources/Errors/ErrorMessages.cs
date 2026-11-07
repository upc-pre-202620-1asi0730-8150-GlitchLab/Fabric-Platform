using Microsoft.Extensions.Localization;

namespace FabricPlatform.API.Batches.Resources.Errors;

public class ErrorMessages
{
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public ErrorMessages(IStringLocalizer<ErrorMessages> localizer)
    {
        _localizer = localizer;
    }

    public string BatchNotFound(int id) => 
        _localizer["BatchNotFound", id];

    public string BatchCodeAlreadyExists(string code) => 
        _localizer["BatchCodeAlreadyExists", code];

    public string InvalidBatchStageTransition(string currentStage, string targetStage) => 
        _localizer["InvalidBatchStageTransition", currentStage, targetStage];
}