using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MemberViewModels;
using GymSystemBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get All Trainers

        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        #endregion

        #region Create Trainer

        public ActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Fields.");
                return View(nameof(CreateTrainer), createdTrainer);
            }

            bool result = _trainerService.CreateTrainer(createdTrainer);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Get Trainer Details

        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number.";
                return RedirectToAction(nameof(Index));
            }
            var trainerDetails = _trainerService.GetTrainerDetails(id);
            if (trainerDetails == null)
            {
                TempData["ErrorMessage"] = "Trainer Not found !.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainerDetails);
        }

        #endregion

        #region Edit Trainer

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {   
                TempData["ErrorMessage"] = "Id Cannot be 0 or negative !";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found !";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel updatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvaild", "Check Data And missing fields");
                return View(updatedTrainer);
            }

            var result = _trainerService.UpdateTrainerDetails(updatedTrainer, id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Trainer";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Delete Trainer

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be 0 or negative !";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            bool result = _trainerService.RemoveTrainer(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
