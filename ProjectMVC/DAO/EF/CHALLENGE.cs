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
        }

        public int ID { get; set; }

        public int? CompeteID { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Problem { get; set; }

        public int? LanguageCode { get; set; }

        [Column(TypeName = "text")]
        public string InputQuestion { get; set; }

        [Column(TypeName = "text")]
        public string OutputQuestion { get; set; }

        public short? LevelChallenge { get; set; }

        public int? TimeDo { get; set; }

        public int Score { get; set; }

        [Column(TypeName = "text")]
        public string Solution { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ANSWER> ANSWERs { get; set; }

        public virtual COMPETE COMPETE { get; set; }

        public virtual LANGUAGE_CODE LANGUAGE_CODE { get; set; }
    }
}
