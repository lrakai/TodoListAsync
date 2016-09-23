using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AsyncTodo.Controllers;
using InMemoryDataLayer;
using AsyncTodo.Models;
using System.Web.Http.Results;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAsync.Tests
{
    [TestClass]
    public class TodoControllerTest
    {
        TodoController m_controller;

        [TestInitialize]
        public void Setup()
        {
            m_controller = new TodoController(new TodoControllerOptions
            {
                DataLayer = new InMemoryDataLayer<Todo>()
            });
        }

        [TestMethod]
        public async Task GetByIdWorksAsync()
        {
            var note = new TodoNote { Note = "testing" };
            var todo = new Todo(note);
            await m_controller.DataLayer.InsertAsync(todo);

            var actionResult = await m_controller.GetAsync(todo.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Todo>;
            
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(todo.Id, contentResult.Content.Id);
            Assert.AreEqual(todo.Note, contentResult.Content.Note);
        }

        [TestMethod]
        public async Task GetNonExistentByIdNotFoundAsync()
        {
            var actionResult = await m_controller.GetAsync(Guid.Empty);
            
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetByPageWorksAsync()
        {
            var note = new TodoNote { Note = "testingEarly" };
            var todoEarly = new Todo(note);
            await m_controller.DataLayer.InsertAsync(todoEarly);
            await Task.Delay(1000);
            note = new TodoNote { Note = "testingLate" };
            var todoLate = new Todo(note);
            await m_controller.DataLayer.InsertAsync(todoLate);

            var actionResult = await m_controller.GetAsync(1, 1);
            var contentResult = actionResult as OkNegotiatedContentResult<PagedTodos>;
            
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(todoEarly.Id, contentResult.Content.Items.First().Id);
            Assert.AreEqual(todoEarly.Note, contentResult.Content.Items.First().Note);
            Assert.IsNull(contentResult.Content.NextPageLink);
        }

        [TestMethod]
        public async Task InsertWorksAsync()
        {
            var note = new TodoNote { Note = "testing" };

            var actionResult = await m_controller.PostAsync(note);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Todo>;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreNotEqual(Guid.Empty, createdResult.RouteValues["id"]);
            Assert.AreNotEqual(Guid.Empty, createdResult.Content.Id);
            Assert.AreNotEqual(new DateTime().Ticks, createdResult.Content.DateTime.Ticks);
        }

        [TestMethod]
        public async Task UpdateWorksAsync()
        {
            var note = new TodoNote { Note = "testing" };
            var todo = new Todo(note);
            await m_controller.DataLayer.InsertAsync(todo);

            var noteUpdate = new TodoNote { Note = "testing update" };
            var todoUpdate = new Todo(noteUpdate);
            todoUpdate.Id = todo.Id;
            var actionResult = await m_controller.PutAsync(todoUpdate);
            var okResult = actionResult as OkResult;

            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task UpdateNonExistentIsBadRequestAsync()
        {
            var note = new TodoNote { Note = "testing" };
            var todo = new Todo(note);

            var actionResult = await m_controller.PutAsync(todo);
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            Assert.IsNotNull(badRequestResult);
            Assert.IsNotNull(badRequestResult.Message);
        }

        [TestMethod]
        public async Task DeleteWorksAsync()
        {
            var note = new TodoNote { Note = "testing" };
            var todo = new Todo(note);
            await m_controller.DataLayer.InsertAsync(todo);
            
            var actionResult = await m_controller.DeleteAsync(todo.Id);
            var okResult = actionResult as OkResult;

            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task DeleteNonExistentIsBadRequestAsync()
        { 
            var actionResult = await m_controller.DeleteAsync(Guid.Empty);
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            Assert.IsNotNull(badRequestResult);
            Assert.IsNotNull(badRequestResult.Message);
        }
    }
}
