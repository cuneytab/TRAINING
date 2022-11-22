using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo_api.Models;

namespace todo_api.Contracts
{
    public interface ITodoListRepository
    {
        Task<List<TodoList>> GetTodoLists(string owner);
        Task<TodoList> GetTodoList(string id);
        Task CreateTodoList(TodoList todoList);
        Task UpdateTodoList(TodoList todoList,
                            string id,
                            string name,
                            string owner /*?*/);
        Task<IActionResult> DeleteTodoList(TodoList todoList);
        Task<IActionResult> DeleteTodoLists(List<TodoList> todoLists);
    }
}
