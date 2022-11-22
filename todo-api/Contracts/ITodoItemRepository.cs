using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using todo_api.Models;

namespace todo_api.Contracts
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetTodoItems(string listId);
        Task<List<TodoItem>> GetTodoItemsByOwner(string owner);
        Task<TodoItem> GetTodoItem(string id);
        Task CreateTodoItem(TodoItem todoItem);
        Task UpdateTodoItem(TodoItem todoItem,
                             string id,
                             string text,
                             DateTime dueDate,
                             string owner /*?*/,
                             string list,
                             string notes,
                             bool check,
                             string listId);
        Task DeleteTodoItem(TodoItem todoItem);
        Task DeleteTodoItems(List<TodoItem> todoItems);
        Task DeleteTodoItemsByOwner(List<TodoItem> todoItems);
    }
}
