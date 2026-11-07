using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Domain.Model.Errors;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Batches.Application.CommandServices;
using FabricPlatform.API.Shared.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;

namespace FabricPlatform.API.Batches.Application.Internal.CommandServices;

public class BatchCommandService : ICreateBatchCommandService
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
        // 1. Validar e instanciar el objeto de valor del Dominio
        var batchCode = new BatchCode(command.Code);

        // 2. Verificar la regla de negocio de unicidad mediante el repositorio
        var existingBatch = await _batchRepository.FindByCodeAsync(batchCode, cancellationToken);
        if (existingBatch != null)
        {
            return Result<int>.Failure(BatchErrors.CodeAlreadyExists);
        }

        // 3. Instanciar la Raíz del Agregado (lo cual dispara internamente el Evento de Dominio)
        var batch = new Batch(batchCode, command.ProjectedQuantity, command.DeliveryDate);

        // 4. Registrar la entidad en el repositorio
        await _batchRepository.AddAsync(batch, cancellationToken);

        // 5. Confirmar la transacción a través de la UnitOfWork (lo que activa el Interceptor Observer)
        await _unitOfWork.CompleteAsync(cancellationToken);

        // 6. Retornar el ID exitoso
        return Result<int>.Success(batch.Id.Value);
    }
}