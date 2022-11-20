using AutoMapper;
using coreServices.DTOs.Product;
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


        [HttpPost("create")]
        [Authorize(Roles = "Seller")]
        public IActionResult CreateProduct(ProductDTO product)
        {
            var currentUser = GetLoggedUser();
            if (currentUser == null)
                return Unauthorized("user token is invalid");

            if (product == null || product.Name.IsNullOrEmpty())
                return BadRequest("product data is invalid");

            if ((product.Cost * 100) % 5 != 0)
                return BadRequest("Invalid price: The Machine only accepts 5, 10, 20,50,100 cent coin, so please set the cost according to it.");
            
            product.SellerId = currentUser.Id;
            var result = _productService.AddProduct(product);

            if(result == null)
                return NotFound("couldn't add the product");
            return Ok(result);
        }
    }
}
