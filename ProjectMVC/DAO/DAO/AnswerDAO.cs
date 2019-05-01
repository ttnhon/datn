using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace DAO.DAO
{
    public class AnswerDAO
    {
        DCCourseCoding db = null;
        public AnswerDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// insert answer of user to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean Insert(ANSWER entity)
        {
            try
            {
                db.ANSWERS.Add(entity);
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