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

        public List<COMMENT> GetAll(int id)
        {
            return db.COMMENTS.Where(table => table.ID == id).ToList();
        }

        public List<COMMENT> GetAllSortBy(int id,int sort)
        {
            var comments = db.COMMENTS.Where(table => table.ID == id);

            switch (sort)
            {
                case 1:
                    comments.OrderByDescending(table => table.CreateDate);
                    break;
                case 2:
                    comments.OrderByDescending(table => table.Likes);
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
    }
}
