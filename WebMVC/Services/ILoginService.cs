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
        Task<bool> ResetPasswordAsync(string userId, LoginBindingModel model);
        Task<bool> CreateNewLoginAsync(string userId, string loginName, string password);
        Task<bool> SetMonthlyTrafficAsync(string userId, LoginConfigureBindingModel model);
    }
}
