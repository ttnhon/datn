﻿using System;
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
                    c.DisCompileTest = entity.DisCompileTest;
                    c.DisCustomTestcase = entity.DisCustomTestcase;
                    c.DisSubmissions = entity.DisSubmissions;
                    c.PublicTestcase = entity.PublicTestcase;
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
        public bool UpdateLanguage(CHALLENGE entity)
        {
            try
            {
                var c = db.CHALLENGES.Find(entity.ID);
                if (c.ID > 0)
                {
                    c.LanguageCSharp = entity.LanguageCSharp;
                    c.LanguageCpp = entity.LanguageCpp;
                    c.LanguageJava = entity.LanguageJava;
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
        /// Update editorial challenge
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEditorial(CHALLENGE entity)
        {
            try
            {
                var c = db.CHALLENGES.Find(entity.ID);
                if (c.ID > 0)
                {
                    c.RequiredKnowledge = entity.RequiredKnowledge;
                    c.TimeComplexity = entity.TimeComplexity;
                    c.Editorialist = entity.Editorialist;
                    c.PartialEditorial = entity.PartialEditorial;
                    c.Approach = entity.Approach;
                    c.ProblemSetter = entity.ProblemSetter;
                    c.SetterCode = entity.SetterCode;
                    c.ProblemTester = entity.ProblemTester;
                    c.TesterCode = entity.SetterCode;
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