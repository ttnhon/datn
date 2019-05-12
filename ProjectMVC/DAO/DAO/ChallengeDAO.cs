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

        public List<CHALLENGE> GetAll(int id)
        {
            return db.CHALLENGES.Where(table => table.USER_INFO.ID == id).ToList();
        }

        public List<CHALLENGE> GetAllByCompeteID(int id)
        {
            return db.CHALLENGE_COMPETES.Where(table => table.CompeteID == id)
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                .Select(item => item.p).ToList();
        }
    }
}