using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using todo_api.Contracts;
using todo_api.Models;

namespace todo_api.Repositories
{
    public class TodoUserRepository : ITodoUserRepository
    {
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private string databaseId = string.Empty;
        private string containerId = string.Empty;
        private IConfiguration _iconfig;
        public TodoUserRepository(IConfiguration iconfig)
        {
            _iconfig = iconfig;
            string connectionString = iconfig["CosmosDbConnectionString"];
            databaseId = "todo";
            containerId = "users";

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
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/id");
        }

        public async Task CreateTodoUser(TodoUser todoUser)
        {
            try
            {
                ItemResponse<TodoUser> itemResponse = await container.ReadItemAsync<TodoUser>(todoUser.Id, new PartitionKey(todoUser.Id));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                await container.CreateItemAsync(todoUser, new PartitionKey(todoUser.Id));
            }
        }

        public async Task<TodoUser> GetTodoUser(string owner)
        {
            try
            {
                var qry = string.Format("SELECT * FROM users WHERE users.user = '{0}'", owner);

                QueryDefinition queryDefinition = new QueryDefinition(qry);
                FeedIterator<TodoUser> queryIterator = container.GetItemQueryIterator<TodoUser>(queryDefinition);

                TodoUser result = new TodoUser();

                while (queryIterator.HasMoreResults)
                {
                    FeedResponse<TodoUser> resultSet = await queryIterator.ReadNextAsync();
                    foreach (TodoUser todoUserRes in resultSet)
                    {
                        result.Id = todoUserRes.Id;
                        result.User = todoUserRes.User;
                    }
                }
                return result;
            }
            catch (CosmosException ex)
            {
                throw new System.Exception(string.Format("Error occurs: Error: {0}", ex.Message));
            }
        }

        public async Task<List<TodoUser>> GetTodoUserByUser(string user)
        {
            try
            {
                var qry = string.Format("SELECT * FROM users WHERE users.user = '{0}'", user);

                QueryDefinition queryDefinition = new QueryDefinition(qry);
                FeedIterator<TodoUser> queryIterator = container.GetItemQueryIterator<TodoUser>(queryDefinition);

                List<TodoUser> result = new List<TodoUser>();

                while (queryIterator.HasMoreResults)
                {
                    FeedResponse<TodoUser> resultSet = await queryIterator.ReadNextAsync();
                    foreach (TodoUser todoUserRes in resultSet)
                    {
                        result.Add(todoUserRes);
                    }
                }
                return result;
            }
            catch (CosmosException ex)
            {
                throw new System.Exception(string.Format("Error occurs: Error: {0}", ex.Message));
            }
        }

        public async Task<List<TodoUser>> ControlTodoUser(string user)
        {
            var qry = string.Format("SELECT * FROM users WHERE users.user = '{0}'", user);

            QueryDefinition queryDefinition = new QueryDefinition(qry);
            FeedIterator<TodoUser> queryIterator = container.GetItemQueryIterator<TodoUser>(queryDefinition);

            List<TodoUser> result = new List<TodoUser>();

            while (queryIterator.HasMoreResults)
            {
                FeedResponse<TodoUser> resultSet = await queryIterator.ReadNextAsync();
                foreach (TodoUser todoUserRes in resultSet)
                {
                    result.Add(todoUserRes);
                }
            }
            return result;
        }

        public async Task DeleteTodoUser(TodoUser todoUser)
        {
            var partitionKeyValue = todoUser.Id;
            var idRes = todoUser.Id;

            await this.container.DeleteItemAsync<TodoUser>(idRes, new PartitionKey(partitionKeyValue));
        }
    }
}