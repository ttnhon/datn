using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.EF;

namespace DAO.DAO
{
    public class QuestionDAO
    {
        DCCourseCoding db = null;
        public QuestionDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// Insert to table QUESTION
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(QUESTION entity)
        {
            try
            {
                db.QUESTIONS.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        /// <summary>
        /// Get one challenge
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<QUESTION> GetAllByCompeteID(int id)
        {
            return db.QUESTIONS.Where(table => table.CompeteID == id).ToList();
        }
    }
}
