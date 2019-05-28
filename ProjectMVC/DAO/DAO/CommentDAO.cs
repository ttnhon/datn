using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO.EF;

namespace DAO.DAO
{
    public class CommentDAO
    {
        DCCourseCoding db = null;
        public CommentDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// Get one challenge
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public COMMENT GetOne(int id)
        {
            return db.COMMENTS.Find(id);
        }

        public COMMENT GetNewest()
        {
            return db.COMMENTS.OrderByDescending(table => table.ID).First();
        }

        public List<COMMENT> GetAll(int id)
        {
            return db.COMMENTS.Where(table => table.ID == id).ToList();
        }

        public List<COMMENT> GetAllByChallenge(int id,int sort)
        {
            var comments = db.COMMENTS.Where(table => table.ChallengeID == id).Distinct();

            switch (sort)
            {
                case 1:
                    comments = comments.OrderByDescending(table => table.ID);
                    break;
                case 2:
                    comments = comments.OrderByDescending(table => table.Likes);
                    break;
            }
            return comments.ToList();
            
        }

        public List<COMMENT> GetAllByUser(int id)
        {
            return db.COMMENTS.Where(table => table.OwnerID == id).ToList();
        }

        public List<COMMENT> GetAllByChallenge(int id)
        {
            return db.COMMENTS.Where(table => table.ChallengeID == id).ToList();
        }

        public Boolean Insert(COMMENT entity)
        {
            try
            {
                db.COMMENTS.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean UpdateLikes(COMMENT entity)
        {
            try
            {
                var u = db.COMMENTS.Find(entity.ID);
                if (u.ID > 0)
                {
                    u.Likes = entity.Likes;
                }
                db.SaveChanges();
                return true;
            } catch (Exception e)
            {
                return false;
            }
        }
    }
}
