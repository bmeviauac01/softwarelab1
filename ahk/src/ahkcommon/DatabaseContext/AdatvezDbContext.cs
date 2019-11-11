using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace adatvez.DatabaseContext
{
    public partial class AdatvezDbContext : DbContext
    {
        public AdatvezDbContext()
        {
        }

        public AdatvezDbContext(DbContextOptions<AdatvezDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Afa> Afa { get; set; }
        public virtual DbSet<FizetesMod> FizetesMod { get; set; }
        public virtual DbSet<Kategoria> Kategoria { get; set; }
        public virtual DbSet<Megrendeles> Megrendeles { get; set; }
        public virtual DbSet<MegrendelesTetel> MegrendelesTetel { get; set; }
        public virtual DbSet<Statusz> Statusz { get; set; }
        public virtual DbSet<Szamla> Szamla { get; set; }
        public virtual DbSet<SzamlaKiallito> SzamlaKiallito { get; set; }
        public virtual DbSet<SzamlaTetel> SzamlaTetel { get; set; }
        public virtual DbSet<Telephely> Telephely { get; set; }
        public virtual DbSet<Termek> Termek { get; set; }
        public virtual DbSet<Vevo> Vevo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Afa>(entity =>
            {
                entity.ToTable("AFA");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<FizetesMod>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Mod).HasMaxLength(20);
            });

            modelBuilder.Entity<Kategoria>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nev).HasMaxLength(50);

                entity.HasOne(d => d.SzuloKategoriaNavigation)
                    .WithMany(p => p.InverseSzuloKategoriaNavigation)
                    .HasForeignKey(d => d.SzuloKategoria);
            });

            modelBuilder.Entity<Megrendeles>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Datum).HasColumnType("datetime");

                entity.Property(e => e.FizetesModId).HasColumnName("FizetesModID");

                entity.Property(e => e.Hatarido).HasColumnType("datetime");

                entity.Property(e => e.StatuszId).HasColumnName("StatuszID");

                entity.Property(e => e.TelephelyId).HasColumnName("TelephelyID");

                entity.HasOne(d => d.FizetesMod)
                    .WithMany(p => p.Megrendeles)
                    .HasForeignKey(d => d.FizetesModId);

                entity.HasOne(d => d.Statusz)
                    .WithMany(p => p.Megrendeles)
                    .HasForeignKey(d => d.StatuszId);

                entity.HasOne(d => d.Telephely)
                    .WithMany(p => p.Megrendeles)
                    .HasForeignKey(d => d.TelephelyId);
            });

            modelBuilder.Entity<MegrendelesTetel>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MegrendelesId).HasColumnName("MegrendelesID");

                entity.Property(e => e.StatuszId).HasColumnName("StatuszID");

                entity.Property(e => e.TermekId).HasColumnName("TermekID");

                entity.HasOne(d => d.Megrendeles)
                    .WithMany(p => p.MegrendelesTetel)
                    .HasForeignKey(d => d.MegrendelesId);

                entity.HasOne(d => d.Statusz)
                    .WithMany(p => p.MegrendelesTetel)
                    .HasForeignKey(d => d.StatuszId);

                entity.HasOne(d => d.Termek)
                    .WithMany(p => p.MegrendelesTetel)
                    .HasForeignKey(d => d.TermekId);
            });

            modelBuilder.Entity<Statusz>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nev).HasMaxLength(20);
            });

            modelBuilder.Entity<Szamla>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FizetesiHatarido).HasColumnType("datetime");

                entity.Property(e => e.FizetesiMod).HasMaxLength(20);

                entity.Property(e => e.KiallitasDatum).HasColumnType("datetime");

                entity.Property(e => e.KiallitoId).HasColumnName("KiallitoID");

                entity.Property(e => e.MegrendelesId).HasColumnName("MegrendelesID");

                entity.Property(e => e.MegrendeloIr)
                    .HasColumnName("MegrendeloIR")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.MegrendeloNev).HasMaxLength(50);

                entity.Property(e => e.MegrendeloUtca).HasMaxLength(50);

                entity.Property(e => e.MegrendeloVaros).HasMaxLength(50);

                entity.Property(e => e.TeljesitesDatum).HasColumnType("datetime");

                entity.HasOne(d => d.Kiallito)
                    .WithMany(p => p.Szamla)
                    .HasForeignKey(d => d.KiallitoId);

                entity.HasOne(d => d.Megrendeles)
                    .WithMany(p => p.Szamla)
                    .HasForeignKey(d => d.MegrendelesId);
            });

            modelBuilder.Entity<SzamlaKiallito>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Adoszam)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ir)
                    .HasColumnName("IR")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Nev).HasMaxLength(50);

                entity.Property(e => e.Szamlaszam)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Utca).HasMaxLength(50);

                entity.Property(e => e.Varos).HasMaxLength(50);
            });

            modelBuilder.Entity<SzamlaTetel>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Afakulcs).HasColumnName("AFAKulcs");

                entity.Property(e => e.MegrendelesTetelId).HasColumnName("MegrendelesTetelID");

                entity.Property(e => e.Nev).HasMaxLength(50);

                entity.Property(e => e.SzamlaId).HasColumnName("SzamlaID");

                entity.HasOne(d => d.MegrendelesTetel)
                    .WithMany(p => p.SzamlaTetel)
                    .HasForeignKey(d => d.MegrendelesTetelId);

                entity.HasOne(d => d.Szamla)
                    .WithMany(p => p.SzamlaTetel)
                    .HasForeignKey(d => d.SzamlaId);
            });

            modelBuilder.Entity<Telephely>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fax)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Ir)
                    .HasColumnName("IR")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Utca).HasMaxLength(50);

                entity.Property(e => e.Varos).HasMaxLength(50);

                entity.Property(e => e.VevoId).HasColumnName("VevoID");

                entity.HasOne(d => d.Vevo)
                    .WithMany(p => p.Telephely)
                    .HasForeignKey(d => d.VevoId);
            });

            modelBuilder.Entity<Termek>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Afaid).HasColumnName("AFAID");

                entity.Property(e => e.KategoriaId).HasColumnName("KategoriaID");

                entity.Property(e => e.Kep).HasColumnType("image");

                entity.Property(e => e.Leiras).HasColumnType("xml");

                entity.Property(e => e.Nev).HasMaxLength(50);

                entity.HasOne(d => d.Afa)
                    .WithMany(p => p.Termek)
                    .HasForeignKey(d => d.Afaid);

                entity.HasOne(d => d.Kategoria)
                    .WithMany(p => p.Termek)
                    .HasForeignKey(d => d.KategoriaId);
            });

            modelBuilder.Entity<Vevo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Jelszo).HasMaxLength(50);

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Nev).HasMaxLength(50);

                entity.Property(e => e.Szamlaszam)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.KozpontiTelephelyNavigation)
                    .WithMany(p => p.VevoNavigation)
                    .HasForeignKey(d => d.KozpontiTelephely)
                    .HasConstraintName("Vevo_KozpontiTelephely");
            });
        }
    }
}
