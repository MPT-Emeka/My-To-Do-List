using Microsoft.EntityFrameworkCore;

namespace My_To_Do_List.Entities
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<ToDoList> ToDoLists { get; set; }
    }

}
