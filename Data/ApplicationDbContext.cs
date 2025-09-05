using Microsoft.EntityFrameworkCore;
using OGCBackend.Models;

namespace OGCBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Partida> Partidas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Partida>(entity =>
            {
                entity.ToTable("partidas");
                
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityColumn();
                
                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsRequired();
                
                entity.Property(e => e.Familia)
                    .HasColumnName("familia")
                    .HasMaxLength(100)
                    .IsRequired();
                
                entity.Property(e => e.SubPartida)
                    .HasColumnName("sub_partida")
                    .HasMaxLength(200);
                
                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);
                
                entity.Property(e => e.Aprobado)
                    .HasColumnName("aprobado")
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);
                
                entity.Property(e => e.Pagado)
                    .HasColumnName("pagado")
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);
                
                entity.Property(e => e.PorLiquidar)
                    .HasColumnName("por_liquidar")
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);
                
                entity.Property(e => e.Actual)
                    .HasColumnName("actual")
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);
                
                entity.Property(e => e.FechaCarga)
                    .HasColumnName("fecha_carga")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(e => e.ArchivoOrigen)
                    .HasColumnName("archivo_origen")
                    .HasMaxLength(255);                

                entity.HasIndex(e => e.Familia)
                    .HasDatabaseName("idx_partidas_familia");
                
                entity.HasIndex(e => e.FechaCarga)
                    .HasDatabaseName("idx_partidas_fecha_carga");
            });
        }
    }
}