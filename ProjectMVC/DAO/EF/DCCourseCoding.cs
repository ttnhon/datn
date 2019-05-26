namespace DAO.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DCCourseCoding : DbContext
    {
        public DCCourseCoding()
            : base("name=DCCourseCoding")
        {
        }

        public virtual DbSet<ADD_DATA> ADD_DATAS { get; set; }
        public virtual DbSet<ANSWER> ANSWERS { get; set; }
        public virtual DbSet<CHALLENGE> CHALLENGES { get; set; }
        public virtual DbSet<CHALLENGE_COMPETE> CHALLENGE_COMPETES { get; set; }
        public virtual DbSet<CHALLENGE_EDITOR> CHALLENGE_EDITORS { get; set; }
        public virtual DbSet<CHALLENGE_LANGUAGE> CHALLENGE_LANGUAGES { get; set; }
        public virtual DbSet<COMMENT> COMMENTS { get; set; }
        public virtual DbSet<COMPETE> COMPETES { get; set; }
        public virtual DbSet<LANGUAGE> LANGUAGES { get; set; }
        public virtual DbSet<REPLY> REPLIES { get; set; }
        public virtual DbSet<SCHOOL> SCHOOLS { get; set; }
        public virtual DbSet<TESTCASE> TESTCASES { get; set; }
        public virtual DbSet<USER_DATA> USER_DATAS { get; set; }
        public virtual DbSet<USER_INFO> USER_INFOS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ADD_DATA>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<ADD_DATA>()
                .Property(e => e.Data)
                .IsUnicode(false);

            modelBuilder.Entity<ANSWER>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.Slug)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.InputFormat)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.OutputFormat)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.Solution)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.ANSWERs)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.CHALLENGE_LANGUAGE)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.CHALLENGE_COMPETE)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.CHALLENGE_EDITOR)
                .WithRequired(e => e.CHALLENGE)
                .HasForeignKey(e => e.ChallegenID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.TESTCASEs)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.COMMENTs)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<COMMENT>()
                .Property(e => e.Text)
                .IsUnicode(false);

            modelBuilder.Entity<COMMENT>()
                .HasMany(e => e.REPLies)
                .WithRequired(e => e.COMMENT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<COMPETE>()
                .Property(e => e.Slug)
                .IsUnicode(false);

            modelBuilder.Entity<COMPETE>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<COMPETE>()
                .HasMany(e => e.CHALLENGE_COMPETE)
                .WithRequired(e => e.COMPETE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LANGUAGE>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<LANGUAGE>()
                .HasMany(e => e.CHALLENGE_LANGUAGE)
                .WithRequired(e => e.LANGUAGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<REPLY>()
                .Property(e => e.Text)
                .IsUnicode(false);

            modelBuilder.Entity<SCHOOL>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<SCHOOL>()
                .HasMany(e => e.USER_INFO)
                .WithRequired(e => e.SCHOOL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TESTCASE>()
                .Property(e => e.Input)
                .IsUnicode(false);

            modelBuilder.Entity<TESTCASE>()
                .Property(e => e.Output)
                .IsUnicode(false);

            modelBuilder.Entity<USER_DATA>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<USER_DATA>()
                .Property(e => e.Data)
                .IsUnicode(false);

            modelBuilder.Entity<USER_INFO>()
                .Property(e => e.PhotoURL)
                .IsUnicode(false);

            modelBuilder.Entity<USER_INFO>()
                .Property(e => e.About)
                .IsUnicode(false);

            modelBuilder.Entity<USER_INFO>()
                .Property(e => e.FacebookLink)
                .IsUnicode(false);

            modelBuilder.Entity<USER_INFO>()
                .Property(e => e.GoogleLink)
                .IsUnicode(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.ANSWERs)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.CHALLENGEs)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.CHALLENGE_EDITOR)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.EditorID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.COMMENTs)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.COMPETEs)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.REPLies)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasOptional(e => e.USER_DATA)
                .WithRequired(e => e.USER_INFO);
        }
    }
}
