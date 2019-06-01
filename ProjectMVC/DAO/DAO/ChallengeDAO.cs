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

        public List<CHALLENGE> GetAllByEditorId(int editorId)
        {
            return db.CHALLENGES.Join(db.CHALLENGE_EDITORS, t => t.ID, p => p.ChallegenID, (t, p) => new { t, p })
                .Where(table => table.p.EditorID == editorId).Select(table => table.t).ToList();
        }

        public List<CHALLENGE> GetAllByCompeteID(int id)
        {
            return db.CHALLENGE_COMPETES.Where(table => table.CompeteID == id)
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                .Select(item => item.p).ToList();
        }

        /// <summary>
        /// Update info challenge
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(CHALLENGE entity)
        {
            try
            {
                var c = db.CHALLENGES.Find(entity.ID);
                if(c.ID > 0)
                {
                    c.Title = entity.Title;
                    c.Slug = entity.Slug;
                    c.Description = entity.Description;
                    c.InputFormat = entity.InputFormat;
                    c.OutputFormat = entity.OutputFormat;
                    c.ChallengeDifficulty = entity.ChallengeDifficulty;
                    c.Constraints = entity.Constraints;
                    c.Tags = entity.Tags;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Update setting challenge
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateSetting(CHALLENGE entity)
        {
            try
            {
                var c = db.CHALLENGES.Find(entity.ID);
                if (c.ID > 0)
                {
                    
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Get list moderator by challenge id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<USER_INFO> GetModeratorByChallengeID(int id)
        {
            return db.CHALLENGE_EDITORS.Where(table => table.ChallegenID == id)
                .Join(db.USER_INFOS, t => t.EditorID, p => p.ID, (t, p) => new { t, p }).Select(item => item.p).ToList();
        }

        /// <summary>
        /// Get userinfo by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public USER_INFO GetUserByName(string name)
        {
            return db.USER_INFOS.First(table => table.UserName == name);
        }

        /// <summary>
        /// Add moderator (insert challenge_editor)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddModerator(CHALLENGE_EDITOR entity)
        {
            try
            {
                db.CHALLENGE_EDITORS.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Get list test case of challenge by challenge id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TESTCASE> GetTestCaseByID(int id)
        {
            return db.TESTCASES.Where(table => table.ChallengeID == id).ToList();
        }

        /// <summary>
        /// Add test case of challenge
        /// </summary>
        /// <param name="testcase"></param>
        /// <returns></returns>
        public bool AddTestCase(TESTCASE testcase)
        {
            try
            {
                //entity.ID = GetNextIDModerator();
                db.TESTCASES.Add(testcase);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Get next test case id of the challenge by challenge id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetTestCaseNextID(int id)
        {
            return db.TESTCASES.Where(table => table.ChallengeID == id).Count();
        }
    }
}