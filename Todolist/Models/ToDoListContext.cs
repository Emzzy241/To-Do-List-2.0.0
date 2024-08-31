using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class ToDoListContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Item> Items { get; set; }

        public DbSet<Tag> Tags { get; set; } // After this, EF core will recognize the Tag class as the Tag entity. Remeber to create a new migration and update our database

        public DbSet<ItemTag> ItemTags { get; set; }
        
        public ToDoListContext(DbContextOptions options) : base(options) { }
    
        // We'll update ToDoListContext.cs so that it extends from IdentityDbContext instead of DbContext
    }
}