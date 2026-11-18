import { Service, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import {
    IngestionBatch,
    CreateIngestionBatchDto,
    UpdateProgressDto,
    FailBatchDto
} from "../models/ingestion-batch.model";

@Service()
export class IngestionBatchService {
    private http = inject(HttpClient);
    private apiUrl = "https://localhost:5190/api/ingestionbatches";

    getAllBatches(): Observable<IngestionBatch[]> {
        return this.http.get<IngestionBatch[]>(this.apiUrl);
    }

    getBatchById(id: string): Observable<IngestionBatch> {
        return this.http.get<IngestionBatch>(`${this.apiUrl}/${id}`);
    }

    createBatch(dto: CreateIngestionBatchDto): Observable<IngestionBatch> {
        return this.http.post<IngestionBatch>(this.apiUrl, dto);
    }

    startBatch(id: string): Observable<IngestionBatch> {
        return this.http.post<IngestionBatch>(`${this.apiUrl}/${id}/start`, {});
    }

    progressBatch(id: string, dto: UpdateProgressDto): Observable<IngestionBatch> {
        return this.http.post<IngestionBatch>(`${this.apiUrl}/${id}/progress`, dto);
    }

    completePatch(id: string): Observable<IngestionBatch> {
        return this.http.post<IngestionBatch>(`${this.apiUrl}/${id}/complete`, {});
    }

    failBatch(id: string, dto: FailBatchDto): Observable<IngestionBatch> {
        return this.http.post<IngestionBatch>(`${this.apiUrl}/${id}/fail`, dto);
    }
}
