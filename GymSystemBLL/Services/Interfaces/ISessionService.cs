using GymSystemBLL.ViewModels.SeesionsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    internal interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel GetSessionById(int id);

        bool CreateSession(CreateSessionViewModel CreatedSession);

        UpdateSessionViewModel GetSessionForUpdate(int id);
        bool UpdateSession(UpdateSessionViewModel UpdatedSession, int id);

        bool DeleteSession(int id);



    }
}
