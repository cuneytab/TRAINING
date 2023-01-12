using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using todo_api.Contracts;
using todo_api.Models;

namespace todo_api.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private string databaseId = string.Empty;
        private string containerId = string.Empty;
        private IConfiguration _iconfig;
        public TodoItemRepository(IConfiguration iconfig)
        {
            _iconfig = iconfig;
            string connectionString = _iconfig["CosmosDbConnectionString"];
            databaseId = "todo";
            containerId = "items";

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
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/listId");
        }

        public async Task CreateTodoItem(TodoItem todoItem)
        {
            try
            {
                ItemResponse<TodoItem> itemResponse = await container.ReadItemAsync<TodoItem>(todoItem.Id, new PartitionKey(todoItem.ListId));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                await container.CreateItemAsync(todoItem, new PartitionKey(todoItem.ListId));
            }
        }

        public async Task DeleteTodoItem(TodoItem todoItem)
        {
            var partitionKeyValue = todoItem.ListId;
            var idRes = todoItem.Id;

            await this.container.DeleteItemAsync<TodoItem>(idRes, new PartitionKey(partitionKeyValue));
        }

        public async Task DeleteTodoItems(List<TodoItem> todoItems)
        {
            foreach (var todoItem in todoItems)
            {
                var partitionKeyValue = todoItem.ListId;
                var idRes = todoItem.Id;

                await this.container.DeleteItemAsync<TodoItem>(idRes, new PartitionKey(partitionKeyValue));
            }
        }

        public async Task DeleteTodoItemsByOwner(List<TodoItem> todoItems)
        {
            foreach (var todoItem in todoItems)
            {
                var partitionKeyValue = todoItem.ListId;
                var idRes = todoItem.Id;

                await this.container.DeleteItemAsync<TodoItem>(idRes, new PartitionKey(partitionKeyValue));
            }
        }

        public async Task<TodoItem> GetTodoItem(string id)
        {
            try
            {
                var qry = string.Format("SELECT * FROM items WHERE items.id = '{0}'", id);

                QueryDefinition queryDefinition = new QueryDefinition(qry);
                FeedIterator<TodoItem> queryIterator = container.GetItemQueryIterator<TodoItem>(queryDefinition);

                TodoItem result = new TodoItem();

                while (queryIterator.HasMoreResults)
                {
                    FeedResponse<TodoItem> resultSet = await queryIterator.ReadNextAsync();
                    foreach (TodoItem todoItemRes in resultSet)
                    {
                        result.Id = todoItemRes.Id;
                        result.Text = todoItemRes.Text;
                        result.DueDate = todoItemRes.DueDate;
                        result.Owner = todoItemRes.Owner;
                        result.List = todoItemRes.List;
                        result.Notes = todoItemRes.Notes;
                        result.Check = todoItemRes.Check;
                        result.ListId = todoItemRes.ListId;
                    }
                }
                return result;
            }
            catch (CosmosException ex)
            {
                throw new System.Exception(string.Format("Error occurs: Error: {0}", ex.Message));
            }
        }

        public async Task<List<TodoItem>> GetTodoItems(string listId)
        {
            var qry = string.Format("SELECT * FROM items WHERE items.listId = '{0}'", listId);

            QueryDefinition queryDefinition = new QueryDefinition(qry);
            FeedIterator<TodoItem> queryIterator = container.GetItemQueryIterator<TodoItem>(queryDefinition);

            List<TodoItem> result = new List<TodoItem>();

            while (queryIterator.HasMoreResults)
            {
                FeedResponse<TodoItem> resultSet = await queryIterator.ReadNextAsync();
                foreach (TodoItem todoItemRes in resultSet)
                {
                    result.Add(todoItemRes);
                }
            }
            return result;
        }

        public async Task<List<TodoItem>> GetTodoItemsByOwner(string owner)
        {
            var qry = string.Format("SELECT * FROM items WHERE items.owner = '{0}'", owner);

            QueryDefinition queryDefinition = new QueryDefinition(qry);
            FeedIterator<TodoItem> queryIterator = container.GetItemQueryIterator<TodoItem>(queryDefinition);

            List<TodoItem> result = new List<TodoItem>();

            while (queryIterator.HasMoreResults)
            {
                FeedResponse<TodoItem> resultSet = await queryIterator.ReadNextAsync();
                foreach (TodoItem todoItemRes in resultSet)
                {
                    result.Add(todoItemRes);
                }
            }
            return result;
        }

        public async Task UpdateTodoItem(TodoItem todoItem, string id, string text, DateTime dueDate, string owner, string list, string notes, bool check, string listId)
        {
            ItemResponse<TodoItem> response = await this.container.ReadItemAsync<TodoItem>(id, new PartitionKey(listId));

            var result = response.Resource;

            result.Id = todoItem.Id;
            result.Text = todoItem.Text;
            result.DueDate = todoItem.DueDate;
            result.Owner = todoItem.Owner;
            result.List = todoItem.List;
            result.Notes = todoItem.Notes;
            result.Check = todoItem.Check;
            result.ListId = todoItem.ListId;

            await this.container.ReplaceItemAsync<TodoItem>(result, result.Id, new PartitionKey(todoItem.ListId));
        }
    }
}
