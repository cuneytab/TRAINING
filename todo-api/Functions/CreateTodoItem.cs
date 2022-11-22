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
using System.Text;
using todo_api.Models;

namespace todo_api.Functions
{
    public class CreateTodoItem
    {
        private readonly ILogger<CreateTodoItem> _logger;
        private readonly ITodoItemRepository _todoItemRepository;
        public CreateTodoItem(ITodoItemRepository todoItemRepository, ILogger<CreateTodoItem> logger)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
        }

        [FunctionName("AddTodoItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/todo/{owner}/lists/{listId}/items")] HttpRequest req,
            string owner, string listId)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to add an item to the list with id: {listId}.");

            IActionResult result;
            try
            {
                using var read = new StreamReader(req.Body, Encoding.UTF8);
                var incomingReq = await read.ReadToEndAsync();

                if(!string.IsNullOrEmpty(incomingReq))
                {
                    var todoItemReq = JsonConvert.DeserializeObject<TodoItem>(incomingReq);

                    var todoItemNew = new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Text = todoItemReq.Text,
                        DueDate = todoItemReq.DueDate,
                        Owner = todoItemReq.Owner,
                        List = todoItemReq.List,
                        Notes = todoItemReq.Notes,
                        Check = false,
                        ListId = listId
                    };
                    
                    await _todoItemRepository.CreateTodoItem(todoItemNew);
                    result = new OkObjectResult(todoItemNew);
                }
                else
                {
                    result = new StatusCodeResult(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
            return result;
        }
    }
}
