using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace DAO.DAO
{
    public class TestCaseDAO
    {
        DCCourseCoding db = null;
        public TestCaseDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// Get all test case of 1 challenge
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<TESTCASE> GetAllByChallengeID(int challengeID)
        {
            return db.TESTCASES.Where(item => item.ChallengeID == challengeID).ToList();
        }

        /// <summary>
        /// Get first test case of 1 challenge
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public TESTCASE GetOneByChallengeID(int challengeID)
        {
            return db.TESTCASES.FirstOrDefault(item => item.ChallengeID == challengeID);
        }
    }
}