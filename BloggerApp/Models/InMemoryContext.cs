using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggerApp.Models
{
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options)
            :base(options)
        {
            
        }

        public DbSet<Blogger> Bloggers { get; set; }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blogger>()
                .HasMany(b => b.Posts)
                .WithOne();
        }
    }
}
