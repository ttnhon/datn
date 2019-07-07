using DAO.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAO
{
    public class LanguageDAO
    {
        DCCourseCoding db = null;
        public LanguageDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// Get language list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<LANGUAGE> GetList()
        {
            return db.LANGUAGES.ToList();
        }

        /// <summary>
        /// Get language by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LANGUAGE GetLanguageByID(int id)
        {
            return db.LANGUAGES.Find(id);
        }

        /// <summary>
        /// Get language by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LANGUAGE GetLanguageByName(string name)
        {
            return db.LANGUAGES.SingleOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get list challenge by language name
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public List<CHALLENGE> GetChallenge(string language)
        {
            return db.CHALLENGE_LANGUAGES
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p})
                .Join(db.LANGUAGES, t => t.t.LanguageID, p => p.ID, (t,p) => new { t, p })
                .Where(table => table.p.Name.Contains(language)).Select(item => item.t.p).ToList();
        }

        /// <summary>
        /// Get number of challenge by language name
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public int GetChallengeCount(string language)
        {
            return db.CHALLENGE_LANGUAGES
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                .Join(db.LANGUAGES, t => t.t.LanguageID, p => p.ID, (t, p) => new { t, p })
                .Count(table => table.p.Name.Contains(language));
        }

        /// <summary>
        /// Get number of compete by language name
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int GetCompeteCount()
        {
            return db.COMPETES.Count();
        }
        

        ///// <summary>
        ///// Get next challenge by user id and language name
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public CHALLENGE GetNextChallengeByID(int id, string language)
        //{
        //    var res = db.CHALLENGES.Join(db.CHALLENGE_LANGUAGES, t => t.ID, p => p.ChallengeID, (t, p) => new { t, p })
        //        .Join(db.LANGUAGES, t => t.p.LanguageID, p => p.ID, (t, p) => new { t, p })
        //        .Where(table => table.p.Name.Contains(language)).Select(item => item.t.t.ID)
        //        .Except(
        //        db.CHALLENGES.Join(db.ANSWERS, t => t.ID, p => p.ChallengeID, (t, p) => new { t, p })
        //        .Join(db.CHALLENGE_LANGUAGES, t => t.t.ID, p => p.ChallengeID, (t, p) => new { t, p })
        //        .Join(db.LANGUAGES, t => t.p.LanguageID, p => p.ID, (t, p) => new { t, p })
        //        .Where(table => table.t.t.p.UserId == id && table.p.Name.Contains(language)).Select(item => item.t.t.t.ID)
        //        ).ToList();
        //    return db.CHALLENGES.Where(table => res.Contains(table.ID)).FirstOrDefault();
        //}

        /// <summary>
        /// Get number of answer of language by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetAnswerCountByID(int id, string language)
        {
            return db.ANSWERS.Where(table => table.UserId == id).Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t,p) => new { t, p})
                .Join(db.CHALLENGE_LANGUAGES, t => t.p.ID, p => p.ChallengeID, (t, p) => new { t, p })
                .Join(db.LANGUAGES, t => t.p.LanguageID, p => p.ID, (t, p) => new { t, p })
                .Count(table => table.p.Name.Contains(language));
        }

        /// <summary>
        /// Get number of answer of language by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<LANGUAGE> GetByChallengeID(int id)
        {
            return db.CHALLENGE_LANGUAGES.Where(table => table.ChallengeID == id)
                .Join(db.LANGUAGES, t => t.LanguageID, p => p.ID, (t, p) => new { t, p })
                .Select(item => item.p).ToList();
        }

        public List<CHALLENGE> GetSolvedAndPublic(string languageName, int userID)
        {
            var challenge = db.CHALLENGE_LANGUAGES
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                .Join(db.LANGUAGES, t => t.t.LanguageID, p => p.ID, (t, p) => new { t, p })
                .Where(table => table.p.Name.Contains(languageName)).Select(item => item.t.p);
            return challenge.Where(table => table.IsPublic).GroupJoin(db.ANSWERS, t => t.ID, p => p.ChallengeID, (t, p) => new { t, p })
                .Where(table => table.p.FirstOrDefault(list => list.UserId == userID) != null)
                .Select(table => table.t).ToList();
        }

        public List<CHALLENGE> GetUnsolvedAndPublic(string languageName, int userID)
        {
            var challenge = db.CHALLENGE_LANGUAGES
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                .Join(db.LANGUAGES, t => t.t.LanguageID, p => p.ID, (t, p) => new { t, p })
                .Where(table => table.p.Name.Contains(languageName)).Select(item => item.t.p);
            return challenge.Where(table => table.IsPublic).GroupJoin(db.ANSWERS, t => t.ID, p => p.ChallengeID, (t, p) => new { t, p })
                .Where(table => table.p.FirstOrDefault(list => list.UserId == userID) == null)
                .Select(table => table.t).ToList();
        }
    }
}
