using DataLayer.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsyncTodo.Models
{
    /// <summary>
    /// An identifiable todo note with a DateTime specified.
    /// </summary>
    public class Todo : TodoNote, IIdentifiable, IDateTimeProvider
    {
        /// <summary>
        /// Parameterless constructor for deserialization.
        /// </summary>
        [JsonConstructor]
        private Todo()
        {
        }

        /// <summary>
        /// Construct a Todo with the provided note and random Id and current DateTime.
        /// </summary>
        /// <param name="note"></param>
        public Todo(TodoNote note)
        {
            Note = note.Note;
            Id = Guid.NewGuid();
            DateTime = DateTime.Now;
        }

        /// <summary>
        /// Unique identifier of the Todo.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Created time of the Todo.
        /// </summary>
        [Required]
        public DateTime DateTime { get; private set; }
    }
}