using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotNet5Crud.Models
{
    /*
     * Alex Vergara
     * 4/24/2022
     * cst-451
     * Capstone
     * 
     * This the context model for our audio file
     */
    public partial class AudioFileDBContext : DbContext
    {
        // constructor
        public AudioFileDBContext(DbContextOptions<AudioFileDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        // getters and setters
        public virtual DbSet<AudioFile> AudioFiles { get; set; }

        // 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Model entity parses values
            modelBuilder.Entity<AudioFile>(entity =>
            {
                entity.Property(e => e.Genre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BPM)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
       

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
