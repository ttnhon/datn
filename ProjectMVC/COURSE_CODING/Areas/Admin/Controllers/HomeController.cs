using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonProject;
using CommonProject.Helper;
using COURSE_CODING.Models;
using DAO.DAO;

namespace COURSE_CODING.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.Title = "Dash board";
            GeneralDAO dao = new GeneralDAO();
            ViewBag.numberUser = dao.CountUsers();
            ViewBag.numberCompete = dao.CountCompetes();
            ViewBag.numberChallenge = dao.CountChallenges();
            ViewBag.numberLanguage = dao.CountLanguages();

            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

        public JsonResult GetAllDataUsers()
        {
            GeneralDAO dao = new GeneralDAO();
            return Json(
                dao.GetAllDataUsers()
                );
        }
        public JsonResult GetAllDataChallenges()
        {
            GeneralDAO dao = new GeneralDAO();
            return Json(
                dao.GetAllDataChallenges()
                );
        }
        public JsonResult GetAllDataCompetes()
        {
            GeneralDAO dao = new GeneralDAO();
            return Json(
                dao.GetAllDataCompetes()
                );
        }
        public JsonResult GetAllDataLanguages()
        {
            GeneralDAO dao = new GeneralDAO();
            return Json(
                dao.GetAllDataLanguages()
                );
        }

        [HttpPost]
        public ActionResult SendMail(EmailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string name = model.Name.ToString();
                    string phone = model.Mobile.ToString();
                    string email = model.Email.ToString();
                    string Text = model.Content.ToString();
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Page/pages/ContentMail.html"));
                    content = content.Replace("{{CustomerName}}", name);
                    content = content.Replace("{{Phone}}", phone);
                    content = content.Replace("{{Email}}", email);
                    content = content.Replace("{{Content}}", Text);
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new Email_Helper().SendMail(email, "Invitation from coursecoding", content);
                    SetAlert("Send mail successfull", "success");
                }

            }
            catch (Exception ex)

            {
                SetAlert("Send mail fail", "error");
            }
            return View("Mail");
        }
        public ActionResult About()
        {
            ViewBag.title = "About";
            return View();
        }
        public ActionResult ComfirmRequest(string searchString, int? page, int pageSize = 5)
        {
            var dao = new UserDAO();
            ViewBag.Title = "Comfirm request";
            var model = dao.ListAllPagingRequestAdmin(searchString, page ?? 1, pageSize);
            ViewBag.ChuoiTimKiem = searchString;
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var dao = new UserDataDAO();
                dao.Delete(id);
                return RedirectToAction("ComfirmRequest");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Accept(int id)
        {
            try
            {
                var userDataDao = new UserDataDAO();
                var userDao = new UserDAO();
                var user = userDao.GetUserById(id);
                userDao.UpdateRole(id, CommonConstant.ROLE_TEACHER);
                userDataDao.Delete(id);
                return RedirectToAction("ComfirmRequest");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Deny(int id)
        {
            try
            {
                var dao = new UserDataDAO();
                dao.Delete(id);
                return RedirectToAction("ComfirmRequest");
            }
            catch
            {
                return View();
            }
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }

        // GET: Admin/Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Admin/Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
