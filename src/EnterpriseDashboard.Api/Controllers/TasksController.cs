using EnterpriseDashboard.Application.DTOs.Tasks;
using EnterpriseDashboard.Application.Interfaces;
using EnterpriseDashboard.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseDashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IValidator<CreateTaskDto> _createTaskValidator;

        public TasksController(ITaskRepository taskRepository, IValidator<CreateTaskDto> createTaskValidator)
        {
            _taskRepository = taskRepository;
            _createTaskValidator = createTaskValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync(cancellationToken);
            return Ok(tasks);
        }

        [HttpGet("{id:guid}", Name = "GetTaskById")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(id, cancellationToken);

            if (task is null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto, CancellationToken cancellationToken)
        {
            var validationResult = await _createTaskValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest();

            var task = new TaskItem(
                dto.Title,
                dto.Description,
                (PriorityLevel)dto.Priority,
                dto.DueDateUtc
            );

            await _taskRepository.AddAsync(task, cancellationToken);

            return CreatedAtRoute("GetTaskById", new { id = task.Id }, task);
        }
    }
}
