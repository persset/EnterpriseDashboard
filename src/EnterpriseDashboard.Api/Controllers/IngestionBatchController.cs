using EnterpriseDashboard.Application.DTOs.IngestionBatches;
using EnterpriseDashboard.Application.Interfaces;
using EnterpriseDashboard.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseDashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngestionBatchController : ControllerBase
    {
        private readonly IIngestionBatchRepository _batchRepository;

        public IngestionBatchController(IIngestionBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        [HttpPost]
        public async Task<ActionResult<IngestionBatch>> CreateBatch([FromBody] CreateIngestionBatchDto dto, CancellationToken cancellationToken)
        {
            var batch = new IngestionBatch(dto.fileName, dto.fileSizeBytes, dto.totalRecords);

            await _batchRepository.AddAsync(batch, cancellationToken);

            return CreatedAtAction(nameof(GetBatchById), new { id = batch.Id }, batch);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngestionBatch>>> GetAllBatches(CancellationToken cancellationToken)
        {
            var batches = await _batchRepository.GetAllAsync(cancellationToken);
            return Ok(batches);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IngestionBatch>> GetBatchById(Guid id, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(id, cancellationToken);

            if (batch == null)
                return NotFound();

            return Ok(batch);
        }

        [HttpPost("{id:guid}/start")]
        public async Task<IActionResult> StartBatch(Guid id, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(id, cancellationToken);
            if (batch == null) return NotFound();

            batch.MarkAsProcessing();
            await _batchRepository.UpdateAsync(batch, cancellationToken);

            return Ok(batch);
        }

        [HttpPost("{id:guid}/progress")]
        public async Task<IActionResult> ProgressBatch(Guid id, [FromBody] UpdateProgressDto dto, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(id, cancellationToken);
            if (batch == null) return NotFound();

            batch.ProgressRecordCount(dto.NewlyProcessedCount);
            await _batchRepository.UpdateAsync(batch, cancellationToken);

            return Ok(batch);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> CompleteBatch(Guid id, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(id, cancellationToken);
            if (batch == null) return NotFound();

            batch.Complete();
            await _batchRepository.UpdateAsync(batch, cancellationToken);

            return Ok(batch);
        }

        [HttpPost("{id:guid}/fail")]
        public async Task<IActionResult> FailBatch(Guid id, [FromBody] FailBatchDto dto, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(id, cancellationToken);
            if (batch == null) return NotFound();

            batch.Fail(dto.ErrorSummary);
            await _batchRepository.UpdateAsync(batch, cancellationToken);

            return Ok(batch);
        }
    }
}
