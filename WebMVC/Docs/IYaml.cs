using System.Collections.Generic;

namespace WebMVC.Docs
{
    public interface IYaml
    {
        bool IsYamlFileExist { get; }
        List<Article> ReadGraph();
        void WriteGraph();
        void WriteGraph(List<Article> articles);
    }
}