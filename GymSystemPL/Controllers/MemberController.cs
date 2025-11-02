using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace GymSystemPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #region Get All members

        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        #endregion

        #region Get Member info
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number.";
                return RedirectToAction(nameof(Index));
            }
            var memberDetails = _memberService.GetMemberDeatails(id);
            if (memberDetails == null)
            {
                TempData["ErrorMessage"] = "Member Not found !.";
                return RedirectToAction(nameof(Index));
            }
            return View(memberDetails);
        }
        #endregion

        #region Get Health Record Info

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number.";
                return RedirectToAction(nameof(Index));
            }
            var healthRecordDetails = _memberService.GetMemberHealthRecord(id);
            if (healthRecordDetails == null)
            {
                TempData["ErrorMessage"] = "Health Record Not found !.";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecordDetails);
        }

        #endregion

        #region Create Member

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Fields.");
                return View(nameof(Create), CreatedMember);
            }

            bool Result = _memberService.CreateMember(CreatedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Member";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Member

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be 0 or negative !";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberToUpdate(id);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member not found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel MemberToUpdate)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvaild", "Check Data And missing fields");
                return View(MemberToUpdate);
            }

            var Result = _memberService.UpdateMember(id, MemberToUpdate);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Member";
            }
            return RedirectToAction(nameof(Index));
        }
        #region Debug

        //[HttpPost]
        //public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel MemberToUpdate)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                // Log model state errors for diagnosis
        //                var errors = ModelState
        //                    .Where(kvp => kvp.Value.Errors.Count > 0)
        //                    .Select(kvp => new
        //                    {
        //                        Key = kvp.Key,
        //                        Errors = kvp.Value.Errors.Select(e => e.ErrorMessage + (e.Exception != null ? " | Ex: " + e.Exception.Message : ""))
        //                    });

        //                foreach (var e in errors)
        //                {
        //                    Console.WriteLine($"{e.Key}: {string.Join(", ", e.Errors)}");
        //                }
        //                return View(MemberToUpdate);
        //            }

        //            var Result = _memberService.UpdateMember(id, MemberToUpdate);
        //            if (Result)
        //            {
        //                TempData["SuccessMessage"] = "Member Created Successfully";
        //            }
        //            else
        //            {
        //                TempData["ErrorMessage"] = "Failed to Create Member";
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }

        #endregion
        #endregion

        #region Delete Member

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be 0 or negative !";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberDeatails(id);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member not found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = Member.Name;
            return View();
        }

        public ActionResult DeleteConfirmed([FromForm]int id) 
        {

            bool Result = _memberService.DeleteMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Member";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
