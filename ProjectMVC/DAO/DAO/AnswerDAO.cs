using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;


namespace DAO.DAO
{
    public class AnswerDAO
    {
        DCCourseCoding db = null;
        public AnswerDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// insert answer of user to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean Insert(ANSWER entity)
        {
            try
            {
                db.ANSWERS.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean InsertOrUpdate(ANSWER entity)
        {
            try
            {
                ANSWER update_answer = db.ANSWERS.Where(table => table.ChallengeID == entity.ChallengeID)
                        .Where(table => table.UserId == entity.UserId).FirstOrDefault();
                if (update_answer != null)
                {
                    update_answer.Content = entity.Content;
                    update_answer.Result = entity.Result;
                    update_answer.TimeDone = entity.TimeDone;
                    db.SaveChanges();
                }
                else
                {
                    db.ANSWERS.Add(entity);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<ANSWER> GetAllChallengeDone(int userId) { 
        
            return db.ANSWERS.Where(table => table.UserId == userId).ToList();
        }

        /// <summary>
        /// Get number success challenge by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CountSuccessAnswerByUserID(int id)
        {
            return db.ANSWERS.Count(item => item.UserId == id);
        }
    }
}