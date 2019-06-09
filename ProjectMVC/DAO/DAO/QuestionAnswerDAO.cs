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
    }
}
