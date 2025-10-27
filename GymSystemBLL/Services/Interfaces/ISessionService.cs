using GymSystemBLL.ViewModels.SeesionsViewModel;
using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel GetSessionById(int id);

        bool CreateSession(CreateSessionViewModel CreatedSession);

        UpdateSessionViewModel GetSessionForUpdate(int id);
        bool UpdateSession(UpdateSessionViewModel UpdatedSession, int id);

        bool DeleteSession(int id);

        IEnumerable<TrainerSelectViewModel> GetTrainerForSessions();
        IEnumerable<CategorySelectViewModel> GetCategoryForSessions();

    }
}
