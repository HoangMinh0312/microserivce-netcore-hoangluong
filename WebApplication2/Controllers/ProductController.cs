using MassTransit;
using Messages.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Config;
using WebApplication2.Filters;
using WebApplication2.Models;
using WebApplication2.Services;
using WebApplication2.ViewModel;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        public IProductService _productService;
        private readonly MySettings settings;
        private IBus _bus;
        public ProductController(IProductService productService, IOptions<MySettings> mysettings, IBus bus)
        {
            this.settings = mysettings.Value;
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
            _bus.Send<SubmitProduct>(new { ProductName = model.ProductName, SupplierId = model.SupplierId, Package = model.Package }).GetAwaiter().GetResult();
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
