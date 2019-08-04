using System.Threading.Tasks;
using BloggerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloggerController : Controller
    {
        private readonly InMemoryContext memoryContext;
        public BloggerController(InMemoryContext memoryContext)
        {
            this.memoryContext = memoryContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blogger>> GetBlogger(string id)
        {
            var blogger = await this.memoryContext.Bloggers
                .Include(x => x.Posts)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(blogger == null)
            {
                return NotFound(id);
            }

            return blogger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blogger>>> GetBloggers()
        {
            return await this.memoryContext.Bloggers
                .Include(blog => blog.Posts)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Blogger>> CreateBlogger(Blogger blogger)
        {
            var existingBlogger = await this.memoryContext.Bloggers.SingleOrDefaultAsync(x => x.Id == blogger.Id);
            if (existingBlogger != null)
            {
                existingBlogger.Update(blogger);
                this.memoryContext.Bloggers.Update(existingBlogger);
            }
            else
            {
                this.memoryContext.Bloggers.Add(blogger);
            }

            await this.memoryContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlogger), new { id = blogger.Id }, blogger);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlogger(string id)
        {
            var blogger = await this.memoryContext.Bloggers.SingleAsync(x => x.Id == id);
            if (blogger != null)
            {
                this.memoryContext.Bloggers.Remove(blogger);
                await this.memoryContext.SaveChangesAsync();
                return Ok();
            }

            return NotFound(id);
        }
    }
}
