namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COMMENT")]
    public partial class COMMENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMMENT()
        {
            REPLies = new HashSet<REPLY>();
        }

        public int ID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public int? Likes { get; set; }

        public int OwnerID { get; set; }

        public int ChallengeID { get; set; }

        public virtual CHALLENGE CHALLENGE { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REPLY> REPLies { get; set; }
    }
}
