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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REPLY()
        {
            LIKE_STATUS = new HashSet<LIKE_STATUS>();
        }

        public int ID { get; set; }

        public int CommentID { get; set; }

        public int OwnerID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public int? Likes { get; set; }

        public virtual COMMENT COMMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LIKE_STATUS> LIKE_STATUS { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }
    }
}
