using Microsoft.EntityFrameworkCore;
using JLearning.Models;


namespace JLearning.Data
{
    
    public class WebContext : DbContext
    {
        public WebContext(DbContextOptions<WebContext> options )
                :base(options)
                {
            Database.EnsureCreated();
            }
        //setup DB context
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<UserDetail> UserDetail { get; set; } = default!;
        public DbSet<UserCourse> UserCourse { get; set; } = default!;
        // for many to many relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
