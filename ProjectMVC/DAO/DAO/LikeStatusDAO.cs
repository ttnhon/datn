using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.EF;


namespace DAO.DAO
{
    public class LikeStatusDAO
    {
        DCCourseCoding db = null;
        public LikeStatusDAO()
        {
            db = new DCCourseCoding();
        }

        public List<LIKE_STATUS> GetAll()
        {
            return db.LIKE_STATUS.ToList();
        }

        public List<LIKE_STATUS> GetAllByUser(int id)
        {
            try
            {
                return db.LIKE_STATUS.Where(table => table.OwnerID == id).ToList();
            } catch
            {
                return null;
            }
            
        }

        public LIKE_STATUS GetOneByComment(int userID, int? commentID)
        {
            return db.LIKE_STATUS.Where(table => table.OwnerID == userID && table.CommentID == commentID).FirstOrDefault();
        }

        public LIKE_STATUS GetOneByReply(int userID, int? replyID)
        {
            return db.LIKE_STATUS.Where(table => table.OwnerID == userID && table.ReplyID == replyID).FirstOrDefault();
        }

        public Boolean CheckLikeStatus(LIKE_STATUS s)
        {
            return db.LIKE_STATUS.Count(u => u.OwnerID == s.OwnerID && u.CommentID == s.CommentID && u.ReplyID == s.ReplyID) > 0;
        }

        public Boolean Insert(LIKE_STATUS status)
        {
            try
            {
                db.LIKE_STATUS.Add(status);
                db.SaveChanges();
                return true;

            } catch(Exception e)
            {
                return false;
            }
        }

        public Boolean Delete(LIKE_STATUS status)
        {
            try
            {
                LIKE_STATUS s = new LIKE_STATUS();
                if(status.ReplyID == null)
                {
                    s = GetOneByComment(status.OwnerID, status.CommentID);
                }
                if(status.CommentID == null)
                {
                    s = GetOneByReply(status.OwnerID, status.ReplyID);
                }
                
                db.LIKE_STATUS.Remove(s);
                db.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
