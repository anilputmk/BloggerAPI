using System;
using System.Threading.Tasks;
using BloggerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BloggerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionController : Controller
    {
        private const string INVALID_BLOGGER_DATA = "Invalid blogger data";
        private const string NO_CONNECTION_MESSAGE = "No connection between bloggers";

        private InMemoryContext memoryContext;

        public ConnectionController(InMemoryContext memoryContext)
        {
            this.memoryContext = memoryContext;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Connection>> CreateConnection([FromQuery][Required] string blogger1,
            [FromQuery][Required] string blogger2)
        {
            var bloggerOne = await this.memoryContext.Bloggers.FindAsync(blogger1);
            var bloggerTwo = await this.memoryContext.Bloggers.FindAsync(blogger2);

            if (bloggerOne == null || bloggerTwo == null)
            {
                return NotFound(INVALID_BLOGGER_DATA);
            }

            this.memoryContext.Connections.Add(new Connection() { BloggerOne = bloggerOne, BloggerTwo = bloggerTwo });
            await this.memoryContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Connection>>> GetConnections()
        {
            return await this.memoryContext.Connections
                .Include(x => x.BloggerOne)
                .Include(x => x.BloggerTwo)
                .ToListAsync();
        }

        [HttpDelete]
        public async Task<ActionResult<Connection>> RemoveConnection([FromQuery][Required] string blogger1
            ,[FromQuery][Required] string blogger2)
        {
            var connection = this.memoryContext.Connections
                .FirstOrDefault(x => x.BloggerOne.Id == blogger1 && x.BloggerTwo.Id == blogger2);

            if (connection == null)
            {
                return NotFound(string.Format("No connection exists between {0}:{1}", blogger1, blogger2));
            }

            this.memoryContext.Connections.Remove(connection);
            await this.memoryContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("hops")]
        public async Task<IActionResult> GetHops([FromQuery][Required] string blogger1
            ,[FromQuery][Required] string blogger2)
        {
            var connections = await this.memoryContext.Connections
                .Include(x => x.BloggerOne)
                .Include(x => x.BloggerTwo)
                .ToListAsync();

            var connectionExists = connections
                .Any(x => x.BloggerOne.Id.Equals(blogger1) || x.BloggerOne.Id.Equals(blogger2));

            if (!connectionExists)
            {
                return NotFound(string.Format("Connection doesn't exists between {0}:{1}", blogger1, blogger2));
            }

            var result = this.FetchNumberOfHops(blogger1, blogger2);

            if(result.Key)
            {
                return Ok(string.Format("Number of hops: {0}", result.Value));
            }
            else
            {
                return Ok(NO_CONNECTION_MESSAGE);
            }
        }

        private KeyValuePair<bool, int> FetchNumberOfHops(string blogger1, string blogger2)
        {
            // To handle reverse connection use-case.
            var isFromSourceBlogger = this.memoryContext.Connections
                .Any(x => x.BloggerOne.Id == blogger1 || x.BloggerTwo.Id == blogger2);

            List<KeyValuePair<string, List<string>>> connectionsByGroup = null;
            if (isFromSourceBlogger)
            {
                connectionsByGroup = this.memoryContext.Connections
                .GroupBy(c => c.BloggerOne)
                .ToDictionary(c => c.Key.Id, y => y.Select(connection => connection.BloggerTwo.Id).ToList())
                .OrderByDescending(c => c.Key == blogger1)
                .ThenBy(c => c.Key)
                .ToList();
            }
            else
            {
                connectionsByGroup = this.memoryContext.Connections
                .GroupBy(c => c.BloggerTwo)
                .ToDictionary(c => c.Key.Id, y => y.Select(connection => connection.BloggerOne.Id).ToList())
                .OrderByDescending(c => c.Key == blogger1)
                .ThenBy(c => c.Key)
                .ToList();
            }

            return this.GetNumberOfHops(blogger1, blogger2, 0, connectionsByGroup, new List<string>());
        }

        private KeyValuePair<bool, int> GetNumberOfHops(string source,
            string destination,
            int numberOfHops,
            List<KeyValuePair<string, List<string>>> connectionsByGroup,
            List<string> visited)
        {
            if (visited.Any(x => x == source))
            {
                return new KeyValuePair<bool, int>(false, numberOfHops);
            }

            visited.Add(source);
            var sourceObject = connectionsByGroup.FirstOrDefault(x => x.Key == source);
            if (sourceObject.Key == null || sourceObject.Value == null)
            {
                return new KeyValuePair<bool, int>(false, numberOfHops);
            }

            var found = sourceObject.Value.Any(x => x == destination);
            numberOfHops++;
            if (found)
            {
                return new KeyValuePair<bool, int>(true, numberOfHops);
            }
            else
            {
                foreach (var val in sourceObject.Value)
                {
                    var result = GetNumberOfHops(val, destination, numberOfHops, connectionsByGroup, visited);
                    if (result.Key)
                    {
                        return result;
                    }
                }
            }

            return new KeyValuePair<bool, int>(false, numberOfHops);
        }
    }
}
