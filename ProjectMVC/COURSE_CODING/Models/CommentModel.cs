using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAO.EF;

namespace COURSE_CODING.Models
{
    public class CommentModel
    {

        public COMMENT comment { get; set; }
        public USER_INFO owner { get; set; }
        public List<ReplyModel> replies { get; set; }

        public CommentModel()
        {
            replies = new List<ReplyModel>();
        }
    }
}