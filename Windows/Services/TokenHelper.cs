using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace Windows
{
    public partial class TokenHelper : ITokenHelper
    {
        public readonly string TokenEndPoint;
        private readonly string TokenFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "access_token.txt");
        public TokenHelper()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["IdentityUrl"];
            this.TokenEndPoint = url + "/connect/token";
        }

        public async Task<TokenResult> GetBearerTokenAsync(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(this.TokenEndPoint, new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = "wpf",
                ["grant_type"] = "password",
                ["username"] = username,
                ["password"] = password,
                ["client_secret"] = "secret"
            }));
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResult>(json);
                result.created = DateTime.Now;
                return result;
            }
            return null;
        }

        public void WriteTokenToDisk(TokenResult token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            File.WriteAllText(this.TokenFilePath, JsonConvert.SerializeObject(token));
        }

        public TokenResult ReadTokenFromDisk()
        {
            if (!File.Exists(this.TokenFilePath))
            {
                return null;
            }
            var json = File.ReadAllText(this.TokenFilePath);
            var token = JsonConvert.DeserializeObject<TokenResult>(json);
            if (token.created.AddSeconds(token.expires_in) < DateTime.Now)
            {
                return null;
            }
            return token;
        }
    }
}
