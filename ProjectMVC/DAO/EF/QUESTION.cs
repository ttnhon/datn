namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QUESTION")]
    public partial class QUESTION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QUESTION()
        {
            QUESTION_ANSWER = new HashSet<QUESTION_ANSWER>();
        }

        public int ID { get; set; }

        public int CompeteID { get; set; }

        [Column(TypeName = "ntext")]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        public string Choise { get; set; }

        public short? Type { get; set; }

        public int? Score { get; set; }

        [StringLength(255)]
        public string Result { get; set; }

        public virtual COMPETE COMPETE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QUESTION_ANSWER> QUESTION_ANSWER { get; set; }
    }
}
