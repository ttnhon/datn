using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace DAO.DAO
{
    public class ChallengeDAO
    {
        DCCourseCoding db = null;
        public ChallengeDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// Get one challenge
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public CHALLENGE GetOne( int id)
        {
            return db.CHALLENGES.Find(id);
        }
    }
}