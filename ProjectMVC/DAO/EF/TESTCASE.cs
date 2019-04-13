namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TESTCASE")]
    public partial class TESTCASE
    {
        public int ID { get; set; }

        public int ChallengeID { get; set; }

        [Required]
        [StringLength(256)]
        public string Input { get; set; }

        [Required]
        [StringLength(256)]
        public string Output { get; set; }

        public virtual CHALLENGE CHALLENGE { get; set; }
    }
}
