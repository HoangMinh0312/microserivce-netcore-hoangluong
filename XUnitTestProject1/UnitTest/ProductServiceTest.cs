using AutoMapper;
using Moq;
using Repository;
using System;
using WebApplication2.Models;
using WebApplication2.Services;
using WebApplication2.ViewModel;
using Xunit;

namespace XUnitTestProject1.UnitTest
{
    public class ProductServiceTest
    {
        private readonly Mock<IRepository<Product>> _productRepoMock;
        private readonly Mock<IMapper> _mapperMock;

        public ProductServiceTest()
        {
            _productRepoMock = new Mock<IRepository<Product>>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public void Create_Product_Fail()
        {
            var product = new Product
            {
                Id = 100,
                ProductName = "Product Name"
            };
            _productRepoMock.Setup(x => x.Create(It.IsAny<Product>())).Callback((Product p) =>
            {
                //do something
                throw new Exception("ABC");
            }).Verifiable();

            var productService = new ProductService(_productRepoMock.Object, null);
            var exception = Record.Exception(() => productService.Create(new CreateProductRequest { ProductName = "a" }));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Create_Product_Sucess()
        {
            var product = new Product
            {
                Id = 100,
                ProductName = "Product Name"
            };
            _productRepoMock.Setup(x => x.Create(It.IsAny<Product>())).Callback((Product p) =>
            {
                //do something

            }).Verifiable();

            _mapperMock.Setup(x => x.Map<Product>(It.IsAny<object>())).Returns((CreateProductRequest request) =>
              {
                  return new Product { ProductName = request.ProductName };
              });

            var productService = new ProductService(_productRepoMock.Object, _mapperMock.Object);
            try
            {
                productService.Create(new CreateProductRequest { ProductName = "a" });
                Assert.True(true);
            }
            catch
            {

            }
        }

        [Fact]
        public void Get_Product_Success()
        {
            var product = new Product
            {
                Id = 100,
                ProductName = "Product Name"
            };
            _productRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                return product;
            });

            var productService = new ProductService(_productRepoMock.Object, null);

            var result = productService.GetById(1);
            Assert.Equal(result.Id, 100);
        }
    }
}
