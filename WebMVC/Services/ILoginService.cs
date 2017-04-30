using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.ViewModels;
using SharedProject;

namespace WebMVC.Services
{
    public interface ILoginService
    {
        Task<IEnumerable<AcctRaw>> GetAcctRawAsync();
        Task<IEnumerable<LoginStatus>> GetWithStatusAsync(bool asAdministrator = false);
        Task<bool> ResetPasswordAsync(LoginBindingModel model);
        Task<CreateLoginResult> CreateNewLoginAsync(string loginName, string password);
        Task<bool> SetMonthlyTrafficAsync(LoginConfigureBindingModel model);
    }
}
