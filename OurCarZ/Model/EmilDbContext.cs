﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OurCarZ.Model
{
    public partial class EmilDbContext : DbContext
    {
        public EmilDbContext()
        {
        }

        public EmilDbContext(DbContextOptions<EmilDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Institution> Institutions { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<RatingDatabase> RatingDatabases { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRoute> UserRoutes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=emilzealanddb.database.windows.net;Initial Catalog=emil-db;User ID=emiladmin;Password=Sql12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.RoadName).IsUnicode(false);
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.LicensePlate)
                    .HasName("PK__tmp_ms_x__026BC15DF578E93D");

                entity.Property(e => e.LicensePlate).IsUnicode(false);

                entity.Property(e => e.Model).IsUnicode(false);

                entity.Property(e => e.Seats).IsUnicode(false);

                entity.Property(e => e.Year).IsUnicode(false);
            });

            modelBuilder.Entity<Institution>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Zipcode).IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.MessageText).IsUnicode(false);

                entity.HasOne(d => d.MessageFromNavigation)
                    .WithMany(p => p.MessageMessageFromNavigations)
                    .HasForeignKey(d => d.MessageFrom)
                    .HasConstraintName("FK__Messages__Messag__3B40CD36");

                entity.HasOne(d => d.MessageToNavigation)
                    .WithMany(p => p.MessageMessageToNavigations)
                    .HasForeignKey(d => d.MessageTo)
                    .HasConstraintName("FK__Messages__Messag__3A4CA8FD");
            });

            modelBuilder.Entity<RatingDatabase>(entity =>
            {
                entity.HasOne(d => d.UserRated)
                    .WithMany()
                    .HasForeignKey(d => d.UserRatedId)
                    .HasConstraintName("FK__RatingDat__UserR__2A164134");

                entity.HasOne(d => d.UserRating)
                    .WithMany()
                    .HasForeignKey(d => d.UserRatingId)
                    .HasConstraintName("FK__RatingDat__UserR__2B0A656D");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.Property(e => e.StartTime).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.FinishPointNavigation)
                    .WithMany(p => p.RouteFinishPointNavigations)
                    .HasForeignKey(d => d.FinishPoint)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Route_ToAdress2");

                entity.HasOne(d => d.StartPointNavigation)
                    .WithMany(p => p.RouteStartPointNavigations)
                    .HasForeignKey(d => d.StartPoint)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Route_ToAdress1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Routes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Route_ToUser");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ConfirmPassword).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.LicensePlate).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.HasOne(d => d.LicensePlateNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LicensePlate)
                    .HasConstraintName("FK_User_ToCar");
            });

            modelBuilder.Entity<UserRoute>(entity =>
            {
                entity.Property(e => e.UserRouteId).ValueGeneratedNever();

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.UserRoutes)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("FK_UserRoute_ToRoute");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoutes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRoute_ToUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}