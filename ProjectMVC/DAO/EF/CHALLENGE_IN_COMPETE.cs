namespace DAO.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CHALLENGE_IN_COMPETE
    {
        public int ID { get; set; }

        public int ChallengeID { get; set; }

        public int CompeteID { get; set; }

        public int SerialNumber { get; set; }

        public virtual CHALLENGE CHALLENGE { get; set; }

        public virtual COMPETE COMPETE { get; set; }
    }
}
