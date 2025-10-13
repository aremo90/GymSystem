using GymSystemDAL.Data.Context;
using GymSystemDAL.Data.Repositroies.Interfaces;
using GymSystemDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Repositroies.Classes
{
    public class PlanRepository : IPlanRepoository
    {
        private readonly GymSystemDbContext _dbContext;

        public PlanRepository(GymSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Plan> GetAllPlans()
        {
            return _dbContext.Plans.ToList();
        }

        public Plan? GetPlanById(int id)
        {
            return _dbContext.Plans.Find(id);
        }

        public int updatePlan(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }
    }
}
