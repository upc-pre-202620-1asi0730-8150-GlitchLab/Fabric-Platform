using FabricPlatform.API.Batches.Application.CommandServices;
using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.Commands;
using FabricPlatform.API.Batches.Domain.Model.Errors;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Shared.Application.Model;
using FabricPlatform.API.Shared.Domain.Repositories;

namespace FabricPlatform.API.Batches.Application.Internal.CommandServices;

public class ProductionOperatorCommandService : ICreateProductionOperatorCommandService
{
    private readonly IProductionOperatorRepository _operatorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductionOperatorCommandService(
        IProductionOperatorRepository operatorRepository,
        IUnitOfWork unitOfWork)
    {
        _operatorRepository = operatorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductionOperator>> HandleAsync(
        CreateProductionOperatorCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name) || command.Name.Length > 150 ||
            string.IsNullOrWhiteSpace(command.Specialty) || command.Specialty.Length > 100)
        {
            return Result<ProductionOperator>.Failure(BatchErrors.InvalidRequest);
        }

        if (await _operatorRepository.FindByNameAsync(command.Name, cancellationToken) != null)
        {
            return Result<ProductionOperator>.Failure(BatchErrors.OperatorAlreadyExists);
        }

        var productionOperator = new ProductionOperator(command.Name, command.Specialty);
        await _operatorRepository.AddAsync(productionOperator, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<ProductionOperator>.Success(productionOperator);
    }
}
