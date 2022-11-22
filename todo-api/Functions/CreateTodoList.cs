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
    public class CreateTodoList
    {
        private readonly ILogger<CreateTodoList> _logger;
        private readonly ITodoListRepository _todoListRepository;
        public CreateTodoList(ITodoListRepository todoListRepository, ILogger<CreateTodoList> logger)
        {
            _logger = logger;
            _todoListRepository = todoListRepository;
        }

        [FunctionName("AddTodoList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/todo/{owner}/lists")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to add a list");

            IActionResult result;

            try
            {
                using var read = new StreamReader(req.Body, Encoding.UTF8);
                var incomingReq = await read.ReadToEndAsync();

                if(!string.IsNullOrEmpty(incomingReq))
                {
                    var todoListReq = JsonConvert.DeserializeObject<TodoList>(incomingReq);

                    var todoListNew = new TodoList
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = todoListReq.Name,
                        Owner = todoListReq.Owner
                    };

                    await _todoListRepository.CreateTodoList(todoListNew);
                    result = new OkObjectResult(todoListNew);
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
