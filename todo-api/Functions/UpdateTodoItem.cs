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
    public class UpdateTodoItem
    {
        private readonly ILogger<UpdateTodoItem> _logger;
        private readonly ITodoItemRepository _todoItemRepository;
        public UpdateTodoItem(ITodoItemRepository todoItemRepository, ILogger<UpdateTodoItem> logger)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
        }

        [FunctionName("UpdateTodoItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/todo/{owner}/lists/{listId}/items/{id}")] HttpRequest req,
            string listId, string id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to add a new item to the list with id: {listId}.");

            IActionResult result;

            try
            {
                var todoList = await _todoItemRepository.GetTodoItem(id);

                if (todoList == null)
                {
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                    _logger.LogInformation($"List with id: {id} does not exist.");
                }
                else
                {
                    using var read = new StreamReader(req.Body, Encoding.UTF8);
                    var incomingReq = await read.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(incomingReq))
                    {
                        var todoItemReq = JsonConvert.DeserializeObject<TodoItem>(incomingReq);

                        var todoItemToUpdate = new TodoItem
                        {
                            Id = id,
                            Text = todoItemReq.Text,
                            DueDate = todoItemReq.DueDate,
                            Owner = todoItemReq.Owner,
                            List = todoItemReq.List,
                            Notes = todoItemReq.Notes,
                            Check = todoItemReq.Check,
                            ListId = todoItemReq.ListId
                        };
                        await _todoItemRepository.UpdateTodoItem(todoItemToUpdate, 
                                                                 id, 
                                                                 todoItemToUpdate.Text, 
                                                                 todoItemToUpdate.DueDate,
                                                                 todoItemToUpdate.Owner,
                                                                 todoItemToUpdate.List,
                                                                 todoItemToUpdate.Notes,
                                                                 todoItemToUpdate.Check,
                                                                 todoItemToUpdate.ListId
                                                                 );
                        result = new StatusCodeResult(StatusCodes.Status201Created);
                    }
                    else
                    {
                        result = new StatusCodeResult(StatusCodes.Status400BadRequest);
                    }
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
