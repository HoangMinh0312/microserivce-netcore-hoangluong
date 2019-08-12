using MassTransit;
using Messages.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication2.Services;
using WebApplication1.ViewModel;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        public IProductService _productService;
        private IBus _bus;
        public ProductController(IProductService productService, IBus bus)
        {
            _productService = productService;
            _bus = bus;
        }

        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            var products = _productService.GetAll();
            return products;
        }


        [HttpGet("{id}")]
        public Product GetById(int id)
        {
            var existingProduct = _productService.GetById(id);

            if (existingProduct == null)
            {
                return null;
            }

            return existingProduct;
        }

        [HttpPost]
        public Product Create([FromBody]CreateProductRequest model)
        {
            _bus.Send<SubmitProduct>(new { Id = Guid.Empty });
            var result = _productService.Create(model);
            return result;
        }


        [HttpPut]
        public Product Update([FromBody]UpdateProductRequest model, [FromServices] IAnotherService anotherService)
        {
            var result = _productService.Update(model);
            return result;
        }


        [HttpDelete("{id}")]
        public Product Delete(int id)
        {
            var result = _productService.Delete(new DeleteProductRequest { Id = id });
            return result;
        }
    }
}
