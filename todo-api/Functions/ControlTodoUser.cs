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
    public class ControlTodoUser
    {
        private readonly ILogger<ControlTodoUser> _logger;
        private readonly ITodoUserRepository _todoUserRepository;
        private readonly ITodoListRepository _todoListRepository;
        public ControlTodoUser(ITodoUserRepository todoUserRepository, ITodoListRepository todoListRepository, ILogger<ControlTodoUser> logger)
        {
            _logger = logger;
            _todoUserRepository = todoUserRepository;
            _todoListRepository = todoListRepository;
        }

        [FunctionName("ControlTodoUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/todo/login")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to control user from database");

            IActionResult result;

            try
            {
                using var read = new StreamReader(req.Body, Encoding.UTF8);
                var incomingReq = await read.ReadToEndAsync();

                if (!string.IsNullOrEmpty(incomingReq))
                {
                    var todoUser = JsonConvert.DeserializeObject<TodoUser>(incomingReq);

                    var todoUserRes = await _todoUserRepository.ControlTodoUser(todoUser.User);

                    if (todoUserRes.Count != 0)
                    {
                        //var todoLists = await _todoListRepository.GetTodoLists(todoUser.User);

                        //if (todoLists.Count == 0)
                        //{
                         //   result = new StatusCodeResult(StatusCodes.Status204NoContent);
                        //    _logger.LogInformation($"Lists does not exist.");
                        //}
                        //else
                        //{
                            result = new OkObjectResult(todoUserRes);
                        //}
                    }
                    else
                    {
                        result = new StatusCodeResult(StatusCodes.Status404NotFound);
                            _logger.LogInformation($"User does not exist.");
                    }
                }
                else
                {
                    result = new StatusCodeResult(StatusCodes.Status400BadRequest);
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
