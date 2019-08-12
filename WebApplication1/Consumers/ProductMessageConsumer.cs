using MassTransit;
using Messages.Contract;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Consumers
{
    public class ProductMessageConsumer : IConsumer<SubmitProduct>
    {

        private IRepository<Product> _repository;

        public ProductMessageConsumer(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public Task Consume(ConsumeContext<SubmitProduct> context)
        {
            var product = new Product
            {
                ProductName=context.Message.ProductName,
                Package= context.Message.Package,
                IsDiscontinued=context.Message.IsDiscontinued,
                SupplierId= context.Message.SupplierId,
                UnitPrice=context.Message.UnitPrice
            };

            _repository.Create(product);
            return Task.CompletedTask;
        }
    }    
}