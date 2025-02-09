using Microsoft.EntityFrameworkCore;
using ToDoApp.Models; 

namespace ToDoApp.Models
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }

        public DbSet<TaskModel> tasks { get; set; }
    }
}
