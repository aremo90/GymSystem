using GymSystemBLL.ViewModels.PlansViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();

        PlanViewModel GetPlanById(int id);

        UpdatePlanViewModel? GetPlanToUpdate(int id);

        bool UpdatePlan(int id, UpdatePlanViewModel UpdatedPlan);
        bool ToggleStatus(int id);
    }
}
