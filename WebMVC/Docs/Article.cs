using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Docs
{
    public class Article
    {
        public string Title { get; set; }
        public string TitleFull { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string FileName { get; set; }
        public List<string> Tags { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
