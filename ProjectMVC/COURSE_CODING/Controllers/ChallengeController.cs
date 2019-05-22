using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COURSE_CODING.Models;
using DAO.DAO;

namespace COURSE_CODING.Controllers
{
    public class ChallengeController : BaseController
    {
        // GET: Challenge
        public ActionResult Problem( int id)
        {
            
            //Prepare model
            ChallengeModel model = new ChallengeModel();

            //Fill data
            model.challenge = (new ChallengeDAO()).GetOne(id);
            model.OwnerName = (new UserDAO()).GetNameByID(model.challenge.OwnerID);
            model.languages = (new LanguageDAO()).GetByChallengeID(id);

            return View(model);
        }

        public ActionResult Discussion(int id)
        {
            var models = new CommentListModel();

            //Lay user login info
            models.Info = (new UserDAO().GetUserById(2));
            models.challenge = (new ChallengeDAO().GetOne(id));
            models.comments = new List<CommentModel>();

            var commentList = (new CommentDAO().GetAllByChallenge(id));
            if(commentList.Count > 0)
            {
                for (int i = 0; i < commentList.Count; i++)
                {
                    var model = new CommentModel();
                    model.comment = commentList[i];
                    model.owner = (new UserDAO().GetUserById(model.comment.OwnerID));
                    model.replies = new List<ReplyModel>();

                    var replyList = (new ReplyDAO().GetAllByComment(model.comment.ID));
                    if (replyList.Count > 0)
                    {
                        for (int j = 0; j < replyList.Count; j++)
                        {
                            var reply = new ReplyModel();
                            reply.reply = replyList[i];
                            reply.owner = (new UserDAO().GetUserById(reply.reply.OwnerID));

                            model.replies.Add(reply);
                        }
                    }

                    models.comments.Add(model);
                }
            }

            return View(models);
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