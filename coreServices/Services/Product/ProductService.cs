using AutoMapper;
using coreServices.DTOs.Product;
using coreServices.Infrastructure.Base;
using dbContext.VendingMachine;
using dbContext.VendingMachine.Entities;
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
        private readonly VendingMachineContext _dbContext;
        public ProductService(VendingMachineContext dbContext ,IMapper mapper) : base(mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper; 
        }

        public IEnumerable<ProductDTO> GetAllAvailableProducts()
        {
            var products = _dbContext.Products.Where(x=>x.AmountAvailable != 0).ToList();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public ProductDTO AddProduct(ProductDTO product)
        {
            var cost = product.Cost * 100;
            var existingProduct = _dbContext.Products.FirstOrDefault(x=> x.Name == product.Name && x.Cost == cost);
            
            if(existingProduct != null)
            {
                existingProduct.AmountAvailable++;
                _dbContext.SaveChanges();
                return _mapper.Map<ProductDTO>(existingProduct);
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var newproduct = new Products()
                {
                    ProductId = new Guid(),
                    Name = product.Name,
                    Cost = cost,
                    AmountAvailable = 1,
                    SellerId = product.SellerId,
                };
                
                _dbContext.Products.Add(newproduct);
                _dbContext.SaveChanges();
                transaction.Commit();
                return _mapper.Map<ProductDTO>(newproduct);

            }catch (Exception)
            {
                transaction.Rollback();
                return null;
            }

        }
    }
}
