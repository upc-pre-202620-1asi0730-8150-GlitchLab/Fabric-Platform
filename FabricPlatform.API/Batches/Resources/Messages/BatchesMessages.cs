using Microsoft.Extensions.Localization;

namespace FabricPlatform.API.Batches.Resources.Messages;

public class BatchesMessages
{
    private readonly IStringLocalizer<BatchesMessages> _localizer;

    public BatchesMessages(IStringLocalizer<BatchesMessages> localizer)
    {
        _localizer = localizer;
    }

    public string BatchCreatedSuccessfully(string code) => 
        _localizer["BatchCreatedSuccessfully", code];

    public string BatchStageUpdatedSuccessfully(string code, string stage) => 
        _localizer["BatchStageUpdatedSuccessfully", code, stage];
}