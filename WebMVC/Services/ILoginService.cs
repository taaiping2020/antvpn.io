using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.ViewModels;

namespace WebMVC.Services
{
    public interface ILoginService
    {
        Task<IEnumerable<LoginStatus>> GetWithStatusAsync(string userId);
        Task ResetPasswordAsync(string userId, string loginName);
        Task CreateNewLoginAsync(string userId, string loginName, string password);
    }
}
