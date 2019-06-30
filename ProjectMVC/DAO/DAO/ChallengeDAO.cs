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
                .Where(table => table.p.EditorID == editorId)
               // .Join(db.USER_INFOS, t => t.p.EditorID, p => p.ID, (t, p) => new { t, p })
                .Select(table => table.t).ToList();
        }

        public List<CHALLENGE> GetAllByCompeteID(int id)
        {
            return db.CHALLENGE_COMPETES.Where(table => table.CompeteID == id)
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                .Select(item => item.p).ToList();
        }

        public dynamic GetAllWithAnswerByCompeteID(int competeID, int userID)
        {
            //return db.QUESTIONS.Where(table => table.CompeteID == id).ToList();
            var challenges = db.CHALLENGE_COMPETES.Where(table => table.CompeteID == competeID)
                    .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                    .Select(item => item.p)
                      .GroupJoin(db.ANSWERS.Where(table => table.UserId == userID)
                            , q => q.ID, qa => qa.ChallengeID, (f, b) => new { f, b })
                            .SelectMany(z => z.b.DefaultIfEmpty(), (z, g) => new {
                                ID = z.f.ID,
                                Title = z.f.Title,
                                Difficulty = z.f.ChallengeDifficulty,
                                Score = z.f.Score,
                                isSolved = g == null ? false : true,
                                TimeDone = g != null ? g.TimeDone : default(DateTime)
                            }).ToList();
            return challenges;
        }

        public int GetBackChallenge(int competeID, int challengeID)
        {
            var list = db.CHALLENGE_COMPETES.Where(table => table.CompeteID == competeID)
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                .Select(item => item.p.ID).ToList();
            if (list.Count <= 0) return -2;
            int index = list.IndexOf(challengeID);
            if (index == 0)
            {
                return -1;
            }
            return list[index - 1];
        }

        public int GetNextChallenge(int competeID, int challengeID)
        {
            var list = db.CHALLENGE_COMPETES.Where(table => table.CompeteID == competeID)
                .Join(db.CHALLENGES, t => t.ChallengeID, p => p.ID, (t, p) => new { t, p })
                .Select(item => item.p.ID).ToList();
            if (list.Count <= 0) return -2;
            int index = list.IndexOf(challengeID);
            if (index == (list.Count - 1))
            {
                return -1;
            }
            return list[index + 1];
        }

        /// <summary>
        /// check if is owner
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsOwner(int challengeId, int ownerId)
        {
            return db.CHALLENGES.Where(table => table.ID == challengeId && table.OwnerID == ownerId).Count() > 0;
        }

        /// <summary>
        /// check if is editor
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsEditor(int challengeId, int ownerId)
        {
            return db.CHALLENGE_EDITORS.Where(table => table.ChallegenID == challengeId && (table.EditorID == ownerId || table.CHALLENGE.OwnerID == ownerId)).Count() > 0;
        }

        /// <summary>
        /// check if is available for user
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public bool IsAvailable(int challengeId, int userId)
        {
            return db.CHALLENGES
                .Where(table => table.ID == challengeId 
                && (table.IsPublic 
                || table.OwnerID == userId 
                || table.CHALLENGE_COMPETE.FirstOrDefault(item => item.ChallengeID == challengeId).COMPETE.COMPETE_PARTICIPANTS.FirstOrDefault(item => item.UserID == userId) != null))
                .Count() > 0;
        }

        /// <summary>
        /// check challenge is public
        /// </summary>
        /// <param name="challengeID"></param>
        /// <returns></returns>
        public bool IsPublic(int? challengeID)
        {
            if (challengeID == null)
            {
                return false;
            }
            bool is_public = db.CHALLENGES
                .Where(table => table.ID == challengeID)
                .Select(table => table.IsPublic).FirstOrDefault();
            return is_public;
        }

        /// <summary>
        /// check user joined challenge
        /// </summary>
        /// <param name="challengeID"></param>
        /// <returns></returns>
        public bool IsJoined(int userID, int challengeID, int? competeID)
        {
            var count = db.COMPETE_PARTICIPANTSS.Where(cp => cp.CompeteID == competeID && cp.UserID == userID)
                  .GroupJoin(db.CHALLENGE_COMPETES.Where(cc => cc.ChallengeID == challengeID)
                        , q => q.CompeteID, qa => qa.CompeteID, (f, b) => new { f, b }).FirstOrDefault();
            if (count == null || count.b.Count() < 1 )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// check challenge is public
        /// </summary>
        /// <param name="challengeID"></param>
        /// <returns></returns>
        public bool CanAccess(int userID, int challengeID, int? competeID)
        {
            if (competeID == null)
            {
                return this.IsPublic(challengeID);
            }
            else
            {
                return this.IsJoined(userID, challengeID, competeID);
            }
        }

        /// <summary>
        /// Update info challenge
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(CHALLENGE entity)
        {
            try
            {
                entity.DisCompileTest = false;
                entity.DisSubmissions = false;
                entity.PublicSolutions = false;
                entity.IsPublic = true;
                db.CHALLENGES.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddChallengetoCompete(int challengeID, int competeID)
        {
            try
            {
                CHALLENGE_COMPETE c = new CHALLENGE_COMPETE();
                c.CompeteID = competeID;
                c.ChallengeID = challengeID;
                db.CHALLENGE_COMPETES.Add(c);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
                    c.Score = entity.Score;
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

        public bool DeleteChallenge(CHALLENGE_COMPETE c)
        {
            try
            {
                var v = db.CHALLENGE_COMPETES.Find(c.ChallengeID, c.CompeteID);
                var u = db.CHALLENGES.Find(c.ChallengeID);
                db.CHALLENGE_COMPETES.Remove(v);
                if (u.ID > 0)
                {
                    db.CHALLENGES.Remove(u);
                }
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
                    c.DisCompileTest = entity.DisCompileTest;
                    c.IsPublic = entity.IsPublic;
                    c.DisSubmissions = entity.DisSubmissions;
                    c.PublicSolutions = entity.PublicSolutions;
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
        /// Update language list challenge
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateLanguage(int ID, bool LanguageCpp, bool LanguageCSharp, bool LanguageJava)
        {
            try
            {
                List<int> addList = new List<int>();
                if (LanguageCpp)
                {
                    addList.Add(1);
                }
                else
                {
                    addList.Add(-1);
                }
                if (LanguageCSharp)
                {
                    addList.Add(2);
                }
                else
                {
                    addList.Add(-2);
                }
                if (LanguageJava)
                {
                    addList.Add(3);
                }
                else
                {
                    addList.Add(-3);
                }
                foreach (var id in addList)
                {
                    if (id < 0)
                    {
                        var item = db.CHALLENGE_LANGUAGES.FirstOrDefault(table => table.ChallengeID == ID && table.LanguageID == -id);
                        if(item != null)
                        {
                            db.CHALLENGE_LANGUAGES.Remove(item);
                            db.SaveChanges();
                        }
                        continue;
                    }
                    bool res = db.CHALLENGE_LANGUAGES.Where(table => table.ChallengeID == ID && table.LanguageID == id).Count() > 0;
                    if (!res)
                    {
                        CHALLENGE_LANGUAGE temp = new CHALLENGE_LANGUAGE()
                        {
                            ChallengeID = ID,
                            LanguageID = id,
                            CodeStub = null
                        };
                        db.CHALLENGE_LANGUAGES.Add(temp);
                        db.SaveChanges();
                    }
                }
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
        /// Get code stubs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CHALLENGE_LANGUAGE> GetCodeStubs(int id)
        {
            return db.CHALLENGE_LANGUAGES.Where(table => table.ChallengeID == id).ToList();
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
        /// Get one test case of challenge by test case id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TESTCASE GetOneTestCase(int id)
        {
            return db.TESTCASES.Find(id);
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
        /// Delete test case of challenge by test case id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTestCase(int id)
        {
            try
            {
                var t = db.TESTCASES.Find(id);
                db.TESTCASES.Remove(t);
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
            var list = db.TESTCASES.Where(table => table.ChallengeID == id).ToList();
            int res = 0;
            List<int> exist = new List<int>();
            for(int i = 0; i < list.Count; i++)
            {
                string[] s = list[i].Output.Split('_');
                string[] n = s[3].Split('.');
                int number = Int32.Parse(n[0]);
                exist.Add(number);
            }
            if(exist.Count > 0)
            {
                exist.Sort();
                foreach (var item in exist)
                {
                    if (res == item)
                    {
                        res++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Update codestub by challenge id and language id
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public bool UpdateCodestub(int challengeId, int languageId, string codestub)
        {
            try
            {
                var t = db.CHALLENGE_LANGUAGES.SingleOrDefault(table => table.ChallengeID == challengeId && table.LanguageID == languageId);
                if (t != null)
                {
                    t.CodeStub = codestub;
                    db.SaveChanges();
                }
                else
                {
                    CHALLENGE_LANGUAGE temp = new CHALLENGE_LANGUAGE()
                    {
                        ChallengeID = challengeId,
                        LanguageID = languageId,
                        CodeStub = codestub
                    };
                    db.CHALLENGE_LANGUAGES.Add(temp);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}