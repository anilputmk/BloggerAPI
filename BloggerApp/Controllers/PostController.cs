using BloggerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BloggerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private const string INVALID_BLOGGER_DATA = "Invalid blogger data";
        private readonly InMemoryContext memoryContext;

        public PostController(InMemoryContext memoryContext)
        {
            this.memoryContext = memoryContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetPost(string id)
        {
            var post = await this.memoryContext.BlogPosts.SingleOrDefaultAsync(x => x.Id == id);

            if (post == null)
            {
                return NotFound(id);
            }

            return post;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BlogPost>> DeletePost(string id)
        {
            var post = await this.memoryContext.BlogPosts.SingleOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                return NotFound(id);
            }

            this.memoryContext.BlogPosts.Remove(post);
            await this.memoryContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetPosts()
        {
            return await this.memoryContext.BlogPosts.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<BlogPost>> CreatePost([FromBody][Required] InputBlogModel blogData)
        {
            var blogger = await this.memoryContext.Bloggers.FindAsync(blogData.BloggerId);

            if(blogger == null)
            {
                return BadRequest(INVALID_BLOGGER_DATA);
            }

            blogger.AddOrUpdateBlog(blogData.Post);
            this.memoryContext.Bloggers.Update(blogger);
            await this.memoryContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{bulkAdd}")]
        public async Task<ActionResult> CreatePosts([FromBody][Required] List<InputBlogModel> blogData)
        {
            var groupByBloggerIds = blogData
                .Where(x => x.Post != null)
                .GroupBy(x => x.BloggerId)
                .ToDictionary(x => x.Key, y => y.ToList())
                .ToList();

            foreach (var bloggerKeyValuePair in groupByBloggerIds)
            {
                var blogger = await this.memoryContext.Bloggers.FindAsync(bloggerKeyValuePair.Key);
                if (blogger != null)
                {
                    blogger.AddOrUpdateBlogs(bloggerKeyValuePair.Value.Select(x => x.Post).ToList());
                    this.memoryContext.Bloggers.Update(blogger);
                }
            }

            await this.memoryContext.SaveChangesAsync();
            return Ok();
        }
    }
}
