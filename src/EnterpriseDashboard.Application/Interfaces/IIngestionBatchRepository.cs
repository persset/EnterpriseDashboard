using System;
using EnterpriseDashboard.Domain.Entities;

namespace EnterpriseDashboard.Application.Interfaces;

public interface IIngestionBatchRepository
{
    Task<IngestionBatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(IngestionBatch batch, CancellationToken cancellationToken = default);
    Task UpdateAsync(IngestionBatch batch, CancellationToken cancellationToken = default);
}
