using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;
namespace Test
{
    [TestClass]
    public class VeiculoServicoTest
    {
        private static DbContexto CriarContextoDeTeste()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DbContexto(configuration);
        }

        [TestInitialize]
        public void Setup()
        {
            // Ensure database is created and clean before each test
            var context = CriarContextoDeTeste();
            context.Database.EnsureCreated();
            context.Database.ExecuteSqlRaw("DELETE FROM veiculos");
        }

        [TestMethod]
        public void TestIncluir()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            var servico = new VeiculoServico(context);
            
            Veiculo veiculo = new()
            {
                Ano = 2001,
                Marca = "Honda",
                Nome = "Civic"
            };
            
            // Act
            servico.Incluir(veiculo);

            // Assert
            Assert.AreEqual(1, servico.Todos(1).Count);
        }

        [TestMethod]
        public void TestApagar()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            var servico = new VeiculoServico(context);
            
            Veiculo veiculo = new()
            {
                Ano = 2001,
                Marca = "Honda",
                Nome = "Civic"
            };
            
            // Act
            servico.Incluir(veiculo);
            var countBeforeDelete = servico.Todos(1).Count;
            servico.Apagar(veiculo);
            var countAfterDelete = servico.Todos(1).Count;

            // Assert
            Assert.AreEqual(1, countBeforeDelete);
            Assert.AreEqual(0, countAfterDelete);
        }

        [TestMethod]
        public void TestAtualizar()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            var servico = new VeiculoServico(context);
            
            Veiculo veiculo = new()
            {
                Ano = 2020,
                Marca = "Honda",
                Nome = "Civic"
            };
            
            servico.Incluir(veiculo);
            
            // Act
            veiculo.Ano = 2024;
            veiculo.Marca = "Renaut";
            veiculo.Nome = "Qwid";
            servico.Atualizar(veiculo);

            // Assert
            var veiculoAtualizado = servico.BuscaPorId(veiculo.Id);
            Assert.AreEqual(2024, veiculoAtualizado?.Ano);
            Assert.AreEqual("Renaut", veiculoAtualizado?.Marca);
            Assert.AreEqual("Qwid", veiculoAtualizado?.Nome);
        }

        [TestMethod]
        public void TestBuscaPorId()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            var servico = new VeiculoServico(context);
            
            Veiculo veiculo = new()
            {
                Ano = 2024,
                Marca = "GTR",
                Nome = "Porshe"
            };
            
            // Act
            servico.Incluir(veiculo);
            Veiculo? veiculoDoBanco = servico.BuscaPorId(veiculo.Id);

            // Assert
            Assert.IsNotNull(veiculoDoBanco);
            Assert.AreEqual(veiculo.Id, veiculoDoBanco.Id);
            Assert.AreEqual("GTR", veiculoDoBanco.Marca);
            Assert.AreEqual("Porshe", veiculoDoBanco.Nome);
        }
    }
}