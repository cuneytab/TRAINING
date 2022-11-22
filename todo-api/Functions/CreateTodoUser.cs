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
using System.Collections.Generic;

namespace todo_api.Functions
{
    public class CreateTodoUser
    {
        private readonly ILogger<CreateTodoUser> _logger;
        private readonly ITodoUserRepository _todoUserRepository;
        public CreateTodoUser(ITodoUserRepository todoUserRepository, ILogger<CreateTodoUser> logger)
        {
            _logger = logger;
            _todoUserRepository = todoUserRepository;
        }

        [FunctionName("CreateTodoUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/todo/add")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to add a user");

            IActionResult result;

            try
            {
                using var read = new StreamReader(req.Body, Encoding.UTF8);
                var incomingReq = await read.ReadToEndAsync();

                if (!string.IsNullOrEmpty(incomingReq))
                {
                    var todoUserReq = JsonConvert.DeserializeObject<TodoUser>(incomingReq);

                    List<TodoUser> todoUsers = new List<TodoUser>();
                    todoUsers = await _todoUserRepository.GetTodoUserByUser(todoUserReq.User);

                    if (todoUsers.Count == 0)
                    {
                        var todoUserNew = new TodoUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            User = todoUserReq.User,
                        };

                        await _todoUserRepository.CreateTodoUser(todoUserNew);
                        result = new OkObjectResult(todoUserNew);
                    }
                    else
                    {
                        _logger.LogError("User is already exist!");
                        result = new StatusCodeResult(StatusCodes.Status406NotAcceptable);
                    }
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
