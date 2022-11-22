export interface TodoItem {
    id?: string;
    text: string;
    dueDate: Date;
    owner: string;
    list: string;
    notes?: string;
    check?: boolean;
    listId?: string;
}
export interface TodoItemModel {
    id: string;
    text: string;
    dueDate: Date;
    owner: string;
    list: string;
    notes?: string;
    check?: boolean;
    listId: string;
}