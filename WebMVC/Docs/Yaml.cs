using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Docs
{
    public class Yaml : IYaml
    {
        private readonly string _yamlPath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "articles.yml");
        private readonly Serializer serializer = new Serializer();

        public bool IsYamlFileExist => File.Exists(_yamlPath);

        public void WriteGraph()
        {
            var text = serializer.Serialize(ArticlesFactory.Create());
            File.WriteAllText(_yamlPath, text);
        }

        public void WriteGraph(List<Article> articles)
        {
            var text = serializer.Serialize(articles);
            File.WriteAllText(_yamlPath, text);
        }

        public List<Article> ReadGraph()
        {
            var text = File.ReadAllText(_yamlPath);
            var ar = serializer.Deserialize<List<Article>>(text);
            return ar;
        }
    }
}
