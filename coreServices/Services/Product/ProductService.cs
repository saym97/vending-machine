using AutoMapper;
using coreServices.DTOs;
using coreServices.DTOs.Product;
using coreServices.Infrastructure.Base;
using dbContext.VendingMachine;
using dbContext.VendingMachine.Entities;
using Microsoft.Extensions.Hosting;
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

        public ProductDTO UpdatePrice(UpdateProductDTO productDto)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productDto.ProductId);
            if (product == null)
                return null;
            try
            {
                bool ProductExistAlready = _dbContext.Products.Any(x => x.Name == product.Name && x.Cost == productDto.Cost);
                if (ProductExistAlready)
                    throw new Exception("You already have added a product with the same price and name.");
                product.Cost = productDto.Cost.Value;
                _dbContext.SaveChanges();
                return _mapper.Map<ProductDTO>(product);
            }catch(Exception ex)
            {
                throw;
            }
            
        }

        public ProductDTO UpdateProductName(UpdateProductDTO productDto)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productDto.ProductId);
            if (product == null)
                return null;
            try
            {
                bool ProductExistAlready = _dbContext.Products.Any(x => x.Name == productDto.Name && x.Cost == product.Cost);
                if (ProductExistAlready)
                    throw new Exception("You already have added a product with the same name and price.");
               
                product.Name = productDto.Name;
                _dbContext.SaveChanges();
                
                return _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public GenericResponse DeleteProduct(Guid productId)
        {
            var retval = new GenericResponse()
            {
                Success = false,
                Message = "Error occured while deleting the product."
            };
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product == null)
            {
                retval.Message = "Couldn't find the product";
                return retval;
            }
            
            
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            retval.Success = true;
            retval.Message = $"Successfully deleted the Product: {product.Name} Cost: {product.Cost}";
            return retval;
        }

        public ProductDTO UpdateQuantity(UpdateProductDTO productDto)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productDto.ProductId);
            if (product == null)
                return null;
            
            product.AmountAvailable = (int) productDto.Amount;
            _dbContext.SaveChanges();

            return _mapper.Map<ProductDTO>(product);
        }
    }
}
