using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.EF;

namespace DAO.DAO
{
    public class GeneralDAO
    {
        DCCourseCoding db = null;
        public GeneralDAO()
        {
            db = new DCCourseCoding();
        }
        public int CountUsers()
        {
            return db.USER_INFOS.Count();
        }
        public int CountCompetes()
        {
            return db.COMMENTS.Count();
        }
        public int CountChallenges()
        {
            return db.CHALLENGES.Count();
        }
        public int CountLanguages()
        {
            return db.LANGUAGES.Count();
        }
        public IOrderedQueryable<USER_INFO> GetAllDataUsers()
        {
            //return db.USER_INFOS;
            //var grouped = (from u in db.USER_INFOS
            //               group u by new { month = u.CreateDate.Month, year = u.CreateDate.Year } into d
            //               select new { dt = string.Format("{0}/{1}", d.Key.month, d.Key.year), count = d.Count() }).OrderByDescending(g => g.dt);
            //return grouped;

            return null;
        }
        public IQueryable<COMPETE> GetAllDataCompetes()
        {
            return db.COMPETES;
        }
        public IQueryable<CHALLENGE> GetAllDataChallenges()
        {
            return db.CHALLENGES;
        }
        public IQueryable<LANGUAGE> GetAllDataLanguages()
        {
            return db.LANGUAGES;
        }

    }
}
