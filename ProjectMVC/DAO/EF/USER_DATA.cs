namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_DATA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Data { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }
    }
}
