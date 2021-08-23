using Microsoft.EntityFrameworkCore;

namespace OSKI_Solutions_Test.Models
{
    public partial class OSKI_DBContext : DbContext
    {
        public OSKI_DBContext()
        {
        }

        public OSKI_DBContext(DbContextOptions<OSKI_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answers> Answers { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<Test_User_Junction> Test_User_Junction { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=Taras;Database=OSKI_DB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answers>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnName("Text");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Test");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Text).IsRequired();
            });

            modelBuilder.Entity<Test_User_Junction>(entity =>
            {
                entity.HasKey(t => new { t.TestId, t.UserId });

                //entity.ToTable("Test_User_Junction");

                //entity.HasIndex(e => new { e.TestId, e.UserId })
                //    .HasName("Test_User")
                //    .IsUnique();

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Test_User_Junction)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Test_User_Junction_Test_User_Junction");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Test_User_Junction)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Test_User_Junction_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
