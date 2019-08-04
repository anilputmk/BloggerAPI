using System;
using System.Collections.Generic;
using System.Linq;

namespace BloggerApp.Models
{
    public class Blogger
    {
        public string Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<BlogPost> Posts { get; set; }

        public Blogger(string id, string firstName, string lastName)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Posts = new List<BlogPost>();
        }

        public void AddOrUpdateBlog(BlogPost blogPost)
        {
            BlogPost post = this.Posts.FirstOrDefault(x => x.Id.Equals(blogPost.Id, StringComparison.InvariantCulture));
            if (post == null)
            {
                this.Posts.Add(blogPost);
            }
            else
            {
                this.Posts[this.Posts.IndexOf(post)] = blogPost;
            }
        }

        public bool RemoveBlog(BlogPost blogPost)
        {
            if(this.Posts.Contains(blogPost))
            {
                this.Posts.Remove(blogPost);
                return true;
            }

            return false;
        }

        public void AddOrUpdateBlogs(List<BlogPost> blogPosts)
        {
            blogPosts.ForEach(post => this.AddOrUpdateBlog(post));
        }

        public void DeleteBlogs(List<BlogPost> blogPosts)
        {
            foreach(var post in blogPosts)
            {
                this.Posts.Remove(post);
            }
        }

        public void Update(Blogger blogger)
        {
            this.FirstName = blogger.FirstName;
            this.LastName = blogger.LastName;
            this.Posts.AddRange(blogger.Posts);
        }
    }
}
