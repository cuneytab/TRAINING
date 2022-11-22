using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using todo_api;
using todo_api.Contracts;
using todo_api.Repositories;

[assembly: FunctionsStartup(typeof(Startup))]
namespace todo_api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables()
                         .Build();

            builder.Services.AddSingleton<IConfiguration>(config);
            builder.Services.AddTransient<ITodoListRepository, TodoListRepository>();
            builder.Services.AddTransient<ITodoItemRepository, TodoItemRepository>();
            builder.Services.AddTransient<ITodoUserRepository, TodoUserRepository>();
        }
    }
}