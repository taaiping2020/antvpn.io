namespace Login.Sqlserver
{
    public class LoginRepo
    {
        //private readonly HttpClient client;
        private readonly string[] loginServers;
        public LoginRepo(params string[] loginServers)
        {
            //if (!loginServers?.Any() ?? throw new ArgumentNullException())
            //{
            //    throw new ArgumentNullException();
            //}
            this.loginServers = loginServers;
            //this.client = new HttpClient();
        }
        //public void CreateLogin(Login login)
        //{
        //    foreach (var ls in loginServers)
        //    {
        //        var response = client.PostAsJsonAsync(ls + "/api/Users", login);
        //        response.EnsureSuccessStatusCode();
        //    }
        //}
    }
}
