using AsyncTodo.Models;
using DataLayer.Contract;
using FileSystemDataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace AsyncTodo.Controllers
{
    public class TodoControllerOptions
    {
        public IDataLayer<Todo> DataLayer { get; set; }
        public int? PageSize { get; set; }
    }

    public class TodoController : ApiController
    {
        private readonly IDataLayer<Todo> m_dataLayer;
        public IDataLayer<Todo> DataLayer
        {
            get
            {
                return m_dataLayer;
            }
        }
        private readonly int m_pageSize = 10;
        public int PageSize
        {
            get
            {
                return m_pageSize;
            }
        }

        public TodoController()
        {
            m_dataLayer = new FileSystemDataLayer<Todo>(Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "filesData"));
        }

        public TodoController(TodoControllerOptions options)
        {
            m_dataLayer = options.DataLayer ?? new FileSystemDataLayer<Todo>(Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "filesData"));
            m_pageSize = options.PageSize ?? m_pageSize;
        }

        public TodoController(IDataLayer<Todo> dataLayer, int pageSize)
        {
            m_dataLayer = dataLayer;
        }

        // GET api/todo
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Todo>))]
        public async Task<IHttpActionResult> GetAsync(int? top, int skip = 0)
        {
            if(!top.HasValue)
            {
                top = PageSize;
            }
            var todos = await DataLayer.FindAsync(skip, top.Value + 1);
            if(top > 0 && todos.Count() > top)
            {
                var nextPageLink = new Uri(Url.Link("DefaultApi", new HttpRouteValueDictionary(RequestContext.RouteData.Values)
                {
                    { "skip", skip + top },
                    { "top", top }
                }));
                return Ok(new PagedTodos { Items = todos.Take(top.Value), NextPageLink = nextPageLink });
            }
            return Ok(new PagedTodos { Items = todos.Take(top.Value) });
        }

        // GET api/todo/5
        [HttpGet]
        [ResponseType(typeof(Todo))]
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            var todo = await DataLayer.FindAsync(id);
            if(todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        // POST api/todo
        [HttpPost]
        [ResponseType(typeof(Todo))]
        public async Task<IHttpActionResult> PostAsync(TodoNote note)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            do
            {
                try
                {
                    var todo = new Todo(note);
                    await DataLayer.InsertAsync(todo);
                    return CreatedAtRoute("DefaultApi", new { id = todo.Id }, todo);
                }
                catch (DataLayerAlreadyExistsException)
                {
                    // generate a new Id
                    continue;
                }
            } while (true);
        }

        // PUT api/todo/5
        [HttpPut]
        public async Task<IHttpActionResult> PutAsync(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await DataLayer.UpdateAsync(todo);
            if (updated)
            {
                return Ok();
            }
            var message = String.Format("Failed to update resource with Id {0}", todo.Id);
            return BadRequest(message);
        }

        // DELETE api/todo/5
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            var removed = await DataLayer.RemoveAsync(id);
            if (removed)
            {
                return Ok();
            }
            var message = String.Format("Failed to delete resource with Id {0}", id);
            return BadRequest(message);
        }
    }
}
