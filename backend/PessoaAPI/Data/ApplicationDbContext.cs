using Microsoft.EntityFrameworkCore;
using PessoaAPI.Models;

namespace PessoaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; } = null!;
        public DbSet<PessoaV2> PessoasV2 { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para CPF único
            modelBuilder.Entity<Pessoa>()
                .HasIndex(p => p.CPF)
                .IsUnique();

            modelBuilder.Entity<PessoaV2>()
                .HasIndex(p => p.CPF)
                .IsUnique();

            // Configuração para email único (quando preenchido)
            modelBuilder.Entity<Pessoa>()
                .HasIndex(p => p.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");

            modelBuilder.Entity<PessoaV2>()
                .HasIndex(p => p.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");
        }
    }
}
