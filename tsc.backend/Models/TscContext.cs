using System;
using Microsoft.EntityFrameworkCore;

namespace tsc.backend.Models
{
    public class TscContext : DbContext
    {
        public TscContext(DbContextOptions<TscContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Subdivision> Subdivisions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // catalogs
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("countries");

                entity.HasIndex(e => e.CountryCode)
                    .HasName("ak_countries_country_code")
                    .IsUnique();

                entity.HasIndex(e => e.Alfa2)
                    .HasName("ak_countries_alfa2")
                    .IsUnique();

                entity.HasIndex(e => e.Alfa3)
                    .HasName("ak_countries_alfa3")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CommonName)
                    .IsRequired()
                    .HasColumnName("common_name")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.IsoName)
                    .IsRequired()
                    .HasColumnName("iso_name")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("numeric_iso");

                entity.Property(e => e.Alfa2)
                    .IsRequired()
                    .HasColumnName("alfa2")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Alfa3)
                    .IsRequired()
                    .HasColumnName("alfa3")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.PhonePrefix)
                    .IsRequired()
                    .HasColumnName("phone_prefix")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasColumnName("row_version")
                    .IsRowVersion();
            });

            // catalogs
            modelBuilder.Entity<Subdivision>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("subdivisions");


                entity.HasOne(d => d.CountryNav)
                    .WithMany(p => p.Subdivisions)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_countries_to_subdivisions_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasColumnName("country_id");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .HasColumnName("row_version")
                    .IsRowVersion();
            });
        }
    }
}
