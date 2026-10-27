export interface TaskItem {
    id: string;
    title: string;
    description: string;
    priority: number;
    isCompleted: boolean;
    dueDateUtc: string;
}

export interface CreateTaskDto {
    title: string;
    description: string;
    priority: number;
    dueDateUtc: string;
}