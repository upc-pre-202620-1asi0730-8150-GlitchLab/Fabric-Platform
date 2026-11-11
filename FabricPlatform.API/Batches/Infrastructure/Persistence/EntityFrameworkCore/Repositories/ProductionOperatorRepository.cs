using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ProductionOperatorRepository : IProductionOperatorRepository
{
    private readonly AppDbContext _context;

    public ProductionOperatorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProductionOperator>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<ProductionOperator>()
            .AsNoTracking()
            .OrderBy(operatorEntity => operatorEntity.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<ProductionOperator?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();
        return _context.Set<ProductionOperator>()
            .AsNoTracking()
            .FirstOrDefaultAsync(operatorEntity => operatorEntity.Name == normalizedName, cancellationToken);
    }

    public async Task AddAsync(
        ProductionOperator productionOperator,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<ProductionOperator>().AddAsync(productionOperator, cancellationToken);
    }
}
