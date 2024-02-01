using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sbo.AndEmiliTest.Database;

public partial class SboAndEmiliTestContext : DbContext
{
    public SboAndEmiliTestContext()
    {
    }

    public SboAndEmiliTestContext(DbContextOptions<SboAndEmiliTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NbaPlayer> NbaPlayers { get; set; }

    public virtual DbSet<NbaPlayerStat> NbaPlayerStats { get; set; }

    public virtual DbSet<Top100NbaScorer> Top100NbaScorers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserFavouritePlayer> UserFavouritePlayers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=Sbo.AndEmiliTest.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NbaPlayer>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INT");
            entity.Property(e => e.Name).HasColumnType("VARCHAR(100)");
        });

        modelBuilder.Entity<NbaPlayerStat>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INT");
            entity.Property(e => e.Date).HasColumnType("DATE");
            entity.Property(e => e.GameId).HasColumnType("INT");
            entity.Property(e => e.NbaPlayerId).HasColumnType("INT");
            entity.Property(e => e.Points).HasColumnType("INT");
        });

        modelBuilder.Entity<Top100NbaScorer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Top100NbaScorers");

            entity.Property(e => e.NbaPlayerId).HasColumnType("INT");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INT");
            entity.Property(e => e.Email).HasColumnType("VARCHAR(50)");
        });

        modelBuilder.Entity<UserFavouritePlayer>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INT");
            entity.Property(e => e.PlayerId).HasColumnType("INT");
            entity.Property(e => e.UserId).HasColumnType("INT");

            entity.HasOne(d => d.Player).WithMany(p => p.UserFavouritePlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.UserFavouritePlayers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
