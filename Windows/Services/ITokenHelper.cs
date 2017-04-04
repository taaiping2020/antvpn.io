using System.Threading.Tasks;

namespace Windows
{
    public interface ITokenHelper
    {
        Task<TokenResult> GetBearerTokenAsync(string username, string password);
    }
}