using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonProject;
using COURSE_CODING.Areas.Admin.Models;
using COURSE_CODING.Common;
using DAO.DAO;
using DAO.EF;

namespace COURSE_CODING.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private UserDAO UDAO;
        public UserController()
        {
            UDAO = new UserDAO();
        }
        public ActionResult Index(string searchString, int? page, int pageSize = 5)
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

        private void AddDataCombobox(int ? selectedStatus=null, int ? selectedRole=null)
        {
            List<StatusDefinition> statusDefinition = new List<StatusDefinition>();
            statusDefinition.Add(new StatusDefinition(CommonConstant.STATUS_LOCK_ACCOUNT, "Locked"));
            statusDefinition.Add(new StatusDefinition(CommonConstant.STATUS_RIGHT_ACCOUNT, "Not Lock"));
            List<RoleDefinition> roleDefinition = new List<RoleDefinition>();
            roleDefinition.Add(new RoleDefinition(CommonConstant.ROLE_ADMIN, "Admin"));
            roleDefinition.Add(new RoleDefinition(CommonConstant.ROLE_MEMBER, "Member"));
            roleDefinition.Add(new RoleDefinition(CommonConstant.ROLE_TEACHER, "Teacher"));
            SelectList statusList = new SelectList(statusDefinition, "ID", "NameStatus",selectedStatus);
            SelectList roleList = new SelectList(roleDefinition, "ID", "NameRole",selectedRole);
            ViewBag.statusList = statusList;
            ViewBag.roleList = roleList;
        }

        [HttpGet]
        public ActionResult Create()
        {
            AddDataCombobox();
            return View(); 
        }

        // POST: Admin/User/Create
        [HttpPost]
        public ActionResult Create(UserModel model)
        {
          
                if (ModelState.IsValid)
                {
                    var DAO = new UserDAO();
                    if (DAO.CheckUserNameExist(model.UserName))
                    {
                        ModelState.AddModelError(String.Empty, "This user name is existed");
                    }
                    else if (DAO.CheckUserNameExist(model.Email))
                    {
                        ModelState.AddModelError(String.Empty, "Your email is registerd");
                    }
                    else
                    {
                        USER_INFO u = new USER_INFO();
                        u.FirstName = model.FirstName;
                        u.LastName = model.LastName;
                        u.UserName = model.UserName;
                        u.PasswordUser = HashMD5.HashStringMD5(model.PasswordUser);
                        u.RoleUser = model.RoleUser;
                        u.StatusUser = model.StatusUser;
                        u.Country = model.Country;
                        u.Email = model.Email;
                        u.CreateDate = DateTime.Now;
                        u.About = model.About;
                        u.SchoolID = 1;     
                        u.YearGraduation = model.YearGraduation;
                        Boolean result = DAO.Insert(u);
                        if (result)
                        {
                            var sessionLogin = new InfoLogIn();
                            sessionLogin.ID = u.ID;
                            sessionLogin.Name = u.UserName;
                            Session.Add(CommonConstant.SESSION_INFO_LOGIN, sessionLogin);
                            ViewBag.Success = "Create sussesfull";
                            model = new UserModel();
                            return Redirect("/User/Dashboard");
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, "Fail register your account");
                        }
                    }
                }
                AddDataCombobox();
                return View("Create");
           
        }

        [HttpGet]
        // GET: Admin/User/Edit/5
        public ActionResult Edit(int id)
        {
            UserDAO UDAO = new UserDAO();
            USER_INFO user = UDAO.GetUserById(id);
            UserModel model = new UserModel();
            model.ID = user.ID;
            model.UserName = user.UserName;
            model.LastName = user.LastName;
            model.FirstName = user.FirstName;
            model.Email = user.Email;
            model.PasswordUser = user.PasswordUser;
            model.Country = user.Country;
            model.YearGraduation = user.YearGraduation;
            //model.RoleUser = user.RoleUser;
            //model.StatusUser = user.StatusUser;
            AddDataCombobox(user.StatusUser, user.RoleUser);
            model.About = user.About;
            model.ComfirmPasswordUser = user.PasswordUser;
            return View(model);
        }


        // POST: Admin/User/Edit/5
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var DAO = new UserDAO();
                if (DAO.CheckUserNameExist(model.UserName))
                {
                    ModelState.AddModelError(String.Empty, "This user name is existed");
                }
                else if (DAO.CheckUserNameExist(model.Email))
                {
                    ModelState.AddModelError(String.Empty, "Your email is registerd");
                }
                else
                {
                    USER_INFO user = UDAO.GetUserById(model.ID);
                    user.UserName = model.UserName;
                    user.LastName = model.LastName;
                    user.FirstName = model.FirstName;
                    user.Email = model.Email;
                    user.PasswordUser = model.PasswordUser;
                    user.Country = model.Country;
                    user.YearGraduation = model.YearGraduation;
                    user.RoleUser = model.RoleUser;
                    user.StatusUser = model.StatusUser;
                    user.About = model.About;
                    Boolean result = UDAO.Update(user);
                    if (result)
                    {
                        var sessionLogin = new InfoLogIn();
                        sessionLogin.ID = user.ID;
                        sessionLogin.Name = user.UserName;
                        Session.Add(CommonConstant.SESSION_INFO_LOGIN, sessionLogin);
                        ViewBag.Success = "Edit sussesfull";
                        model = new UserModel();
                        return Redirect("/User/Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Fail Edit this account");
                    }
                }
            }
            AddDataCombobox();
            return View("Edit");
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
