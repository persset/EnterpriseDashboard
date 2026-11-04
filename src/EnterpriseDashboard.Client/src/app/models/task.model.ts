export enum TaskStatus {
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3
}
export interface TaskItem {
    id: string;
    title: string;
    description: string;
    priority: number;
    status: TaskStatus;
    dueDateUtc: string;
}

export interface CreateTaskDto {
    title: string;
    description: string;
    priority: number;
    dueDateUtc: string;
}