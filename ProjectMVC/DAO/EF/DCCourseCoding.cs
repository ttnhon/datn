namespace DAO.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DCCourseCoding : DbContext
    {
        public DCCourseCoding()
            : base("name=CODING_CHALLENGE")
        {
        }

        public virtual DbSet<ANSWER> ANSWERS { get; set; }
        public virtual DbSet<CHALLENGE> CHALLENGES { get; set; }
        public virtual DbSet<CHALLENGE_EDITOR> CHALLENGE_EDITORS { get; set; }
        public virtual DbSet<CHALLENGE_IN_COMPETE> CHALLENGE_IN_COMPETES { get; set; }
        public virtual DbSet<COMPETE> COMPETES { get; set; }
        public virtual DbSet<LANGUAGE_CODE> LANGUAGE_CODES { get; set; }
        public virtual DbSet<TESTCASE> TESTCASES { get; set; }
        public virtual DbSet<USER_INFO> USER_INFOS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ANSWER>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<ANSWER>()
                .Property(e => e.TimeDone)
                .IsFixedLength();

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
                .Property(e => e.Languages)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.ANSWER)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.CHALLENGE_EDITOR)
                .WithRequired(e => e.CHALLENGE)
                .HasForeignKey(e => e.ChallegenID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.CHALLENGE_IN_COMPETE)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.TESTCASE)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<COMPETE>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<COMPETE>()
                .HasMany(e => e.CHALLENGE_IN_COMPETE)
                .WithRequired(e => e.COMPETE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LANGUAGE_CODE>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<TESTCASE>()
                .Property(e => e.Input)
                .IsUnicode(false);

            modelBuilder.Entity<TESTCASE>()
                .Property(e => e.Output)
                .IsUnicode(false);

            modelBuilder.Entity<USER_INFO>()
                .Property(e => e.PhotoURL)
                .IsUnicode(false);

            modelBuilder.Entity<USER_INFO>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.ANSWER)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.CHALLENGE)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.CHALLENGE_EDITOR)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.EditorID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.COMPETE)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(false);
        }
    }
}
