using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows
{
    public interface IVpnManagementAgent
    {
        Task AddOrUpdateProfileAsync(string serverAddress);
        Task<VpnManagementErrorStatus> ConnectProfileAsync(string username, string password);
        Task<VpnManagementErrorStatus> DisconnectProfileAsync();
        Task<VpnManagementErrorStatus> DeleteProfileAsync();
    }
}
