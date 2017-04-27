using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.RouterReporter
{
    public static class SSCommand
    {
        public static byte[] AddUser(int port, string username, string password)
        {
            var bytes = Encoding.ASCII.GetBytes($@"add: {{""server_port"": {port}, ""password"":""{username}{password}""}}");
            return bytes;
        }

        public static byte[] RemoveUser(int port) => Encoding.ASCII.GetBytes($@"remove: {{""server_port"": {port}}}");

        public static byte[] Ping => Encoding.ASCII.GetBytes("ping");
    }
}
