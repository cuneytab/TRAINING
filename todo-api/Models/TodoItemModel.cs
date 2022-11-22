using System;
using Newtonsoft.Json;

namespace todo_api.Models
{
    public class TodoItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("dueDate")]
        public DateTime DueDate { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("list")]
        public string List { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("check")]
        public bool Check { get; set; }
        
        [JsonProperty("listId")]
        public string ListId { get; set; }
    }
}
