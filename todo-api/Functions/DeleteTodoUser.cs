using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using todo_api.Contracts;
using System.Collections.Generic;
using todo_api.Models;

namespace todo_api.Functions
{
    public class DeleteTodoUser
    {
        private readonly ILogger<DeleteTodoUser> _logger;
        private readonly ITodoUserRepository _todoUserRepository;
        private readonly ITodoListRepository _todoListRepository;
        private readonly ITodoItemRepository _todoItemRepository;
        public DeleteTodoUser(ITodoUserRepository todoUserRepository, 
                              ITodoListRepository todoListRepository, 
                              ITodoItemRepository todoItemRepository, 
                              ILogger<DeleteTodoUser> logger)
        {
            _logger = logger;
            _todoUserRepository = todoUserRepository;
            _todoListRepository = todoListRepository;
            _todoItemRepository = todoItemRepository;
        }
        
        [FunctionName("DeleteTodoUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/todo/{owner}/delete")] HttpRequest req,
            string owner)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to delete a user");

            IActionResult result;

            try
            {
                TodoUser todoUser = await _todoUserRepository.GetTodoUser(owner);
                if (todoUser.User == owner)
                {
                    await _todoUserRepository.DeleteTodoUser(todoUser);
                }
                
                List<TodoList> todoLists = await _todoListRepository.GetTodoLists(todoUser.User);
                if (todoLists.Count > 0)
                {
                    await _todoListRepository.DeleteTodoLists(todoLists);
                }
                
                List<TodoItem> todoItems = await _todoItemRepository.GetTodoItemsByOwner(todoUser.User);
                if (todoItems.Count > 0)
                {
                    await _todoItemRepository.DeleteTodoItemsByOwner(todoItems);
                }
                
                result = new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
