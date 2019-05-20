using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonProject;

namespace COURSE_CODING.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //API_Helper apiHelper = new API_Helper();
            //Source src = new Source();
            //src.stringSource = "Write.console";
            //src.versionFramework = "2.3";
            //src.userKey = "01";
            //var result = apiHelper.RequestAPI(CommonConstant.TYPE_CSHARP_COMPILER, src);

           // return Redirect("/User/Dashboard");
            return Redirect("/Authen/login");
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}