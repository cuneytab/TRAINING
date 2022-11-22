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
using todo_api.Models;

namespace todo_api.Functions
{
    public class DeleteTodoItem
    {
        private readonly ILogger<DeleteTodoItem> _logger;
        private readonly ITodoItemRepository _todoItemRepository;
        public DeleteTodoItem(ITodoItemRepository todoItemRepository, ILogger<DeleteTodoItem> logger)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
        }

        [FunctionName("DeleteTodoItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/todo/{owner}/lists/{listId}/items/{id}")] HttpRequest req,
            string listId, string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to delete an item with id: {id} in list with id: {listId}.");

            IActionResult result;

            try
            {
                TodoItem todoItem = await _todoItemRepository.GetTodoItem(id);

                await _todoItemRepository.DeleteTodoItem(todoItem);
                
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
