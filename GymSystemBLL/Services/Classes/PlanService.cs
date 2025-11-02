using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.PlansViewModels;
using GymSystemDAL.Models;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        #region DB Coneect

        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion
        #region crud methods
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return [];
            return plans.Select(plan => new PlanViewModel
            {
                ID = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            });
        }
        public PlanViewModel GetPlanById(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetByID(id);
            if (plan == null) return null!;
            return new PlanViewModel
            {
                ID = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }
        public UpdatePlanViewModel? GetPlanToUpdate(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetByID(id);
            if (plan is null || plan.IsActive == false || HasActiveMemberships(id)) return null;
            return new UpdatePlanViewModel
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };
        }
        public bool UpdatePlan(int id, UpdatePlanViewModel UpdatedPlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetByID(id);
            if (plan is null || HasActiveMemberships(id)) return false;
            try
            {
                // Truples
                (plan.Description, plan.DurationDays, plan.Price, plan.Name, plan.UpdatedAt) =
                    (UpdatedPlan.Description, UpdatedPlan.DurationDays, UpdatedPlan.Price, UpdatedPlan.PlanName, DateTime.Now);
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ToggleStatus(int id)
        {
            var repo = _unitOfWork.GetRepository<Plan>();

            var plan = repo.GetByID(id);

            if (plan is null || HasActiveMemberships(id)) return false;

            plan.IsActive = plan.IsActive == true ? false : true;

            plan.UpdatedAt = DateTime.Now;

            try
            {
                repo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion
        #region Helper

        private bool HasActiveMemberships(int planId)
        {
           var ActiveMemberShips = _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.PlanId == planId && m.Status == "Active");
            return ActiveMemberShips.Any();
        }

        #endregion
    }
}
