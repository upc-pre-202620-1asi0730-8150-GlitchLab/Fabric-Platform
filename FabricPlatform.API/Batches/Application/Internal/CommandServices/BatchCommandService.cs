using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Domain.Model.Errors;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Batches.Application.CommandServices;
using FabricPlatform.API.Shared.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;

namespace FabricPlatform.API.Batches.Application.Internal.CommandServices;

public class BatchCommandService : ICreateBatchCommandService, IAssignOperatorToBatchCommandService, IUpdateBatchStageCommandService
{
    private readonly IBatchRepository _batchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BatchCommandService(IBatchRepository batchRepository, IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> HandleAsync(CreateBatchCommand command, CancellationToken cancellationToken = default)
    {
        var batchCode = new BatchCode(command.Code);

        var existingBatch = await _batchRepository.FindByCodeAsync(batchCode, cancellationToken);
        if (existingBatch != null)
        {
            return Result<int>.Failure(BatchErrors.CodeAlreadyExists);
        }

        var batch = new Batch(batchCode, command.ProjectedQuantity, command.DeliveryDate);

        await _batchRepository.AddAsync(batch, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<int>.Success(batch.Id.Value);
    }

    public async Task<Result<bool>> HandleAsync(AssignOperatorToBatchCommand command, CancellationToken cancellationToken = default)
    {
        var batchId = new BatchId(command.BatchId);
        var batch = await _batchRepository.FindByIdAsync(batchId, cancellationToken);

        if (batch == null)
        {
            return Result<bool>.Failure(BatchErrors.NotFound);
        }

        var stageName = new StageName(command.Stage);
        batch.AssignOperator(command.OperatorId, command.FullName, command.Specialty, stageName, DateTimeOffset.UtcNow);

        _batchRepository.Update(batch);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
    
    public async Task<Result<bool>> HandleAsync(UpdateBatchStageCommand command, CancellationToken cancellationToken = default)
    {
        var batchId = new BatchId(command.BatchId);
        var batch = await _batchRepository.FindByIdAsync(batchId, cancellationToken);

        if (batch == null)
        {
            return Result<bool>.Failure(BatchErrors.NotFound);
        }
        
        var newStageName = new StageName(command.NewStage);
        batch.UpdateStage(newStageName, command.ProcessedQuantity);
        
        _batchRepository.Update(batch);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}