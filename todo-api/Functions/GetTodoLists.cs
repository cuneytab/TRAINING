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
using System.Text;
using System.Collections.Generic;

namespace todo_api.Functions
{
    public class GetTodoLists
    {
        private readonly ILogger<GetTodoLists> _logger;
        private readonly ITodoListRepository _todoListRepository;
        public GetTodoLists(ITodoListRepository todoListRepository, ILogger<GetTodoLists> logger)
        {
            _logger = logger;
            _todoListRepository = todoListRepository;
        }
        [FunctionName("GetTodoLists")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/todo/{owner}/lists")] HttpRequest req,
            string owner)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to get all lists");

            IActionResult result;

            try
            {
                var todoLists = await _todoListRepository.GetTodoLists(owner);

                if (todoLists.Count == 0)
                {
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                    _logger.LogInformation($"Lists does not exist.");
                }
                else
                {
                    result = new OkObjectResult(todoLists);
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
