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
            ANSWERs = new HashSet<ANSWER>();
            CHALLENGE_LANGUAGE = new HashSet<CHALLENGE_LANGUAGE>();
            CHALLENGE_COMPETE = new HashSet<CHALLENGE_COMPETE>();
            CHALLENGE_EDITOR = new HashSet<CHALLENGE_EDITOR>();
            TESTCASEs = new HashSet<TESTCASE>();
            COMMENTs = new HashSet<COMMENT>();
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ANSWER> ANSWERs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHALLENGE_LANGUAGE> CHALLENGE_LANGUAGE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHALLENGE_COMPETE> CHALLENGE_COMPETE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHALLENGE_EDITOR> CHALLENGE_EDITOR { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TESTCASE> TESTCASEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENT> COMMENTs { get; set; }
    }
}
