using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO.DAO;

namespace COURSE_CODING.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index(string searchString, int? page, int pageSize = 2)
        {
            var dao = new UserDAO();
            var model = dao.ListAllPaging(searchString, page ?? 1, pageSize);
            ViewBag.ChuoiTimKiem = searchString;
            return View(model);
        }

        // GET: Admin/User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
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

        // GET: Admin/User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/User/Edit/5
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


        // POST: Admin/User/Delete/5
       
        public ActionResult Delete(int id)
        {
            try
            {
                var dao = new UserDAO();
                dao.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult UpdateStatus(int id)
        {
            try
            {
                var dao = new UserDAO();
                int result = dao.UpdateStatus(id);
                return Json(new
                {
                    status = result
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = ""
                });
            }
        }
    }
}
