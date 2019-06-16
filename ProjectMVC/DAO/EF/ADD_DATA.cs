namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADD_DATA
    {
        public int ID { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(256)]
        public string Data { get; set; }
    }
}
