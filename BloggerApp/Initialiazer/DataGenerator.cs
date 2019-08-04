using System;
using System.Collections.Generic;
using BloggerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BloggerApp.Initialiazer
{
    public class DataGenerator
    {
        public DataGenerator()
        {
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new InMemoryContext(
                serviceProvider.GetRequiredService<DbContextOptions<InMemoryContext>>()))
            {
                List<BlogPost> posts101 = new List<BlogPost>() {
                    new BlogPost("1002", "Sample subject", "Sample body"),
                    new BlogPost("1003", "Sample subject", "Sample body") };

                List<BlogPost> posts102 = new List<BlogPost>() {
                    new BlogPost("1004", "Sample subject", "Sample body"),
                    new BlogPost("1005", "Sample subject", "Sample body") };

                List<BlogPost> posts103 = new List<BlogPost>() {
                    new BlogPost("1006", "Sample subject", "Sample body"),
                    new BlogPost("1007", "Sample subject", "Sample body") };

                List<BlogPost> posts104 = new List<BlogPost>() {
                    new BlogPost("1008", "Sample subject", "Sample body"),
                    new BlogPost("1009", "Sample subject", "Sample body") };

                var blogger1 = new Blogger("101", "A1", "A2") { Posts = posts101 };
                var blogger2 = new Blogger("102", "B1", "B2") { Posts = posts102 };
                var blogger3 = new Blogger("103", "C1", "C2") { Posts = posts103 };
                var blogger4 = new Blogger("104", "D1", "D2") { Posts = posts104 };

                context.Bloggers.AddRange(blogger1, blogger2, blogger3, blogger4);

                context.Connections.AddRange(
                    new Connection() { BloggerOne = blogger1, BloggerTwo = blogger2 },
                    new Connection() { BloggerOne = blogger2, BloggerTwo = blogger3 },
                    new Connection() { BloggerOne = blogger3, BloggerTwo = blogger4 },
                    new Connection() { BloggerOne = blogger2, BloggerTwo = blogger4 });

                context.SaveChanges();
            }
        }
    }
}
