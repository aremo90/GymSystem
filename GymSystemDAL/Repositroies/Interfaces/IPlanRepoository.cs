using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Interfaces
{
    public interface IPlanRepoository
    {
        Plan? GetPlanById(int id);
        IEnumerable<Plan> GetAllPlans();
        int updatePlan(Plan plan);
    }
}
