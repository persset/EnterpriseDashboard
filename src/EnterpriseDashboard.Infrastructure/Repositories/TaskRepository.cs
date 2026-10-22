using System;
using EnterpriseDashboard.Application.Interfaces;
using EnterpriseDashboard.Domain.Entities;
using EnterpriseDashboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseDashboard.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        await _context.Tasks.AddAsync(taskItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Remove(taskItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Update(taskItem);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
