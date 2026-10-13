using System;
using EnterpriseDashboard.Domain.Common;

namespace EnterpriseDashboard.Domain.Entities;

public enum IngestionStatus
{
    Queued = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3
}
public class IngestionBatch : Entity
{
    public string FileName { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public IngestionStatus Status { get; private set; } = IngestionStatus.Queued;
    public int ProcessedRecords { get; private set; }
    public int TotalRecords { get; private set; }
    public string? ErrorSummary { get; private set; }

    private IngestionBatch() {} // Constructor for entity framework

    public IngestionBatch(string fileName, long fileSizeBytes, int totalRecords)
    {
        FileName = fileName;
        FileSizeBytes = fileSizeBytes;
        TotalRecords = totalRecords;
        Status = IngestionStatus.Queued;
    }

    public void MarkProcessing()
    {
        Status = IngestionStatus.Processing;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ProgressRecordCount(int newlyProcessedCount)
    {
        ProcessedRecords += newlyProcessedCount;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = IngestionStatus.Completed;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Fail(string error)
    {
        Status = IngestionStatus.Failed;
        ErrorSummary = error;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
