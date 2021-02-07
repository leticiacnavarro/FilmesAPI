using API_Filmes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Filmes.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Voto> Votos { get; set; }
        public DbSet<Ator> Atores { get; set; }
        public DbSet<AtorFilme> AtoresFilmes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //   modelBuilder.Entity<AtorFilme>().HasKey(c => new { c.FilmeId, c.AtorId });
            //       modelBuilder.Entity<Filme>()(c => new { c.FilmeId, c.AtorId });

            modelBuilder.Entity<Filme>()
                .HasMany(p => p.Atores)
                .WithMany(p => p.Filmes)
                .UsingEntity<AtorFilme>(
                    j => j
                        .HasOne(pt => pt.Ator)
                        .WithMany(t => t.AtoresFilmes)
                        .HasForeignKey(pt => pt.AtorId),
                    j => j
                        .HasOne(pt => pt.Filme)
                        .WithMany(p => p.AtoresFilmes)
                        .HasForeignKey(pt => pt.FilmeId),
                    j =>
                    {
                        j.HasKey(t => new { t.FilmeId, t.AtorId });
                    }); 

            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Filme>().ToTable("Filme");
            modelBuilder.Entity<Voto>().ToTable("Voto");
            modelBuilder.Entity<Ator>().ToTable("Ator");
            modelBuilder.Entity<AtorFilme>().ToTable("AtorFilme");



        }
    }
}