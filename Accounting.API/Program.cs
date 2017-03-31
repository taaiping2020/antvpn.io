using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using System.Net.Http;

namespace Accounting.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
            var autoEvent = new AutoResetEvent(false);
            timer = new Timer(CallTaskJob, autoEvent, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));

            host.Run();
        }

        static Timer timer;
        public static void CallTaskJob(object state)
        {
            try
            {
                var client = new HttpClient();
                client.GetStringAsync("http://localhost:5200/api/task");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"job fail. message: {ex.Message}");
            }
           
        }
    }
}
