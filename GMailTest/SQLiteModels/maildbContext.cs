using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MailScan.SQLiteModels
{
    public partial class maildbContext : DbContext
    {
        public maildbContext()
        {
        }

        public maildbContext(DbContextOptions<maildbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlockedAddress> BlockedAddresses { get; set; }
        public virtual DbSet<BlockedDomain> BlockedDomains { get; set; }
        public virtual DbSet<BodyKeyword> BodyKeywords { get; set; }
        public virtual DbSet<MailDetail> MailDetails { get; set; }
        public virtual DbSet<SubjectKeyword> SubjectKeywords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             var location = Environment.CurrentDirectory;

            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=" + location + @"\maildb.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlockedAddress>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Emai).HasColumnType("STRING");
            });

            modelBuilder.Entity<BlockedDomain>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Domain).HasColumnType("STRING");
            });

            modelBuilder.Entity<BodyKeyword>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId)
                    .HasColumnType("INT")
                    .ValueGeneratedNever()
                    .HasColumnName("pkID");

                entity.Property(e => e.KeyWord).HasColumnType("STRING");
            });

            modelBuilder.Entity<MailDetail>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("pkID");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasColumnType("STRING");

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.From)
                    .IsRequired()
                    .HasColumnType("STRING");

                entity.Property(e => e.MailId)
                    .IsRequired()
                    .HasColumnType("STRING")
                    .HasColumnName("MailID");

                entity.Property(e => e.Rank).HasColumnType("INT");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("STRING");
            });

            modelBuilder.Entity<SubjectKeyword>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.KeyWord).HasColumnType("STRING");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
