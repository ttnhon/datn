using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class CommentListModel
    {
        public List<CommentModel> comments { get; set; }
        public USER_INFO Info { get; set; }
        public CHALLENGE challenge { get; set; }

        public List<LIKE_STATUS> like_status { get; set; }

        public CommentListModel()
        {
            comments = new List<CommentModel>();
        }
    }
}