using System;
using System.Collections.Generic;

namespace BloggerApp.Models
{
    public class BloggerManager
    {
        private IDictionary<Blogger, IList<Blogger>> bloggerConnection;

        public List<Blogger> Bloggers { get; set; }

        public BloggerManager()
        {
            this.bloggerConnection = new Dictionary<Blogger, IList<Blogger>>();
        }

        public void ConnectBlogger(Blogger blogger1, Blogger blogger2)
        {
            if(this.bloggerConnection[blogger1] == null)
            {
                this.bloggerConnection[blogger1] = new List<Blogger>();
            }

            this.bloggerConnection[blogger1].Add(blogger2);
        }

        public bool RemoveConnectionToBlogger(Blogger blogger1, Blogger blogger2)
        {
            if(this.bloggerConnection[blogger1].Contains(blogger2))
            {
                return this.bloggerConnection[blogger1].Remove(blogger2);
            }

            return false;
        }
    }
}
