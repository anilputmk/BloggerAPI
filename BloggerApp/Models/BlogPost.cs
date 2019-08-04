using System;
namespace BloggerApp.Models
{
    public class BlogPost
    {
        public string Id { get; private set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public BlogPost(string Id, string Subject, string Body)
        {
            this.Id = Id;
            this.Subject = Subject;
            this.Body = Body;
        }
    }
}
