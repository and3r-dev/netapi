using Microsoft.EntityFrameworkCore;
using VeiculoAPI.Models;

namespace VeiculoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Carro> Carros { get; set; }
        public DbSet<Caminhao> Caminhoes { get; set; }
        public DbSet<Revisao> Revisoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Veiculo>().HasData(
                new Veiculo { Id = 1, Placa = "ABC1234", Ano = 2020, Cor = "Preto", Modelo = "Sedan" },
                new Veiculo { Id = 2, Placa = "XYZ5678", Ano = 2018, Cor = "Branco", Modelo = "Caminhão" }
            );

            modelBuilder.Entity<Carro>().HasData(
                new Carro { Id = 1, CapacidadePassageiro = 5, VeiculoId = 1 }
            );

            modelBuilder.Entity<Caminhao>().HasData(
                new Caminhao { Id = 1, CapacidadeCarga = 15000, VeiculoId = 2 }
            );

            modelBuilder.Entity<Revisao>().HasData(
                new Revisao { Id = 1, Km = 10000, Data = new DateTime(2021, 01, 15), ValorDaRevisao = 500, VeiculoId = 1 },
                new Revisao { Id = 2, Km = 20000, Data = new DateTime(2022, 06, 30), ValorDaRevisao = 800, VeiculoId = 1 },
                new Revisao { Id = 3, Km = 50000, Data = new DateTime(2023, 03, 20), ValorDaRevisao = 1200, VeiculoId = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
