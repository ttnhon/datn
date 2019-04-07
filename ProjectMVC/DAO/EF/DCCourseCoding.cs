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

        public virtual DbSet<ANSWER> ANSWERS { get; set; }
        public virtual DbSet<CHALLENGE> CHALLENGES { get; set; }
        public virtual DbSet<COMPETE> COMPETES { get; set; }
        public virtual DbSet<LANGUAGE_CODE> LANGUAGE_CODES { get; set; }
        public virtual DbSet<USER_INFO> USER_INFOS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ANSWER>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.Problem)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.InputQuestion)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.OutputQuestion)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .Property(e => e.Solution)
                .IsUnicode(false);

            modelBuilder.Entity<CHALLENGE>()
                .HasMany(e => e.ANSWERs)
                .WithRequired(e => e.CHALLENGE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<COMPETE>()
                .Property(e => e.Describe)
                .IsUnicode(false);

            modelBuilder.Entity<LANGUAGE_CODE>()
                .Property(e => e.Describe)
                .IsUnicode(false);

            modelBuilder.Entity<LANGUAGE_CODE>()
                .HasMany(e => e.CHALLENGEs)
                .WithOptional(e => e.LANGUAGE_CODE)
                .HasForeignKey(e => e.LanguageCode);

            modelBuilder.Entity<USER_INFO>()
                .Property(e => e.PhotoURL)
                .IsUnicode(false);

            modelBuilder.Entity<USER_INFO>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.ANSWERs)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_INFO>()
                .HasMany(e => e.COMPETEs)
                .WithRequired(e => e.USER_INFO)
                .HasForeignKey(e => e.TeacherID)
                .WillCascadeOnDelete(false);
        }
    }
}
