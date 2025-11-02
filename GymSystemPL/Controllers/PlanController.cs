using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.PlansViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class PlanController : Controller
    {
        #region 

        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        #endregion
        #region Index

        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }
        #endregion

        #region Details

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or negative Number!";
                return RedirectToAction(nameof(Index));

            }
            var Plan = _planService.GetPlanById(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }


        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or negative Number!";
                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanToUpdate(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel UpdatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Again!");
                return View("UpdatedPlan");
            }
            var Result = _planService.UpdatePlan(id, UpdatedPlan);
            if (!Result)
            {
                TempData["ErrorMessage"] = "Failt to Update Plan!";
                return RedirectToAction(nameof(Index));

            }
            TempData["SucessMessage"] = "Plan Updated Successfully!";
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Soft Delete

        [HttpPost]
        public ActionResult Active(int id)
        {
            var Res = _planService.ToggleStatus(id);
            if (Res)
            {
                TempData["SucessMessage"] = "Plan Status Changed Successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failt to Change Plan Status!";
                return RedirectToAction(nameof(Index));
            }
            #endregion
        }
    }
}