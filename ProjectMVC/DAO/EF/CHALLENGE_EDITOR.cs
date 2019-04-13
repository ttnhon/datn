namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CHALLENGE_EDITOR
    {
        public int ID { get; set; }

        public int ChallegenID { get; set; }

        public int EditorID { get; set; }

        public virtual CHALLENGE CHALLENGE { get; set; }

        public virtual USER_INFO USER_INFO { get; set; }
    }
}
