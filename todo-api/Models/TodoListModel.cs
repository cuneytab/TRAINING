using Newtonsoft.Json;

namespace todo_api.Models
{
    public class TodoList
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("owner")]
        public string Owner { get; set; }
    }
}
