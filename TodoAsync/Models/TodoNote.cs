using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsyncTodo.Models
{
    /// <summary>
    /// A brief note.
    /// </summary>
    public class TodoNote
    {
        /// <summary>
        /// Brief note.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Note { get; set; }
    }
}