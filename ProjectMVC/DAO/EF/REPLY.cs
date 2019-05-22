namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("REPLY")]
    public partial class REPLY
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int CommentID { get; set; }

        public int OwnerID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public int? Likes { get; set; }

        public virtual COMMENT COMMENT { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }
    }
}
