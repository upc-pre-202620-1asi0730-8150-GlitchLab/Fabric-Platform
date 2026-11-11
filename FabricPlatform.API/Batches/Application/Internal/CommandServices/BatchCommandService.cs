using FabricPlatform.API.Batches.Application.CommandServices;
using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Domain.Model.Errors;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Repositories;

namespace FabricPlatform.API.Batches.Application.Internal.CommandServices;

public class BatchCommandService : ICreateBatchCommandService, IAssignOperatorToBatchCommandService,
    IUpdateBatchStageCommandService, IBatchOperationsCommandService
{
    private readonly IBatchRepository _batchRepository;
    private readonly IProductionOperatorRepository _operatorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BatchCommandService(
        IBatchRepository batchRepository,
        IProductionOperatorRepository operatorRepository,
        IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _operatorRepository = operatorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> HandleAsync(CreateBatchCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.GarmentModel) || command.GarmentModel.Length > 200 ||
            command.ProjectedQuantity <= 0 || command.DeliveryDate == default)
        {
            return Result<int>.Failure(BatchErrors.InvalidRequest);
        }

        var batch = new Batch(
            new BatchCode("PENDING"),
            command.GarmentModel,
            command.ProjectedQuantity,
            "Cutting",
            10,
            command.DeliveryDate,
            notes: command.Notes);

        batch.UpdateStage(StageName.Cutting, 10);
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _batchRepository.AddAsync(batch, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            batch.AssignGeneratedCode();
            _batchRepository.Update(batch);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }, cancellationToken);

        return Result<int>.Success(batch.Id.Value);
    }

    public async Task<Result<int>> HandleAsync(AssignOperatorToBatchCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.BatchId <= 0 || string.IsNullOrWhiteSpace(command.FullName) || command.FullName.Length > 150)
        {
            return Result<int>.Failure(BatchErrors.InvalidRequest);
        }

        var batchId = new BatchId(command.BatchId);
        var batch = await _batchRepository.FindByIdAsync(batchId, cancellationToken);

        if (batch == null)
        {
            return Result<int>.Failure(BatchErrors.NotFound);
        }

        var productionOperator = await _operatorRepository.FindByNameAsync(command.FullName, cancellationToken);
        if (productionOperator == null)
        {
            return Result<int>.Failure(BatchErrors.OperatorNotFound);
        }

        var stageName = batch.CurrentStage;
        var assignedOperator = batch.AssignOperator(
            productionOperator.Id,
            productionOperator.Name,
            productionOperator.Specialty,
            stageName,
            DateTimeOffset.UtcNow);

        _batchRepository.Update(batch);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<int>.Success(assignedOperator.Id);
    }

    public async Task<Result<bool>> HandleAsync(UpdateBatchStageCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.BatchId <= 0 || command.Notes?.Length > 1000 ||
            command.ProcessedQuantity < 0 ||
            !StageName.TryParseProductionStage(command.NewStage, out var stage, out var progressPercentage))
        {
            return Result<bool>.Failure(BatchErrors.InvalidRequest);
        }

        var batchId = new BatchId(command.BatchId);
        var batch = await _batchRepository.FindByIdAsync(batchId, cancellationToken);

        if (batch == null)
        {
            return Result<bool>.Failure(BatchErrors.NotFound);
        }

        var updatedAt = DateTimeOffset.UtcNow;
        batch.UpdateProductionDetails(stage, progressPercentage, command.ProcessedQuantity, command.Notes, updatedAt);
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _batchRepository.Update(batch);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }, cancellationToken);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> HandleAsync(UpdateBatchMovementCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.MovementId <= 0 || command.QtyIn < 0 || command.QtyOut < 0 ||
            command.Stage?.Length > 100 || command.Responsible?.Length > 150 || command.Status?.Length > 50 ||
            (command.Status != null && !IsMovementStatus(command.Status)))
        {
            return Result<bool>.Failure(BatchErrors.InvalidRequest);
        }

        var batch = await _batchRepository.FindByMovementIdAsync(command.MovementId, cancellationToken);
        if (batch == null)
        {
            return Result<bool>.Failure(BatchErrors.MovementNotFound);
        }

        var existingMovement = batch.Movements.First(movement => movement.Id == command.MovementId);
        var resultingQtyIn = command.QtyIn ?? existingMovement.QtyIn;
        var resultingQtyOut = command.QtyOut ?? existingMovement.QtyOut;
        if (command.Difference.HasValue && command.Difference.Value != resultingQtyOut - resultingQtyIn)
        {
            return Result<bool>.Failure(BatchErrors.InvalidRequest);
        }

        var updated = batch.UpdateMovement(
            command.MovementId,
            string.IsNullOrWhiteSpace(command.Stage) ? null : new StageName(command.Stage),
            command.QtyIn,
            command.QtyOut,
            command.Difference,
            command.Responsible,
            command.DateTime,
            command.Status);
        if (!updated)
        {
            return Result<bool>.Failure(BatchErrors.MovementNotFound);
        }

        _batchRepository.Update(batch);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result<bool>.Success(true);
    }

    public async Task<Result<int>> HandleAsync(CreateBatchObservationCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.BatchId <= 0 || string.IsNullOrWhiteSpace(command.Text) || command.Text.Length > 500)
        {
            return Result<int>.Failure(BatchErrors.InvalidRequest);
        }

        var batch = await _batchRepository.FindByIdAsync(new BatchId(command.BatchId), cancellationToken);
        if (batch == null)
        {
            return Result<int>.Failure(BatchErrors.NotFound);
        }

        var observation = batch.AddObservation("System", "General", command.Text, DateTimeOffset.UtcNow);
        _batchRepository.Update(batch);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result<int>.Success(observation.Id);
    }

    private static bool IsMovementStatus(string? status) =>
        string.Equals(status, "Current", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase);
}