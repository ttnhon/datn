namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COMPETE")]
    public partial class COMPETE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMPETE()
        {
            CHALLENGE_IN_COMPETE = new HashSet<CHALLENGE_IN_COMPETE>();
        }

        public int ID { get; set; }

        public int OwnerID { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(256)]
        public string Slug { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [StringLength(256)]
        public string Rules { get; set; }

        public int TotalScore { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeEnd { get; set; }

        public int ParticipantCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHALLENGE_IN_COMPETE> CHALLENGE_IN_COMPETE { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }
    }
}
