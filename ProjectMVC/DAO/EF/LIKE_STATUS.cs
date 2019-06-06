namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LIKE_STATUS
    {
        public int ID { get; set; }

        public int OwnerID { get; set; }

        public int? CommentID { get; set; }

        public int? ReplyID { get; set; }

        public virtual COMMENT COMMENT { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }

        public virtual REPLY REPLY { get; set; }
    }
}
