using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Windows
{
    public interface IServerService
    {
        Task<IEnumerable<ServerListItemViewModel>> GetServersAsync(string token);
    }
}
