export enum IngestionStatus {
    Queued = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3
}

export interface IngestionBatch {
    id: string;
    fileName: string;
    fileSizeBytes: number;
    status: IngestionStatus;
    processedRecords: number;
    totalRecords: number;
    errorSummary?: string;
    createdAtUtc: string;
    updatedAtUtc?: string;
}

export interface CreateIngestionBatchDto {
    fileName: string;
    fileSizeBytes: number;
    totalRecords: number;
}

export interface UpdateProgressDto {
    newlyProcessedCount: number;
}

export interface FailBatchDto {
    errorSummary: string;
}