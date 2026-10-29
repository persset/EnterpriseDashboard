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
        private readonly IValidator<UpdateTaskDto> _updateTaskValidator;

        public TasksController(ITaskRepository taskRepository,
        IValidator<CreateTaskDto> createTaskValidator,
        IValidator<UpdateTaskDto> updateTaskValidator)
        {
            _taskRepository = taskRepository;
            _createTaskValidator = createTaskValidator;
            _updateTaskValidator = updateTaskValidator;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto, CancellationToken cancellationToken)
        {
            // not really sure if this makes sense, will have to revisit it
            // cause i understand its useful, but I also assume there will be times that the dto might just bring the changes
            // and them the null properties must be ignored.
            var validationResult = await _updateTaskValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest();

            var existingTask = await _taskRepository.GetByIdAsync(id, cancellationToken);
            if (existingTask == null)
                return NotFound();

            existingTask.UpdateStatus(dto.Status); // for now at least

            await _taskRepository.UpdateAsync(existingTask);

            return Ok(existingTask);
        }
    }
}
