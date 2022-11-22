using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo_api.Models;

namespace todo_api.Contracts
{
    public interface ITodoUserRepository
    {
        Task CreateTodoUser(TodoUser todoUser);
        Task<List<TodoUser>> ControlTodoUser(string user);
        Task<TodoUser> GetTodoUser(string id);
        Task<List<TodoUser>> GetTodoUserByUser(string user);
        Task DeleteTodoUser(TodoUser todoUser);
    }
}