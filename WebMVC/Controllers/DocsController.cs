using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebMVC.Extensions;
using System.IO;
using Markdig;
using WebMVC.Docs;

namespace WebMVC.Controllers
{
    public class DocsController : Controller
    {
        private readonly IYaml _yaml;
        public DocsController(IYaml yaml)
        {
            _yaml = yaml;

            if (!_yaml.IsYamlFileExist)
            {
                _yaml.WriteGraph();
            }
        }
        static DocsController()
        {
            if (!System.IO.Directory.Exists(_markdownBasePath))
            {
                Directory.CreateDirectory(_markdownBasePath);
            }

        }
        private static readonly string _markdownBasePath = Path.Combine(Directory.GetCurrentDirectory(), "Content");
        public IActionResult Index()
        {
            var articles = _yaml.ReadGraph();
            if (articles == null || !articles.Any())
            {
                return NotFound();
            }

            var gArticles = articles.GroupBy(c => c.Category);

            return View(gArticles);
        }

        [Route("Articles/{title}")]
        public IActionResult Articles(string title)
        {
            if (String.IsNullOrEmpty(title))
            {
                return NotFound();
            }

            var articles = _yaml.ReadGraph();
            //_yaml.WriteGraph(articles);
            var article = articles.FirstOrDefault(c => c.Title == title.Replace("-", " "));
            if (article == null)
            {
                return NotFound();
            }
            var fileName = Path.Combine(_markdownBasePath, article.FileName + ".md");
            if (!System.IO.File.Exists(fileName))
            {
                return NotFound("file not exists.");
            }
            var md = System.IO.File.ReadAllText(fileName);
            var html = Markdown.ToHtml(md);

            ViewBag.Content = AddImgResponsiveAttribute(html);

            return View(article);
        }

        public string AddImgResponsiveAttribute(string text)
        {
            return text?.Replace("<img", "<img class=\"img-responsive\"");
        }
    }
}
