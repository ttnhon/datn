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
            return db.CHALLENGES.Where(item => item.Languages.Contains(language)).ToList();
        }

        /// <summary>
        /// Get number of challenge by language name
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public int GetChallengeCount(string language)
        {
            return db.CHALLENGES.Count(item => item.Languages.Contains(language));
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

        /// <summary>
        /// Get number success challenge by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetNumberSuccessChallengeByID(int id)
        {
            return db.ANSWERS.Count(item => item.ID == id);
        }

        /// <summary>
        /// Get next challenge by user id and language name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CHALLENGE GetNextChallengeByID(int id, string language)
        {
            var res = db.CHALLENGES.Where(table => table.Languages.Contains(language)).Select(item => item.ID)
                .Except(
                db.CHALLENGES.Join(db.ANSWERS, t => t.ID, p => p.ChallengeID, (t, p) => new { t, p })
                .Where(table => table.p.UserId == id && table.t.Languages.Contains(language))
                .Select(item => item.t.ID)
                ).ToList();
            return db.CHALLENGES.Where(table => res.Contains(table.ID)).FirstOrDefault();
        }

        /// <summary>
        /// Get number of answer of language by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetAnswerCountByID(int id, string language)
        {
            return db.ANSWERS.Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t,p) => new { t, p})
                .Count(u => u.t.UserId == id && u.p.Languages.Contains(language));
        }
    }
}
