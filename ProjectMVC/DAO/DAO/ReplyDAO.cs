using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO.EF;


namespace DAO.DAO
{
    public class ReplyDAO
    {
        DCCourseCoding db = null;
        public ReplyDAO()
        {
            db = new DCCourseCoding();
        }

        public REPLY GetOne(int id)
        {
            return db.REPLIES.Find(id);
        }

        public List<REPLY> GetAll(int id)
        {
            return db.REPLIES.Where(table => table.ID == id).ToList();
        }

        public List<REPLY> GetAllByComment(int id)
        {
            return db.REPLIES.Where(table => table.CommentID == id)
                .ToList();
        }

    }
}
