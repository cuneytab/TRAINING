using Newtonsoft.Json;

namespace todo_api.Models
{
    public class TodoUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }
    }
}