using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.EF;

namespace DAO.DAO
{
    public class QuestionDAO
    {
        DCCourseCoding db = null;
        public QuestionDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// Insert to table QUESTION
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(QUESTION entity)
        {
            try
            {
                db.QUESTIONS.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        /// <summary>
        /// Insert to table QUESTION
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(QUESTION entity)
        {
            try
            {
                var res = db.QUESTIONS.Find(entity.ID);
                if(res != null)
                {
                    res.Title = entity.Title;
                    res.Choise = entity.Choise;
                    res.Result = entity.Result;
                    res.Score = entity.Score;
                    res.Type = entity.Type;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public bool Delete(QUESTION entity)
        {
            try
            {
                var u = db.QUESTIONS.Find(entity.ID);
                db.QUESTIONS.Remove(u);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        /// <summary>
        /// Get all question in compete
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<QUESTION> GetAllByCompeteID(int id)
        {
            return db.QUESTIONS.Where(table => table.CompeteID == id).ToList();
        }

        /// <summary>
        /// Get all question in compete of a user
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public dynamic GetAllWithAnswerByCompeteID(int competeID, int userID)
        {
            //return db.QUESTIONS.Where(table => table.CompeteID == id).ToList();
            var questions = db.QUESTIONS.Where(table => table.CompeteID == competeID)
                  .GroupJoin(db.QUESTION_ANSWERS.Where(table => table.UserId == userID)
                        , q => q.ID, qa => qa.QuestionID, (f, b) => new { f, b })
                        .SelectMany(z => z.b.DefaultIfEmpty(), (z, g) => new {
                            Question = z.f,
                            Chosen = g
                        }).ToList();
            return questions;
        }

        /// <summary>
        /// Get one challenge
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public QUESTION GetOne(int id)
        {
            return db.QUESTIONS.Find(id);
        }
        
    }
}
