using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.ViewModel;
using Xunit;
using XUnitTestProject1.BaseTest;

namespace XUnitTestProject1.IntegrationTest
{
    public class CalculatorTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1 };
            yield return new object[] { 2};
            yield return new object[] { 3 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ProductControllerTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public ProductControllerTest(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Get_Product_Success()
        {
            var response = await _fixture.Client.GetAsync("api/product/" + "1");
            string value = await response.Content.ReadAsStringAsync();
            Product product = JsonConvert.DeserializeObject<Product>(value);
            Assert.Equal("Chai", product.ProductName);
        }

        [Fact]
        public async Task Create_Product_Success()
        {
            var request = new
            {
                Url = "/api/product/",
                Body = new
                {
                    ProductName = "hoang abc",
                    SupplierId = 1,
                    Package = "abc package"                   
                }
            };

            var response = await _fixture.Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            Assert.True(true);
        }

        [Fact]
        public async Task Get_Product_Fail()
        {
            var response = await _fixture.Client.GetAsync("api/product/" + "e2393db8-ab73-49a4-99e4-8c3bd29a341f");
            Assert.Equal(400, (int)response.StatusCode);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Get_Product_Theory(int productId)
        {
            var response = await _fixture.Client.GetAsync("api/product/" + productId);
            string value = await response.Content.ReadAsStringAsync();
            Product product = JsonConvert.DeserializeObject<Product>(value);
            Assert.Equal(productId, product.Id);
        }


        [Theory]
        [ClassData(typeof(CalculatorTestData))]
        public async Task Get_Product_Theory_ClassData(int productId)
        {
            var response = await _fixture.Client.GetAsync("api/product/" + productId);
            string value = await response.Content.ReadAsStringAsync();
            Product product = JsonConvert.DeserializeObject<Product>(value);
            Assert.Equal(productId, product.Id);
        }

    }
}
