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
        /// Get one challenge
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<QUESTION> GetAllByChallengeID(int id)
        {
            return db.QUESTIONS.Where(table => table.CompeteID == id).ToList();
        }
    }
}
