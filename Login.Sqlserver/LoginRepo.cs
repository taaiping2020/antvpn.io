using Microsoft.SqlServer.Server;
using System.IO;
using System.Net;

namespace Login.Sqlserver
{
    public class LoginRepo
    {
        private readonly string[] loginServers;
        public LoginRepo(params string[] loginServers)
        {
            this.loginServers = loginServers;
        }
        public void Apply(Login login, string httpMethod, bool allowFail, SqlPipe pipe)
        {
            pipe.Send(login.ToString());
            try
            {
                foreach (var ls in loginServers)
                {
                    var request = (HttpWebRequest)WebRequest.Create(ls + "/api/users");
                    request.ContentType = "application/json; charset=utf-8";
                    request.Method = httpMethod;

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = $@"{{""name"": ""{login.LoginName}"",""password"": ""{login.Password}"",""enabled"": {login.Enabled.ToString().ToLower()},""allowDialIn"": {login.AllowDialIn.ToString().ToLower()},""groupName"": ""{login.GroupName}""}}";
                        pipe.Send(json);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }

                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }
            }
            catch (System.Exception ex)
            {
                if (!allowFail)
                {
                    throw ex;
                }
            }
         
        }
    }
}
