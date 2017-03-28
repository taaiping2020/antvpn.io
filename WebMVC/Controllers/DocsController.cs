using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebMVC.Extensions;
using System.IO;
using Markdig;

namespace WebMVC.Controllers
{
    public class DocsController : Controller
    {
        public DocsController()
        {
            if (!System.IO.Directory.Exists(_markdownBasePath))
            {
                Directory.CreateDirectory(_markdownBasePath);
            }
        }
        private static readonly string _markdownBasePath = Path.Combine(Directory.GetCurrentDirectory(), "Content");
        public IActionResult Index(string id)
        {
            if (id == null)
            {
                id = "haha";
            }
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var fileName = Path.Combine(_markdownBasePath, id + ".md");
            if (!System.IO.File.Exists(fileName))
            {
                return NotFound("file not exists.");
            }
            var md = System.IO.File.ReadAllText(fileName);
            var html = Markdown.ToHtml(md);
            ViewData["Title"] = id;
            return View((object)html);
        }
    }
}
