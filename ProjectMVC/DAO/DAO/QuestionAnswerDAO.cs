using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace DAO.DAO
{
    public class QuestionAnswerDAO
    {
        DCCourseCoding db = null;
        public QuestionAnswerDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// insert answer of user to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean Insert(QUESTION_ANSWER entity)
        {
            try
            {
                db.QUESTION_ANSWERS.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// insert or update answer of user to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean InsertOrUpdate(QUESTION_ANSWER entity)
        {
            try
            {
                QUESTION_ANSWER update_answer = db.QUESTION_ANSWERS.Where(table => table.QuestionID == entity.QuestionID)
                        .Where(table => table.UserId == entity.UserId).FirstOrDefault();
                if (update_answer != null)
                {
                    update_answer.Content = entity.Content;
                    update_answer.Result = entity.Result;
                    update_answer.TimeDone = entity.TimeDone;
                    db.SaveChanges();
                }
                else
                {
                    db.QUESTION_ANSWERS.Add(entity);
                    db.SaveChanges();
                }
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
