using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Interfaces
{
    public interface ISessionRepoository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithDetails();

        int GetCountOfBookedSlots(int sessionId);

        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
