using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace iCollegueWebAPI.Models
{
    public partial class iColleagueContext : DbContext
    {
        public iColleagueContext()
        {
        }

        public iColleagueContext(DbContextOptions<iColleagueContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FileTable> FileTables { get; set; } = null!;
        public virtual DbSet<TblKnowledgeBase> TblKnowledgeBases { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=iColleagueConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileTable>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK__FileTabl__C2C6FFDC5B4CEFB2");

                entity.ToTable("FileTable");

                entity.Property(e => e.FileId).HasColumnName("fileId");

                entity.Property(e => e.FileContent).HasColumnName("fileContent");

                entity.Property(e => e.FileName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("fileName");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("filePath");
            });

            modelBuilder.Entity<TblKnowledgeBase>(entity =>
            {
                entity.ToTable("tblKnowledgeBase");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Answer)
                    .HasColumnType("text")
                    .HasColumnName("answer");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Question)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("question");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
