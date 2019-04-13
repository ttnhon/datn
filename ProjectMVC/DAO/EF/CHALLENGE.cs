namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CHALLENGE")]
    public partial class CHALLENGE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CHALLENGE()
        {
            ANSWER = new HashSet<ANSWER>();
            CHALLENGE_EDITOR = new HashSet<CHALLENGE_EDITOR>();
            CHALLENGE_IN_COMPETE = new HashSet<CHALLENGE_IN_COMPETE>();
            TESTCASE = new HashSet<TESTCASE>();
        }

        public int ID { get; set; }

        public int OwnerID { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(256)]
        public string Slug { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Description { get; set; }

        [Column(TypeName = "text")]
        public string InputFormat { get; set; }

        [Column(TypeName = "text")]
        public string OutputFormat { get; set; }

        public short ChallengeDifficulty { get; set; }

        [StringLength(256)]
        public string Constraints { get; set; }

        public int? TimeDo { get; set; }

        public int Score { get; set; }

        [Column(TypeName = "text")]
        public string Solution { get; set; }

        [StringLength(256)]
        public string Tags { get; set; }

        [Required]
        [StringLength(256)]
        public string Languages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ANSWER> ANSWER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHALLENGE_EDITOR> CHALLENGE_EDITOR { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHALLENGE_IN_COMPETE> CHALLENGE_IN_COMPETE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TESTCASE> TESTCASE { get; set; }
    }
}
