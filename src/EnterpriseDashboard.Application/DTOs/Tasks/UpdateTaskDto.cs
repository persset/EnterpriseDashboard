using System;

namespace EnterpriseDashboard.Application.DTOs.Tasks;

public record UpdateTaskDto
(
    string Title,
    string Description,
    int Priority,
    Domain.Entities.TaskStatus Status
);


