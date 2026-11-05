using System;

namespace EnterpriseDashboard.Application.DTOs.IngestionBatches;

public record CreateIngestionBatchDto
(
    string fileName,
    long fileSizeBytes,
    int totalRecords
);
