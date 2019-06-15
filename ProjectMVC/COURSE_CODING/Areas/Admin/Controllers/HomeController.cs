using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonProject.Helper;
using COURSE_CODING.Models;

namespace COURSE_CODING.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.Title = "Dash board";
            return View();
        }
        public ActionResult Contact()
        {
            return View();
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

        // GET: Admin/Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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
