using System;
using EnterpriseDashboard.Application.DTOs.Tasks;
using FluentValidation;

namespace EnterpriseDashboard.Application.Validators.Tasks;

public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(150).WithMessage("Task title cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Priority)
            .InclusiveBetween(0, 3).WithMessage("Invalid priority level supplied.");

        RuleFor(x => x.DueDateUtc)
            .Must(date => !date.HasValue || date.Value > DateTime.UtcNow)
            .WithMessage("Due date must be in the future.");
    }
}
