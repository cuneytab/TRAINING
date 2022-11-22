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
    public class GetTodoItems
    {
        private readonly ILogger<GetTodoItems> _logger;
        private readonly ITodoItemRepository _todoItemRepository;
        public GetTodoItems(ITodoItemRepository todoItemRepository, ILogger<GetTodoItems> logger)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
        }
        
        [FunctionName("GetTodoItems")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/todo/{owner}/lists/{listId}/items")] HttpRequest req,
            string listId)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to take items in list: {listId}.");

            IActionResult result;

            try
            {
                var todoItems = await _todoItemRepository.GetTodoItems(listId);
                if (todoItems == null)
                {
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                    _logger.LogInformation($"Items does not exist in list with id: {listId}.");
                }
                else
                {
                    result = new OkObjectResult(todoItems);
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
