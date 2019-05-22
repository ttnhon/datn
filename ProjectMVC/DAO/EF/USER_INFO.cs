namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_INFO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER_INFO()
        {
            ANSWERs = new HashSet<ANSWER>();
            CHALLENGEs = new HashSet<CHALLENGE>();
            CHALLENGE_EDITOR = new HashSet<CHALLENGE_EDITOR>();
            COMMENTs = new HashSet<COMMENT>();
            COMPETEs = new HashSet<COMPETE>();
            REPLies = new HashSet<REPLY>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string PasswordUser { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public int RoleUser { get; set; }

        [StringLength(256)]
        public string PhotoURL { get; set; }

        public int? StatusUser { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [Column(TypeName = "text")]
        public string About { get; set; }

        public int SchoolID { get; set; }

        public int? YearGraduation { get; set; }

        [StringLength(256)]
        public string FacebookLink { get; set; }

        [StringLength(256)]
        public string GoogleLink { get; set; }

        public DateTime CreateDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ANSWER> ANSWERs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHALLENGE> CHALLENGEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHALLENGE_EDITOR> CHALLENGE_EDITOR { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENT> COMMENTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPETE> COMPETEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REPLY> REPLies { get; set; }

        public virtual SCHOOL SCHOOL { get; set; }

        public virtual USER_DATA USER_DATA { get; set; }
    }
}
