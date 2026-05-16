// Controllers/LeaveController.cs
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LeaveTracker.Models;

namespace LeaveTracker.Controllers
{
    public class LeaveController : Controller
    {
        // GET: /Leave/  — Dashboard with stats
        public ActionResult Index()
        {
            ViewBag.Stats  = DbHelper.GetDashboardStats();
            ViewBag.Leaves = DbHelper.GetAllLeaves();
            return View();
        }

        // GET: /Leave/Filter?status=Pending
        public ActionResult Filter(string status)
        {
            ViewBag.Stats        = DbHelper.GetDashboardStats();
            ViewBag.Leaves       = DbHelper.GetAllLeaves(status);
            ViewBag.ActiveFilter = status;
            return View("Index");
        }

        // GET: /Leave/Apply
        public ActionResult Apply()
        {
            PopulateEmployeeDropdown();
            return View(new LeaveRequest { StartDate = DateTime.Today, EndDate = DateTime.Today });
        }

        // POST: /Leave/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apply(LeaveRequest model)
        {
            if (model.EndDate < model.StartDate)
                ModelState.AddModelError("EndDate", "End date cannot be before start date.");

            if (ModelState.IsValid)
            {
                DbHelper.ApplyLeave(model);
                TempData["Success"] = "Leave request submitted successfully.";
                return RedirectToAction("Index");
            }

            PopulateEmployeeDropdown();
            return View(model);
        }

        // GET: /Leave/Details/5
        public ActionResult Details(int id)
        {
            var leave = DbHelper.GetLeaveById(id);
            if (leave == null) return HttpNotFound();
            return View(leave);
        }

        // POST: /Leave/Approve/5
        [HttpPost]
        public ActionResult Approve(int id)
        {
            DbHelper.UpdateLeaveStatus(id, "Approved");
            TempData["Success"] = "Leave approved.";
            return RedirectToAction("Index");
        }

        // POST: /Leave/Reject/5
        [HttpPost]
        public ActionResult Reject(int id)
        {
            DbHelper.UpdateLeaveStatus(id, "Rejected");
            TempData["Success"] = "Leave rejected.";
            return RedirectToAction("Index");
        }

        // POST: /Leave/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            DbHelper.DeleteLeave(id);
            TempData["Success"] = "Leave request deleted.";
            return RedirectToAction("Index");
        }

        // GET: /Leave/ByEmployee/2
        public ActionResult ByEmployee(int id)
        {
            var emp    = DbHelper.GetEmployeeById(id);
            if (emp == null) return HttpNotFound();
            var leaves = DbHelper.GetLeavesByEmployee(id);
            ViewBag.Employee = emp;
            return View(leaves);
        }

        // ── Helpers ──────────────────────────────────────────────────────────
        private void PopulateEmployeeDropdown()
        {
            var employees = DbHelper.GetAllEmployees();
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "FullName");
        }
    }
}
