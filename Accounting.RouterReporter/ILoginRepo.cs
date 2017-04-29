using SharedProject;
using System.Collections.Generic;

namespace Accounting.RouterReporter
{
    public interface IRepo
    {
        IEnumerable<Login> GetLiveUsers();
        void InsertSSEventRaws(IEnumerable<SSEventraw> sseventraw);
    }
}