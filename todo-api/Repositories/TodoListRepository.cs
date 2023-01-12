using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using todo_api.Contracts;
using todo_api.Models;
using Microsoft.AspNetCore.Mvc; //????????????
using Microsoft.AspNetCore.Http; //?????????????????

namespace todo_api.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private string databaseId = string.Empty;
        private string containerId = string.Empty;
        private IConfiguration _iconfig;
        public TodoListRepository(IConfiguration iconfig)
        {
            _iconfig = iconfig;
            string connectionString = _iconfig["CosmosDbConnectionString"];
            databaseId = "todo";
            containerId = "lists";

            cosmosClient = new CosmosClient(connectionString, new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Gateway
            });

            CreateDatabaseAsync().Wait();
            CreateContainerAsync().Wait();
        }

        private async Task CreateDatabaseAsync()
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }

        private async Task CreateContainerAsync()
        {
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/owner");
        }

        public async Task CreateTodoList(TodoList todoList)
        {
            try
            {
                ItemResponse<TodoList> itemResponse = await container.ReadItemAsync<TodoList>(todoList.Id, new PartitionKey(todoList.Owner));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                await container.CreateItemAsync(todoList, new PartitionKey(todoList.Owner));
            }
        }

        public async Task<IActionResult> DeleteTodoList(TodoList todoList/*, string owner*/)
        {
            var partitionKeyValue = todoList.Owner;
            var idRes = todoList.Id;

            await this.container.DeleteItemAsync<TodoList>(idRes, new PartitionKey(partitionKeyValue));

            return new StatusCodeResult(StatusCodes.Status204NoContent); //?????????????
        }

        public async Task<IActionResult> DeleteTodoLists(List<TodoList> todoLists/*, string owner*/)
        {
            foreach (var todoList in todoLists)
            {
                var partitionKeyValue = todoList.Owner;
                var idRes = todoList.Id;

                await this.container.DeleteItemAsync<TodoList>(idRes, new PartitionKey(partitionKeyValue));
            }

            return new StatusCodeResult(StatusCodes.Status204NoContent); //?????????????
        }

        public async Task<TodoList> GetTodoList(string id)
        {
            try
            {
                var qry = string.Format("SELECT * FROM lists WHERE lists.id = '{0}'", id);

                QueryDefinition queryDefinition = new QueryDefinition(qry);
                FeedIterator<TodoList> queryIterator = container.GetItemQueryIterator<TodoList>(queryDefinition);

                TodoList result = new TodoList();

                while (queryIterator.HasMoreResults)
                {
                    FeedResponse<TodoList> resultSet = await queryIterator.ReadNextAsync();
                    foreach (TodoList todoListRes in resultSet)
                    {
                        result.Id = todoListRes.Id;
                        result.Name = todoListRes.Name;
                        result.Owner = todoListRes.Owner;
                    }
                }
                return result;
            }
            catch (CosmosException ex)
            {
                throw new System.Exception(string.Format("Error occurs: Error: {0}", ex.Message));
            }
        }

        public async Task<List<TodoList>> GetTodoLists(string owner)
        {
            var qry = string.Format("SELECT * FROM lists WHERE lists.owner = '{0}'", owner);

            QueryDefinition queryDefinition = new QueryDefinition(qry);
            FeedIterator<TodoList> queryIterator = container.GetItemQueryIterator<TodoList>(queryDefinition);

            List<TodoList> result = new List<TodoList>();

            while (queryIterator.HasMoreResults)
            {
                FeedResponse<TodoList> resultSet = await queryIterator.ReadNextAsync();
                foreach (TodoList todoListRes in resultSet)
                {
                    result.Add(todoListRes);
                }
            }
            return result;
        }

        public async Task UpdateTodoList(TodoList todoList, string id, string name, string owner)
        {
            ItemResponse<TodoList> response = await this.container.ReadItemAsync<TodoList>(id, new PartitionKey(owner));

            var result = response.Resource;

            result.Id = todoList.Id;
            result.Name = todoList.Name;
            result.Owner = todoList.Owner;

            await this.container.ReplaceItemAsync<TodoList>(result, result.Id, new PartitionKey(todoList.Owner));
        }
    }
}
