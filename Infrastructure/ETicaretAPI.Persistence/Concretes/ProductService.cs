using ETicaretAPI.Application.Abstraction;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Concretes
{
    public class ProductService : IProductService
    {
        public List<Product> GetProducts()
            => new()
            {
                new(){Id=Guid.NewGuid(),Name="Product1",Price=123,CreatedDate=DateTime.Now,Stock=1},
                new(){Id=Guid.NewGuid(),Name="Product2",Price=13,CreatedDate=DateTime.Now,Stock=3},
                new(){Id=Guid.NewGuid(),Name="Product3",Price=1223,CreatedDate=DateTime.Now,Stock=5},
                new(){Id=Guid.NewGuid(),Name="Product4",Price=1233,CreatedDate=DateTime.Now,Stock=7},
                new(){Id=Guid.NewGuid(),Name="Product5",Price=12,CreatedDate=DateTime.Now,Stock=9}
            };
        
    }
}
