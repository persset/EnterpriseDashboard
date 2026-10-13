using System;
using EnterpriseDashboard.Domain.Common;

namespace EnterpriseDashboard.Domain.Entities;

public enum TaskStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3
}

public enum PriorityLevel
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

public class TaskItem : Entity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TaskStatus Status { get; private set; } = TaskStatus.Pending;
    public PriorityLevel Priority { get; private set; } = PriorityLevel.Medium;
    public DateTime? DueDateUtc { get; private set; }

    private TaskItem() { } // Constructor for EF core

    public TaskItem(string title, string description, PriorityLevel priority, DateTime? dueDateUtc = null)
    {
        if(string.IsNullOrWhiteSpace((title)))
            throw new ArgumentException("Task title cannot be empty.", nameof(title));

        Title = title;
        Description = description;
        Priority = priority;
        DueDateUtc = dueDateUtc;
        Status = TaskStatus.Pending;
    }

    public void UpdateStatus(TaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
