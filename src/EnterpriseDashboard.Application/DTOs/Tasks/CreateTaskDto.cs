using System;

namespace EnterpriseDashboard.Application.DTOs.Tasks;

public record CreateTaskDto
(
    string Title,
    string Description,
    int Priority,
    DateTime? DueDateUtc
);
