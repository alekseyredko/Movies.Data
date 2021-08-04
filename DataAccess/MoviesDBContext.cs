using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Movies.Data.Models;

#nullable disable

namespace Movies.Data.DataAccess
{
    public partial class MoviesDBContext : DbContext
    {
        public MoviesDBContext(DbContextOptions<MoviesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<MoviesActor> MoviesActors { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Reviewer> Reviewers { get; set; }
        public virtual DbSet<ReviewerWatchHistory> ReviewerWatchHistories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Trace);
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("settings.json")
                    .Build();
                
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
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
                                        
                    .HasConstraintName("FK__Actor__ActorId__2D27B809");
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
                    .UsingEntity<MoviesActor>
                    (
                        j => j.HasOne(d => d.Actor)
                            .WithMany(p => p.MoviesActors)
                            .HasForeignKey(d => d.ActorId)
                            
                            .HasConstraintName("FK__MoviesAct__Actor__4222D4EF"),

                        j => j.HasOne(d => d.Movie)
                            .WithMany(p => p.MoviesActors)
                            .HasForeignKey(d => d.MovieId)
                            
                            .HasConstraintName("FK__MoviesAct__Movie__4316F928"),

                        j => j.HasKey(e => new { e.ActorId, e.MovieId })
                             .HasName("PK__MoviesAc__E30EC30A44E7D562")
                    );

                entity.HasMany(d => d.Genres)
                    .WithMany(d => d.Movies)
                    .UsingEntity<MovieGenre>
                    (                    
                        j => j.HasOne(d => d.Genre)
                            .WithMany(p => p.MovieGenres)
                            .HasForeignKey(d => d.GenreId)
                            
                            .HasConstraintName("FK__MovieGenr__Genre__3E52440B"),

                        j => j.HasOne(d => d.Movie)
                            .WithMany(p => p.MovieGenres)
                            .HasForeignKey(d => d.MovieId)
                            
                            .HasConstraintName("FK__MovieGenr__Movie__3F466844"),

                        j => j.HasKey(e => new { e.GenreId, e.MovieId })
                            .HasName("PK__MovieGen__B7382C3F24545B4F")
                    );


                modelBuilder.Entity<RefreshToken>(entity =>
                {
                    entity.ToTable("RefreshToken");                    

                    entity.Property(e => e.Created).HasColumnType("datetime");

                    entity.Property(e => e.Expires).HasColumnType("datetime");

                    entity.Property(e => e.Token)
                        .IsRequired()
                        .HasMaxLength(64);

                    entity.HasOne(d => d.User)
                        .WithMany(p => p.RefreshTokens)
                        .HasForeignKey(d => d.UserId)
                        .HasConstraintName("FK_RefreshToken_User");
                });

                entity.HasMany(d => d.Reviewers)
                    .WithMany(p => p.Movies)
                    .UsingEntity<ReviewerWatchHistory>
                    (
                        j => j.HasOne(r => r.Reviewer)
                              .WithMany(w => w.ReviewerWatchHistories)
                              .HasForeignKey(r => r.ReviewerId)
                              .HasConstraintName("FK__ReviewerW__Revie__45F365D3"),

                        j => j.HasOne(m => m.Movie)
                              .WithMany(mov => mov.ReviewerWatchHistories)
                              .HasForeignKey(f => f.MovieId)
                              .HasConstraintName("FK__ReviewerW__Movie__44FF419A"),

                        j => j.HasKey(k => new { k.MovieId, k.ReviewerId })
                                .HasName("PK_ReviewerWatchHistory")
                    );
            });

            modelBuilder.Entity<ReviewerWatchHistory>(entity =>
            {
                entity.ToTable("ReviewerWatchHistory");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.PersonName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.ToTable("Producer");

                entity.Property(e => e.ProducerId).ValueGeneratedNever();

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Producer)
                    .HasForeignKey<Producer>(d => d.ProducerId)
                    
                    .HasConstraintName("FK_Producer_Person");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Rate)
                    .IsRequired()
                    .HasDefaultValueSql("((5))");

                entity.Property(e => e.ReviewText)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK__Review__MovieId__3A81B327");

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ReviewerId)
                    .HasConstraintName("FK__Review__Reviewer__3B75D760");
            });

            modelBuilder.Entity<Reviewer>(entity =>
            {
                entity.ToTable("Reviewer");

                entity.Property(e => e.ReviewerId).ValueGeneratedNever();

                entity.Property(e => e.NickName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Reviewer)
                    .HasForeignKey<Reviewer>(d => d.ReviewerId)
                    .HasConstraintName("FK__Reviewer__Review__34C8D9D1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.UserId)
                    .HasConstraintName("FK_User_Person");

                entity.Ignore(x => x.Password);
                entity.Ignore(x => x.Name);
                entity.Ignore(x => x.Token);
                entity.Ignore(x => x.RefreshToken);
                entity.Ignore(x => x.Roles);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
