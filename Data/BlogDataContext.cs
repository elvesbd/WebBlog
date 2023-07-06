using WebBlog.Data.Mappings;
using WebBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace WebBlog.Data
{
    public class BlogDataContext : DbContext
    {
        private const string CONNECTION_STRING = @"
            Server=localhost,1433;Database=blog-modulo-6;User ID=sa;Password=1q2w3e4r@#$;Encrypt=false
        ";

        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(CONNECTION_STRING);
            // options.LogTo(Console.WriteLine); habilitas os logs das consultas no banco
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PostMap());
        }
    }
}