namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ANSWER")]
    public partial class ANSWER
    {
        public int ID { get; set; }

        public int ChallengeID { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Content { get; set; }

        public bool Result { get; set; }

        public virtual CHALLENGE CHALLENGE { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }
    }
}
