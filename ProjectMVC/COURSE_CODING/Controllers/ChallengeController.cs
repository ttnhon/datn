using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COURSE_CODING.Models;
using DAO.DAO;

namespace COURSE_CODING.Controllers
{
    public class ChallengeController : Controller
    {
        // GET: Challenge
        public ActionResult Problem()
        {
            ChallengeModel model = new ChallengeModel()
            {
                Title = "Input and Output",
                Description = "In this challenge, we're practicing reading input from stdin and printing output to stdout.\n"
                + "In C++, you can read a single whitespace-separated token of input using cin, and print output to stdout using cout. For example, let's say we declare the following variables:\n"
                + "string s;\n" 
                + "int n;\n"
                + ".....\n"
                + "Task\n"
                + " Read 3 numbers from stdin and print their sum to stdout.",
                InputFormat = "A single line containing 3 space-separated integers: a, b, and c.",
                OutputFormat = "Print the sum of the three numbers on a single line.",
                ChallengeDifficulty = 0,
                Constraints = "",
                Score = 5,

            };
            return View(model);
        }

        // GET: Challenge/edit/:id
        public ActionResult Edit(int id)
        {
            EditChallengeModel model = new EditChallengeModel()
            {
                Slug = "test-1",
                Name = "test 1"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditChallengeModel model)
        {
            return null;
        }

        [HttpPost]
        public JsonResult UpdateDetails(EditChallengeModel model)
        {
            return Json(new { model });
        }
    }
}