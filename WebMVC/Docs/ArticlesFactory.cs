using System;
using System.Collections.Generic;

namespace WebMVC.Docs
{
    public class ArticlesFactory
    {
        public static List<Article> Create()
        {
            return new List<Article>()
            {
                new Article
                {
                    Title = "Manual Setup for Windows 10",
                    TitleFull = "Manual Setup for Windows 7",
                    Category = "Windows Setup",
                    FileName = "ManualSetupforWindows10.md",
                    Tags = new List<string>() { "windows", "windows 10" },
                    Created = new DateTime(2017,04,30),
                    Updated = new DateTime(2017,04,30),
                },
                new Article
                {
                    Title = "Manual Setup for Windows 7",
                    TitleFull = "Manual Setup for Windows 7",
                    Category = "Windows Setup",
                    FileName = "ManualSetupforWindows7.md",
                    Tags = new List<string>() { "windows", "windows 10" },
                    Created = new DateTime(2017,04,30),
                    Updated = new DateTime(2017,04,30),
                },
                new Article
                {
                    Title = "Manual Setup for Windows 8",
                    TitleFull = "Manual Setup for Windows 8",
                    Category = "Windows Setup",
                    FileName = "ManualSetupforWindows8.md",
                    Tags = new List<string>() { "windows", "windows 8" },
                    Created = new DateTime(2017,04,30),
                    Updated = new DateTime(2017,04,30),
                },
                new Article
                {
                    Title = "Manual Setup for iPhone",
                    TitleFull = "Manual Setup for iPhone",
                    Category = "Mobile Device Setup",
                    FileName = "ManualSetupforiPhone.md",
                    Tags = new List<string>() { "iOS", "iPhone" },
                    Created = new DateTime(2017,04,30),
                    Updated = new DateTime(2017,04,30),
                },
                new Article
                {
                    Title = "Manual Setup for Android",
                    TitleFull = "Manual Setup for Android",
                    Category = "Mobile Device Setup",
                    FileName = "ManualSetupforAndroid.md",
                    Tags = new List<string>() { "Android" },
                    Created = new DateTime(2017,04,30),
                    Updated = new DateTime(2017,04,30),
                }
            };
        }
    }
}
