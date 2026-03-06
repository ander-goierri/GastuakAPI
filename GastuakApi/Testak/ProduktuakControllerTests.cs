using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using GastuakApi.Controllerrak;
using GastuakApi.Modeloak;
using GastuakApi.Repositorioak;

namespace GastuakApi.Testak
{
    public class ProduktuakControllerTests
    {
        [Fact]
        public void Get_OkItzultzenDu_ProduktuakExistitzenDirenean()
        {
            // Definizioa
            var mockRepo = new Mock<ProduktuaRepository>();
            var product = new Produktua(1, "Test", 10.0m);

            mockRepo.Setup(r => r.Get(1)).Returns(product);

            var controller = new ProduktuakController(mockRepo.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduktua = Assert.IsType<Produktua>(okResult.Value);
            Assert.Equal("Test", returnedProduktua.Izena);
        }

        [Fact]
        public void Get_NotFoundItzultzenDu_ProduktuakEzDireneanExistitzen()
        {
            // Arrange
            var mockRepo = new Mock<ProduktuaRepository>();
            mockRepo.Setup(r => r.Get(1)).Returns((Produktua)null);

            var controller = new ProduktuakController(mockRepo.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
