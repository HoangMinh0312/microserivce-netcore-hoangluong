using AutoMapper;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.ViewModel;

namespace WebApplication2.Services
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product Create(CreateProductRequest model);
        Product Update(UpdateProductRequest model);
        Product Delete(DeleteProductRequest model);
    }


    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;

        private IRepository<Product> _productRepository;
        public ProductService(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public Product Create(CreateProductRequest model)
        {
            var product = _mapper.Map<Product>(model);
            _productRepository.Create(product);
            return product;
        }

        public Product Delete(DeleteProductRequest model)
        {
            var product = new Product { Id = model.Id };
            _productRepository.Delete(product);
            return product;
        }

        public List<Product> GetAll()
        {
            return _productRepository.Filter(x => true).ToList();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product Update(UpdateProductRequest model)
        {
            var product = AutoMapper.Mapper.Map<Product>(model);
             _productRepository.Edit(product);
            return product;
        }
    }
}
