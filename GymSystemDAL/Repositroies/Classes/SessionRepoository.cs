using GymSystemDAL.Data.Context;
using GymSystemDAL.Models;
using GymSystemDAL.Repositroies.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Classes
{
    public class SessionRepoository : GenericRepository<Session> , ISessionRepoository
    {
        private readonly GymSystemDbContext _dbContext;

        public SessionRepoository(GymSystemDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Session> GetAllSessionsWithDetails()
        {
            return _dbContext.Sessions
                .Include(s => s.SessionTrainer)
                .Include(s => s.SessionCategory)
                .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(X => X.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext.Sessions
                .Include(s => s.SessionTrainer)
                .Include(s => s.SessionCategory)
                .FirstOrDefault(s => s.Id == sessionId);
        }
    }
}
