import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateTaskDto, TaskItem } from '../models/task.model';

@Service()
export class TaskService {
    private http = inject(HttpClient);
    private apiUrl = "http://localhost:5190/api/tasks";

    getTasks(): Observable<TaskItem[]> {
        return this.http.get<TaskItem[]>(this.apiUrl);
    }

    createTask(dto: CreateTaskDto): Observable<TaskItem> {
        return this.http.post<TaskItem>(this.apiUrl, dto);
    }

    updateTask(id: string, task: Partial<TaskItem>): Observable<TaskItem> {
        return this.http.put<TaskItem>(`${this.apiUrl}/${id}`, task);
    }
}
