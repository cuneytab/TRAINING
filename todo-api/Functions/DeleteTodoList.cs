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
using System.Collections.Generic;

namespace todo_api.Functions
{
    public class DeleteTodoList
    {
        private readonly ILogger<DeleteTodoList> _logger;
        private readonly ITodoListRepository _todoListRepository;
        private readonly ITodoItemRepository _todoItemRepository;
        public DeleteTodoList(ITodoListRepository todoListRepository, ITodoItemRepository todoItemRepository, ILogger<DeleteTodoList> logger)
        {
            _logger = logger;
            _todoListRepository = todoListRepository;
            _todoItemRepository = todoItemRepository;
        }
        [FunctionName("DeleteTodoList")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/todo/{owner}/lists/{id}")] HttpRequest req,
            string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to delete a list.");

            IActionResult result;

            try
            {
                TodoList todoListRes = await _todoListRepository.GetTodoList(id); //?
                
                IActionResult resultOfDeletingList;
                resultOfDeletingList = await _todoListRepository.DeleteTodoList(todoListRes);
                
                result = new StatusCodeResult(StatusCodes.Status204NoContent);

                if (resultOfDeletingList != null) //??????????????????
                {
                    List<TodoItem> todoItemsRes = await _todoItemRepository.GetTodoItems(id);

                    await _todoItemRepository.DeleteTodoItems(todoItemsRes);
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
