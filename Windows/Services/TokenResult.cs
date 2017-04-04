using System;

namespace Windows
{
    public class TokenResult
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string token_type { get; set; }
        public DateTime created { get; set; }
    }

}
