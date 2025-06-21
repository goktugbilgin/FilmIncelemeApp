using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FilmIncelemeApp.Models;

public partial class FilmIncelemeContext : DbContext
{
    public FilmIncelemeContext()
    {
    }

    public FilmIncelemeContext(DbContextOptions<FilmIncelemeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Filmler> Filmlers { get; set; }

    public virtual DbSet<Kategoriler> Kategorilers { get; set; }

    public virtual DbSet<Yorumlar> Yorumlars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=FilmIncelemeDb;User Id=SA;Password=SeninSifren123!;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Filmler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Filmler__3214EC0791CA3BE8");

            entity.ToTable("Filmler");

            entity.Property(e => e.Baslik).HasMaxLength(200);

            entity.HasOne(d => d.Kategori).WithMany(p => p.Filmlers)
                .HasForeignKey(d => d.KategoriId)
                .HasConstraintName("FK__Filmler__Kategor__44FF419A");
        });

        modelBuilder.Entity<Kategoriler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kategori__3214EC07DACA6DD7");

            entity.ToTable("Kategoriler");

            entity.Property(e => e.Ad).HasMaxLength(100);
        });

        modelBuilder.Entity<Yorumlar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Yorumlar__3214EC079C549521");

            entity.ToTable("Yorumlar");

            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.YorumYapan).HasMaxLength(100);

            entity.HasOne(d => d.Film).WithMany(p => p.Yorumlars)
                .HasForeignKey(d => d.FilmId)
                .HasConstraintName("FK__Yorumlar__FilmId__47DBAE45");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
