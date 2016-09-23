using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AsyncTodo.Models
{
    /// <summary>
    /// Represents a page of Todos.
    /// </summary>
    public class PagedTodos
    {
        public IEnumerable<Todo> Items { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri NextPageLink { get; set; }
    }
}