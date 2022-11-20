using AutoMapper;
using coreServices.DTOs.Product;
using coreServices.Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreServices.Services.Product
{
    public class ProductService : BaseService, IProductService
    {
        private IMapper _mapper;
        public ProductService(IMapper mapper) : base(mapper)
        {
            _mapper = mapper; 
        }

        public ProductDTO HelloProductService()
        {
            ProductDTO productDTO = new ProductDTO()
            {
                ProductId = Guid.NewGuid(),
                Cost = 2,
                Name = "Chocolate",
                SellerId = Guid.NewGuid()
            };

            return productDTO;
        }
    }
}
