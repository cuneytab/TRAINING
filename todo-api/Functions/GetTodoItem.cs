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

namespace todo_api.Functions
{
    public class GetTodoItem
    {
        private readonly ILogger<GetTodoItem> _logger;
        private readonly ITodoItemRepository _todoItemRepository;
        public GetTodoItem(ITodoItemRepository todoItemRepository, ILogger<GetTodoItem> logger)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
        }

        [FunctionName("GetTodoItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/todo/{owner}/lists/{listId}/items/{id}")] HttpRequest req,
             string listId, string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for get item");

            IActionResult result;

            try
            {
                var todoItem = await _todoItemRepository.GetTodoItem(id);
                if (todoItem == null)
                {
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                    _logger.LogInformation($"Item with id {id} does not exist.");
                }
                else
                {
                    result = new OkObjectResult(todoItem);
                }
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
