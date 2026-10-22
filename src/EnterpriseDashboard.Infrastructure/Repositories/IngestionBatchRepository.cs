using System;
using EnterpriseDashboard.Application.Interfaces;
using EnterpriseDashboard.Domain.Entities;
using EnterpriseDashboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseDashboard.Infrastructure.Repositories;

public class IngestionBatchRepository : IIngestionBatchRepository
{
    private readonly AppDbContext _context;

    public IngestionBatchRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(IngestionBatch batch, CancellationToken cancellationToken = default)
    {
        await _context.IngestionBatches.AddAsync(batch, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IngestionBatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.IngestionBatches.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(IngestionBatch batch, CancellationToken cancellationToken = default)
    {
        _context.IngestionBatches.Update(batch);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
