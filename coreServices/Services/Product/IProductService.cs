using coreServices.DTOs;
using coreServices.DTOs.Product;
using coreServices.DTOs.Product.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreServices.Services.Product
{
    public interface IProductService
    {
        ProductDTO AddProduct(ProductDTO product);
        IEnumerable<ProductDTO> GetAllAvailableProducts();
        ProductDTO UpdatePrice(Guid userId, UpdateProductDTO productDto);
        ProductDTO UpdateProductName(Guid userId, UpdateProductDTO productDto);
        GenericResponse DeleteProduct(Guid userId, Guid productId);
        ProductDTO UpdateQuantity(Guid userId, UpdateProductDTO productDto);
        DTOs.Product.Out.BuyProductDTO BuyProduct(Guid userId, DTOs.Product.In.BuyProductDTO product);
    }
}
