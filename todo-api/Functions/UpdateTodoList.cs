using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using todo_api.Contracts;
using todo_api.Models;

namespace todo_api.Functions
{
    public class UpdateTodoList
    {
        private readonly ILogger<UpdateTodoList> _logger;
        private readonly ITodoListRepository _todoListRepository;
        public UpdateTodoList(ITodoListRepository todoListRepository, ILogger<UpdateTodoList> logger)
        {
            _logger = logger;
            _todoListRepository = todoListRepository;
        }
        [FunctionName("UpdateTodoList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/todo/{owner}/lists/{id}")] HttpRequest req,
            string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to update a list.");

            IActionResult result;

            try
            {
                var todoList = await _todoListRepository.GetTodoList(id);

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
                        var todoListReq = JsonConvert.DeserializeObject<TodoList>(incomingReq);

                        var todoListToUpdate = new TodoList
                        {
                            Id = id,
                            Name = todoListReq.Name,
                            Owner = todoListReq.Owner
                        };
                        await _todoListRepository.UpdateTodoList(todoListToUpdate, id, todoListToUpdate.Name, todoListToUpdate.Owner);
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
