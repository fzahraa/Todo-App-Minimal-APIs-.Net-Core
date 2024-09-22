using Microsoft.EntityFrameworkCore;
using MinimalAPIToDoProject.Model;

namespace MinimalAPIToDoProject.ToDoDB
{
    public class TodoDB : DbContext
    {
        public TodoDB(DbContextOptions<TodoDB> options) : base(options) { }

        public DbSet<TodoItem> Todos { get; set; }
    }
}
