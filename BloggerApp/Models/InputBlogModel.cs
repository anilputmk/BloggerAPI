using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloggerApp.Models
{
    public class InputBlogModel
    {
        public string BloggerId { get; set; }
        public BlogPost Post { get; set; }
    }
}
