
using MinimalApi.Dominio.Entidades;
namespace Test
{
    [TestClass]
    public class VeiculosTest
    {
        [TestMethod]
        public void TestCriarVeiculo()
        {
            // Arrange & Act
            var veiculo = new Veiculo
            {
                Id = 1,
                Marca = "Honda",
                Nome = "Civic",
                Ano = 2021
            };

            // Assert
            Assert.IsNotNull(veiculo);
            Assert.AreEqual(1, veiculo.Id);
            Assert.AreEqual("Honda", veiculo.Marca);
            Assert.AreEqual("Civic", veiculo.Nome);
            Assert.AreEqual(2021, veiculo.Ano);
        }
    }
}