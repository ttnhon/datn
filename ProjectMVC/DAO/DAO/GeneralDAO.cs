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
        public IQueryable<USER_INFO> GetAllDataUsers()
        {
            return db.USER_INFOS.Where(m => m.CreateDate.Year.Equals(DateTime.Now.Year));
        }
        public IQueryable<USER_INFO> GetAllDataUsersForChart()
        {
           return db.USER_INFOS.Where(m=>m.CreateDate.Year.Equals(DateTime.Now.Year));
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
        public IQueryable<CHALLENGE_LANGUAGE> GetAllDataLanguagesForChart()
        {
            return db.CHALLENGE_LANGUAGES;
        }
    }
}
