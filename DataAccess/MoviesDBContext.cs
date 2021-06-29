using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Movies.Data.Models;

#nullable disable

namespace Movies.Data.DataAccess
{
    public partial class MoviesDBContext : DbContext
    {
        public MoviesDBContext()
        {
        }

        public MoviesDBContext(DbContextOptions<MoviesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }        
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Reviewer> Reviewers { get; set; }
        //public virtual DbSet<ReviewerWatchHistory> ReviewerWatchHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Trace);
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Data Source=MSQL-03467;Initial Catalog=MoviesDB;Integrated Security=True")            
        }       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("Actor");

                entity.Property(e => e.ActorId).ValueGeneratedNever();

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Actor)
                    .HasForeignKey<Actor>(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Actor__ActorId__2D27B809");

                entity.HasMany(d => d.Movies)
                    .WithMany(d => d.Actors)
                    .UsingEntity(t => t.ToTable("MoviesActors"));
               
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.HasIndex(e => e.GenreName, "UQ__Genre__BBE1C339429BC4B1")
                    .IsUnique();

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasMany(d => d.Movies)
                   .WithMany(d => d.Genres)
                   .UsingEntity(t => t.ToTable("MovieGenres"));
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movie");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.MovieName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Movie_Producer");

                entity.HasMany(d => d.Actors)
                    .WithMany(d => d.Movies)                    
                    .UsingEntity(t => t.ToTable("MoviesActors"));


                entity.HasMany(d => d.Genres)
                    .WithMany(d => d.Movies)
                    .UsingEntity(t => t.ToTable("MovieGenres"));

                entity.HasMany(d => d.Reviewers)
                    .WithMany(p => p.Movies)
                    .UsingEntity<ReviewerWatchHistory>
                    (                        

                        j => j.HasOne(r => r.Reviewer)
                              .WithMany(w => w.ReviewerWatchHistories)
                              .HasForeignKey(r => r.ReviewerId),

                        j => j.HasOne(m => m.Movie)
                              .WithMany(mov => mov.ReviewerWatchHistories)
                              .HasForeignKey(f => f.MovieId),

                        j => j.HasKey(k => new { k.MovieId, k.ReviewerId })
                    );
            });


            //modelBuilder.Entity("MovieActors")                

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.HasIndex(e => e.PersonName, "UQ__Person__B88311BE6306F845")
                    .IsUnique();

                entity.Property(e => e.PersonId).ValueGeneratedNever();

                entity.Property(e => e.PersonName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.ToTable("Producer");

                entity.Property(e => e.ProducerId).ValueGeneratedNever();

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Producer)
                    .HasForeignKey<Producer>(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producer_Person");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Rate).HasDefaultValueSql("((5))");

                entity.Property(e => e.RevievText)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Review__MovieId__3A81B327");

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ReviewerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Review__Reviewer__3B75D760");
            });

            modelBuilder.Entity<Reviewer>(entity =>
            {
                entity.ToTable("Reviewer");

                entity.Property(e => e.ReviewerId).ValueGeneratedNever();

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Reviewer)
                    .HasForeignKey<Reviewer>(d => d.ReviewerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reviewer__Review__34C8D9D1");
            });           

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
