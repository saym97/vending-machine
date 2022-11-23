using AutoMapper;
using coreServices.DTOs.Product;
using coreServices.DTOs.Product.In;
using coreServices.Services.Product;
using coreServices.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, ILogger<ProductController> logger, IUserService userService, IProductService productService): base(mapper,logger,userService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<ProductDTO> Get()
        {
            return _productService.GetAllAvailableProducts();
        }

        [HttpPost("buy")]
        [Authorize(Roles = "Buyer")]
        public IActionResult BuyProduct([FromBody] BuyProductDTO product)
        {
            var currentUser = GetLoggedUser();
            if (currentUser == null)
                return Unauthorized("user token is invalid");

            if(product == null || product.Amount <= 0)
                return BadRequest("invalid product data");
            try
            {
                var result = _productService.BuyProduct(currentUser.Id, product);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        [HttpPost("create")]
        [Authorize(Roles = "Seller")]
        public IActionResult CreateProduct(ProductDTO product)
        {
            var currentUser = GetLoggedUser();
            if (currentUser == null)
                return Unauthorized("user token is invalid");

            if (product == null || product.Name.IsNullOrEmpty())
                return BadRequest("product data is invalid");

            if (product.Cost <= 0 || product.Cost % 5 != 0)
                return BadRequest("Invalid price: The Machine only accepts 5, 10, 20, 50, 100 cent coin, so please set the cost according to it.");
            
            product.SellerId = currentUser.Id;
            var result = _productService.AddProduct(product);

            if(result == null)
                return NotFound("couldn't add the product");
            return Ok(result);
        }


        //TODO: Update Price
        [HttpPatch("update-price")]
        [Authorize(Roles = "Seller")]
        public IActionResult UpdateProductPrice(UpdateProductDTO product)
        {
            if(product == null  || product.Cost == 0)
                return BadRequest("Invalid product data");
            try
            {
                ProductDTO result = _productService.UpdatePrice(product);
                if (result == null)
                    return NotFound("Product couldn't not be found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        //TODO: Update Quatity
        [HttpPatch("update-quantity")]
        [Authorize(Roles = "Seller")]
        public IActionResult UpdateProductQuantity(UpdateProductDTO product)
        {
            if (product == null)
                return BadRequest("Invalid product data");

            ProductDTO result = _productService.UpdateQuantity(product);
            if (result == null)
                return NotFound("Product couldn't not be found");

            return Ok(result);
        }

        //TODO: Update Name
        [HttpPatch("update-name")]
        [Authorize(Roles = "Seller")]
        public IActionResult UpdateProductName(UpdateProductDTO product)
        {
            if (product == null || product.Name.IsNullOrEmpty())
                return BadRequest("Invalid product data");
            try
            {
                ProductDTO result = _productService.UpdateProductName(product);
                if (result == null)
                    return NotFound("Product couldn't not be found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //TODO: DeleteProduct
        [HttpDelete("delete")]
        [Authorize(Roles = "Seller")]
        public IActionResult DeleteProduct([FromBody] string productId)
        {
            if (productId.IsNullOrEmpty())
                return BadRequest("product id cannot be empty");

            Guid Id;
            bool IsValidId = Guid.TryParse(productId,out Id);
            if (!IsValidId)
                return BadRequest("The product id is not valid");

            var result = _productService.DeleteProduct(Id);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }
    }
    }
