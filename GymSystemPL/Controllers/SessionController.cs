using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MemberViewModels;
using GymSystemBLL.ViewModels.PlansViewModels;
using GymSystemBLL.ViewModels.SeesionsViewModel;
using GymSystemDAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            var Session = _sessionService.GetAllSessions();
            return View(Session);
        }

        #region Details

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or negative Number!";
                return RedirectToAction(nameof(Index));

            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            LoadDropDownsTrainer();
            LoadDropDownsCategories();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel CreatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsTrainer();
                LoadDropDownsCategories();
                return View(nameof(Create), CreatedSession);
            }

            bool Result = _sessionService.CreateSession(CreatedSession);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                LoadDropDownsTrainer();
                LoadDropDownsCategories();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Session";
                LoadDropDownsTrainer();
                LoadDropDownsCategories();
                return RedirectToAction(nameof(Index));
            }
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
            var Session = _sessionService.GetSessionForUpdate(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not found !";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownsTrainer();
            return View(Session);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel UpdatedSession)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Again!");
                return View("UpdatedPlan");
            }
            var Result = _sessionService.UpdateSession(UpdatedSession, id);
            if (!Result)
            {
                TempData["ErrorMessage"] = "Failt to Update Plan!";
                return RedirectToAction(nameof(Index));

            }
            TempData["SucessMessage"] = "Plan Updated Successfully!";
            return RedirectToAction(nameof(Index));

        }


        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be 0 or negative !";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {

            bool Result = _sessionService.DeleteSession(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Session";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Helper

        private void LoadDropDownsTrainer()
        {
            var Trainers = _sessionService.GetTrainerForSessions();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
            ModelState.AddModelError("DataInValid", "Check Data And Missing Fields.");
        }
        private void LoadDropDownsCategories()
        {
            var Categories = _sessionService.GetCategoryForSessions();
            ViewBag.Categories = new SelectList(Categories, "Id", "CategoryName");
            ModelState.AddModelError("DataInValid", "Check Data And Missing Fields.");
        }

        #endregion
    }
}
