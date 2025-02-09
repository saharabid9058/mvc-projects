using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending"; 
    }
}
