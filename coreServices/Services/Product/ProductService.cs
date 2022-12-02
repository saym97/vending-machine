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
            var existingProduct = _dbContext.Products.FirstOrDefault(x=> x.Name == product.Name && x.Cost == product.Cost);
            
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
                    Cost = product.Cost,
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

        public ProductDTO UpdatePrice(Guid userId, UpdateProductDTO productDto)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productDto.ProductId && x.SellerId.Equals(userId));
            if (product == null)
                return null;
            try
            {
                bool ProductExistAlready = _dbContext.Products.Any(x => x.Name == product.Name && x.Cost == productDto.Cost && x.SellerId.Equals(userId));
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

        public ProductDTO UpdateProductName(Guid userId, UpdateProductDTO productDto)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productDto.ProductId && x.SellerId.Equals(userId));
            if (product == null)
                return null;
            try
            {
                bool ProductExistAlready = _dbContext.Products.Any(x => x.Name == productDto.Name && x.Cost == product.Cost && x.SellerId.Equals(userId));
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

        public GenericResponse DeleteProduct(Guid userId, Guid productId)
        {
            var retval = new GenericResponse()
            {
                Success = false,
                Message = "Error occured while deleting the product."
            };
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productId && x.SellerId.Equals(userId));
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

        public ProductDTO UpdateQuantity(Guid userId, UpdateProductDTO productDto)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productDto.ProductId && x.SellerId.Equals(userId));
            if (product == null)
                return null;
            
            product.AmountAvailable = (int) productDto.Amount;
            _dbContext.SaveChanges();

            return _mapper.Map<ProductDTO>(product);
        }

        public DTOs.Product.Out.BuyProductDTO BuyProduct(Guid userId, DTOs.Product.In.BuyProductDTO productDto)
        {
            try
            {
                //Check if user is good
                var user = _dbContext.Users.FirstOrDefault(x => x.UserId == userId);
                if (user == null)
                    throw new Exception("Error occured while processing the purchase. Try again.");
                
                //check if product is good
                var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == productDto.Id);
                if (product == null)
                    throw new Exception("Product couldn't be found. Try again");

                //check if user has enough money
                int totalCost = product.Cost * productDto.Amount;
                bool UserHaveMoney = user.Deposit >= totalCost;
                if (!UserHaveMoney)
                    throw new Exception($"Sorry, you need to have {totalCost} cents to buy these products. You only have {user.Deposit} cents");
                
                //check if product is enough
                bool isProductAvailable = (product.AmountAvailable > 0 && product.AmountAvailable >= productDto.Amount);
                if (!isProductAvailable)
                    throw new Exception($"Sorry there are not enough {product.Name} in stock. Currently, there are only {product.AmountAvailable} {product.Name} available");
                

                //Decrease product by amount
                product.AmountAvailable -= productDto.Amount;
                //Calculate amount of change
                int change = user.Deposit - totalCost;
                //convert the change into coins
                List<int> coins = (change > 0) ? GetChangeCoins(change) : null ;
                user.Deposit = 0;
                _dbContext.SaveChanges();

                DTOs.Product.Out.BuyProductDTO buyProductDTO = new DTOs.Product.Out.BuyProductDTO()
                {
                    ProductName = product.Name,
                    Amount = productDto.Amount,
                    TotalCost = totalCost,
                    Change = coins
                };

                return buyProductDTO; 

            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<int> GetChangeCoins(int change)
        {
            List<int> changeCoins = new List<int>();
            while(change > 0)
            {
                if (change % 5 != 0)
                    break;
                int coin = 0;

                if (change - 100 >= 0)
                    coin = 100;
                else if (change - 50 >= 0)
                    coin = 50;
                else if (change - 20 >= 0)
                    coin = 20;
                else if (change - 10 >= 0)
                    coin = 10;
                else if (change - 5 >= 0)
                    coin = 5;

                change -= coin;
                changeCoins.Add(coin);
            }
            return changeCoins;
        }
    }
}
