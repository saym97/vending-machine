using coreServices.DTOs.Product.In;
using coreServices.Services.Product;
using coreServices.Services.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unitTesting
{
    internal class BuyTests: DISetup
    {
        public IProductService _productService;

        public BuyTests() : base()
        {
            _productService = (IProductService)_serviceProvider.GetService(typeof(IProductService));
        }

        [Test]
        public void Test_ConfigService()
        {
            Assert.IsNotNull(_productService);
        }

        Guid userId = new Guid("2F4C5D0A-7513-4347-9917-2ABBF3A231DF");


        [Test]
        public void Test_productIsNotAvailable()
        {
            BuyProductDTO buyProductDTO = new BuyProductDTO()
            {
                Id = new Guid(),
                Amount = 2
            };
            try
            {
                var response = _productService.BuyProduct(userId, buyProductDTO);

            }catch (Exception ex)
            {
                Assert.AreEqual("Product couldn't be found. Try again", ex.Message);
            }
        }



        [Test]
        public void Test_UserDoesntHaveEnoughMoney()
        {
            BuyProductDTO _buyProductDTO = new BuyProductDTO()
            {
                Id = new Guid("7f43bafc-964a-4728-85af-905523f6419a"),
                Amount = 2
            };
            try
            {
                var response = _productService.BuyProduct(userId, _buyProductDTO);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("Sorry, you need to have"));
            }
        }

        [Test]
        public void Test_NotEnoughStockInProduct()
        {
            BuyProductDTO _buyProductDTO = new BuyProductDTO()
            {
                Id = new Guid("A8C356D4-2A2B-47A7-96B3-20EC4BA92ADA"),
                Amount = 5
            };
            try
            {
                var response = _productService.BuyProduct(userId, _buyProductDTO);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("Sorry there are not enough"));
            }
        }

        [Test]
        public void Test_ProductBoughtSuccessfully()
        {
            BuyProductDTO _buyProductDTO = new BuyProductDTO()
            {
                Id = new Guid("A8C356D4-2A2B-47A7-96B3-20EC4BA92ADA"),
                Amount = 1
            };
            var response = _productService.BuyProduct(userId, _buyProductDTO);
            
            Assert.IsNotNull(response);
        }


    }
}
