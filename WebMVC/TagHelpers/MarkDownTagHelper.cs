using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using System.IO;

namespace WebMVC.TagHelpers
{
    public class MarkdownTagHelper : TagHelper
    {
        private static readonly string _markdownBasePath = Path.Combine(Directory.GetCurrentDirectory(), "Content");
        public string File { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var fileName = Path.Combine(_markdownBasePath, File + ".md");
            if (!System.IO.File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }
            var md = System.IO.File.ReadAllText(fileName);
            var html = Markdown.ToHtml(md);

            output.Content.SetHtmlContent(html);
        }
    }
}
